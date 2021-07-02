using System;

namespace YCM.CLO.DTO
{
	public class TradeSwapDto
    {
        public int TradeSwapId { get; set; }
        public string Parameters { get; set; }
        public short Status { get; set; }
        public string CreatedBy { get; set; }
        public int FundId { get; set; }
        public int DateId { get; set; }
        public string Error { get; set; }
        
        public DateTime? CreatedOn { get; set; }

        public DateTime? ProcessStartedOn { get; set; }

        public DateTime? ProcessCompletedOn { get; set; }
    }
}
