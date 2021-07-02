using System.ServiceModel;
using System.ServiceModel.Web;

namespace YCM.CLO.CalculationEngine.Contracts
{

	[ServiceContract(Namespace = "http://CalculationEngineService")]
    public interface ICalculationEngine
    {
        [OperationContract]
        [WebInvoke(UriTemplate ="Calculate?dateId={dateId}&user={user}",ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool Calculate(int dateId,string user);

        [OperationContract]
        [WebInvoke(UriTemplate = "ProcessConflicts?user={user}", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool ProcessConflicts(string user);

        [OperationContract]
        [WebInvoke(UriTemplate = "ProcessTradeSwap?tradeSwapId={tradeSwapId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool ProcessTradeSwap(int tradeSwapId);

        [OperationContract]
        [WebInvoke(UriTemplate = "ProcessTradeFile?fundId={fundId}&fileDateId={fileDateId}&dateId={dateId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool ProcessTradeFile(int fundId, int fileDateId, int dateId);

        [OperationContract]
        [WebInvoke(UriTemplate = "ProcessTradeFiles?fileDateId={fileDateId}&dateId={dateId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool ProcessTradeFiles(int fileDateId, int dateId);

        [OperationContract]
        [WebInvoke(UriTemplate = "SendPriceMoverEmail", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool SendPriceMoverEmail();

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateStalePositions", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool UpdateStalePositions();


        [OperationContract]
        [WebInvoke(UriTemplate = "SendRatingChangeEmail", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool SendRatingChangeEmail();


        [OperationContract]
        [WebInvoke(UriTemplate = "SendMismatchEmail", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool SendMismatchEmail();


        [OperationContract]
        [WebInvoke(UriTemplate = "CalculateFrontier?dateId={dateId}&fundId={fundId}&user={user}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST")]
        bool CalculateFrontier(int dateId, int fundId, string user);


    }
}
