using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class MarketData
    {
        public long MarketDataId { get; set; }
        public int DateId { get; set; }
        public int SecurityId { get; set; }
        public int FundId { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public decimal Spread { get; set; }
        public decimal LiborFloor { get; set; }
        public short MoodyCashFlowRatingId { get; set; }
        public short MoodyCashFlowRatingAdjustedId { get; set; }
        public short? MoodyFacilityRatingId { get; set; }
        
        public decimal MoodyRecovery { get; set; }
        public short? SnPIssuerRatingId { get; set; }
        public short? SnPIssuerRatingAdjustedId { get; set; }
        public short? SnPFacilityRatingId { get; set; }
        public short? SnPfacilityRatingAdjustedId { get; set; }
        public short? SnPRecoveryRatingId { get; set; }
        public string MoodyOutlook { get; set; }
        public string MoodyWatch { get; set; }
        public string SnPWatch { get; set; }
        public DateTime? NextReportingDate { get; set; }
        public DateTime? FiscalYearEndDate { get; set; }
        public string AgentBank { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual Fund Fund { get; set; }
        public virtual Rating Rating { get; set; }
        public virtual Rating Rating1 { get; set; }
        public virtual Rating Rating3 { get; set; }
        public virtual Security Security { get; set; }
        public virtual Rating Rating4 { get; set; }
        public virtual Rating Rating5 { get; set; }
        public virtual Rating Rating6 { get; set; }
        public virtual Rating Rating7 { get; set; }
        public virtual Rating Rating8 { get; set; }
    }
}
