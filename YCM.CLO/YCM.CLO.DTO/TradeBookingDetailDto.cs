using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DTO
{
    public class TradeBookingDetailDto
    {
        public int TradeBookingDtId { get; set; }
        public int TradeId { get; set; }
        public int PortFolioId { get; set; }
        public string PortfolioName { get; set; }
        public decimal Existing { get; set; }
        public decimal Exposure { get; set; }
        public decimal Allocated { get; set; }
        public decimal Override { get; set; }
        public decimal FinalQty { get; set; }
        public decimal? NetQty { get; set; }
        public decimal TradeAmount { get; set; }
        public string TradeDetailId { get; set; }
        public bool IsOverride { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Price { get; set; }
        public bool IsIncluded { get; set; }
        //public bool IsSkipped { get; set; }
        public decimal? TotalOverride { get; set; }
        public decimal? TotalRemaining { get; set; }
        public string RuleName { get; set; }
        public string ResponseStatus { get; set; }
    }
}
