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
        public int Id { get; set; }
        public string TradeId { get; set; }
        public int TradeGroupId { get; set; }
        public DateTime TradeDate { get; set; }
        public int TradeTypeId { get; set; }
        public string TradeTypeDesc { get; set; }
        public int TraderId { get; set; }
        public string LoanXId { get; set; }
        public int IssuerId { get; set; }
        public int FacilityId { get; set; }
        public int CounterPartyId { get; set; }
        public int SettleMethodId { get; set; }
        public string SettleMethod { get; set; }
        public int InterestTreatmentId { get; set; }
        public string InterestTreatment { get; set; }
        public decimal Price { get; set; }
        public decimal TotalQty { get; set; }
        public int RuleId { get; set; }
        public string TradeComment { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }

        public bool Cancel { get; set; }
        public int CancelId { get; set; }
        public bool UpdateFlag { get; set; }
        public int UpdateId { get; set; }

        public virtual ICollection<TradeBookingDetail> TradeBookingDetail { get; set; }
    }
}
