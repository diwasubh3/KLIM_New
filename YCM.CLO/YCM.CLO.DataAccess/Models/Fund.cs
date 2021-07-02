using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Fund
    {
        public Fund()
        {
            this.Calculations = new List<Calculation>();
            this.FundRestrictions = new List<FundRestriction>();
            this.MarketDatas = new List<MarketData>();
            this.Positions = new List<Position>();
            this.TradeAllocations = new List<TradeAllocation>();
            this.SellTradeSwappingSnapshots = new List<TradeSwapSnapshot>();
            this.BuyTradeSwappingSnapshots = new List<TradeSwapSnapshot>();
        }

        public int FundId { get; set; }
        public string FundCode { get; set; }
        public string FundDesc { get; set; }
        public DateTime? WSOLastUpdatedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string   LastUpdatedBy   { get; set; }
        public decimal? PrincipalCash   { get; set; }
        public decimal? LiabilityPar    { get; set; }
        public decimal? EquityPar       { get; set; }
        public decimal? TargetPar { get; set; }
        public decimal? RecoveryMultiplier { get; set; }
        public decimal? WALifeAdjustment { get; set; }
		public decimal? AssetParPercentageThreshold { get; set; }
	    public decimal? WSOSpread { get; set; }
	    public decimal? WSOWARF { get; set; }
	    public decimal? WSOMoodyRecovery { get; set; }
	    public decimal? WSOWALife { get; set; }
	    public decimal? WSODiversity { get; set; }

		public decimal? AssetParThreshold => TargetPar * AssetParPercentageThreshold / 100;
		public string CLOFileName { get; set; }
        public bool? IsStale { get; set; }

        public bool? IsWareHouse { get; set; }

        public bool? IsPrincipalCashStale { get; set; }
        public string DisplayText { get; set; }
        public decimal? WALWARFAdj { get; set; }
        public decimal? MaxWarfTrigger { get; set; }

        public decimal? ClassEPar { get; set; }
        public DateTime? WALDate { get; set; }

        public DateTime? ReInvestEndDate { get; set; }

        public decimal? WalDateAdj { get; set; }

        
        public decimal? ProjectedEquityDistribtion { get; set; }

        public string BloombergCode { get; set; }

        public DateTime? PricingDate { get; set; }

        public decimal? MgmtFees { get; set; }

        public decimal? OperatingExpenses { get; set; }

        public DateTime? ClosingDate { get; set; }

        public DateTime? NonCallEndsDate { get; set; }

        public DateTime? FinalMaturity { get; set; }




        public virtual ICollection<Calculation> Calculations { get; set; }

        public virtual ICollection<FundCalculation> FundCalculations { get; set; }
        public virtual ICollection<FundRestriction> FundRestrictions { get; set; }
        public virtual ICollection<MarketData> MarketDatas { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<TradeAllocation> TradeAllocations { get; set; }
        public virtual ICollection<TradeSwapSnapshot> SellTradeSwappingSnapshots { get; set; }
        public virtual ICollection<TradeSwapSnapshot> BuyTradeSwappingSnapshots { get; set; }
        
        public bool IsActive { get; set; }

	    public override string ToString()
		    => $"{FundId} - {FundCode} Active: {IsActive} Stale: {IsStale} Cash Stale: {IsPrincipalCashStale}";

        public bool? CanFilter { get; set; }


    }
}
