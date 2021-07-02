using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Industry
    {
        public Industry()
        {
            this.Securities = new List<Security>();
            this.Securities1 = new List<Security>();
        }

        public short IndustryId { get; set; }
        public string IndustryDesc { get; set; }
        public bool IsSnP { get; set; }
        public bool IsMoody { get; set; }
        public short? MappedSnPIndustryId { get; set; }
        public short? MappedMoodyIndustryId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual ICollection<Security> Securities { get; set; }
        public virtual ICollection<Security> Securities1 { get; set; }
    }
}
