using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class FundCalculation
    {
        public int FundCalculationId { get; set; }
        public int DateId { get; set; }
        public int FundId { get; set; }
        public decimal? Par { get; set; }
        public decimal? BODPar { get; set; }
        public decimal? Spread { get; set; }
        public decimal? BODSpread { get; set; }
        public decimal? TotalCoupon { get; set; }
        public decimal? BODTotalCoupon { get; set; }
        public decimal? WARF { get; set; }
        public decimal? BODWARF { get; set; }
        public decimal? MoodyRecovery { get; set; }
        public decimal? BODMoodyRecovery { get; set; }
        public decimal? Bid { get; set; }
        public decimal? BODBid { get; set; }
        public decimal? PrincipalCash { get; set; }
        public decimal? BODPrincipalCash { get; set; }
        public decimal? Diversity { get; set; }
        public decimal? BODDiversity { get; set; }
        public decimal? CleanNav { get; set; }
        public decimal? BODCleanNav { get; set; }
        public decimal? WAMaturityDays { get; set; }
        public decimal? BODWAMaturityDays { get; set; }
        public decimal? AssetPar { get; set; }
        public decimal? PriorDayExposure { get; set; }
        public decimal? PriorDayPrincipalCash { get; set; }
        public decimal? MatrixImpliedSpread { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual Fund Fund { get; set; }

        public decimal? BBMVOC { get; set; }

        public decimal? WALCushion { get; set; }

        public decimal? TimeToReinvest { get; set; }


        public decimal? B3ToAssetParPct { get; set; }

        public decimal? BMinusToAssetParPct { get; set; }


    }
}
