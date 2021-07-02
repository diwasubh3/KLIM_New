using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Extensions;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class TradeSwapProcessor
    {
        private readonly ILog _logger;
        public TradeSwapProcessor()
        {
            _logger = LogManager.GetLogger(typeof(TradeSwapProcessor));
        }

        private bool CleanTradeSwap(int tradeSwapId)
        {
            using (
               SqlConnection connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand commandCleanUpTradeSwaps = new SqlCommand("CLO.spCleanUpTradeSwaps", connection))
                {
                    commandCleanUpTradeSwaps.CommandType = CommandType.StoredProcedure;
                    commandCleanUpTradeSwaps.Parameters.Add("@tradeSwapId", SqlDbType.Int).Value = tradeSwapId;
                    commandCleanUpTradeSwaps.ExecuteNonQuery();
                }
                connection.Close();
                return true;
            }

            return true;
        }

        public TradeSwap SetTradeSwap(int tradeSwapId, short status,string error)
        {
            using (CLOContext cloContext = new CLOContext())
            {
                TradeSwap tradeSwap = cloContext.TradeSwaps.First(t => t.TradeSwapId == tradeSwapId);
                tradeSwap.Status = status;
                tradeSwap.Error = error;
                if (status == 1)
                {
                    tradeSwap.ProcessStartedOn = DateTime.Now;
                }
                if (status == 2)
                {
                    tradeSwap.ProcessCompletedOn = DateTime.Now;
                }
                cloContext.SaveChanges();
                return tradeSwap;
            }
        }


        private TradeSwap GetTradeSwap(int tradeSwapId)
        {
            using (CLOContext cloContext = new CLOContext())
            {
                return cloContext.TradeSwaps.First(t => t.TradeSwapId == tradeSwapId);
            }
        }

        private IList<vw_Position> GetPositions(string fundCode)
        {
            using (IRepository repository = new Repository())
            {
                return repository.GetPositions(fundCode, true).ToList();
            }
        }

        private IList<Fund> GetAllFunds()
        {
            using (CLOContext cloContext = new CLOContext())
            {
                return cloContext.Funds.Where(f=>f.IsActive).ToList();
            }
        }

        public bool Process(int tradeSwapId)
        {
            CleanTradeSwap(tradeSwapId);
            TradeSwap tradeSwap = SetTradeSwap(tradeSwapId, 1,null);
            TradeSwapParam tradeSwapParam = JsonConvert.DeserializeObject<TradeSwapParam>(tradeSwap.Parameters);
            var buyPositions = GetPositions(null);

            var buyConstraints = new List<Filter>();

            if (tradeSwapParam.Constraints.LiquidityScore.HasValue &&
                tradeSwapParam.Constraints.LiquidityScoreOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "LiquidityScore",
                    Operation =
                        (Op)
                            Enum.Parse(typeof(Op),
                                tradeSwapParam.Constraints.LiquidityScoreOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.LiquidityScore.Value + 0.00999m
                });
            }

            if (tradeSwapParam.Constraints.CreditScore.HasValue &&
               tradeSwapParam.Constraints.CreditScoreOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "CreditScore",
                    Operation =
                        (Op)
                            Enum.Parse(typeof(Op),
                                tradeSwapParam.Constraints.CreditScoreOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.CreditScore.Value + 0.00999m
                });
            }


            if (tradeSwapParam.Constraints.MoodyAdjCfrRank.HasValue &&
   tradeSwapParam.Constraints.MoodyAdjCfrRankOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "MoodyCashFlowRatingAdjustedRank",
                    Operation =
                        (Op)
                            Enum.Parse(typeof(Op),
                                tradeSwapParam.Constraints.MoodyAdjCfrRankOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.MoodyAdjCfrRank.Value
                });
            }


            if (tradeSwapParam.Constraints.Spread.HasValue &&
   tradeSwapParam.Constraints.SpreadOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "Spread",
                    Operation =
                        (Op)
                            Enum.Parse(typeof(Op),
                                tradeSwapParam.Constraints.SpreadOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.Spread.Value
                });
            }


            if (tradeSwapParam.Constraints.MaturityDate.HasValue &&
                tradeSwapParam.Constraints.MaturityDateOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "SecurityMaturityDate",
                    Operation =
                        (Op)
                            Enum.Parse(typeof(Op),
                                tradeSwapParam.Constraints.MaturityDateOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.MaturityDate.Value
                });
            }

            if (tradeSwapParam.Constraints.PctPosition.HasValue &&
                tradeSwapParam.Constraints.PctPositionOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "PctExposureNum",
                    Operation =
                        (Op)
                            Enum.Parse(typeof(Op),
                                tradeSwapParam.Constraints.PctPositionOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.PctPosition.Value / 100 + +0.00999m
                });
            }

            if (tradeSwapParam.Constraints.Recovery.HasValue &&
                tradeSwapParam.Constraints.RecoveryOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "MoodyRecovery",
                    Operation =
                        (Op)
                            Enum.Parse(typeof(Op), tradeSwapParam.Constraints.RecoveryOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.Recovery.Value
                });
            }


            if (tradeSwapParam.Constraints.Yield.HasValue &&
                tradeSwapParam.Constraints.YieldOperatorId.HasValue)
            {
                buyConstraints.Add(new Filter()
                {
                    PropertyName = "YieldBid",
                    Operation =
                        (Op)Enum.Parse(typeof(Op), tradeSwapParam.Constraints.YieldOperatorId.ToString()),
                    Value = tradeSwapParam.Constraints.Yield.Value + 0.00999m
                });
            }

            IList<vw_Position> filteredBuyCollection = null;

            if (buyConstraints.Count > 0)
            {
                var buyDeleg = ExpressionBuilder.GetExpression<vw_Position>(buyConstraints).Compile();
                filteredBuyCollection = buyPositions.Where(buyDeleg).ToList();
            }
            else
            {
                filteredBuyCollection = buyPositions;
            }

            var funds = GetAllFunds();
            ConcurrentStack<TradeSwapSnapshot> snapshots = new ConcurrentStack<TradeSwapSnapshot>();
            var tasks = new Task[funds.Count];
            for (int fundCounter = 0; fundCounter < funds.Count; fundCounter++)
            {
                var counter = fundCounter;
                tasks[fundCounter] = Task.Factory.StartNew(() =>
                {
                    try
                    {

                        var fund = funds[counter];
                        var sellPositions = GetPositions(fund.FundCode).ToList();
                        var sellbuydictionary = new Dictionary<vw_Position, List<vw_Position>>();
                        for (int sellCounter = 0; sellCounter < sellPositions.Count; sellCounter++)
                        {
                            var sell = sellPositions[sellCounter];
                            for (int buyCounter = 0; buyCounter < filteredBuyCollection.Count; buyCounter++)
                            {
                                var buy = filteredBuyCollection[buyCounter];

                                _logger.Info(
                                    $"Processing TradeSwapId:{tradeSwapId}, FundId:{fund.FundId}, SellSecurityId:{sell.SecurityId}, BuySecurityId:{buy.SecurityId}, FundCounter:{counter}, SellCounter:{sellCounter}, BuyCounter:{buyCounter}, SellCount:{sellPositions.Count}, BuyCount:{filteredBuyCollection.Count}");

                                var isMatch = buy.SecurityId != sell.SecurityId;

                                if (isMatch && tradeSwapParam.Criteria.Cash)
                                {
                                    isMatch = sell.BidNum.HasValue && buy.OfferNum.HasValue &&
                                              buy.OfferNum.Value <= sell.BidNum.Value;
                                }

                                if (isMatch && tradeSwapParam.Criteria.Recovery)
                                {
                                    isMatch = buy.MoodyRecovery.HasValue && sell.MoodyRecovery.HasValue && buy.MoodyRecovery >= sell.MoodyRecovery;
                                }

                                if (isMatch && tradeSwapParam.Criteria.MoodyAdjCfr)
                                {
                                    isMatch = sell.MoodyCashFlowRatingAdjustedRank.HasValue &&
                                              buy.MoodyCashFlowRatingAdjustedRank.HasValue &&
                                              buy.MoodyCashFlowRatingAdjustedRank.Value >=
                                              sell.MoodyCashFlowRatingAdjustedRank.Value;
                                }

                                if (isMatch && tradeSwapParam.Criteria.MoodyAdjFacility)
                                {
                                    isMatch = sell.MoodyFacilityRatingAdjustedRank.HasValue &&
                                              buy.MoodyFacilityRatingAdjustedRank.HasValue &&
                                              buy.MoodyFacilityRatingAdjustedRank.Value >=
                                              sell.MoodyFacilityRatingAdjustedRank.Value;
                                }

                                if (isMatch && tradeSwapParam.Criteria.Spread)
                                {
                                    isMatch = sell.Spread.HasValue &&
                                              buy.Spread.HasValue &&
                                              buy.Spread.Value >= sell.Spread.Value;
                                }

                                if (isMatch && sell.BidNum != null && buy.OfferNum != null && sell.TotalParNum != null)
                                {
                                    if (!sellbuydictionary.ContainsKey(sell))
                                    {
                                        sellbuydictionary.Add(sell, new List<vw_Position>());
                                    }
                                    var buy1 = buy;
                                    if (sellbuydictionary[sell].All(sec => sec.SecurityId != buy1.SecurityId))
                                    {
                                        snapshots.Push(new TradeSwapSnapshot()
                                        {
                                            SellFundId = fund.FundId,
                                            TradeSwapId = tradeSwapId,
                                            BuySecurityId = buy.SecurityId.Value,
                                            SellSecurityId = sell.SecurityId.Value,
                                            SellExposure = sell.NumExposure,
                                            SellTotalExposure = sell.TotalParNum,
                                            SellSecurityBidPrice = sell.BidNum,
                                            SellPctPosition = sell.PctExposureNum * 100,
                                            SellMaturityDate = sell.SecurityMaturityDate,
                                            SellIssuer = sell.Issuer,
                                            SellFacility = sell.Facility,
                                            SellSpread = sell.Spread,
                                            SellLiquidityScore = sell.LiquidityScore,
                                            SellMoodyAdjFacility = sell.MoodyFacilityRatingAdjusted,
                                            SellMoodyAdjCFR = sell.MoodyCashFlowRatingAdjusted,
                                            SellRecovery =  sell.MoodyRecovery,
                                            SellYield = sell.YieldOffer,

                                            BuySecurityOfferPrice = buy.OfferNum,
                                            CreatedOn = DateTime.Now,
                                            BuyFundId = fund.FundId,
                                            BuyExposure = sell.NumExposure,
                                            BuyTotalExposure = buy.TotalParNum,
                                            BuyPctPosition = buy.PctExposureNum * 100,
                                            BuyMaturityDate = buy.SecurityMaturityDate,
                                            BuySpread = buy.Spread,
                                            BuyIssuer = buy.Issuer,
                                            BuyFacility = buy.Facility,
                                            BuyLiquidityScore = buy.LiquidityScore,
                                            BuyMoodyAdjFacility = buy.MoodyFacilityRatingAdjusted,
                                            BuyMoodyAdjCFR = buy.MoodyCashFlowRatingAdjusted,
                                            BuyRecovery = buy.MoodyRecovery,
                                            BuyYield = buy.YieldBid,

                                            BuySecurityBidPrice = buy.BidNum,
                                            SellSecurityOfferPrice=sell.OfferNum,

                                            BuySecurityCreditScore = buy.CreditScore,
                                            SellSecurityCreditScore = sell.CreditScore
                                        });
                                        sellbuydictionary[sell].Add(buy);
                                    }
                                }
                            }

                            var checkForTradeSwap = GetTradeSwap(tradeSwapId);
                            if (checkForTradeSwap.Status == 3)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        SetTradeSwap(tradeSwapId, 3,exception.ToString());
                        _logger.Error("Failed Inside Thread", exception);
                    }
                });
            }
            System.Threading.Thread.Sleep(2000);
            Task.WaitAll(tasks);
            var checkForTradeSwapFinal = GetTradeSwap(tradeSwapId);
            if (checkForTradeSwapFinal.Status != 3)
            {
                CopyDataToTable(snapshots.ToList());
                SetTradeSwap(tradeSwapId, 2,null);
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool CopyDataToTable(List<TradeSwapSnapshot> snapshots)
        {
            DataTable dt = new DataTable("Snapshots");
            dt.Columns.Add(new DataColumn("TradeSwapSnapshotId", typeof(Int64)));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns.Add(new DataColumn("TradeSwapId", typeof(int)));
            dt.Columns.Add(new DataColumn("SellSecurityId", typeof(int)));

            dt.Columns.Add(new DataColumn("SellFundId", typeof(int)));
            dt.Columns.Add(new DataColumn("SellExposure", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellTotalExposure", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellSecurityBidPrice", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellPctPosition", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellSpread", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellLiquidityScore", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellMaturityDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("SellIssuer", typeof(string)));
            dt.Columns.Add(new DataColumn("SellFacility", typeof(string)));
            dt.Columns.Add(new DataColumn("SellMoodyAdjCFR", typeof(string)));
            dt.Columns.Add(new DataColumn("SellMoodyAdjFacility", typeof(string)));

            dt.Columns.Add(new DataColumn("BuySecurityId", typeof(int)));
            dt.Columns.Add(new DataColumn("BuySecurityOfferPrice", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BuyFundId", typeof(int)));
            dt.Columns.Add(new DataColumn("BuyExposure", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BuyTotalExposure", typeof(decimal)));

            dt.Columns.Add(new DataColumn("BuyPctPosition", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BuySpread", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BuyLiquidityScore", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BuyMaturityDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("BuyIssuer", typeof(string)));
            dt.Columns.Add(new DataColumn("BuyFacility", typeof(string)));
            dt.Columns.Add(new DataColumn("BuyMoodyAdjCFR", typeof(string)));
            dt.Columns.Add(new DataColumn("BuyMoodyAdjFacility", typeof(string)));



            dt.Columns.Add(new DataColumn("SellRecovery", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BuyRecovery", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellYield", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BuyYield", typeof(decimal)));

            dt.Columns.Add(new DataColumn("BuySecurityBidPrice", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellSecurityOfferPrice", typeof(decimal)));

            dt.Columns.Add(new DataColumn("BuySecurityCreditScore", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SellSecurityCreditScore", typeof(decimal)));


            for (int i = 0; i < snapshots.Count; i++)
            {
                var snapshot = snapshots[i];
                _logger.Info($"Processing Snapshot:{i}, Security:{snapshots[i].BuySecurityId}, FundId:{snapshots[i].SellFundId}, SellSecurity:{snapshots[i].SellSecurityId}");
                var datarow = dt.NewRow();
                datarow[1] = snapshot.TradeSwapId;
                datarow[2] = snapshot.SellSecurityId;
                datarow[3] = snapshot.SellFundId;
                datarow[4] = snapshot.SellExposure.HasValue ? (object)snapshot.SellExposure : DBNull.Value;
                datarow[5] = snapshot.SellTotalExposure.HasValue ? (object)snapshot.SellTotalExposure : DBNull.Value;
                datarow[6] = snapshot.SellSecurityBidPrice.HasValue ? (object)snapshot.SellSecurityBidPrice : DBNull.Value;
                datarow[7] = snapshot.SellPctPosition.HasValue ? (object)snapshot.SellPctPosition : DBNull.Value;
                datarow[8] = snapshot.SellSpread.HasValue ? (object)snapshot.SellSpread : DBNull.Value;
                datarow[9] = snapshot.SellLiquidityScore.HasValue ? (object)snapshot.SellLiquidityScore.Value : DBNull.Value;
                datarow[10] = snapshot.SellMaturityDate.HasValue ? (object)snapshot.SellMaturityDate.Value : DBNull.Value;
                datarow[11] = snapshot.SellIssuer;
                datarow[12] = snapshot.SellFacility;
                datarow[13] = snapshot.SellMoodyAdjCFR;
                datarow[14] = snapshot.SellMoodyAdjFacility;

                datarow[15] = snapshot.BuySecurityId;
                datarow[16] = snapshot.BuySecurityOfferPrice.HasValue ? (object)snapshot.BuySecurityOfferPrice.Value : DBNull.Value;
                datarow[17] = snapshot.BuyFundId;
                datarow[18] = snapshot.BuyExposure.HasValue ? (object)snapshot.BuyExposure.Value : DBNull.Value;
                datarow[19] = snapshot.BuyTotalExposure.HasValue ? (object)snapshot.BuyTotalExposure.Value : DBNull.Value;
                datarow[20] = snapshot.BuyPctPosition.HasValue ? (object)snapshot.BuyPctPosition.Value : DBNull.Value;
                datarow[21] = snapshot.BuySpread.HasValue ? (object)snapshot.BuySpread.Value : DBNull.Value;
                datarow[22] = snapshot.BuyLiquidityScore.HasValue ? (object)snapshot.BuyLiquidityScore.Value : DBNull.Value;
                datarow[23] = snapshot.BuyMaturityDate.HasValue ? (object)snapshot.BuyMaturityDate.Value : DBNull.Value;
                datarow[24] = snapshot.BuyIssuer;
                datarow[25] = snapshot.BuyFacility;
                datarow[26] = snapshot.BuyMoodyAdjCFR;
                datarow[27] = snapshot.BuyMoodyAdjFacility;

                datarow[28] = snapshot.SellRecovery.HasValue ? (object)snapshot.SellRecovery.Value : DBNull.Value;
                datarow[29] = snapshot.BuyRecovery.HasValue ? (object)snapshot.BuyRecovery.Value : DBNull.Value;
                datarow[30] = snapshot.SellYield.HasValue ? (object)snapshot.SellYield.Value : DBNull.Value;
                datarow[31] = snapshot.BuyYield.HasValue ? (object)snapshot.BuyYield.Value : DBNull.Value;

                datarow[32] = snapshot.BuySecurityBidPrice.HasValue ? (object)snapshot.BuySecurityBidPrice.Value : DBNull.Value;
                datarow[33] = snapshot.SellSecurityOfferPrice.HasValue ? (object)snapshot.SellSecurityOfferPrice.Value : DBNull.Value;

                datarow[34] = snapshot.BuySecurityCreditScore.HasValue ? (object)snapshot.BuySecurityCreditScore.Value : DBNull.Value;
                datarow[35] = snapshot.SellSecurityCreditScore.HasValue ? (object)snapshot.SellSecurityCreditScore.Value : DBNull.Value;

                dt.Rows.Add(datarow);
            }

            using (
                SqlConnection connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.BulkCopyTimeout = 600000; // in seconds
                    bulkCopy.DestinationTableName = "CLO.TradeSwapSnapshot";
                    bulkCopy.WriteToServer(dt);
                }
                connection.Close();
            }

            return true;
        }
    }
}
