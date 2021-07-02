using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_MarketData
    {
        public int DateId { get; set; }
        public long MarketDataId { get; set; }
        public int? OverrideMarketDataId { get; set; }
        public int SecurityId { get; set; }
        public int FundId { get; set; }
        public decimal? Bid { get; set; }
        public decimal? Offer { get; set; }
        public decimal Spread { get; set; }
        public decimal LiborFloor { get; set; }
        public string MoodyCashFlowRating { get; set; }
        public string MoodyCashFlowRatingAdjusted { get; set; }
        public string MoodyFacilityRating { get; set; }
        //public string MoodyFacilityRatingAdjusted { get; set; }
        public decimal MoodyRecovery { get; set; }
        public string SnPIssuerRating { get; set; }
        public string SnPIssuerRatingAdjusted { get; set; }
        public string SnPFacilityRating { get; set; }
        public string SnPfacilityRatingAdjusted { get; set; }
        public string SnPRecoveryRating { get; set; }
        public string MoodyOutlook { get; set; }
        public string MoodyWatch { get; set; }
        public string SnpCreditWatch { get; set; }
        public string SnPWatch { get; set; }
        public DateTime? NextReportingDate { get; set; }
        public DateTime? FiscalYearEndDate { get; set; }
        public string AgentBank { get; set; }
        public short? MoodyFacilityRatingId { get; set; }
        public string MoodyDPRating { get; set; }
        public short? MoodyCashFlowRatingAdjustedId { get; set; }

    }
}