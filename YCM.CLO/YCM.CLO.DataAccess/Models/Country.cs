using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Country
    {
        public Country()
        {
            this.Positions = new List<Position>();
        }

        public short CountryId { get; set; }
        public string CountryDesc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
