using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class TradeSwapSnapshot
    {
        public long TradeSwapSnapshotId { get; set; }
        public int TradeSwapId { get; set; }
        public int SellSecurityId { get; set; }
        public int SellFundId { get; set; }
        public decimal? SellExposure { get; set; }
        public decimal? SellTotalExposure { get; set; }
        public decimal? SellSecurityBidPrice { get; set; }
        public decimal? SellPctPosition { get; set; }
        public decimal? SellSpread { get; set; }
        public decimal? SellLiquidityScore { get; set; }
        public DateTime? SellMaturityDate { get; set; }

        public string SellMoodyAdjCFR { get; set; }
        public string SellMoodyAdjFacility { get; set; }

        public string SellIssuer { get; set; }
        public string SellFacility { get; set; }
        public int BuySecurityId { get; set; }
        public decimal? BuySecurityOfferPrice { get; set; }
        public int BuyFundId { get; set; }
        public decimal? BuyExposure { get; set; }
        public decimal? BuyTotalExposure { get; set; }
        public decimal? BuyPctPosition { get; set; }
        public decimal? BuySpread { get; set; }
        public decimal? BuyLiquidityScore { get; set; }
        public DateTime? BuyMaturityDate { get; set; }
        public string BuyIssuer { get; set; }
        public string BuyFacility { get; set; }
        public DateTime? CreatedOn { get; set; }
        public virtual Fund SellFund { get; set; }
        public virtual Fund BuyFund { get; set; }
        public virtual Security BuySecurity { get; set; }
        public virtual Security SellSecurity { get; set; }
        public string BuyMoodyAdjCFR { get; set; }
        public string BuyMoodyAdjFacility { get; set; }
        public virtual TradeSwap TradeSwap { get; set; }

        public decimal? SellRecovery { get; set; }
        public decimal? BuyRecovery { get; set; }

        public decimal? SellYield { get; set; }
        public decimal? BuyYield { get; set; }

        public decimal? BuySecurityBidPrice { get; set; }
        public decimal? SellSecurityOfferPrice { get; set; }

        public decimal? BuySecurityCreditScore { get; set; }
        public decimal? SellSecurityCreditScore { get; set; }

    }
}
