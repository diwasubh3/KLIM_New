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
            this.tradeType = new TradeTypeDto();
            this.traders = new TraderDto();
            this.facility = new FacilityDto();
            this.counterparty = new CounterPartyDto();
            this.settlemethods = new SettleMethodsDto();
            this.interesttreatments = new InterestTreatmentDto();
            this.allocationRule = new AllocationRuleDto();
            this.TradeBookingDetail = new List<TradeBookingDetailDto>();
        }
        public int Id { get; set; }
        public string TradeId { get; set; }
        public int TradeGroupId { get; set; }
        public DateTime TradeDate { get; set; }
        public int TradeTypeId { get; set; }
        public string TradeTypeDesc { get; set; }
        public int TraderId { get; set; }
        public string TraderName { get; set; }
        public string LoanXId { get; set; }
        public int IssuerId { get; set; }
        public string IssuerDesc { get; set; }
        public short FacilityId { get; set; }
        public string FacilityDesc { get; set; }
        public int CounterPartyId { get; set; }
        public string PartyName { get; set; }
        public int SettleMethodId { get; set; }
        public string SettleMethod { get; set; }
        public int InterestTreatmentId { get; set; }
        public string InterestTreatment { get; set; }
        public decimal Price { get; set; }
        public decimal TotalQty { get; set; }
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string TradeComment { get; set; }
        public string ResponseStatus { get; set; }
        public string ErrorMessage { get; set; }
        public TradeTypeDto tradeType { get; set; }
        public TraderDto traders { get; set; }
        public FacilityDto facility { get; set; }
        public CounterPartyDto counterparty { get; set; }
        public SettleMethodsDto settlemethods { get; set; }
        public InterestTreatmentDto interesttreatments { get; set; }
        public AllocationRuleDto allocationRule { get; set; }
        public virtual IEnumerable<TradeBookingDetailDto> TradeBookingDetail { get; set; }
    }
}
