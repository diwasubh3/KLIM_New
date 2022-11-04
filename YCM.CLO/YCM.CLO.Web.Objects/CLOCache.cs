using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using log4net;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using static YCM.CLO.DataAccess.Constants;
using static YCM.CLO.DataAccess.Helper;

namespace YCM.CLO.Web.Objects
{
	public static class CLOCache
	{
		private static readonly MemoryCache _cache = MemoryCache.Default;
		private static readonly ILog _logger = LogManager.GetLogger(typeof(CLOCache));
		private static object _cacheLock = new object();
		private static IRepository _repository = new Repository();
		private static ConcurrentDictionary<string, int> _cacheSettings;

		static CLOCache()
		{
			var settings = GetDataInternal<CacheSetting>(CacheSettingsCacheKey, 180);
			_cacheSettings = new ConcurrentDictionary<string, int>(settings.ToDictionary(k => k.CacheSettingKey, v => v.CacheExpirationInSeconds));
		}

		public static List<vw_AggregatePosition> GetAllPositions()
			=> GetPositionsInternal(true);

		public static List<vw_AggregatePosition> GetPositionsWithExposure()
			=> GetPositionsInternal(false);

		public static bool UpdatePositions(IEnumerable<vw_AggregatePosition> positions)
		{
			lock (_cacheLock)
			{
				var allPositions = GetAllPositions();
				positions.ToList().ForEach(p =>
				{
					var originalPosition = allPositions.FirstOrDefault(a => a.SecurityId == p.SecurityId);
					if (originalPosition != null)
					{
						var originalpositionIndex = allPositions.IndexOf(originalPosition);
						allPositions.RemoveAt(originalpositionIndex);
						allPositions.Insert(originalpositionIndex, p);
					}
				});
			}
			return true;
		}

		public static void Refresh() => Task.Run(() => FillCache<vw_AggregatePosition>(PositionsCacheKey, DateTimeOffset.Now.AddSeconds(GetExpiration(PositionsCacheKey)), true));

		public static void Invalidate() => Refresh();
		public static List<vw_CLOSummary> GetSummaries() =>
			GetDataInternal<vw_CLOSummary>(SummariesCacheKey, GetExpiration(SummariesCacheKey));

		public static List<vw_CLOTestResults> GetTestResults () =>
			GetDataInternal<vw_CLOTestResults>(TestResultsCacheKey, GetExpiration(TestResultsCacheKey));

		private static List<vw_AggregatePosition> GetPositionsInternal(bool includeWithNullExposure)
		{
			var allPositions = GetDataInternal<vw_AggregatePosition>(PositionsCacheKey, GetExpiration(PositionsCacheKey));
			_logger.Info($"Retrieved data from the cache.  Position count: {allPositions.Count}");
			var positions = includeWithNullExposure ? allPositions
				: allPositions.Where(t => Math.Round(t.BODTotalPar, 2) != 0
				                          || Math.Round(t.TotalParNum.GetValueOrDefault(), 2) != 0).ToList();
			_logger.Info($"Returning {positions.Count} positions. Include null exposure: {includeWithNullExposure}");
			return positions;
		}

		private static List<T> GetDataInternal<T>(string key, int expirationInSeconds)
		{
			var data = new List<T>();
			var retries = 0;
			_logger.Info("Checking for existence of cache entry...");
			if (!CacheData)
			{
				_logger.Info($"Not using the cache.  Getting data from the database for {key}...");
				return GetDataFromTheDatabase<T>(key);
			}
			while (_cache.Get(key) == null && retries <= 11)
			{
				_logger.Info($"Attempting to get data. Retry count: {retries}");
				FillCache<T>(key, DateTimeOffset.Now.AddSeconds(expirationInSeconds));
				retries++;
				if (_cache.Get(key) == null)
				{
					_logger.Info($"Unable to get data from the db.  Continuing. Retry count: {retries}");
					continue;
				}
			}

			if (_cache.Get(key) != null)
			{
				if (retries > 0)
					retries--;
				data = _cache.Get(key) as List<T>;
				_logger.Info($"Retrieved data from the cache.  Position count: {data.Count} Retry count: {retries}");
			}

			return data;

		}

		private static void FillCache<T>(string key, DateTimeOffset cacheExpiration, bool refresh = false)
		{
			if (!CacheData)
			{
				_logger.Info("Caching is disabled.");
                _repository.GenerateAggregatedPositions();
				return;
			}
			_logger.Info($"Attempting to get FillCache lock. Refresh: {refresh}");
			lock (_cacheLock)
			{
				_logger.Info("Got FillCache lock.");
				if (_cache.Get(key) == null || refresh)
				{
					try
					{
						_logger.Info($"Does mem cache item {key} exist? {_cache.Get(key) != null}");
						_logger.Info($"Getting {key} from the database...");
						var data = GetDataFromTheDatabase<T>(key);
						_logger.Info($"Got {data.Count} items.  Adding to cache.");
						var cacheItemPolicy = new CacheItemPolicy();
						cacheItemPolicy.AbsoluteExpiration = cacheExpiration;
						cacheItemPolicy.RemovedCallback += CacheItemRemoved<T>;
						_cache.Set(key, data, cacheItemPolicy);
						_logger.Info("Added to cache.");
					}
					catch (Exception ex)
					{
						_logger.Error(ex);
					}
				}
				else
					_logger.Info($"Data exists in the cache.  No need to go to the db. Refresh: {refresh}");
			}
		}

		private static List<T> GetDataFromTheDatabase<T>(string key)
		{
			try
			{
				var data = new List<T>();
				switch (key)
				{
					case CacheSettingsCacheKey:
						data = _repository.GetCacheSettings() as List<T>;
						break;
					case PositionsCacheKey:
						data = _repository.GetAllPositions(false) as List<T>;
						break;
					case SummariesCacheKey:
						data = _repository.GetSummaries(GetPrevDayDateId()) as List<T>;
						break;
					case TestResultsCacheKey:
						data = _repository.GetTestResults(GetPrevDayDateId()) as List<T>;
						break;
				}
				_logger.Info($"Got {data.Count} items.");
				return data;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				throw;
			}
		}

		private static void CacheItemRemoved<T>(CacheEntryRemovedArguments arguments)
		{
			//for now, we only want to refill if the item was removed due to an expiration
			var key = arguments.CacheItem.Key;
			_logger.Info($"Cache item {key} removed.  Reason: {arguments.RemovedReason}");
			if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
			{
				_logger.Info($"Cache expired.  Calling FillCache for item {key}.");
				var expiration = GetExpiration(key);
				FillCache<T>(key, DateTimeOffset.Now.AddSeconds(expiration));
			}
		}

		private static int GetExpiration(string key) => _cacheSettings.ContainsKey(key) ? _cacheSettings[key] : DefaultCacheExpirationInSeconds;

	}
}
