using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using log4net;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Controllers
{
	public class PositionDataController : Controller
    {
        readonly IRepository _repository;
        private readonly IAlertEngine _alertEngine;
        private readonly IRuleEngine _ruleEngine;
	    private readonly ILog _logger;

        //public PositionDataController() : this(new Repository(),new AlertEngine(),new RuleEngine())
        //{

        //}

        public PositionDataController(IRepository repository, IAlertEngine alertEngine, IRuleEngine ruleEngine)
        {
            _repository = repository;
            _alertEngine = alertEngine;
            _ruleEngine = ruleEngine;
	        _logger = LogManager.GetLogger(typeof(PositionDataController));
        }

		public JsonNetResult GetTopBottomPositions(short ruleId, string fundCode)
        {
            int dateId = Helper.GetPrevDayDateId();
            return new JsonNetResult()
            {
                Data = _ruleEngine.Process(ruleId, fundCode,dateId)
            };
        }

        public JsonNetResult GetPositions(string fundCode, bool? onlyWithExposures)
        {
            int dateId = Helper.GetPrevDayDateId();
            var positionSource = Mapper.Map<IEnumerable<vw_Position>, IEnumerable<PositionDto>>(_repository.GetPositions(fundCode, onlyWithExposures)).ToList();
            _alertEngine.ProcessAlerts(positionSource,dateId,fundCode);

            TradesProcessor tradesProcessor = new TradesProcessor();
            tradesProcessor.Process(_repository,positionSource,fundCode);
            return new JsonNetResult()
            {
                Data = positionSource
            };
        }

        public JsonNetResult GetAllPositions(string fundCode, bool? onlyWithExposures)
        {
		
            _logger.Info($"GetAllPositions fund:{fundCode} with exp:{onlyWithExposures.GetValueOrDefault()}");
            int dateId = Helper.GetPrevDayDateId();
			
			_logger.Info($"date id: {dateId}");
	        var positions = onlyWithExposures.GetValueOrDefault()
		        ? CLOCache.GetPositionsWithExposure()
		        : CLOCache.GetAllPositions();

	        _logger.Info($"Position count: {positions.Count}");

	        var positionSource = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(positions);
	        var crap = positionSource.Where(x => x.IsSellCandidate).ToList();
	        var crappy = positionSource.Where(x => x.IsOnWatch.GetValueOrDefault()).ToList();

			_logger.Info($"Mapped position count: {positionSource.Count()}");
	        
			_alertEngine.ProcessAlerts(positionSource, dateId,fundCode);
	        var posse = positionSource.Where(x => x.TRSNumExposure.HasValue).ToList();
	        var nex = positionSource.Where(x => x.SecurityId == 1649 || x.SecurityId == 1661).ToList();

            TradesProcessor tradesProcessor = new TradesProcessor();
            tradesProcessor.Process(_repository, positionSource, fundCode);

            var result = new JsonNetResult()
            {
                Data = positionSource
            };

			_logger.Info($"Returning {positionSource.Count()} rows.");
	        return result;
        }

        public JsonNetResult GetPositionsBasedOnSecurityCodeAndFundCode(string securityCode, string fundCode)
        {
            int dateId = Helper.GetPrevDayDateId();
            var positionSource = Mapper.Map<IEnumerable<vw_Position>, IEnumerable<PositionDto>>(_repository.GetPositions(securityCode, fundCode)).ToList();
            _alertEngine.ProcessAlerts(positionSource, dateId,fundCode);
            TradesProcessor tradesProcessor = new TradesProcessor();
            tradesProcessor.Process(_repository, positionSource, fundCode);
            return new JsonNetResult()
            {
                Data = positionSource
            };
        }

        public JsonNetResult GetAllPositionsBasedOnSecurityCodeAndFundCode(string securityCode, string fundCode)
        {
            int dateId = Helper.GetPrevDayDateId();
            var positionSource = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(_repository.GetAllPositions(securityCode)).ToList();
            _alertEngine.ProcessAlerts(positionSource, dateId,fundCode);
            TradesProcessor tradesProcessor = new TradesProcessor();
            tradesProcessor.Process(_repository, positionSource, fundCode);
            return new JsonNetResult()
            {
                Data = positionSource
            };
        }

        [HttpPost]
        public JsonNetResult GetAllPositionsForSecurities(int[] securityIds,string fundCode)
        {
            //AllPositionsCache allPositionsCache = new AllPositionsCache();
            int dateId = Helper.GetPrevDayDateId();
	        var positionSource = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(CLOCache.GetAllPositions()
		        .Where(a => securityIds.Contains(a.SecurityId))
		        .OrderBy(p => p.Issuer)
		        .ThenBy(p => p.SecurityCode)
		        .ToList());
            //var positionSource = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(allPositionsCache.PositionWithNullExposure.Where(a=>securityIds.Contains(a.SecurityId)).OrderBy(p=>p.Issuer).ThenBy(p=>p.SecurityCode).ToList());
			_alertEngine.ProcessAlerts(positionSource, dateId, fundCode);
            TradesProcessor tradesProcessor = new TradesProcessor();
            tradesProcessor.Process(_repository, positionSource, fundCode);
            return new JsonNetResult()
            {
                Data = positionSource
            };
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }

    }
}