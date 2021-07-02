using System.Web.Mvc;
using YCM.CLO.DataAccess;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;

namespace YCM.CLO.Web.Controllers
{
	[AllowAnonymous]
    public class CalculationController : Controller
    {
        // GET: Calculation
        public JsonNetResult Process(int? dateId)
        {
            dateId = dateId ?? Helper.GetPrevDayDateId();
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            //CalcilationEngineController calculationEngineClient = new CalcilationEngineController();
            var status = calculationEngineClient.Calculate(dateId.Value, "System");
            AllPositionsCache allPositionsCache = new AllPositionsCache();
            allPositionsCache.Invalidate();
            
            return new JsonNetResult() { Data = new {status } };
        }


        public JsonNetResult CalculateFrontier(int? dateId)
        {
            dateId = dateId ?? Helper.GetPrevDayDateId();
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            var status = calculationEngineClient.CalculateFrontier(dateId.Value,-1, "System");
            AllPositionsCache allPositionsCache = new AllPositionsCache();
            allPositionsCache.Invalidate();

            return new JsonNetResult() { Data = new { status } };
        }


        public JsonNetResult SendPriceMoverEmail()
        {
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            return new JsonNetResult() { Data = new { status = calculationEngineClient.SendPriceMoverEmail() } };
        }

        public JsonNetResult SendRatingsChangeEmail()
        {
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            return new JsonNetResult() { Data = new { status = calculationEngineClient.SendRatingChangesEmail() } };
        }

        public JsonNetResult SendMismatchEmail()
        {
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            return new JsonNetResult() { Data = new { status = calculationEngineClient.SendMismatchEmail() } };
        }

        public JsonNetResult ProcessTradeFiles(int? dateId)
        {
            dateId = dateId ?? Helper.GetPrevDayDateId();
            int fileDateId = Helper.GetPrevDayDateIdBasedOnM2F();
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            return new JsonNetResult() { Data = new { status = calculationEngineClient.ProcessTradeFiles(fileDateId, dateId.Value) } };
        }

        public JsonNetResult ProcessTradeFile(int? dateId,int fundId)
        {
            dateId = dateId ?? Helper.GetPrevDayDateId();
            int fileDateId = Helper.GetPrevDayDateIdBasedOnM2F();
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            return new JsonNetResult() { Data = new { status = calculationEngineClient.ProcessTradeFile(fileDateId, dateId.Value,fundId) } };
        }

        public JsonNetResult RefreshStalePositions()
        {
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            return new JsonNetResult() { Data = new { status = calculationEngineClient.UpdateStalePositions() } };
        }
    }
}