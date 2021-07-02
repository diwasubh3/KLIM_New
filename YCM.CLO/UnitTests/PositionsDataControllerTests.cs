using System;
using System.Transactions;
using NUnit.Framework;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.Web.App_Start;
using YCM.CLO.Web.Controllers;
using YCM.CLO.Web.Objects;

namespace UnitTests
{
	[TestFixture]
	public class PositionsDataControllerTests
	{
		private readonly PositionDataController _controller;
		private static readonly WatchDataController _watchDataController;

		static PositionsDataControllerTests()
		{
			var repo = new Repository();
			var alertEngine = new AlertEngine(repo);
			_watchDataController = new WatchDataController(repo, alertEngine, new RuleEngine(repo, alertEngine), new PositionCacheManager());
		}

		public PositionsDataControllerTests()
		{
			var repo = new Repository();
			var alertEngine = new AlertEngine(repo);
			_controller = new PositionDataController(new Repository()
				, new AlertEngine(repo), new RuleEngine(repo, alertEngine));
		}

		[Test]
		public static void WhenIWantToTestDifferentShit()
		{
			try
			{
				var controller = new DataController(new Repository());
				var crap = controller.GetAnalystResearchHeader(2189);
				controller.GetSummaries();
				//controller.DownloadSummaries();
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}

		public static void WhenIWantToTestShit()
		{
			try
			{
				AutoMapperConfig.RegisterMappings();
				using (var scope = new TransactionScope(TransactionScopeOption.Required))
				{
					var watch = new Watch
					{
						WatchObjectId = 2170,
						WatchObjectTypeId = 1,
						WatchTypeId = 2,
						WatchUser = "whogivesacrap",
						WatchLastUpdatedOn = DateTime.Now,
						WatchComments = "agaon who gives a shit"
					};
					var crap = _watchDataController.SaveWatch(watch, "CLO-1");
					//_watchDataController.DeleteWatch(watch.WatchId);
					//scope.Complete();
				}
			}
			catch (Exception ex)
			{
				Assert.Fail($"An exception occurred. {ex}");
			}
		}

		public void WhenAllPositionsAreRequested()
		{
			try
			{
				AutoMapperConfig.RegisterMappings();
				var result = _controller.GetAllPositions("CLO-1", true);
			}
			catch (Exception ex)
			{
				Assert.Fail($"WhenAllPositionsAreRequested failed! {ex}");
			}
		}
	}
}
