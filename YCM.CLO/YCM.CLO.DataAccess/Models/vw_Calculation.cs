using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_Calculation
    {
        public int CalculationId { get; set; }
        public int SecurityId { get; set; }
        public int FundId { get; set; }
        public decimal? YieldBid { get; set; }
        public decimal? YieldOffer { get; set; }
        public decimal? YieldMid { get; set; }
        public decimal? CappedYieldBid { get; set; }
        public decimal? CappedYieldOffer { get; set; }
        public decimal? CappedYieldMid { get; set; }
        public decimal? TargetYieldBid { get; set; }
        public decimal? TargetYieldOffer { get; set; }
        public decimal? TargetYieldMid { get; set; }
        public decimal? BetterWorseBid { get; set; }
        public decimal? BetterWorseOffer { get; set; }
        public decimal? BetterWorseMid { get; set; }
        public decimal? TotalCoupon { get; set; }
        public decimal? WARF { get; set; }
        public decimal? WARFRecovery { get; set; }
        public decimal? Life { get; set; }
        public decimal? TotalPar { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DateId { get; set; }

        public decimal? MatrixImpliedSpread { get; set; }

    }
}
