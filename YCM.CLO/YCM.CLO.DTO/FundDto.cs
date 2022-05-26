using System;

namespace YCM.CLO.DTO
{
    public class FundDto
    {
        public int FundId { get; set; }
        public string FundCode { get; set; }
        public string FundDesc { get; set; }
		public decimal? AssetParPercentageThreshold { get; set; }
        public bool? IsWareHouse { get; set; }
        public decimal? AssetParThreshold => AssetParPercentageThreshold * TargetPar / 100;
		public DateTime? WSOLastUpdatedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public decimal? PrincipalCash { get; set; }
        public decimal? LiabilityPar { get; set; }
        public decimal? EquityPar { get; set; }
        public decimal? TargetPar { get; set; }
        public decimal? RecoveryMultiplier { get; set; }
        public decimal? WALifeAdjustment { get; set; }
	    public decimal? WSOSpread { get; set; }
	    public decimal? WSOWARF { get; set; }
	    public decimal? WSOMoodyRecovery { get; set; }
	    public decimal? WSOWALife { get; set; }
	    public decimal? WSODiversity { get; set; }

        public string CLOFileName { get; set; }
        public bool? IsStale { get; set; }
        public string DisplayText { get; set; }

        public decimal? WALWARFAdj { get; set; }

        public decimal? MaxWarfTrigger { get; set; }

        public decimal? ClassEPar { get; set; }
        public string WALDate { get; set; }

        public string ReInvestEndDate { get; set; }

        public decimal? WalDateAdj { get; set; }
        
        public bool IsActive { get; set; }

        public bool? CanFilter { get; set; }

        public decimal? ProjectedEquityDistribtion { get; set; }

        public string BloombergCode { get; set; }

        public string PricingDate { get; set; }

        public decimal? MgmtFees { get; set; }

        public decimal? OperatingExpenses { get; set; }

        public string ClosingDate { get; set; }

        public string NonCallEndsDate { get; set; }

        public string FinalMaturity { get; set; }
        public string PortfolioName { get; set; }
        public Int32 PortFolioId { get; set; }

    }
}
