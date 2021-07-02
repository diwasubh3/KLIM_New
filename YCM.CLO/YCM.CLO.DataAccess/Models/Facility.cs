using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Facility
    {
        public Facility()
        {
            this.Securities = new List<Security>();
        }

        public short FacilityId { get; set; }
        public string FacilityDesc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual ICollection<Security> Securities { get; set; }
    }
}
