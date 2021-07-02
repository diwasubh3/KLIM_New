using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class TradeSwap
    {
        public TradeSwap()
        {
            this.TradeSwappingSnapshots = new List<TradeSwapSnapshot>();
        }
        public int TradeSwapId { get; set; }
        public string Parameters { get; set; }
        public int DateId { get; set; }
        public short Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? ProcessStartedOn { get;set; }
        public DateTime? ProcessCompletedOn { get; set; }

        public string Error { get; set; }

        public virtual ICollection<TradeSwapSnapshot> TradeSwappingSnapshots { get; set; }

    }
}
