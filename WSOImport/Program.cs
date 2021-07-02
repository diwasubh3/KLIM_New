using System;
using log4net;

namespace WSOImport
{
	class Program
	{
		private static ILog _logger = LogManager.GetLogger(typeof(Program));

		static void Main(string[] args)
		{
			log4net.Config.XmlConfigurator.Configure();
			try
			{
				_logger.Info("Starting data processor...");
				WsoDataProcessor.Start();
				_logger.Info("Data processor started.");
				_logger.Info("Starting archive processor...");
				WsoArchiveProcessor.Start();
				_logger.Info("Archive processor started.");
				_logger.Info("Starting scenario processor...");
				WsoScenarioProcessor.Start();
				_logger.Info("Scenario processor started.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}
