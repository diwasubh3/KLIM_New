using log4net;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace YCM.CLO.Web.Objects
{
    public class CalculationEngineClient 
    {

        private readonly ILog _logger;

        public CalculationEngineClient()
        {
            _logger = LogManager.GetLogger(typeof(CalculationEngineClient));
        }

        private bool SendToCalculationEngine(string url,object request)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 6000000;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(request);
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = bool.Parse(streamReader.ReadToEnd());
                return result;
            }
        }

        public bool Calculate(int dateId, string user)
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/Calculate?dateId=" + dateId + "&user=" + user;
            var request = new { dateId };
            return SendToCalculationEngine(url, request);
        }



        public bool CalculateFrontier(int dateId, int fundId, string user)
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/CalculateFrontier?dateId=" + dateId + "&user=" + user + "&fundid=" + fundId;
            var request = new { dateId };
            return SendToCalculationEngine(url, request);
        }



        public bool StartTradeSwapping(int tradeSwapId)
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/ProcessTradeSwap?tradeSwapId=" + tradeSwapId;
            var request = new { tradeSwapId };
            return SendToCalculationEngine(url, request);
        }



        public bool ProcessTradeFiles(int fileDateId, int dateId)
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/ProcessTradeFiles?fileDateId=" + fileDateId + "&dateId=" + dateId;
            var request = new { dateId };
            return SendToCalculationEngine(url, request);
        }

        public bool ProcessTradeFile(int fileDateId, int dateId, int fundId)
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/ProcessTradeFile?fileDateId=" + fileDateId + "&dateId=" + dateId + "&fundId=" + fundId;
            var request = new { dateId, fundId };
            return SendToCalculationEngine(url, request);
        }

        public bool SendTotalParChangeEmail()
        {
            try
            {
                
                var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/SendTotalParChangeEmail";
                var request = new { };
                _logger.Info(url);
                return SendToCalculationEngine(url, request);
            }
            catch (Exception ex)
            {
                _logger.Error("Error  on Calculation Client method SendTotalParChangeEmail ", ex);
                return false;
            }
        }

        public bool SendRecoveryChangeEmail(int startDateId, int endDateId)
        {
            try
            {
                _logger.Info(" SendRecoveryChangeEmail called");

                var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/SendRecoveryChangeEmail?startDateId="+startDateId+"&endDateId="+endDateId;
                var request = new { };
                _logger.Info(url);
                return SendToCalculationEngine(url, request);
            }
            catch(Exception ex)
            {
                _logger.Error("Error  on Calculation Client method  SendRecoveryChangeEmail", ex);
                return false;
            }
            
        }

        public bool SendPriceMoverEmail()
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/SendPriceMoverEmail";
            var request = new { };
            return SendToCalculationEngine(url, request);
        }

        public bool SendRatingChangesEmail()
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/SendRatingChangeEmail";
            var request = new { };
            return SendToCalculationEngine(url, request);
        }


        public bool SendMismatchEmail()
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/SendMismatchEmail";
            var request = new { };
            return SendToCalculationEngine(url, request);
        }

        public bool UpdateStalePositions()
        {
            var url = ConfigurationManager.AppSettings["ServiceBaseUrl"] + "/UpdateStalePositions";
            var request = new { };
            return SendToCalculationEngine(url, request);
        }




    }
}
