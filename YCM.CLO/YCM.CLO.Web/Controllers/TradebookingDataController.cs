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
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;
using log4net;

namespace YCM.CLO.Web.Controllers
{
    public class TradebookingDataController : Controller
    {
        private IRepository _repository;
        private readonly IAlertEngine _alertEngine;
        private readonly IPositionCacheManager _cacheManager;
        private readonly ILog _logger;
        public TradebookingDataController(IRepository repository, IAlertEngine alertEngine, IPositionCacheManager cacheManager)
        {
            _repository = repository;
            _alertEngine = alertEngine;
            _cacheManager = cacheManager;
            _logger = LogManager.GetLogger(typeof(TradebookingDataController));
        }
        public JsonNetResult GetSourceData()
        {
            try
            {
                var name = System.Web.HttpContext.Current.User.Identity.Name;
                var data = new
                {
                    Facilities = Mapper.Map<IEnumerable<Facility>, IEnumerable<FacilityDto>>(_repository.GetFacilities()),
                    TradeType = Mapper.Map<IEnumerable<TradeType>, IEnumerable<TradeTypeDto>>(_repository.GetTradeType()),
                    Traders = Mapper.Map<IEnumerable<Trader>, IEnumerable<TraderDto>>(_repository.GetTraders()),
                    CounterParty = Mapper.Map<IEnumerable<CounterParty>, IEnumerable<CounterPartyDto>>(_repository.GetCounterParty()),
                    SettleMethods = Mapper.Map<IEnumerable<SettleMethods>, IEnumerable<SettleMethodsDto>>(_repository.GetSettleMethods()),
                    InterestTreatment = Mapper.Map<IEnumerable<InterestTreatment>, IEnumerable<InterestTreatmentDto>>(_repository.GetInterestTreatment()),
                    TradeComment = Mapper.Map<IEnumerable<TradeComment>, IEnumerable<TradeCommentDto>>(_repository.GetTradeComment()),
                    TradeReasons = Mapper.Map<IEnumerable<TradeReason>, IEnumerable<TradeReasonDto>>(_repository.GetTradeReasons()),
                    AssetTypes = Mapper.Map<IEnumerable<AssetType>, IEnumerable<AssetTypeDto>>(_repository.GetAssetTypes()),
                    permissions = _repository.GetPermission(name.Substring(name.IndexOf("\\") + 1).ToLower()).ToList()
                };
                return new JsonNetResult() { Data = data };
            }
            catch (Exception ex)
            {
                return new JsonNetResult();
            }
        }
        public JsonNetResult GetAllocationRule(int tradeTypeId)
        {
            var data = new
            {
                AllocationRule = Mapper.Map<IEnumerable<AllocationRule>, IEnumerable<AllocationRuleDto>>(_repository.GetAllocationRule(tradeTypeId)),
            };
            return new JsonNetResult() { Data = data };
        }

