using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Objects.Contract;
using IAlertEngine = YCM.CLO.Web.Objects.Contract.IAlertEngine;

namespace YCM.CLO.Web.Objects
{
    public  class RuleEngine : IRuleEngine,IDisposable
    {
        private readonly IRepository _repository;
        private readonly IAlertEngine _alertEngine;
        public RuleEngine(IRepository repository,IAlertEngine alertEngine)
        {
            _repository = repository;
            _alertEngine = alertEngine;
        }

        //public RuleEngine() : this(new Repository(),new AlertEngine())
        //{

        //}

        public RuleRestultDto Process(short ruleId, string fundCode,int dateId)
        {
            RuleResult ruleResults;
            //if (ruleId == 2)
            //{
            //    ruleResults = (new Top10Bottom10Cache()).Get(ruleId);
            //}
            //else
            //{
            var rule = _repository.GetRule(ruleId);
            ruleResults = _repository.ExecuteRule(rule, fundCode, dateId);
            //}

            var bottomPositions = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(ruleResults.BottomPositions).ToArray();
            var topPositions = Mapper.Map<IEnumerable<vw_AggregatePosition>, IEnumerable<PositionDto>>(ruleResults.TopPositions).ToArray();
            _alertEngine.ProcessAlerts(bottomPositions, dateId,fundCode);
            _alertEngine.ProcessAlerts(topPositions, dateId,fundCode);
            TradesProcessor tradesProcessor = new TradesProcessor();
            tradesProcessor.Process(_repository, bottomPositions, fundCode);
            tradesProcessor.Process(_repository, topPositions, fundCode);

            var ruleResultsDto = new RuleRestultDto
            {
                BottomPositions = bottomPositions,
                TopPositions = topPositions
            };


            return ruleResultsDto;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
