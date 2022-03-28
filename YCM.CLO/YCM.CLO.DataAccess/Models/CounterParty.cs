using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public partial class CounterParty
    {
        public CounterParty()
        {
        }

        public short PartyId { get; set; }
        public string PartyName { get; set; }
        public int WSOId { get; set; }
        public string MEI { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}

