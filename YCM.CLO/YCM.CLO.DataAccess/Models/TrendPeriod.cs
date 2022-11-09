using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class TrendPeriod
    {
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
