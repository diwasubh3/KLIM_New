using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public partial class TradeBookingResponse
    {
        public TradeBookingResponse()
        {
        }
        public long ResponseId { get; set; }
        public long TradeGroupId { get; set; }
        public string Response { get; set; }
        public string ResponseStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}
