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
using System.Xml.Serialization;
using System.IO;

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
            try
            {
                var data = new
                {
                    Facilities = Mapper.Map<IEnumerable<Facility>, IEnumerable<FacilityDto>>(_repository.GetFacilities()),
                    TradeType = Mapper.Map<IEnumerable<TradeType>, IEnumerable<TradeTypeDto>>(_repository.GetTradeType()),
                    Traders = Mapper.Map<IEnumerable<Trader>, IEnumerable<Trader>>(_repository.GetTraders()),
                    CounterParty = Mapper.Map<IEnumerable<CounterParty>, IEnumerable<CounterParty>>(_repository.GetCounterParty()),
                    SettleMethods = Mapper.Map<IEnumerable<SettleMethods>, IEnumerable<SettleMethods>>(_repository.GetSettleMethods()),
                    InterestTreatment = Mapper.Map<IEnumerable<InterestTreatment>, IEnumerable<InterestTreatment>>(_repository.GetInterestTreatment()),
                    AllocationRule = Mapper.Map<IEnumerable<AllocationRule>, IEnumerable<AllocationRule>>(_repository.GetAllocationRule()),
                };

                return new JsonNetResult() { Data = data };
            }
            catch (Exception ex)
            {
                return new JsonNetResult();
            }
        }

        public JsonNetResult GetIssuerSecurities()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<vw_IssuerSecurity>, IEnumerable<vw_IssuerSecurity>>(_repository.SearchIssuerSecurities())
            };
        }

        public JsonNetResult GetFundAllocations()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Fund>, IEnumerable<FundDto>>(_repository.GetFunds()) };
        }

        [HttpPost]
        public JsonNetResult GenerateTradeXML(TradeBooking data)
        {
            try
            {
                data.Id = 2;
                var allTradeBooking = _repository.GetTradeBookingXML(data.Id).FirstOrDefault();
                var allTradeGroup = _repository.GetTradeGroupXML(data.Id).FirstOrDefault();
                var allTradeBookingDetail = _repository.GetTradeBookingDetailXML(data.Id);
                var portfolios = new List<PORTFOLIOALLOCATION>();

                foreach (var tradebookingdt in allTradeBookingDetail)
                {
                    portfolios.Add(new PORTFOLIOALLOCATION()
                    {
                        portfolioid = tradebookingdt.PortFolioId.ToString(),
                        amountallocation = tradebookingdt.FinalQty.ToString(),
                        name = tradebookingdt.PortfolioName.ToString(),
                        tradeid = tradebookingdt.TradeDetailId.ToString(),
                    });
                }

                var tPORTFOLIOALLOCATION = portfolios;
                var tUDF = new UDF
                {
                    fieldname = "u_sTradeComment",
                    value = allTradeBooking.TradeComment.ToString(),
                };
                var tNOTE = new NOTE
                {
                    notetype = "Other",
                    note = allTradeBooking.TradeComment.ToString(),
                };
                var tTRADE = new TRADE
                {
                    cancel = (allTradeBooking.Cancel) ? "true" : "false",
                    cancelid = allTradeBooking.TradeId.ToString(),
                    counterpartyid = allTradeBooking.CounterPartyId.ToString(),
                    quantity = allTradeBooking.TotalQty.ToString(),
                    price = allTradeBooking.Price.ToString(),
                    settlementmethod = allTradeBooking.SettleMethod.ToString(),
                    tradedate = allTradeBooking.TradeDate.ToString("yyyy-MM-dd"),
                    tradeid = allTradeBooking.TradeId.ToString(),
                    traderid = allTradeBooking.TraderId.ToString(),
                    type = allTradeBooking.TradeTypeDesc.ToString(),
                    update = (allTradeBooking.UpdateFlag) ? "true" : "false",
                    updateid = allTradeBooking.TradeId.ToString(),
                    UDF = tUDF,
                    NOTE = tNOTE,
                    PORTFOLIOALLOCATION = portfolios
                };

                var tTRADEGROUP = new TRADEGROUP
                {
                    cancel = (allTradeBooking.Cancel) ? "true" : "false",
                    cancelreferenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    interesttreatment = allTradeBooking.InterestTreatment.ToString(),
                    readyforsettlement = "true",
                    referenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    settlementplatform = "Automatic"
                };
                var tIDENTIFIER = new IDENTIFIER()
                {
                    tdes = "LoanXID",
                    id = allTradeBooking.LoanXId
                };

                var tPARAMETERS = new PARAMETERS
                {
                    dateformat = "yyyy-MM-dd"
                };

                var newdata = new WSOXML()
                {
                    TRADE_IN_9x = new TRADE_IN_9x()
                    {
                        PARAMETERS = tPARAMETERS,
                        DATA = new DATA()
                        {
                            ASSET = new ASSET()
                            {
                                tid = "4",
                                IDENTIFIER = tIDENTIFIER,
                                TRADEGROUP = tTRADEGROUP,
                                TRADE = tTRADE
                            }
                        }
                    }
                };

                var serializer = new XmlSerializer(typeof(WSOXML));
                using (var stream = new StreamWriter(Server.MapPath("\\Test\\test.xml")))
                    serializer.Serialize(stream, newdata);
                return new JsonNetResult() { Data = newdata };
            }
            catch (Exception ex)
            {
                return new JsonNetResult();
            }
        }
    }
}