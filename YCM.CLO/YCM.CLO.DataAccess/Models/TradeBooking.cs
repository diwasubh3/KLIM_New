using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public partial class TradeBooking
    {
        public TradeBooking()
        {
            TradeBookingDetail = new List<TradeBookingDetail>();
        }
        public long TradeId { get; set; }
        public short TradeGroupId { get; set; }
        public DateTime TradeDate { get; set; }
        public short TradeTypeId { get; set; }
        public short TraderId { get; set; }
        public short LoanXId { get; set; }
        public short IssuerId { get; set; }
        public short FacilityId { get; set; }
        public short CounterPartyId { get; set; }
        public short SettleMethodId { get; set; }
        public short InterestTreatmentId { get; set; }
        public decimal Price { get; set; }
        public int TotalQty { get; set; }
        public short RuleId { get; set; }
        public string TradeComment { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual ICollection<TradeBookingDetail> TradeBookingDetail { get; set; }
    }
}
