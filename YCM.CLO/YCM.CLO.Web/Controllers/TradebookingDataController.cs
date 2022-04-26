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
                    Traders = Mapper.Map<IEnumerable<Trader>, IEnumerable<TraderDto>>(_repository.GetTraders()),
                    CounterParty = Mapper.Map<IEnumerable<CounterParty>, IEnumerable<CounterPartyDto>>(_repository.GetCounterParty()),
                    SettleMethods = Mapper.Map<IEnumerable<SettleMethods>, IEnumerable<SettleMethodsDto>>(_repository.GetSettleMethods()),
                    InterestTreatment = Mapper.Map<IEnumerable<InterestTreatment>, IEnumerable<InterestTreatmentDto>>(_repository.GetInterestTreatment()),
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

        public JsonNetResult GetTradeFundAllocation(decimal totalQty, string ruleName,string issuerId, decimal price)
        {
            try
            {
                var data = Mapper.Map<IEnumerable<TradeBookingDetail>, IEnumerable<TradeBookingDetailDto>>(_repository.GetTradeFundAllocation(ruleName, issuerId));
                data.First().TotalQuantity = totalQty;
                data.First().RuleName = ruleName;
                data.ForEach(x =>
                {
                    x.Price = price;
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

        public JsonNetResult getCalculatedData(IEnumerable<TradeBookingDetailDto> data) //, decimal totalQty, string ruleName
        {
            string Jsonfilecontent = "";
            string ruleName = data.First().RuleName;
            decimal totalQty = data.First().TotalQuantity.Value;
            var jsonfilepath = ConfigurationManager.AppSettings["RuleJasonfile"];

            //Jsonfile = "{ 	'type': 'TotalPar', 	'rules': [ 		{ 			'rulename': 'TotalParEvaluateForSkippedRecords', 			'conditions': { 				'all': [ 					{ 						'fact': 'Existing', 						'operator': 'greaterThan', 						'value': 0 					}, 					{ 						'fact': 'TotalQuantity', 						'operator': 'greaterThan', 						'value': 0 					}, 					{ 						'fact': 'IsIncluded', 						'operator': 'equal', 						'value': 'False' 					} 				] 			}, 			'evaluate': [ 				{ 					'type': 'EvaluateOutput', 					'value': 0 				} 			], 			'result': [ 				{ 					'type': 'Allocated', 					'value': '[EvaluateOutput]' 				}, 				{ 					'type': 'FinalQty', 					'value': '[EvaluateOutput]' 				} 			] 		}, 		{ 			'rulename': 'TotalParEvaluateForOverride', 			'conditions': { 				'all': [ 					{ 						'fact': 'Existing', 						'operator': 'greaterThan', 						'value': 0 					}, 					{ 						'fact': 'TotalQuantity', 						'operator': 'greaterThan', 						'value': 0 					}, 					{ 						'fact': 'IsIncluded', 						'operator': 'equal', 						'value': 'True' 					}, 					{ 						'fact': 'IsOverride', 						'operator': 'equal', 						'value': 'True' 					} 				] 			}, 			'evaluate': [ 				{ 					'type': 'EvaluateOutput', 					'value': 0 				}, 				{ 					'type': 'EvaluateOutput1', 					'value': '[Override]' 				} 				 			], 			'result': [ 				{ 					'type': 'Allocated', 					'value': '[EvaluateOutput]' 				}, 				{ 					'type': 'FinalQty', 					'value': '[EvaluateOutput1]' 				} 			] 		}, 		{ 			'rulename': 'TotalParEvaluateForOther', 			'conditions': { 				'all': [ 					{ 						'fact': 'Existing', 						'operator': 'greaterThan', 						'value': 0 					}, 					{ 						'fact': 'TotalQuantity', 						'operator': 'greaterThan', 						'value': 0 					}, 					{ 						'fact': 'IsIncluded', 						'operator': 'equal', 						'value': 'True' 					}, 					{ 						'fact': 'IsOverride', 						'operator': 'equal', 						'value': 'False' 					} 				] 			}, 			'evaluate': [ 				{ 					'type': 'EvaluateOutput', 					'value': 0 				}, 				{ 					'type': 'EvaluateOutput1', 					'value': '[Override]' 				} 			], 			'result': [ 				{ 					'type': 'Allocated', 					'value': '[EvaluateOutput]' 				}, 				{ 					'type': 'FinalQty', 					'value': '[EvaluateOutput1]' 				} 			] 		} 	] }";
            //Jsonfile = "{ 	'type': 'TotalPar', 	'rules': [{ 		'rulename': 'TotalParEvaluateForSkippedRecords', 		'conditions': { 			'all': [{ 				'fact': 'Existing', 				'operator': 'greaterThan', 				'value': 0 			}, 			{ 				'fact': 'TotalQuantity', 				'operator': 'greaterThan', 				'value': 0 			}, 			{ 				'fact': 'IsIncluded', 				'operator': 'equal', 				'value': 'False' 			}] 		}, 		'evaluate': [{ 			'type': 'EvaluateOutput', 			'value': 0 		}], 		'result': [ 		{ 			'type': 'Allocated', 			'value': '[EvaluateOutput]' 		}, 		{ 			'type': 'FinalQty', 			'value': '[EvaluateOutput]' 		}] 	}, 	{ 		'rulename': 'TotalParEvaluateForOverride', 		'conditions': { 			'all': [{ 				'fact': 'Existing', 				'operator': 'greaterThan', 				'value': 0 			}, 			{ 				'fact': 'TotalQuantity', 				'operator': 'greaterThan', 				'value': 0 			}, 			{ 				'fact': 'IsIncluded', 				'operator': 'equal', 				'value': 'True' 			}, 			{ 				'fact': 'IsOverride', 				'operator': 'equal', 				'value': 'True' 			}] 		}, 		'evaluate': [{ 			'type': 'EvaluateOutput', 			'value': '[Override]' 		}], 		'result': [ 		{ 			'type': 'FinalQty', 			'value': '[EvaluateOutput]' 		}] 	}, 	{ 		'rulename': 'TotalParEvaluateForOther', 		'conditions': { 			'all': [{ 				'fact': 'Existing', 				'operator': 'greaterThan', 				'value': 0 			}, 			{ 				'fact': 'TotalQuantity', 				'operator': 'greaterThan', 				'value': 0 			}, 			{ 				'fact': 'IsIncluded', 				'operator': 'equal', 				'value': 'True' 			}, 			{ 				'fact': 'IsOverride', 				'operator': 'equal', 				'value': 'False' 			}] 		}, 		'evaluate': [{ 			'type': 'AllocatedQtyOutput', 			'value': '[TotalQuantity] * [Existing] / [GrandTotal]' 		}, 		{ 			'type': 'AdditionalAllocationQty', 			'value': '[Existing] / [TotalOverride] * [TotalRemaining]' 		}], 		'result': [{ 			'type': 'Allocated', 			'value': '[AllocatedQtyOutput]' 		}, 		{ 			'type': 'FinalQty', 			'value': '[AllocatedQtyOutput] + [AdditionalAllocationQty]' 		}] 	}] }";

            using (StreamReader r = new StreamReader(jsonfilepath))
            {
                Jsonfilecontent = r.ReadToEnd();
            }
            //JObject.Parse(Jsonfilecontent);
            var ruleDetailsToken = JToken.Parse(Jsonfilecontent);
            string ruleTypeName = (string)ruleDetailsToken["type"];
            var ruleJson = Convert.ToString((JObject.Parse(Jsonfilecontent)["Methods"]).Children<JObject>().FirstOrDefault(m => m.Property("type").Value.ToString() == ruleName));

            decimal GrandTotal = data.Select(x => x.IsIncluded == true ? x.Existing : 0).Sum();
            data.ForEach(x =>
            {
                x.GrandTotal = x.IsIncluded == true ? x.Existing : 0;
                x.Override = x.IsIncluded == true ? x.Override : 0;
                x.Allocated = x.IsIncluded == true ? (x.Existing * totalQty / GrandTotal) : 0;
            });
            
            decimal TotalExisting_Override = data.Select(p => p.Override > 0 ? p.Existing : 0).Sum();
            decimal TotalOverride = data.Select(p => p.Override > 0 ? p.Override : 0).Sum();
            decimal TotalAutoAllocationOverride = data.Select(p => p.Override > 0 ? p.Allocated : 0).Sum();
            data.ForEach(x =>
            {
                x.TotalQuantity = totalQty;
                x.GrandTotal = GrandTotal;                
                x.IsOverride = (x.Override > 0);
                x.TotalOverride = (TotalOverride > 0 ? GrandTotal - TotalExisting_Override :  0);
                x.TotalRemaining = (TotalOverride > 0 ? TotalAutoAllocationOverride - TotalOverride : 0);                
            });
            Recon.RuleEngine.Engine ruleEngine = new Recon.RuleEngine.Engine();
            var stroutPut = ruleEngine.RunRuleEngine(ruleJson, JsonConvert.SerializeObject(data));
            return new JsonNetResult() { Data = JsonConvert.DeserializeObject<List<TradeBookingDetailDto>>(stroutPut) };
        }

        public JsonNetResult GetTradeBooking()
        {
            try
            {                
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<TradeBooking>, IEnumerable<TradeBookingDto>>(_repository.GetTradeBookings()) };
            }
            catch (Exception ex)
            {
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

                var tradeBookingdetail = Mapper.Map<IEnumerable<TradeBookingDetail>, IEnumerable<TradeBookingDetailDto>>(_repository.RefreshTradeBookingDetail(TradeId));
                tradeBooking.TradeBookingDetail = tradeBookingdetail;
                return new JsonNetResult() { Data = tradeBooking };
            }
            catch (Exception ex)
            {
                return new JsonNetResult();
            }
        }

        [HttpPost]
        public JsonNetResult GenerateTradeXML(TradeBooking data)
        {
            try
            {
                data.TradeTypeId = data.tradeType.TradeTypeId;
                data.TraderId = data.traders.TraderId;
                data.FacilityId = data.facility.FacilityId;
                data.CounterPartyId = data.counterparty.WSOId;
                data.SettleMethodId = data.settlemethods.MethodId;
                data.InterestTreatmentId = data.interesttreatments.Id;
                data.RuleId = data.allocationRule.Id;
                data.TradeComment = data.TradeComment == null ? "" : data.TradeComment;
                var tradebookingId = _repository.SaveTradeBooking(data, User.Identity.Name);
                if (tradebookingId > 0)
                {                    
                    _repository.SaveTradeBookingDetails(data.TradeBookingDetail, tradebookingId);
                }
                
                data.Id = tradebookingId;
                var allTradeBooking = _repository.GetTradeBookingXML(data.Id).FirstOrDefault();
                var allTradeGroup = _repository.GetTradeGroupXML(data.Id).FirstOrDefault();
                var allTradeBookingDetail = _repository.GetTradeBookingDetailXML(data.Id);
                var portfolios = new List<PORTFOLIOALLOCATION>();

                foreach (var tradebookingdt in allTradeBookingDetail)
                {
                    portfolios.Add(new PORTFOLIOALLOCATION()
                    {
                        portfolioid = tradebookingdt.PortFolioId.ToString(),
                        amountallocation = tradebookingdt.NetQty.ToString(),
                        name = tradebookingdt.PortfolioName.ToString(),
                        tradeid = tradebookingdt.TradeDetailId.ToString(),
                    });
                }

                var tPORTFOLIOALLOCATION = portfolios;
                var tUDF = new UDF
                {
                    fieldname = "u_sTradeCommentsYork",
                    value = allTradeBooking.TradeComment.ToString(),
                };
                var tNOTE = new NOTE
                {
                    notetype = "Other",
                    note = allTradeBooking.TradeComment.ToString(),
                };
                var tTRADE = new TRADE
                {
                    cancel = "false",
                    cancelid = allTradeBooking.TradeId.ToString(),
                    counterpartyid = allTradeBooking.CounterPartyId.ToString(),
                    quantity = allTradeBooking.TotalQty.ToString(),
                    price = allTradeBooking.Price.ToString(),
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

                var tTRADEGROUP = new TRADEGROUP
                {
                    cancel = "false",
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

                if (ConfigurationManager.AppSettings.AllKeys.Contains("WSOXML"))
                {
                    using (var stream = new StreamWriter(ConfigurationManager.AppSettings["WSOXML"] + allTradeBooking.LoanXId.ToString() + "_" + allTradeGroup.ReferenceTicketId.ToString() + ".xml"))
                        serializer.Serialize(stream, newdata);
                }                    
                return new JsonNetResult();
            }
            catch (Exception ex)
            {
                return new JsonNetResult();
            }
        }

        [HttpPost]
        public JsonNetResult Save(TradeBooking data)
        {
            try
            {
                data.Id = 2;
                //var allTradeBooking = _repository.GetTradeBookingXML(data.Id).FirstOrDefault();
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
                    fieldname = "u_sTradeCommentsYork",
                    value = data.TradeComment.ToString(),
                };
                var tNOTE = new NOTE
                {
                    notetype = "Other",
                    note = data.TradeComment.ToString(),
                };
                var tTRADE = new TRADE
                {
                    cancel = "false",
                    cancelid = data.TradeId.ToString(),
                    counterpartyid = data.counterparty.PartyId.ToString(),
                    quantity = data.TotalQty.ToString(),
                    price = data.Price.ToString(),
                    settlementmethod = data.settlemethods.MethodName.ToString(),
                    tradedate = data.TradeDate.ToString("yyyy-MM-dd"),
                    tradeid = data.TradeId.ToString(),
                    traderid = data.traders.TraderId.ToString(),
                    type = data.tradeType.TradeTypeDesc.ToString(),
                    update = "false",
                    updateid = data.TradeId.ToString(),
                    UDF = tUDF,
                    NOTE = tNOTE,
                    PORTFOLIOALLOCATION = portfolios
                };

                var tTRADEGROUP = new TRADEGROUP
                {
                    cancel = "false",
                    cancelreferenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    interesttreatment = data.interesttreatments.Description.ToString(),
                    readyforsettlement = "true",
                    referenceticketid = allTradeGroup.ReferenceTicketId.ToString(),
                    settlementplatform = "Automatic"
                };
                var tIDENTIFIER = new IDENTIFIER()
                {
                    tdes = "LoanXID",
                    id = data.LoanXId.ToString()
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