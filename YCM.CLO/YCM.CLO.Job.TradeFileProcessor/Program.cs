using CsvHelper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.Job.TradeFileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            //            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "YCM.CLO.Job.TradeFileProcessor.exe.config"));
            //            var _logger = LogManager.GetLogger(typeof(Program));
            //            IRepository repository = new Repository();  
            //            using (CLOContext context = new CLOContext())
            //            {
            //                _logger.Info("Started On " + DateTime.Now.ToString());
            //                context.Funds.Where(f=>f.CLOFileName != null).ToList().ForEach(fund =>
            //                {
            //                    if (!(fund.IsStale.HasValue && fund.IsStale.Value))
            //                    {
            //                        var clofile =
            //                            Path.Combine(ConfigurationManager.AppSettings["SourceDirectory"], fund.CLOFileName)
            //                                .Replace("{dateid}", Helper.GetPrevDayDateIdBasedOnM2F().ToString());

            //                        var archiveFile =
            //                            Path.Combine(ConfigurationManager.AppSettings["ArchiveDirectory"], fund.CLOFileName)
            //                                .Replace("{dateid}", Helper.GetPrevDayDateId().ToString());

            //                        _logger.Info("Processing Fund : " + fund.FundCode);
            //                        if (File.Exists(clofile))
            //                        {
            //                            using (TextReader fileReader = File.OpenText(clofile))
            //                            {
            //                                var csv = new CsvReader(fileReader);
            //                                var allValues = csv.GetRecords<dynamic>();
            //                                decimal balance = 0;
            //                                _logger.Info("Existing Cash Balance : " + fund.PrincipalCash.Value.ToString());
            //                                allValues.ToList().ForEach(a =>
            //                                {
            //                                    balance += decimal.Parse(a.CostAmount.ToString());
            //                                });

            //                                repository.CaptureDailySnapshot(fund.FundId, Helper.GetPrevDayDateId());
            //                                fund.PrincipalCash = balance;
            //                                fund.IsPrincipalCashStale = false;
            //                                _logger.Info("New Cash Balance : " + fund.PrincipalCash.Value.ToString());
            //                                context.Entry(fund).State = System.Data.Entity.EntityState.Modified;
            //                            }

            //                            MoveFile(clofile, archiveFile);
            //                        }
            //                        else
            //                        {
            //                            fund.IsPrincipalCashStale = true;
            //                            repository.CleanPositionsBasedOnPrincipalCash(fund.FundId,Helper.GetPrevDayDateId());
            //                        }
            //                    }
            //                });
            //                context.SaveChanges();
            //                _logger.Info("Completed On " + DateTime.Now.ToString());
            //            }
            //        }

            //        static void MoveFile(string source,string dest)
            //        {
            //            if (File.Exists(dest))
            //            {
            //                File.Delete(dest);
            //            }
            //            File.Move(source, dest);
        }

    }
}
