using System;
using System.Linq;
using log4net;
using YCM.CLO.CalculationEngine.Contracts;
using YCM.CLO.CalculationEngine.Objects;
using YCM.CLO.DataAccess.Models;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess;
using System.IO;
using CsvHelper;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;

namespace YCM.CLO.CalculationEngine
{
    public class CalculationEngine : ICalculationEngine
    {
        private readonly ILog _logger;

        public CalculationEngine()
        {
            _logger = LogManager.GetLogger(typeof(CalculationEngine));
        }

        public bool Calculate(int dateId, string user)
        {
            try
            {
                _logger.Info("Received Request for Calculation for dateId:" + dateId);


                YieldCalculator yieldCalculator = new YieldCalculator();
                WarfRecoveryCalculator warfRecoveryCalculator = new WarfRecoveryCalculator();
                SnPAssetRecoveryRatingCalculator snPAssetRecoveryRatingCalculator = new SnPAssetRecoveryRatingCalculator();
                var snpWarfCalculator = new SnpWarfCalculator();
                var moodysLgdCalculator = new MoodysLgdCalculator();
                var yieldAvgLgdCalculator = new YieldAvgLgdCalculator();
                var snpAAARecoveryCalculator = new SnpAAARecoveryCalculator();
                TotalParLifeCalculator totalParLifeCalculator = new TotalParLifeCalculator();
                MoodyFacilityRatingAdjustedCalculator moodyFacilityRatingAdjustedCalculator = new MoodyFacilityRatingAdjustedCalculator();
                MoodyCashFlowRatingAdjustedCalculator moodyCashFlowRatingAdjustedCalculator = new MoodyCashFlowRatingAdjustedCalculator();

                using (CLOContext cloContext = new CLOContext())
                {
                    _logger.Info("Get calculation for dateId:" + dateId);
                    var calculations = cloContext.Calculations.Where(c => c.DateId == dateId).ToList();
                    _logger.Info("calculations data for dateId:" + calculations.Count());

                    _logger.Info("Get MarketData for dateId:" + dateId);
                    var marketDatas = cloContext.vw_MarketData.AsNoTracking().Where(d => d.DateId == dateId).AsNoTracking().ToList();
                    _logger.Info("marketData data for dateId:" + marketDatas.Count());

                    _logger.Info("Get Security & Fund Details");
                    var securities =
                        cloContext.vwSecurityFunds.OrderBy(v => v.SecurityId).ThenBy(v => v.FundCode).ToList();
                    _logger.Info("securities data for dateId:" + securities.Count());

                    _logger.Info("Get Position data for Active Fudn And dateId:" + dateId);
                    var groupedPositionsDictionary = cloContext.Positions
                        .Include(x => x.Fund)
                        .Include(p => p.Security)
                        .Where(p => p.DateId == dateId && p.Fund.IsActive)
                        .GroupBy(p => p.SecurityId).AsNoTracking()
                        .ToDictionary(p => p.Key, p => p.ToList());
                    _logger.Info("Position data for dateId:" + groupedPositionsDictionary.Count());

                    _logger.Info("Get Parameter Values");
                    var parameterValues = cloContext.ParameterValues.Include(c => c.ParameterType).AsNoTracking().ToList();
                    _logger.Info("parameterValues for dateId:" + calculations.Count());

                    _logger.Info("Loop all Securities details");
                    securities.ForEach(security =>
                    {
                        _logger.Info("  Security started:" + security.SecurityId);
                        if (groupedPositionsDictionary.ContainsKey(security.SecurityId))
                        {
                            var securityPositions = groupedPositionsDictionary[security.SecurityId].ToList();
                            _logger.Info("  securityPositions" + securityPositions.Count());
                            //securityPositions.ForEach(gpf =>
                            //{
                            _logger.Info("  Security fund:" + security.FundId);
                            var calculation =
                                calculations.FirstOrDefault(
                                    c => c.SecurityId == security.SecurityId && c.FundId == security.FundId);

                            var marketData =
                                marketDatas.FirstOrDefault(
                                    m => m.SecurityId == security.SecurityId && m.FundId == security.FundId);

                            var allmarketDatas =
                                marketDatas.Where(
                                    m => m.SecurityId == security.SecurityId).ToList();

                            if (marketData == null)
                            {
                                marketData = marketDatas.FirstOrDefault(m => m.SecurityId == security.SecurityId);
                            }

                            if (calculation == null)
                            {
                                calculation = new Calculation()
                                {
                                    SecurityId = security.SecurityId,
                                    FundId = security.FundId,
                                    DateId = dateId,
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = user
                                };
                                calculations.Add(calculation);
                                cloContext.Calculations.Add(calculation);
                            }
                            else
                            {
                                calculation.LastUpdatedBy = user;
                                calculation.LastUpdatedOn = DateTime.Now;
                            }

                            try
                            {
                                _logger.Info("  Yeild calculation started");
                                yieldCalculator.Calculate(cloContext, calculation, marketData, securityPositions,
                                    security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }


                            try
                            {
                                _logger.Info("  warfRecoveryCalculator calculation started");
                                warfRecoveryCalculator.Calculate(cloContext, calculation, marketData, securityPositions,
                               security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }

                            try
                            {
                                _logger.Info("  totalParLifeCalculator calculation started");
                                totalParLifeCalculator.Calculate(cloContext, calculation, marketData, securityPositions,
                              security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }

                            try
                            {
                                _logger.Info("  moodyFacilityRatingAdjustedCalculator calculation started");
                                moodyFacilityRatingAdjustedCalculator.Calculate(cloContext, calculation, marketData, securityPositions,
                               security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }

                            try
                            {
                                _logger.Info("  moodyCashFlowRatingAdjustedCalculator calculation started");
                                moodyCashFlowRatingAdjustedCalculator.Calculate(cloContext, calculation, marketData, securityPositions,
                               security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }

                            try
                            {
                                _logger.Info("  snPAssetRecoveryRatingCalculatorsnPAssetRecoveryRatingCalculator calculation started");
                                snPAssetRecoveryRatingCalculator.Calculate(cloContext, calculation, marketData, securityPositions,
                               security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }

                            try
                            {
                                _logger.Info("  warfRecoveryCalculator calculation started");
                                //NOTE: this relies on calculations in snPAssetRecoveryRatingCalculator, so it must come after it
                                snpWarfCalculator.Calculate(cloContext, calculation, marketData, securityPositions, security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }

                            try
                            {
                                _logger.Info("  moodysLgdCalculator calculation started ");
                                //Depends on Warf calculator, must come after
                                moodysLgdCalculator.Calculate(cloContext, calculation, marketData, securityPositions, security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }

                            try
                            {
                                _logger.Info("  yieldAvgLgdCalculator calculation started");
                                //NOTE: depends on yieldcalculator, snpWarfCalculator and moodysLgdCalculator. Must come afterwards
                                yieldAvgLgdCalculator.Calculate(cloContext, calculation, marketData, securityPositions, security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }
                            try
                            {
                                _logger.Info("  warfRecoveryCalculator calculation started");
                                //NOTE: depends on snPAssetRecoveryRatingCalculator. Must come afterwards
                                snpAAARecoveryCalculator.Calculate(cloContext, calculation, marketData, securityPositions, security, parameterValues, null, dateId, allmarketDatas);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error(exception);
                            }
                        }
                    });

                    _logger.Info("Group Calculation details by FundId");
                    calculations.GroupBy(c => c.FundId).ToList().ForEach(groupedCalculations =>
                    {
                        _logger.Info("  Group Calculation started:" + groupedCalculations.ToString());
                        var gc = groupedCalculations.ToList();
                        TaregtBidRecovery cappedBidYieldsRecovery = new TaregtBidRecovery();
                        TaregtBidRecovery cappedOfferYieldsRecovery = new TaregtBidRecovery();
                        TaregtBidRecovery cappedMidYieldsRecovery = new TaregtBidRecovery();

                        gc.Where(gcc => gcc.CappedYieldBid.HasValue && gcc.CappedYieldBid.Value > 0 && gcc.WARFRecovery.HasValue && gcc.WARFRecovery > 0)
                        .ToList().ForEach(gcc =>
                        {
                            cappedBidYieldsRecovery.Yields.Add((double)gcc.CappedYieldBid.Value);
                            cappedBidYieldsRecovery.WarfRecoveries.Add((double)gcc.WARFRecovery.Value);
                        });

                        gc.Where(gcc => gcc.CappedYieldOffer.HasValue && gcc.CappedYieldOffer.Value > 0 && gcc.WARFRecovery.HasValue && gcc.WARFRecovery > 0)
                        .ToList().ForEach(gcc =>
                        {
                            cappedOfferYieldsRecovery.Yields.Add((double)gcc.CappedYieldOffer.Value);
                            cappedOfferYieldsRecovery.WarfRecoveries.Add((double)gcc.WARFRecovery.Value);
                        });

                        gc.Where(gcc => gcc.CappedYieldMid.HasValue && gcc.CappedYieldMid.Value > 0 && gcc.WARFRecovery.HasValue && gcc.WARFRecovery > 0)
                        .ToList().ForEach(gcc =>
                        {
                            cappedMidYieldsRecovery.Yields.Add((double)gcc.CappedYieldMid.Value);
                            cappedMidYieldsRecovery.WarfRecoveries.Add((double)gcc.WARFRecovery.Value);
                        });

                        try
                        {
                            TargetBetterWorseYieldCalculator targetBetterWorseYieldCalculator = new TargetBetterWorseYieldCalculator(dateId,
                           cappedBidYieldsRecovery,
                           cappedOfferYieldsRecovery,
                           cappedMidYieldsRecovery);

                            gc.ForEach(calculation =>
                            {
                                targetBetterWorseYieldCalculator.Calculate(cloContext, calculation, null, null, null, null, null, dateId, null);
                            });
                        }
                        catch (Exception exception)
                        {
                            _logger.Error(exception);
                        }
                        _logger.Info("  Group Calculation ended:" + groupedCalculations.ToString());
                    });

                    try
                    {
                        _logger.Info("Save changes to Database");
                        cloContext.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(exception);
                    }

                    try
                    {
                        _logger.Info("ProcessConflicts");
                        ProcessConflicts(user);
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(exception);
                    }

                    try
                    {
                        _logger.Info("Process AddMissingPositions");
                        AddMissingPositions();
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(exception);
                    }

                }

                using (IRepository repository = new Repository())
                {
                    _logger.Info("Process GenerateAggregatedPositions");
                    repository.GenerateAggregatedPositions();
                    _logger.Info("Process CalculateSummaries");
                    repository.CalculateSummaries();
                }

                _logger.Info("Process CalculateFrontier for dateId:" + dateId);
                CalculateFrontier(dateId, -1, user);

                _logger.Info("Completed Calculation");
                EmailHelper.SendEmail("Completed CLO Calculation", "CLO Calculation");
                return true;
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception has occurred during CLO Calculation");
                _logger.Error("Failed Calculation", exception);
                return false;
            }

        }

        private bool AddMissingPositions()
        {
            using (
               SqlConnection connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["CLOContext"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand commandCleanUpTradeSwaps = new SqlCommand("CLO.spAddPositionsForTempSecurities", connection))
                {
                    commandCleanUpTradeSwaps.CommandType = CommandType.StoredProcedure;
                    commandCleanUpTradeSwaps.ExecuteNonQuery();
                }
                connection.Close();
                return true;
            }
        }

        public bool ProcessConflicts(string user)
        {
            LoanAttributeOverrideReconProcessor loanAttributeOverrideReconProcessor = new LoanAttributeOverrideReconProcessor();
            return loanAttributeOverrideReconProcessor.Process(user);
        }

        public bool ProcessTradeSwap(int tradeSwapId)
        {
            TradeSwapProcessor tradeSwapProcessor = new TradeSwapProcessor();
            try
            {

                int retryCount = 0;
                bool swapStatus = false;

                do
                {
                    swapStatus = tradeSwapProcessor.Process(tradeSwapId);
                    if (!swapStatus)
                    {
                        retryCount++;
                    }
                } while (!swapStatus && retryCount <= 3);
                EmailHelper.SendEmail("Completed Trade Swap calculation", "CLO Trade Swap");
                return swapStatus;
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception has occurred during CLO Calculation");
                tradeSwapProcessor.SetTradeSwap(tradeSwapId, 3, exception.ToString());
                _logger.Error("Failed Trade Swapping", exception);
                return false;
            }
        }

        public bool ProcessTradeFile(int fundId, int fileDateId, int dateId)
        {
            using (CLOContext context = new CLOContext())
            {
                var fund = context.Funds.FirstOrDefault(f => f.IsActive && f.CLOFileName != null && f.FundId == fundId);
                IRepository repository = new Repository();
                ProcessTradeFile(repository, context, fund, fileDateId, dateId);
                return true;
            }
        }

        private bool ProcessTradeFile(IRepository repository, CLOContext context, Fund fund, int fileDateId, int dateId)
        {
            try
            {
                _logger.Info("Started On " + DateTime.Now.ToString() + "for FundId :" + fund.FundId);

                _logger.Info("  Check (fund.IsStale.HasValue: " + fund.IsStale.HasValue.ToString() + " && fund.IsStale.Value: " + fund.IsStale.Value + ") (fund.IsPrincipalCashStale.HasValue: " + fund.IsPrincipalCashStale.HasValue.ToString() + " && fund.IsPrincipalCashStale.Value: " + fund.IsPrincipalCashStale.Value + ")");
                if (!(fund.IsPrincipalCashStale.HasValue && fund.IsPrincipalCashStale.Value || fund.IsStale.HasValue && fund.IsStale.Value))
                {
                    _logger.Info("  Started Processing CaptureDailySnapshot function for FundId :" + fund.FundId);
                    repository.CaptureDailySnapshot(fund.FundId, dateId);
                    _logger.Info("  Completed Processing CaptureDailySnapshot function for FundId :" + fund.FundId);
                }

                _logger.Info("  Started Processing CreateStalePositions function for FundId :" + fund.FundId);
                repository.CreateStalePositions(fund.FundId);
                _logger.Info("  Completed Processing CreateStalePositions function for FundId :" + fund.FundId);

                context.Entry(fund).Reload();
                _logger.Info("  Processing for file :" + fund.CLOFileName);
                if (!string.IsNullOrEmpty(fund.CLOFileName))
                {
                    _logger.Info("      Started Processing ReadTradeFile function for FundId :" + fund.FundId);
                    ReadTradeFile(repository, fund, context, fileDateId, dateId);
                    _logger.Info("      Extra Check for FundId :" + fund.FundId + "   (fund.IsStale.Value: " + fund.IsStale.Value + "      && fund.IsPrincipalCashStale.Value: " + fund.IsPrincipalCashStale.Value + ")");
                    _logger.Info("      Completed Processing ReadTradeFile function for FundId :" + fund.FundId);
                    context.SaveChanges();
                    if (fund.IsPrincipalCashStale.HasValue && fund.IsPrincipalCashStale.Value)
                    {
                        _logger.Info("      Started Processing CleanPositionsBasedOnPrincipalCash function for FundId :" + fund.FundId + " and dateId:" + dateId.ToString());
                        repository.CleanPositionsBasedOnPrincipalCash(fund.FundId, dateId);
                        _logger.Info("      Extra Check for FundId :" + fund.FundId + "   (fund.IsStale.Value: " + fund.IsStale.Value + "      && fund.IsPrincipalCashStale.Value: " + fund.IsPrincipalCashStale.Value + ")");
                        _logger.Info("      Completed Processing CleanPositionsBasedOnPrincipalCash function for FundId :" + fund.FundId + " and dateId:" + dateId.ToString());
                    }
                    else
                    {
                        _logger.Info("      Started Processing RefillPositionsBasedOnPrincipalCash function for FundId :" + fund.FundId + " and dateId:" + dateId.ToString());
                        repository.RefillPositionsBasedOnPrincipalCash(fund.FundId, dateId);
                        _logger.Info("      Extra Check for FundId :" + fund.FundId + "   (fund.IsStale.Value: " + fund.IsStale.Value + "      && fund.IsPrincipalCashStale.Value: " + fund.IsPrincipalCashStale.Value + ")");
                        _logger.Info("      Completed Processing RefillPositionsBasedOnPrincipalCash function for FundId :" + fund.FundId + " and dateId:" + dateId.ToString());
                    }
                    _logger.Info("      Started Processing CreateStalePositions (2) function for FundId :" + fund.FundId);
                    repository.CreateStalePositions(fund.FundId);
                    _logger.Info("  Extra Check for FundId :" + fund.FundId + "   (fund.IsStale.Value: " + fund.IsStale.Value + "      && fund.IsPrincipalCashStale.Value: " + fund.IsPrincipalCashStale.Value + ")");
                    _logger.Info("      Completed Processing CreateStalePositions (2) function for FundId :" + fund.FundId);
                }

                _logger.Info("Completed On " + DateTime.Now.ToString());
                return true;
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                throw;
            }
        }

        //private void ReadTradeFile(IRepository repository, Fund fund, CLOContext context, int fileDateId, int dateId)
        //{
        //    if (!(fund.IsStale.HasValue && fund.IsStale.Value) || fund.IsPrincipalCashStale.HasValue && fund.IsPrincipalCashStale.Value)
        //    {
        //        var clofile =
        //            Path.Combine(ConfigurationManager.AppSettings["SourceDirectory"], fund.CLOFileName)
        //                .Replace("{dateid}", fileDateId.ToString());

        //        var archiveFile =
        //            Path.Combine(ConfigurationManager.AppSettings["ArchiveDirectory"], fund.CLOFileName)
        //                .Replace("{dateid}", fileDateId.ToString());

        //        _logger.Info("Processing Fund : " + fund.FundCode);
        //        if (File.Exists(clofile))
        //        {
        //            using (TextReader fileReader = File.OpenText(clofile))
        //            {
        //                var csv = new CsvReader(fileReader);
        //                var allValues = csv.GetRecords<dynamic>();
        //                decimal balance = 0;
        //                _logger.Info($"Existing Cash Balance : {fund.PrincipalCash.GetValueOrDefault()}");

        //                allValues.ToList().ForEach(a =>
        //                {
        //                    balance += decimal.Parse(a.CostAmount.ToString());
        //                });

        //                fund.PrincipalCash = balance;
        //                fund.IsPrincipalCashStale = false;
        //                _logger.Info($"New Cash Balance : {fund.PrincipalCash.GetValueOrDefault()}");

        //                context.Entry(fund).State = EntityState.Modified;
        //            }
        //            //commented  By Gravitas(Rakesh)
        //            //MoveFile(clofile, archiveFile);
        //        }
        //        else
        //        {
        //            fund.IsPrincipalCashStale = true;
        //        }
        //    }
        //}

        private void ReadTradeFile(IRepository repository, Fund fund, CLOContext context, int fileDateId, int dateId)
        {
            try
            {
                _logger.Info("          Check (fund.IsStale.HasValue: " + fund.IsStale.HasValue.ToString() + " && fund.IsStale.Value: " + fund.IsStale.Value + ") (fund.IsPrincipalCashStale.HasValue: " + fund.IsPrincipalCashStale.HasValue.ToString() + " && fund.IsPrincipalCashStale.Value: " + fund.IsPrincipalCashStale.Value + ")");
                if (!(fund.IsStale.HasValue && fund.IsStale.Value || fund.IsPrincipalCashStale.HasValue && fund.IsPrincipalCashStale.Value))
                {
                    var clofile = Convert.ToString(ConfigurationManager.AppSettings["SourceDirectory"]).Replace("{dateid}", fileDateId.ToString()); ;
                    _logger.Info("          Processing File : " + clofile + "( fund:" + fund.FundCode + ")");

                    if (File.Exists(clofile))
                    {
                        _logger.Info("          File exists");
                        using (TextReader fileReader = File.OpenText(clofile))
                        {
                            var csv = new CsvReader(fileReader);
                            var allValues = csv.GetRecords<dynamic>().ToList();
                            _logger.Info($"Existing Cash Balance : {fund.PrincipalCash.GetValueOrDefault()} for fund:" + fund.FundCode);

                            var filter = allValues.Where(w => w.Fund == fund.FundCode).FirstOrDefault();
                            if (filter != null && allValues.Any())
                                fund.PrincipalCash = Convert.ToDecimal(filter.TradeCashValue);

                            fund.IsPrincipalCashStale = false;
                            _logger.Info($"New Cash Balance : {fund.PrincipalCash.GetValueOrDefault()} for fund:" + fund.FundCode);

                            context.Entry(fund).State = EntityState.Modified;
                        }

                        //MoveFile(clofile, archiveFile);
                    }
                    else
                    {
                        _logger.Info("          File not exists, update fund: " + fund.FundCode + " as stale");
                        fund.IsPrincipalCashStale = true;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                throw;
            }
        }


        static void MoveFile(string source, string dest)
        {
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }
            File.Move(source, dest);
        }

        public bool ProcessTradeFiles(int fileDateId, int dateId)
        {
            try
            {
                _logger.Info("Started On " + DateTime.Now.ToString() + " for fileDateId:" + fileDateId.ToString() + " and dateid:" + dateId.ToString());
                using (CLOContext context = new CLOContext())
                {
                    IRepository repository = new Repository();
                    context.Funds.Where(f => f.IsActive).ToList().ForEach(fund =>
                    {
                        ProcessTradeFile(repository, context, fund, fileDateId, dateId);
                    });

                    SendStaleStatusEmail();
                    _logger.Info("Completed On " + DateTime.Now.ToString());

                    return true;
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                throw;
            }
        }

        private void SendStaleStatusEmail()
        {
            _logger.Info("Started Stale email at " + DateTime.Now.ToString());
            IRepository repository = new Repository();
            /* As requested by user (wendy):send email only when any fund has stale data
             * code addedd to checkisanyfuns has stale data or Pricipal Cash is stale  
             * 20-Sep-2021
             */
            var FUNDS = repository.GetFunds();
            if (FUNDS.Where(w => (w.IsStale.HasValue && w.IsStale.Value)).Any() || FUNDS.Where(w => (w.IsPrincipalCashStale.HasValue && w.IsPrincipalCashStale.Value)).Any())
            {
                _logger.Info("Stale Fund Or Principal Cash found");

                WebClient client = new WebClient();
                string message = client.DownloadString(ConfigurationManager.AppSettings["FundDailySnapshotURL"]);
                string subject = ConfigurationManager.AppSettings["FundDailySnapshotSubject"];

                subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());
                Console.WriteLine(message);

                var msg = new MailMessage();
                msg.Body = message;
                msg.IsBodyHtml = true;
                msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));

                var emailTos = ConfigurationManager.AppSettings["FundDailySnapshotToEmailIds"].Split(new char[] { ',', ';' });

                emailTos.ToList().ForEach(e =>
                {
                    msg.To.Add(e);
                });

                string ccList = ConfigurationManager.AppSettings["FundDailySnapshotCCEmailIds"];
                if (!string.IsNullOrEmpty(ccList))
                {
                    ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                    {
                        msg.CC.Add(e);
                    });
                }

                msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                msg.Subject = subject;
                msg.Body = message;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(msg);
            }
            else
                _logger.Info("NO - Stale Fund Or Principal Cash found and skipping email send functionality");
            _logger.Info("Completed sending Stale email at " + DateTime.Now.ToString());
        }

        public bool SendPriceMoverEmail()
        {
            try
            {
                _logger.Info("SendPriceMoverEmail started");
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;//added By Gravitas
                _logger.Info("Client URL:" + ConfigurationManager.AppSettings["PriceMoverURL"]);
                string message = client.DownloadString(ConfigurationManager.AppSettings["PriceMoverURL"]);
                string subject = ConfigurationManager.AppSettings["PriceMoverSubject"];

                subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());
                Console.WriteLine(message);
                _logger.Info("message:" + message);

                var msg = new MailMessage();
                msg.Body = message;
                msg.IsBodyHtml = true;
                msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));

                var emailTos = ConfigurationManager.AppSettings["PriceMoverToEmailIds"].Split(new char[] { ',', ';' });
                emailTos.ToList().ForEach(e =>
                {
                    msg.To.Add(e);
                });

                string ccList = ConfigurationManager.AppSettings["PriceMoverCCEmailIds"];
                if (!string.IsNullOrEmpty(ccList))
                {
                    ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                    {
                        msg.CC.Add(e);
                    });
                }

                msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                msg.Subject = subject;
                msg.Body = message;

                _logger.Info("SendPriceMoverEmail ~~ message:" + message + "  ;From:" + msg.From + " ;Subject:" + msg.Subject);
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(msg);


                return true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
                _logger.Error(exception);
                return false;
            }
        }

        public bool SendRatingChangeEmail()
        {
            try
            {
                _logger.Info("Method SendRatingChangeEmail started");
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                _logger.Info("Rating Change URL:" + ConfigurationManager.AppSettings["RatingChangeUrl"]);
                string message = client.DownloadString(ConfigurationManager.AppSettings["RatingChangeUrl"]);
                string subject = ConfigurationManager.AppSettings["RatingChangeSubject"];

                subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());

                var msg = new MailMessage();
                msg.Body = message;
                _logger.Info("message:" + message);
                msg.IsBodyHtml = true;
                msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));

                var emailTos = ConfigurationManager.AppSettings["RatingChangeToEmailIds"].Split(new char[] { ',', ';' });
                emailTos.ToList().ForEach(e =>
                {
                    msg.To.Add(e);
                });

                string ccList = ConfigurationManager.AppSettings["RatingChangeCCEmailIds"];
                if (!string.IsNullOrEmpty(ccList))
                {
                    ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                    {
                        msg.CC.Add(e);
                    });
                }

                msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                msg.Subject = subject;
                msg.Body = message;
                _logger.Info("SendRatingChangeEmail ~~ message:" + message + "  ;From:" + msg.From + " ;Subject:" + msg.Subject);

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(msg);
                return true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
                _logger.Error(exception);
                return false;
            }
            finally
            {
                _logger.Info("Method SendRatingChangeEmail Ended");
            }
        }

