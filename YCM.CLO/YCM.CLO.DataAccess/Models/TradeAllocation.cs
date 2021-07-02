using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class TradeAllocation
    {
        public long TradeAllocationId { get; set; }
        public long TradeId { get; set; }
        public decimal? CurrentAllocation { get; set; }
        public decimal? NewAllocation { get; set; }
        public int FundId { get; set; }
        public decimal? FinalAllocation { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual Fund Fund { get; set; }
        public virtual Trade Trade { get; set; }
    }
}
