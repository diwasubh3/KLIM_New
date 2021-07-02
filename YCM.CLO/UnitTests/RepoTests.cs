using System;
using System.Linq;
using NUnit.Framework;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;

namespace UnitTests
{
	[TestFixture]
    public class RepoTests
	{
		private readonly IRepository _repo;
	    public RepoTests()
	    {
		    _repo = new Repository();
	    }

		[Test]
		public void WhenIWantToTestShit()
		{
			try
			{
				var crap = _repo.GetCacheSettings();
				var crappier = crap;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Assert.Fail($"{ex.Message}");
			}
		}

		public void WhenAnIssuerIsFound()
		{
			try
			{
				var crap = _repo.GetIssuers().OrderByDescending(x => x.IssuerCode).ToList();
				var ish = crap.FirstOrDefault(x => !string.IsNullOrEmpty(x.IssuerCode)
					&& x.IssuerCode.Equals("United Rentals (North America), Inc.", StringComparison.CurrentCultureIgnoreCase));
				Assert.Pass();
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}

		public void WhenIJustWantToTestShit()
		{
			try
			{
				var crap = _repo.AddOrUpdateWatch(new Watch
				{
					WatchObjectId = 2170,
					WatchObjectTypeId = 1,
					WatchTypeId = 2,
					WatchUser = "whogivesacrap",
					WatchLastUpdatedOn = DateTime.Now,
					WatchComments = "agaon who gives a shit"
				}, 20181120);
				Assert.Pass();
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}
		public void WhenFundRestrictionsAreRetrieved()
		{
			try
			{
				var crap = _repo.GetFundRestrictions(7);
				Assert.Pass();
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}

		public void WhenTopBottomOnePriceMoversAreRetrieved()
		{
			try
			{
				var fromDateId = Helper.GetPrevDayDateId();
				var toDateId = Helper.GetDateId(Helper.GetPrevBusinessDay(1));
				var movers = _repo.GetPriceMove("Bottom", fromDateId, toDateId);
				var crap = movers;
				Assert.Pass();
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}

		public void WhenTopBottomPositionsAreRetrieved()
		{
			try
			{
				var rule = _repo.GetRule(2);
				var dateId = _repo.GetPrevDayDateId();
				var result = _repo.ExecuteRule(rule, "CLO-1", dateId);
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}

		public void WhenAllPositionsAreRetrieved()
		{
			try
			{
				var positions = _repo.GetAllPositions(false).ToList();
				var count = positions.Count;
				Assert.IsTrue(count > 0);
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}
    }
}