        public bool SendMismatchEmail()
        {
            try
            {
                _logger.Info("Method SendMismatchEmail Started");
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                _logger.Info("Moodys Adjusted Cfr Mismatch URL:" + ConfigurationManager.AppSettings["MoodysAdjustedCfrMismatchURL"]);
                string message = client.DownloadString(ConfigurationManager.AppSettings["MoodysAdjustedCfrMismatchURL"]);
                string subject = ConfigurationManager.AppSettings["MoodysAdjustedCfrMismatchSubject"];

                subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());

                var msg = new MailMessage();
                msg.Body = message;
                msg.IsBodyHtml = true;
                msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));

                var emailTos = ConfigurationManager.AppSettings["MoodysAdjustedCfrMismatchEmailIds"].Split(new char[] { ',', ';' });
                emailTos.Where(e => !string.IsNullOrEmpty(e) && !string.IsNullOrWhiteSpace(e)).ToList().ForEach(e =>
                  {
                      msg.To.Add(e);
                  });

                string ccList = ConfigurationManager.AppSettings["MoodysAdjustedCfrMismatchCCEmailIds"];
                if (!string.IsNullOrEmpty(ccList))
                {
                    ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                    {
                        msg.CC.Add(e);
                    });
                }

                msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                msg.Subject = subject;
                msg.Body = message;
                _logger.Info("SendMismatchEmail ~~ message:" + message + "  ;From:" + msg.From + " ;Subject:" + msg.Subject);

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(msg);
                return true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
                _logger.Error(exception);
                return false;
            }
            finally
            {
                _logger.Info("Method SendMismatchEmail Ended");
            }
        }
        public bool UpdateStalePositions()
        {
            try
            {
                IRepository repository = new Repository();
                using (CLOContext context = new CLOContext())
                {
                    context.Funds.Where(f => f.IsActive).ToList().ForEach(fund =>
                    {
                        repository.CreateStalePositions(fund.FundId);
                    });
                }
                return true;
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                throw;
            }
        }

        public bool SendTotalParChangeEmail(int startDateId, int endDateId)
        {
            try
            {
                _logger.Info("SendTotalParChangeEmail started");

                IRepository repository = new Repository();
                var totalParChanges = repository.GetTotalParChange(startDateId, endDateId);
                if (totalParChanges != null && totalParChanges.Any())
                {
                    _logger.Info("Total Par Change Found:");
                    WebClient client = new WebClient();
                    client.Credentials = CredentialCache.DefaultCredentials;
                    _logger.Info("Client URL to get Total Par content:" + ConfigurationManager.AppSettings["TotalParChangeURL"]);
                    string message = client.DownloadString(ConfigurationManager.AppSettings["TotalParChangeURL"]);
                    string subject = ConfigurationManager.AppSettings["TotalParChangeURLSubject"];

                    subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());
                    Console.WriteLine(message);
                    _logger.Info("Email Message to be sent:" + message);

                    var msg = new MailMessage();
                    msg.IsBodyHtml = true;
                    msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));
                    _logger.Info("Email Message object created");

                    var emailTos = ConfigurationManager.AppSettings["TotalParChangeToEmailIds"].Split(new char[] { ',', ';' });
                    _logger.Info("Email Message to be sent to:" + emailTos);
                    emailTos.ToList().ForEach(e =>
                    {
                        msg.To.Add(e);
                    });

                    string ccList = ConfigurationManager.AppSettings["TotalParChangeCCEmailIds"];
                    _logger.Info("Email Message to be CC:" + ccList);
                    if (!string.IsNullOrEmpty(ccList))
                    {
                        ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                        {
                            msg.CC.Add(e);
                        });
                    }

                    msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                    msg.Subject = subject;
                    msg.Body = message;

                    _logger.Info("SendTotalParChangeEmail ~~ message:" + message + "  ;From:" + msg.From + " ;Subject:" + msg.Subject);
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Send(msg);
                    _logger.Info("TotalPar Change Email Sent successfully");
                }
                else
                    _logger.Info("No change in Total Par Found and hence no email will be sent today");

                return true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
                _logger.Info("Exeption while sending email message on TotalPar Change");
                _logger.Error(exception);
                return false;
            }
        }

        public bool SendRecoveryChangeEmail(int startDateId, int endDateId)
        {
            try
            {
                _logger.Info("SendRecoveryChangeEmail started");

                IRepository repository = new Repository();
                var moodyRecoveryChanges = repository.GetMoodyRecoveryChange(startDateId, endDateId);
                if (moodyRecoveryChanges != null && moodyRecoveryChanges.Any())
                {
                    _logger.Info("Moody Recovery Change Found:");
                    WebClient client = new WebClient();
                    client.Credentials = CredentialCache.DefaultCredentials;
                    _logger.Info("Client URL to get Moody Recovery content:" + ConfigurationManager.AppSettings["MoodyRecoveryChangeURL"]);
                    string message = client.DownloadString(ConfigurationManager.AppSettings["MoodyRecoveryChangeURL"]);
                    string subject = ConfigurationManager.AppSettings["MoodyRecoveryChangeURLSubject"];

                    subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());
                    Console.WriteLine(message);
                    _logger.Info("Email Message to be sent:" + message);

                    var msg = new MailMessage();
                    msg.IsBodyHtml = true;
                    msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));
                    _logger.Info("Email Message object created");

                    var emailTos = ConfigurationManager.AppSettings["MoodyRecoveryChangeToEmailIds"].Split(new char[] { ',', ';' });
                    _logger.Info("Email Message to be sent to:" + emailTos);
                    emailTos.ToList().ForEach(e =>
                    {
                        msg.To.Add(e);
                    });

                    string ccList = ConfigurationManager.AppSettings["MoodyRecoveryChangeCCEmailIds"];
                    _logger.Info("Email Message to be CC:" + ccList);
                    if (!string.IsNullOrEmpty(ccList))
                    {
                        ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                        {
                            msg.CC.Add(e);
                        });
                    }

                    msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                    msg.Subject = subject;
                    msg.Body = message;

                    _logger.Info("SendRecoveryChangeEmail ~~ message:" + message + "  ;From:" + msg.From + " ;Subject:" + msg.Subject);
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Send(msg);
                    _logger.Info("SendRecoveryChangeEmail Change Email Sent successfully");
                }
                else
                    _logger.Info("No change in Moody Recovery Found and hence no email will be sent today");

                return true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
                _logger.Info("Exeption while sending email message on Moody Recovery Change");
                _logger.Error(exception);
                return false;
            }
        }

        public bool CalculateFrontier(int dateId, int fundId, string user)
        {
            try
            {
                ImpliedMatrixSpreadCalculator impliedMatrixSpreadCalculator = new ImpliedMatrixSpreadCalculator();
                var status = impliedMatrixSpreadCalculator.Calculate(dateId, fundId, user, _logger);
                if (status && fundId != -1)
                {
                    using (IRepository repository = new Repository())
                    {
                        repository.GenerateAggregatedPositions();
                    }
                }
                return status;
            }
            catch (Exception exception)
            {
                _logger.Error(exception);

                EmailHelper.SendEmail(exception.ToString(), "Exception has occurred during Frontier Calculation");
                throw;
            }
        }

        public bool SendDataExceptionReportingEmail()
        {
            try
            {
                _logger.Info("SendDataExceptionReportingEmail started");

                IRepository repository = new Repository();
                List<List<DataExceptionObject>> reconReport = repository.GetDataExceptionReporting();
                if (reconReport != null && reconReport.Any())
                {
                    if (reconReport.Count > 0)
                    {
                        _logger.Info("Checking for rows");
                        int count = 0;
                        for (int i = 0; i < reconReport.Count; i++)
                        {
                            if (reconReport[i].Count == 0)
                            {
                                count++;
                            }
                        }

                        if (count == reconReport.Count)
                        {
                            _logger.Info("No Data Exception Found and hence no email will be sent today");
                            return false;
                        }
                    }
                    _logger.Info("Recon Change Found:");
                    WebClient client = new WebClient
                    {
                        Credentials = CredentialCache.DefaultCredentials
                    };
                    _logger.Info("Client URL to get Recon content:" + ConfigurationManager.AppSettings["DataExceptionReportingEmailURL"]);
                    string message = client.DownloadString(ConfigurationManager.AppSettings["DataExceptionReportingEmailURL"]);
                    string subject = ConfigurationManager.AppSettings["DataExceptionReportingEmailSubject"];

                    subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());
                    Console.WriteLine(message);
                    _logger.Info("Email Message to be sent:" + message);

                    var msg = new MailMessage();
                    msg.IsBodyHtml = true;
                    msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));
                    _logger.Info("Email Message object created");

                    var emailTos = ConfigurationManager.AppSettings["DataExceptionReportingToEmailIds"].Split(new char[] { ',', ';' });
                    _logger.Info("Email Message to be sent to:" + emailTos);
                    emailTos.ToList().ForEach(e =>
                    {
                        msg.To.Add(e);
                    });

                    string ccList = ConfigurationManager.AppSettings["DataExceptionReportingCCEmailIds"];
                    _logger.Info("Email Message to be CC:" + ccList);
                    if (!string.IsNullOrEmpty(ccList))
                    {
                        ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                        {
                            msg.CC.Add(e);
                        });
                    }

                    msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                    msg.Subject = subject;
                    msg.Body = message;

                    _logger.Info("SendDataExceptionReportingEmail ~~ message:" + message + "  ;From:" + msg.From + " ;Subject:" + msg.Subject);
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Send(msg);
                    _logger.Info("SendDataExceptionReportingEmail Change Email Sent successfully");
                }
                else
                    _logger.Info("No Data Exception Found and hence no email will be sent today");

                return true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
                _logger.Info("Exeption while sending email message on Moody Recovery Change");
                _logger.Error(exception);
                return false;
            }
        }
    }
}
