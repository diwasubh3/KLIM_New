using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Binder;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace YCM.CLO.TradeFileImporter
{
    class Program
    {

        static public IConfigurationRoot  Configuration { get; set; }

        public AppSettingsConfig AppSettingsConfig { get; set; }
        static void Main(string[] args)
        {
            DateTime dtJob = DateTime.Today;

            if (args.Length > 0)
            {
                var dateIdText =  args[0];
                int dateId;
                if (!string.IsNullOrEmpty(dateIdText) && dateIdText.Trim().Length == 8 &&  int.TryParse(dateIdText, out dateId))
                {
                    dtJob = DateTime.ParseExact(dateIdText, "yyyyMMdd", null);
                }
            }

            var builder = new ConfigurationBuilder()
               .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            

            Configuration = builder.Build();
            var mySettingsConfig = new AppSettingsConfig();
            Configuration.GetSection("AppSettings").Bind(mySettingsConfig);

            Console.WriteLine(mySettingsConfig.FileDirectory);
            Console.WriteLine(mySettingsConfig.FileName);

            try
            {
                using (TradeBlotterContext context = new TradeBlotterContext())
                {
                    TradeBlotterJob tradeBlotterJob = new TradeBlotterJob();
                    tradeBlotterJob.DateId = int.Parse(dtJob.ToString("yyyyMMdd"));
                    tradeBlotterJob.StartedOn = DateTime.Now;
                    tradeBlotterJob.FileName = mySettingsConfig.FileName.Replace("yyyyMMDD", dtJob.ToString("yyyyMMdd"));
                    context.TradeBlotterJobs.Add(tradeBlotterJob);
                    context.SaveChanges();

                    using (var reader = new StreamReader(Path.GetFullPath(Path.Combine(mySettingsConfig.FileDirectory, tradeBlotterJob.FileName))))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            csv.Configuration.HasHeaderRecord = false;
                            csv.Configuration.MissingFieldFound = null;
                            csv.Configuration.HasHeaderRecord = true;
                            csv.Configuration.RegisterClassMap<StageTradeCsvMap>();
                            var records = csv.GetRecords<StageTradeBlotter>();
                            int rownum = 0;
                            foreach (var rec in records)
                            {
                                rec.TradeBlotterJob = tradeBlotterJob;
                                rec.TradeBlotterJobId = tradeBlotterJob.Id;
                                Console.WriteLine($"Row : {rownum++} " + rec.AssetPrimaryId);
                                context.StageTradeBlotters.Add(rec);
                            }
                        }
                    }

                    context.SaveChanges();

                    tradeBlotterJob.CompletedOn = DateTime.Now;
                    context.Entry(tradeBlotterJob).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("EXEC CLO.MergeTradeBlotter " + tradeBlotterJob.Id);

                    Console.WriteLine("Completed!!");
                }
            }
            catch (DbUpdateException exception)
            {
                // Output expected EntityExceptions.
                Logger.Log(exception.ToString());
                Console.WriteLine(exception.ToString());
                if (exception.InnerException != null)
                {
                    // Output unexpected InnerExceptions.
                    Logger.Log(exception.InnerException.ToString());
                    Console.WriteLine(exception.ToString());
                }
                throw;
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logger.Log(exception.Message);
                Console.WriteLine(exception.ToString());
                throw;
            }

        }
    }
}
