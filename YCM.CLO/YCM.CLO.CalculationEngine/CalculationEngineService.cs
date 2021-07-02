using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Xml;
using log4net;
using YCM.CLO.CalculationEngine.Contracts;

namespace YCM.CLO.CalculationEngine
{
	public class CalculationEngineService : IService
    {
        private static WebServiceHost _webHost = null;

        public void Start()
        {
            Console.WriteLine("CLO Service Started");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "YCM.CLO.CalculationEngine.exe.config"));
            _webHost = new WebServiceHost(typeof(CalculationEngine), new Uri(ConfigurationManager.AppSettings["ServiceBaseUrl"]));

            WebHttpBinding binding2 = new WebHttpBinding();
            XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();
            myReaderQuotas.MaxStringContentLength = 2147483647;
            myReaderQuotas.MaxArrayLength = 2147483647;
            myReaderQuotas.MaxBytesPerRead = 2147483647;
            myReaderQuotas.MaxDepth = 2147483647;
            myReaderQuotas.MaxNameTableCharCount = 2147483647;

            binding2.GetType().GetProperty("ReaderQuotas").SetValue(binding2, myReaderQuotas, null);
            binding2.MaxBufferSize = 2147483647;
            binding2.MaxReceivedMessageSize = 2147483647;

            ServiceEndpoint ep = _webHost.AddServiceEndpoint(typeof(ICalculationEngine), binding2, "");
            ServiceDebugBehavior stp = _webHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = true;
            stp.IncludeExceptionDetailInFaults = true;
            _webHost.Open();
        }

        public void Stop()
        {
            Console.WriteLine("Stopping the service ...");
            if (_webHost != null)
            {
                _webHost.Close();
                _webHost = null;
            }
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Info(" CLO Service Stopped");
        }
    }
}
