using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class Pricing
    {
        public long PricingId { get; set; }
        public int DateId { get; set; }
        public int SecurityId { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual Security Security { get; set; }
    }
}
