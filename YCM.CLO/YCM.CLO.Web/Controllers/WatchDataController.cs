using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;
using YCM.CLO.Web.Objects.Contract;
using YCM.CLO.DataAccess;
using log4net;

namespace YCM.CLO.Web.Controllers
{
	public class WatchDataController : Controller
    {
	    private readonly IRepository _repository;
        private readonly IAlertEngine _alertEngine;
        private readonly IRuleEngine _ruleEngine;
        private readonly IPositionCacheManager _cacheManager;
	    private static readonly ILog _logger = LogManager.GetLogger(typeof(WatchDataController));

        //public WatchDataController() : this(new Repository(),new AlertEngine(),new RuleEngine(),new PositionCacheManager())
        //{

        //}

		public WatchDataController(IRepository repository, IAlertEngine alertEngine, IRuleEngine ruleEngine,IPositionCacheManager cacheManager)
        {
            _repository = repository;
            _alertEngine = alertEngine;
            _ruleEngine = ruleEngine;
            _cacheManager = cacheManager;
        }

        [HttpPost]
        public JsonNetResult SaveWatch(Watch watch, string fundCode)
        {
	        try
	        {
		        watch.WatchLastUpdatedOn = DateTime.Now;
		        watch.WatchUser = User?.Identity?.Name;

				_logger.Info(watch);

		        var positions = _repository.AddOrUpdateWatch(watch, Helper.GetPrevDayDateId()).ToArray();
                _repository.GenerateAggregatedPositions();
		        var allpositions = (_repository as IRepository).GetAllPositions(positions.Select(p => p.SecurityId.Value).ToArray()).ToList();
		        _cacheManager.Update(allpositions);
		        var allpositionsDtos = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(allpositions);
		        var crap = allpositionsDtos.Where(x => x.IsSellCandidate).ToList();
		        _alertEngine.ProcessAlerts(allpositionsDtos, Helper.GetPrevDayDateId(), fundCode);
		        TradesProcessor tradesProcessor = new TradesProcessor();
		        tradesProcessor.Process(_repository, allpositionsDtos, fundCode);


		        return new JsonNetResult() { Data = allpositionsDtos };
	        }
			catch (Exception ex)
	        {
		        _logger.Error(ex);
		        throw;
	        }
        }

        [HttpDelete]
        public JsonNetResult DeleteWatch(int watchId)
        {
            var positions =
                Mapper.Map<IEnumerable<vw_Position>, IEnumerable<PositionDto>>(_repository.DeleteWatch(watchId,
                    Helper.GetPrevDayDateId())).ToArray();

            _repository.GenerateAggregatedPositions();
            var allpositions = (_repository as IRepository).GetAllPositions(positions.Select(p => p.SecurityId).ToArray()).ToList();
            _cacheManager.Update(allpositions);

            var allpositionsDtos = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(allpositions);

            if (positions != null)
            {
                _alertEngine.ProcessAlerts(allpositionsDtos, Helper.GetPrevDayDateId(), positions.First().FundCode);
                TradesProcessor tradesProcessor = new TradesProcessor();
                tradesProcessor.Process(_repository, allpositionsDtos, positions.First().FundCode);
            }
            return new JsonNetResult() { Data = allpositionsDtos };
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }

    }
}