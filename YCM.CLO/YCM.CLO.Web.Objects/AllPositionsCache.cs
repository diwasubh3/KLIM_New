using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Objects
{
	public class AllPositionsCache : ICache<vw_AggregatePosition>
    {
	    private const string PositionsKey = "Positions";

	    private static readonly ILog _logger;
		private static readonly MemoryCache _cache = MemoryCache.Default;
	    private static readonly int PositionCacheExpirationInHours;
		static IRepository _repository;
        static object _lock ;

        static AllPositionsCache()
        {
	        _logger = LogManager.GetLogger(typeof(AllPositionsCache));
            _repository = new Repository();
	        var configValue = ConfigurationManager.AppSettings[nameof(PositionCacheExpirationInHours)];
	        int val;
	        int.TryParse(configValue, out val);
	        PositionCacheExpirationInHours = val > 0 ? val : 2;
			_logger.Info($"Cache will expire in {PositionCacheExpirationInHours} hours.");
			_lock = new object();
			Task.Run(() => FillCache());
        }

	    public static bool Check() => true;

	    private static List<vw_AggregatePosition> GetPositionsInternal(bool includeWithNullExposure)
	    {
			var positions = new List<vw_AggregatePosition>();
		    var retries = 0;
		    _logger.Info("Checking for existence of cache entry...");
		    while (_cache.Get(PositionsKey) == null && retries <= 11)
		    {
			    _logger.Info($"Attempting to get data. Retry count: {retries}");
				FillCache();
			    if (_cache.Get(PositionsKey) == null)
			    {
				    _logger.Info($"Unable to get data from the db.  Continuing. Retry count: {retries}");
				    continue;
			    }
			    retries++;
			}

			if(retries > 0)
				retries--;
		    if (_cache.Get(PositionsKey) != null)
		    {
			    var allPositions = _cache.Get(PositionsKey) as List<vw_AggregatePosition>;
			    _logger.Info($"Retrieved data from the cache.  Position count: {allPositions.Count} Retry count: {retries}");
			    positions = includeWithNullExposure ? allPositions
				    : allPositions.Where(t => Math.Round(t.BODTotalPar, 2) != 0
				                              || Math.Round(t.TotalParNum.GetValueOrDefault(), 2) != 0).ToList();
			    _logger.Info($"Returning {positions.Count} positions. Include null exposure: {includeWithNullExposure} Retry count: {retries}");
		    }

			return positions;

	    }

	    private static void FillCache(bool refresh = false)
	    {
		    _logger.Info($"Attempting to get FillCache lock. Refresh: {refresh}");
		    lock (_lock)
		    {
				_logger.Info("Got FillCache lock.");
			    if (_cache.Get(PositionsKey) == null || refresh)
			    {
				    try
				    {
						_logger.Info($"Does mem cache item {PositionsKey} exist? {_cache.Get(PositionsKey) != null}");
					    _logger.Info("Getting positions from the database...");
					    var positions = _repository.GetAllPositions(false).ToList();
					    _logger.Info($"Got {positions.Count} positions.  Adding to cache.");
					    var cacheItemPolicy = new CacheItemPolicy();
					    cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(PositionCacheExpirationInHours);
						cacheItemPolicy.RemovedCallback += CacheItemRemoved;
					    _cache.Set(PositionsKey, positions, cacheItemPolicy);
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

	    private static void CacheItemRemoved(CacheEntryRemovedArguments arguments)
	    {
		    //for now, we only want to refill if the item was removed due to an expiration
		    var key = arguments.CacheItem.Key;
		    _logger.Info($"Cache item {key} removed.  Reason: {arguments.RemovedReason}");
		    if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
		    {
			    _logger.Info($"Calling FillCache for item {key}.");
			    FillCache();
		    }
	    }

	    public static List<vw_AggregatePosition> GetAllPositions()
		    => GetPositionsInternal(true);

	    public static List<vw_AggregatePosition> GetPositionsWithExposure()
		    => GetPositionsInternal(false);

        public bool UpdatePositions(IEnumerable<vw_AggregatePosition> positions)
        {
            lock (_lock)
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

	    public static void Refresh()
		    => FillCache(true);

	    public void Invalidate() => Refresh();
    }
}
