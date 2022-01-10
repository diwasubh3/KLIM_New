using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Trade
    {
        public Trade()
        {
            this.TradeAllocations = new List<TradeAllocation>();
        }

        public long TradeId { get; set; }
        public int SecurityId { get; set; }
        public int DateId { get; set; }
        public bool? IsBuy { get; set; }
        public decimal? TradeAmount { get; set; }
        public decimal? TradePrice { get; set; }
        public bool? SellAll { get; set; }
        public bool? KeepOnBlotter { get; set; }
        public decimal? BidOfferPrice { get; set; }
        public string Comments { get; set; }
        public bool? IsCancelled { get; set; }
        public decimal? FinalAllocation { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual Security Security { get; set; }
        public virtual ICollection<TradeAllocation> TradeAllocations { get; set; }
    }

    public partial class TradeHistory
    {
        public string TradeDate { get; set; }
        public string TradeType { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Counterparty { get; set; }
    }
}