        public JsonNetResult GetIssuerSecurities()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<vw_IssuerSecurity>, IEnumerable<vw_IssuerSecurity>>(_repository.SearchIssuerSecurities())
            };
        }

        public JsonNetResult GetIssuerList()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<vw_PositionIssuers>, IEnumerable<vw_PositionIssuers>>(_repository.GetIssuerList())
            };
        }

        public JsonNetResult GetTradeFundAllocation(TradeBooking tradeBooking)
        {
            try
            {
                var data = Mapper.Map<IEnumerable<TradeBookingDetail>, IEnumerable<TradeBookingDetailDto>>(_repository.GetTradeFundAllocation(tradeBooking.allocationRule.RuleName, tradeBooking.IssuerId, tradeBooking.LoanXId, tradeBooking.tradeType.TradeTypeDesc));
                data.ForEach(x =>
                {
                    x.TotalQuantity = tradeBooking.TotalQty;
                    x.RuleName = tradeBooking.allocationRule.RuleName;
                    x.Price = tradeBooking.Price;
                    x.TradeType = tradeBooking.tradeType.TradeTypeDesc;
                });
                return getCalculatedData(data);
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                return new JsonNetResult();
            }
        }

        //public JsonNetResult getCalculatedData_OldLogic(IEnumerable<TradeBookingDetailDto> data) //, decimal totalQty, string ruleName
        //{
        //    string Jsonfilecontent = "";
        //    decimal totalQty = data.First().TotalQuantity.Value;

        //    var jsonfilepath = ConfigurationManager.AppSettings["RuleJasonfile"];

        //    using (StreamReader r = new StreamReader(jsonfilepath))
        //    {
        //        Jsonfilecontent = r.ReadToEnd();
        //    }
        //    var jsonClass = JObject.Parse(Jsonfilecontent);

        //    var ruleJson = Convert.ToString((JObject.Parse(Jsonfilecontent)["Methods"]).Children<JObject>().FirstOrDefault(m => m.Property("type").Value.ToString() == data.First().RuleName && m.Property("tradetype").Value.ToString() == data.First().TradeType));

        //    decimal GrandExistingTotal = data.Select(x => x.IsIncluded == true ? x.Existing : 0).Sum();
        //    decimal GrandExposureTotal = data.Select(x => x.IsIncluded == true ? x.Exposure : 0).Sum();

        //    if (data.First().TradeType == "Sell" && data.First().RuleName == "Sell All")
        //    {
        //        data.ForEach(x =>
        //        {
        //            x.Override = x.IsIncluded == true ? x.Override : 0;
        //            x.Allocated = x.IsIncluded == true ? x.Exposure : 0;
        //        });
        //    }
        //    else
        //    {
        //        data.ForEach(x =>
        //        {
        //            x.Override = x.IsIncluded == true ? x.Override : 0;
        //            x.Allocated = x.IsIncluded == true && GrandExistingTotal > 0 ? (x.Existing * totalQty / GrandExistingTotal) : 0;
        //        });
        //    }            

        //    decimal TotalExisting_Override = data.Select(p => p.Override > 0 ? p.Existing : 0).Sum();
        //    decimal TotalOverride = data.Select(p => p.Override > 0 ? p.Override : 0).Sum();
        //    decimal TotalAutoAllocationOverride = data.Select(p => p.Override > 0 ? p.Allocated : 0).Sum();
        //    data.ForEach(x =>
        //    {
        //        x.TotalQuantity = totalQty;
        //        x.GrandTotal = GrandExistingTotal;                
        //        x.IsOverride = (x.Override > 0);
        //        x.TotalOverride = (TotalOverride > 0 ? GrandExistingTotal - TotalExisting_Override :  0);
        //        x.TotalRemaining = (TotalOverride > 0 ? TotalAutoAllocationOverride - TotalOverride : 0);                
        //    });
        //    Recon.RuleEngine.Engine ruleEngine = new Recon.RuleEngine.Engine();
        //    var stroutPut = ruleEngine.RunRuleEngine(ruleJson, JsonConvert.SerializeObject(data));
        //    return new JsonNetResult() { Data = JsonConvert.DeserializeObject<List<TradeBookingDetailDto>>(stroutPut) };
        //}

        public JsonNetResult getCalculatedData(IEnumerable<TradeBookingDetailDto> data, decimal totalQty = 0) //, decimal totalQty, string ruleName
        {
            string Jsonfilecontent = "";
            bool isBuyTrade = data.First().TradeType == "Buy";
            bool isSellTrade = data.First().TradeType == "Sell";
            if (totalQty == 0)
                totalQty = data.First().TotalQuantity.Value;

            var jsonfilepath = ConfigurationManager.AppSettings["RuleJasonfile"];

            using (StreamReader r = new StreamReader(jsonfilepath))
            {
                Jsonfilecontent = r.ReadToEnd();
            }
            var jsonClass = JObject.Parse(Jsonfilecontent);

            var ruleJson = Convert.ToString((JObject.Parse(Jsonfilecontent)["Methods"]).Children<JObject>().FirstOrDefault(m => m.Property("type").Value.ToString() == data.First().RuleName && m.Property("tradetype").Value.ToString() == data.First().TradeType));

            decimal ExistingTotal = 0;
            decimal ExposureTotal = 0;

            decimal TotalExisting_Override = 0;
            decimal TotalExposure_Override = 0;
            decimal TotalOverride = 0;
            int TotalFundCount = 0;
            ExistingTotal = data.Select(x => x.IsIncluded == true ? x.Existing : 0).Sum();
            ExposureTotal = data.Select(x => x.IsIncluded == true ? x.Exposure : 0).Sum();

            TotalExisting_Override = data.Select(x => x.Override > 0 && x.IsIncluded == true ? x.Existing : 0).Sum();
            TotalExposure_Override = data.Select(x => x.Override > 0 && x.IsIncluded == true ? x.Exposure : 0).Sum();
            TotalOverride = data.Select(p => p.Override > 0 && p.IsIncluded == true ? p.Override : 0).Sum();
            TotalFundCount = data.Select(t => t.IsIncluded == true && t.Override == 0 ? 1 : 0).Sum();

            data.ForEach(x =>
            {
                x.TotalQuantity = totalQty - TotalOverride;
                x.TotalExisting = ExistingTotal - TotalExisting_Override;
                x.TotalExposure = ExposureTotal - TotalExposure_Override;
                x.IsOverride = (x.Override > 0);
                x.FundCount = TotalFundCount;
            });
            //if (isBuyTrade)
            //{

            //}
            //else if (isSellTrade)
            //{
            //    ExistingTotal = data.Select(x => x.IsIncluded == true ? x.Existing : 0).Sum();
            //    ExposureTotal = data.Select(x => x.IsIncluded == true ? x.Exposure : 0).Sum();

            //    TotalExisting_Override = data.Select(x => x.Override != 0 && x.IsIncluded == true ? x.Existing : 0).Sum();
            //    TotalExposure_Override = data.Select(x => x.Override != 0 && x.IsIncluded == true ? x.Exposure : 0).Sum();
            //    TotalOverride = data.Select(p => p.Override != 0 && p.IsIncluded == true ? p.Override : 0).Sum();

            //    data.ForEach(x =>
            //    {
            //        x.TotalQuantity = totalQty + TotalOverride;
            //        x.TotalExisting = ExistingTotal - TotalExisting_Override;
            //        x.TotalExposure = ExposureTotal - TotalExposure_Override;
            //        x.IsOverride = (x.Override != 0);
            //    });
            //}
            Recon.RuleEngine.Engine ruleEngine = new Recon.RuleEngine.Engine();
            var stroutPut = ruleEngine.RunRuleEngine(ruleJson, JsonConvert.SerializeObject(data));
            var outPutData = JsonConvert.DeserializeObject<List<TradeBookingDetailDto>>(stroutPut);
            if (outPutData.Where(w => w.Allocated < 0).Any())
            {
                outPutData.Where(w => w.Allocated < 0).ForEach(x =>
                {
                    x.IsIncluded = false;
                });
                return getCalculatedData(outPutData, totalQty);
            }
            else
                return new JsonNetResult() { Data = JsonConvert.DeserializeObject<List<TradeBookingDetailDto>>(stroutPut) };
            //if (isBuyTrade)
            //{

            //}
            //else if (isSellTrade)
            //{
            //    var outPutData = JsonConvert.DeserializeObject<List<TradeBookingDetailDto>>(stroutPut);
            //    if (outPutData.Where(w => w.Allocated > 0).Any())
            //    {
            //        outPutData.Where(w => w.Allocated > 0).ForEach(x =>
            //        {
            //            x.IsIncluded = false;
            //        });
            //        return getCalculatedData(outPutData, totalQty);
            //    }
            //    else
            //        return new JsonNetResult() { Data = JsonConvert.DeserializeObject<List<TradeBookingDetailDto>>(stroutPut) };
            //}
            //return new JsonNetResult() { Data = JsonConvert.DeserializeObject<List<TradeBookingDetailDto>>(stroutPut) };
        }

        public JsonNetResult GetTradeBooking()
        {
            try
            {
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<TradeBooking>, IEnumerable<TradeBookingDto>>(_repository.GetTradeBookings()) };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetTradeBooking " + ex);
                return new JsonNetResult();
            }
        }

        public JsonNetResult RefreshTradeBooking(Int64 TradeId)
        {
            try
            {
                var tradeBooking = Mapper.Map<TradeBooking, TradeBookingDto>(_repository.RefreshTradeBooking(TradeId));

                tradeBooking.tradeType.TradeTypeId = tradeBooking.TradeTypeId;
                tradeBooking.tradeType.TradeTypeDesc = tradeBooking.TradeTypeDesc;
                tradeBooking.traders.TraderId = tradeBooking.TraderId;
                tradeBooking.traders.TraderName = tradeBooking.TraderName;
                tradeBooking.facility.FacilityId = tradeBooking.FacilityId;
                tradeBooking.facility.FacilityDesc = tradeBooking.FacilityDesc;
                tradeBooking.counterparty.WSOId = tradeBooking.CounterPartyId;
                tradeBooking.counterparty.PartyName = tradeBooking.PartyName;
                tradeBooking.settlemethods.MethodId = tradeBooking.SettleMethodId;
                tradeBooking.settlemethods.MethodName = tradeBooking.SettleMethod;
                tradeBooking.interesttreatments.Id = tradeBooking.InterestTreatmentId;
                tradeBooking.interesttreatments.Description = tradeBooking.InterestTreatment;
                tradeBooking.allocationRule.Id = tradeBooking.RuleId;
                tradeBooking.allocationRule.RuleName = tradeBooking.RuleName;
                tradeBooking.tradeComments1.CommentId = tradeBooking.TradeCommentId1?? 0;
                tradeBooking.tradeComments1.Comment = tradeBooking.TradeComment1;

                tradeBooking.tradeComments2.CommentId = tradeBooking.TradeCommentId2?? 0;
                tradeBooking.tradeComments2.Comment = tradeBooking.TradeComment2;

                tradeBooking.tradeReasons.TradeReasonId = tradeBooking.TradeReasonId ?? 0;
                tradeBooking.tradeReasons.TradeReasonDesc = tradeBooking.TradeReason;

                tradeBooking.assetTypes.AssetId = tradeBooking.AssetId ?? 0;
                tradeBooking.assetTypes.AssetName = tradeBooking.AssetName;

                var tradeBookingdetail = Mapper.Map<IEnumerable<TradeBookingDetail>, IEnumerable<TradeBookingDetailDto>>(_repository.RefreshTradeBookingDetail(TradeId));
                tradeBooking.TradeBookingDetail = tradeBookingdetail;
                return new JsonNetResult() { Data = tradeBooking };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in RefreshTradeBooking " + ex);
                return new JsonNetResult();
            }
        }

        [HttpPost]
        public JsonNetResult GenerateTradeXML(TradeBooking data)
        {
            try
            {
                _logger.Info($"GenerateTradeXML Started");
                data.TradeTypeId = data.tradeType.TradeTypeId;
                data.TraderId = data.traders.TraderId;
                data.FacilityId = data.facility.FacilityId;
                data.CounterPartyId = data.counterparty.WSOId;
                data.SettleMethodId = data.settlemethods.MethodId;
                data.InterestTreatmentId = data.interesttreatments.Id;
                data.RuleId = data.allocationRule.Id;
                data.TradeCommentId1 = data.tradeComments1.CommentId;
                data.TradeCommentId2 = data.tradeComments2.CommentId;

                data.TradeReasonId = data.tradeReasons.TradeReasonId;
                data.AssetId = data.assetTypes.AssetId;
                string TradeComment = data.tradeComments1.Comment == null ? "" : data.tradeComments1.Comment + "; " + (data.tradeComments2.Comment == null ? "" : data.tradeComments2.Comment);
                string TradeReason = data.tradeReasons.TradeReasonDesc == null ? "" : data.tradeReasons.TradeReasonDesc;
                var tradebookingId = _repository.SaveTradeBooking(data, User.Identity.Name);
                if (tradebookingId > 0)
                {
                    _logger.Info($"Saved Tradebooking ");
                    _repository.SaveTradeBookingDetails(data.TradeBookingDetail, tradebookingId);
                    _repository.UpdateSubmitDetails(tradebookingId);
                    _logger.Info($"Saved Tradebooking deatils  and Updated Submit Details  ");
                }

                data.Id = tradebookingId;
                var allTradeBooking = _repository.GetTradeBookingXML(data.Id).FirstOrDefault();
                var allTradeGroup = _repository.GetTradeGroupXML(data.Id).FirstOrDefault();
                var allTradeBookingDetail = _repository.GetTradeBookingDetailXML(data.Id);
                var portfolios = new List<PORTFOLIOALLOCATION>();

                _logger.Info($"Adding Portfolios ");
                foreach (var tradebookingdt in allTradeBookingDetail)
                {
                    portfolios.Add(new PORTFOLIOALLOCATION()
                    {
                        portfolioid = tradebookingdt.PortFolioId.ToString(),
                        amountallocation = tradebookingdt.NetPosition.ToString(),
                        name = tradebookingdt.PortfolioName.ToString(),
                        tradeid = tradebookingdt.TradeDetailId.ToString(),
                    });
                }

                var tPORTFOLIOALLOCATION = portfolios;
                _logger.Info($"PortFolios Added");

                var tUDF = new UDF
                {
                    fieldname = "u_sTradeCommentsYork",
                    value = TradeComment.ToString(),
                };
                var tNOTE = new NOTE
                {
                    notetype = "Other",
                    note = TradeComment.ToString(),
                };
                _logger.Info($"Adding Trade");
                var tTRADE = new TRADE
                {
                    cancel = "false",
                    cancelid = allTradeBooking.TradeId.ToString(),
                    counterpartyid = allTradeBooking.CounterPartyId.ToString(),
                    quantity = allTradeBooking.TotalQty.ToString(),
                    price = allTradeBooking.Price.ToString(),
                    reasonfortrade = TradeReason.ToString(),
                    primarycommencementdate = allTradeBooking.SettleMethod.ToString() == "Primary" ? allTradeBooking.TradeDate.ToString("yyyy-MM-dd") : "",
                    settlementmethod = allTradeBooking.SettleMethod.ToString(),
                    tradedate = allTradeBooking.TradeDate.ToString("yyyy-MM-dd"),
                    tradeid = allTradeBooking.TradeId.ToString(),
                    traderid = allTradeBooking.TraderId.ToString(),
                    type = allTradeBooking.TradeTypeDesc.ToString(),
                    update = "false",
                    updateid = allTradeBooking.TradeId.ToString(),
                    UDF = tUDF,
                    NOTE = tNOTE,
                    PORTFOLIOALLOCATION = portfolios
                };
                _logger.Info($" Trade added");
                var tTRADEGROUP = new TRADEGROUP
                {
                    cancel = "false",
                    cancelreferenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    interesttreatment = allTradeBooking.InterestTreatment.ToString(),
                    readyforsettlement = allTradeBooking.CounterPartyId.ToString() == "2438" ? "false" : "true",
                    referenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    settlementplatform = "Automatic"
                };
                _logger.Info($" Trade Group added");
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
                _logger.Info($"Creating XML");
                if (ConfigurationManager.AppSettings.AllKeys.Contains("WSOXML"))
                {
                    using (var stream = new StreamWriter(ConfigurationManager.AppSettings["WSOXML"] + allTradeBooking.LoanXId.ToString() + "_" + allTradeGroup.ReferenceTicketId.ToString() + ".xml"))
                        serializer.Serialize(stream, newdata);
                }
                _logger.Info($"XML Created");
                return new JsonNetResult();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GenerateTradeXML " + ex);
                return new JsonNetResult();
            }
        }

        public JsonNetResult GetTradeBookingHistory()
        {
            try
            {
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<TradeBooking>, IEnumerable<TradeBookingDto>>(_repository.GetTradeBookingHistory()) };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetTradeBookingHistory " + ex);
                return new JsonNetResult();
            }
        }

        public JsonNetResult GetFilteredTrades(string startDate , string endDate )
        {
            try
            {
                DateTime SDate = DateTime.Parse(startDate);
                DateTime EDate = DateTime.Parse(endDate);

                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<TradeBooking>, IEnumerable<TradeBookingDto>>(_repository.GetFilteredTrades(SDate, EDate)) };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetFilteredTrades " + ex);
                return new JsonNetResult();
            }
        }

        [HttpPost]
        public JsonNetResult CancelTrade(TradeBooking data)
        {
            try
            {
                _logger.Info($"Cancel Trade Started");
                var tradeId = data.Id;
                var tradecancelreason = data.TradeComment;
                data.TradeCommentId1 = data.tradeComments1.CommentId;
                data.TradeCommentId2 = data.tradeComments2.CommentId;

                data.TradeReasonId = data.tradeReasons.TradeReasonId;
                data.AssetId = data.assetTypes.AssetId;
                string TradeComment = data.tradeComments1.Comment == null ? "" : data.tradeComments1.Comment + "; " + (data.tradeComments2.Comment == null ? "" : data.tradeComments2.Comment);
                string TradeReason = data.tradeReasons.TradeReasonDesc == null ? "" : data.tradeReasons.TradeReasonDesc;



                var allTradeBooking = _repository.GetTradeBookingXML(data.Id).FirstOrDefault();
                var allTradeGroup = _repository.GetTradeGroupXML(data.Id).FirstOrDefault();
                var allTradeBookingDetail = _repository.GetTradeBookingDetailXML(data.Id);
                var portfolios = new List<PORTFOLIOALLOCATION>();
                _repository.CancelTradeBooking(tradeId);
                _logger.Info($"Canceled Tradebooking ");
                foreach (var tradebookingdt in allTradeBookingDetail)
                {
                    portfolios.Add(new PORTFOLIOALLOCATION()
                    {
                        portfolioid = tradebookingdt.PortFolioId.ToString(),
                        amountallocation = tradebookingdt.NetPosition.ToString(),
                        name = tradebookingdt.PortfolioName.ToString(),
                        tradeid = tradebookingdt.TradeDetailId.ToString(),
                    });
                }

                var tPORTFOLIOALLOCATION = portfolios;
                var tUDF = new UDF
                {
                    fieldname = "u_sTradeCommentsYork",
                    value = TradeComment.ToString(),
                };
                var tNOTE = new NOTE
                {
                    notetype = "Other",
                    note = TradeComment.ToString(),
                };
                _logger.Info($"Adding Trade");
                var tTRADE = new TRADE
                {
                    cancel = "true",
                    cancelid = allTradeBooking.TradeId.ToString(),
                    counterpartyid = allTradeBooking.CounterPartyId.ToString(),
                    quantity = allTradeBooking.TotalQty.ToString(),
                    price = allTradeBooking.Price.ToString(),
                    reasonfortrade = TradeReason.ToString(),
                    primarycommencementdate = allTradeBooking.SettleMethod.ToString() == "Primary" ? allTradeBooking.TradeDate.ToString("yyyy-MM-dd") : "",
                    settlementmethod = allTradeBooking.SettleMethod.ToString(),
                    tradedate = allTradeBooking.TradeDate.ToString("yyyy-MM-dd"),
                    tradeid = allTradeBooking.TradeId.ToString(),
                    traderid = allTradeBooking.TraderId.ToString(),
                    type = allTradeBooking.TradeTypeDesc.ToString(),
                    update = "false",
                    updateid = allTradeBooking.TradeId.ToString(),
                    UDF = tUDF,
                    NOTE = tNOTE,
                    PORTFOLIOALLOCATION = portfolios
                };
                _logger.Info($" Trade added");

                var tTRADEGROUP = new TRADEGROUP
                {
                    cancel = "true",
                    cancelreferenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    interesttreatment = allTradeBooking.InterestTreatment.ToString(),
                    readyforsettlement = allTradeBooking.CounterPartyId.ToString() == "2438" ? "false" : "true",
                    referenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    settlementplatform = "Automatic"
                };
                _logger.Info($" Trade Group added");
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
                _logger.Info($"Creating XML");
                if (ConfigurationManager.AppSettings.AllKeys.Contains("WSOXML"))
                {
                    using (var stream = new StreamWriter(ConfigurationManager.AppSettings["WSOXML"] + allTradeBooking.LoanXId.ToString() + "_" + allTradeGroup.ReferenceTicketId.ToString() + ".xml"))
                        serializer.Serialize(stream, newdata);
                }
                _logger.Info($"XML Created");
                return new JsonNetResult();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in CancelTrade " + ex);
                return new JsonNetResult();
            }
        }
    }
}