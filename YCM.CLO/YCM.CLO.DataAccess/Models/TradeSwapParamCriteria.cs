namespace YCM.CLO.DataAccess.Models
{
	public class TradeSwapParamCriteria
    {
        public bool SelectAll { get; set; }

        public int FundId { get; set; }

        public bool Cash { get; set; }

        public bool MoodyAdjCfr { get; set; }

        public bool MoodyAdjFacility { get; set; }

        public bool Recovery { get; set; }

        public bool Spread { get; set; }

        public bool ExcludeZeroTotalExposure { get; set; }

        public TradeSwapParamCriteria()
        {
            ExcludeZeroTotalExposure = true;
        }
    }
}
