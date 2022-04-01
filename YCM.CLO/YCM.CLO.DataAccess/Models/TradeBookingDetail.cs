using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public partial class TradeBookingDetail
    {
        public TradeBookingDetail()
        {
        }

        public int TradeBookingDtId { get; set; }
        public int TradeId { get; set; }
        public int PortFolioId { get; set; }
        public string PortfolioName { get; set; }
        public decimal Existing { get; set; }
        public decimal Allocated { get; set; }
        public decimal Override { get; set; }
        public decimal FinalQty { get; set; }
        public decimal TradeAmount { get; set; }
        public string TradeDetailId { get; set; }
        public bool IsActive { get; set; }
    }
}
