using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class MoodyRecoveryChange
    {
        public string Fund { get; set; }

        public decimal? OldRecovery { get; set; }
        public decimal? NewRecovery { get; set; }
    }
}
