using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;
using YCM.CLO.Web.Objects.Contract;
using AutoMapper;
using YCM.CLO.DataAccess;

namespace YCM.CLO.Web.Controllers
{    
    public class TradebookingDataController : Controller
    {
        private IRepository _repository;
        private readonly IAlertEngine _alertEngine;
        private readonly IPositionCacheManager _cacheManager;
        public TradebookingDataController(IRepository repository, IAlertEngine alertEngine, IPositionCacheManager cacheManager)
        {
            _repository = repository;
            _alertEngine = alertEngine;
            _cacheManager = cacheManager;
        }

        public JsonNetResult GetSourceData()
        {
            var data = new
            {
                TradeType = Mapper.Map<IEnumerable<TradeType>, IEnumerable<TradeType>>(_repository.GetTradeType()),
                Traders = Mapper.Map<IEnumerable<Trader>, IEnumerable<Trader>>(_repository.GetTraders()),
                Facilities = Mapper.Map<IEnumerable<Facility>, IEnumerable<FacilityDto>>(_repository.GetFacilities()),
                CounterParty = Mapper.Map<IEnumerable<CounterParty>, IEnumerable<CounterParty>>(_repository.GetCounterParty()),
                SettleMethods = Mapper.Map<IEnumerable<SettleMethods>, IEnumerable<SettleMethods>>(_repository.GetSettleMethods()),
                InterestTreatment = Mapper.Map<IEnumerable<InterestTreatment>, IEnumerable<InterestTreatment>>(_repository.GetInterestTreatment()),
                AllocationRule = Mapper.Map<IEnumerable<AllocationRule>, IEnumerable<AllocationRule>>(_repository.GetAllocationRule()),
            };

            return new JsonNetResult() { Data = data };
        }

        public JsonNetResult GetTradeTypeData()
        {
            var data = new
            {
                TradeType = Mapper.Map<IEnumerable<TradeType>, IEnumerable<TradeType>>(_repository.GetTradeType()),
            };

            return new JsonNetResult() { Data = data };
        }
    }
}