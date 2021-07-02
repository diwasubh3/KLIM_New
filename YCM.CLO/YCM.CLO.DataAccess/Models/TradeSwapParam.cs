namespace YCM.CLO.DataAccess.Models
{
	public class TradeSwapParam
    {
        public TradeSwapParamCriteria Criteria { get; set; }

        public TradeSwapParamConstraints Constraints { get; set; }

        public TradeSwapParam()
        {
            Criteria = new TradeSwapParamCriteria();
            Constraints = new TradeSwapParamConstraints();
        }
    }
}
