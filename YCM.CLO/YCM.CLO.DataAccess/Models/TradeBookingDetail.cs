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

        public long TradeBookingDtId { get; set; }
        public long TradeId { get; set; }
        public short PortFolioId { get; set; }
        public decimal Existing { get; set; }
        public decimal Allocated { get; set; }
        public decimal Override { get; set; }
        public decimal FinalQty { get; set; }
        public bool IsActive { get; set; }
    }
}
