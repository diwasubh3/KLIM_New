using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DTO
{
    public class TradeBookingDto
    {
        public TradeBookingDto()
        {
            this.TradeBookingDetail = new List<TradeBookingDetailDto>();
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
        public string IssuerDesc { get; set; }
        public int FacilityId { get; set; }
        public int CounterPartyId { get; set; }
        public int SettleMethodId { get; set; }
        public int InterestTreatmentId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalQty { get; set; }
        public int RuleId { get; set; }
        public string TradeComment { get; set; }
        public virtual ICollection<TradeBookingDetailDto> TradeBookingDetail { get; set; }
    }
}
