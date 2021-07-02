using System;

namespace YCM.CLO.DTO
{
    public sealed class TradeAllocationDto
    {
        public long TradeAllocationId { get; set; }
        public long TradeId { get; set; }
        public decimal? CurrentAllocation { get; set; }
        public decimal? NewAllocation { get; set; }

        public decimal? TradeCash { get; set; }

        public decimal? FinalAllocation { get; set; }
        public int FundId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public FundDto Fund { get; set; }

        public string SearchText { get; set; }

        public bool IsVisible { get { return true; } }

        public string Bid { get; set; }

        public string Offer { get; set; }

        public void ProcessSearchText (TradeDto trade)
        {
            TradeCash = NewAllocation * trade.TradePrice/100;
            SearchText = trade.SearchText  +"|" + (Fund.FundCode + "|" + NewAllocation + "|" + TradeCash + "|" + FinalAllocation).ToLower();
        }


    }
}
