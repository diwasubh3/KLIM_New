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
	public class TradeDataController : Controller
    {

        private IRepository _repository;
        private readonly IAlertEngine _alertEngine;
        private readonly IPositionCacheManager _cacheManager;

        //public TradeDataController() : this(new Repository(), new AlertEngine(), new PositionCacheManager())
        //{

        //}

        public TradeDataController(IRepository repository, IAlertEngine alertEngine, IPositionCacheManager cacheManager)
        {
            _repository = repository;
            _alertEngine = alertEngine;
            _cacheManager = cacheManager;
        }

        public JsonNetResult GetTradeAllocations(int securityId)
        {

            var positions = _repository.GetPositionExposures(securityId).ToList();
            var tradeAllocationsDto = new List<TradeAllocationDto>();
            if (positions.Count == 0)
            {
                var funds = _repository.GetFunds();
                funds.ForEach(f =>
                {
                    tradeAllocationsDto.Add(new TradeAllocationDto()
                    {
                        CurrentAllocation = 0,
                        FinalAllocation = 0,
                        TradeCash =  0,
                        NewAllocation = 0,
                        FundId = f.FundId,
                        Fund = Mapper.Map<Fund, FundDto>(f)
                    });
                });
            }
            else
            {
                positions.ForEach(pos =>
                {
                    tradeAllocationsDto.Add(new TradeAllocationDto()
                    {
                        CurrentAllocation = string.IsNullOrEmpty(pos.Exposure) ? 0.0m : pos.NumExposure,
                        FinalAllocation = string.IsNullOrEmpty(pos.Exposure) ? 0.0m : pos.NumExposure,
                        TradeCash = 0,
                        NewAllocation = 0,
                        FundId = pos.FundId.Value,
                        Bid = pos.Bid,
                        Offer = pos.Offer,
                        Fund = new FundDto() { FundId = pos.FundId.Value, FundCode = pos.FundCode, FundDesc = pos.FundDesc }
                    });
                });

            }

            return new JsonNetResult() { Data = tradeAllocationsDto };
        }


        public JsonNetResult GetTrades(bool includeCancelled, string fundCode)
        {
            int dateId = Helper.GetPrevDayDateId();
            var trades = Mapper.Map<IEnumerable<Trade>, IEnumerable<TradeDto>>(_repository.GetTrades(includeCancelled, dateId)).ToArray();

            trades.ForEach(tr =>
            {
                tr.FundCode = fundCode;
                tr.TradeAllocations.ForEach(tra => tra.ProcessSearchText(tr));
            });

            _alertEngine.ProcessAlerts(trades, dateId,fundCode);

            if (trades.Any())
            {
                WatchProcessor watchProcessor = new WatchProcessor();
                watchProcessor.Process(_repository, trades, trades.First().FundCode);
            }

            return new JsonNetResult() { Data = trades };
        }

        public JsonNetResult GetSourceData()
        {
            var data = new
            {
                Securities = Mapper.Map<IEnumerable<vw_Security>, IEnumerable<VwSecurityDto>>(_repository.GetSecurities()),
                Issuers = Mapper.Map<IEnumerable<Issuer>, IEnumerable<IssuerDto>>(_repository.GetIssuers()),
                Facilities = Mapper.Map<IEnumerable<Facility>, IEnumerable<FacilityDto>>(_repository.GetFacilities()),
                LienTypes = Mapper.Map<IEnumerable<LienType>, IEnumerable<LienTypeDto>>(_repository.GetLienTypes()),
                SnpIndustries = Mapper.Map<IEnumerable<Industry>, IEnumerable<IndustryDto>>(_repository.GetSnPIndustries()),
                MoodyIndustries = Mapper.Map<IEnumerable<Industry>, IEnumerable<IndustryDto>>(_repository.GetMoodyIndustries()),
                Ratings = Mapper.Map<IEnumerable<Rating>, IEnumerable<RatingDto>>(_repository.GetRatings()),
                Funds = Mapper.Map<IEnumerable<Fund>, IEnumerable<FundDto>>(_repository.GetFunds()),
            };

            return new JsonNetResult() { Data = data };
        }

        public JsonNetResult GetBloombergSecurityInfo(string securityCode)
        {
            BloombergProcessor bloombergProcessor = new BloombergProcessor();
            return new JsonNetResult() { Data = bloombergProcessor.Process(securityCode, _repository, User.Identity.Name) };
        }


        [HttpPost]
        public JsonNetResult SaveTrade(Trade trade,bool processSavedTrade)
        {
            TradeDto savedTrade;
            var dateId = ProcessTrade(trade, out savedTrade);

            savedTrade.FundCode = savedTrade.TradeAllocations.First(f => f.Fund != null).Fund.FundCode;
            savedTrade.TradeAllocations.ForEach(tra => tra.ProcessSearchText(savedTrade));
            List<TradeDto> savedTrades = new List<TradeDto>() {savedTrade};
            
            var allpositions = _repository.GetAllPositions(trade.SecurityId);
            _cacheManager.Update(allpositions);

            if (processSavedTrade)
            {
                _alertEngine.ProcessAlerts(savedTrades, dateId,savedTrade.FundCode).ToArray();
                WatchProcessor watchProcessor = new WatchProcessor();
                watchProcessor.Process(_repository, savedTrades, savedTrade.FundCode);
            }

            
            
            return new JsonNetResult() { Data = savedTrades[0]};
        }

        private int ProcessTrade(Trade trade, out TradeDto savedTrade)
        {
            int dateId = Helper.GetPrevDayDateId();
            if (trade.TradeId <= 0)
            {
                trade.DateId = dateId;
            }

            if (trade.IsCancelled.HasValue && trade.IsCancelled.Value)
            {
                trade.KeepOnBlotter = false;
                trade.DateId = dateId;
            }

            savedTrade = Mapper.Map<Trade, TradeDto>(_repository.SaveTrade(trade, User.Identity.Name));
            return dateId;
        }


        [HttpPost]
        public JsonNetResult SaveSecurity(TempSecurityDto tempSecurityDto)
        {

            Security security = Mapper.Map<TempSecurityDto,Security>(tempSecurityDto);
            security.SourceId = 1;
            security.SecurityCode = "LX_" + security.SecurityCode;

            if (!tempSecurityDto.SnPIndustryId.HasValue)
            {
                security.SnPIndustryId = -1;
            }
            if (!tempSecurityDto.MoodyIndustryId.HasValue)
            {
                security.MoodyIndustryId = -1;
            }

            _repository.AddUpdateSecurity(security,User.Identity.Name);
            var overridableFields = _repository.GetSecurityOverrideableFields().ToArray();

            List<SecurityOverride> securityOverrides = new List<SecurityOverride>();
            if (!string.IsNullOrEmpty(tempSecurityDto.CallDate))
            {
                var callDateField = overridableFields.First(f => f.FieldName == "CallDate");
                SecurityOverride callDateSecurityOverride = new SecurityOverride
                {
                    Field = callDateField,
                    Security = security,
                    EffectiveFrom = DateTime.Now,
                    ExistingValue = DateTime.Parse(tempSecurityDto.CallDate).ToString("MM/dd/yyyy"),
                    OverrideValue = DateTime.Parse(tempSecurityDto.CallDate).ToString("MM/dd/yyyy"),
                    CreatedBy = User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                securityOverrides.Add(callDateSecurityOverride);
            }

            if (!string.IsNullOrEmpty(tempSecurityDto.MaturityDate))
            {
                var maturityDateField = overridableFields.First(f => f.FieldName == "MaturityDate");
                SecurityOverride maturityDateSecurityOverride = new SecurityOverride
                {
                    Field = maturityDateField,
                    Security = security,
                    EffectiveFrom = DateTime.Now,
                    ExistingValue = DateTime.Parse(tempSecurityDto.MaturityDate).ToString("MM/dd/yyyy"),
                    OverrideValue = DateTime.Parse(tempSecurityDto.MaturityDate).ToString("MM/dd/yyyy"),
                    CreatedBy = User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                securityOverrides.Add(maturityDateSecurityOverride);
            }


            _repository.SaveSecurityOverrides(securityOverrides.ToArray(), User.Identity.Name);
            return new JsonNetResult() { Data = Mapper.Map<Security, SecurityDto>(security) };
        }

    }
}