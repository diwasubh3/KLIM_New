using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class LienType
    {
        public LienType()
        {
            this.Securities = new List<Security>();
        }

        public short LienTypeId { get; set; }
        public string LienTypeDesc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual ICollection<Security> Securities { get; set; }
    }
}
