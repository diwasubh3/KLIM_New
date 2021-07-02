using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            this.AnalystResearches = new List<AnalystResearch>();
            this.AnalystResearches1 = new List<AnalystResearch>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string USerNAme { get; set; }
        public bool? IsCLOAnalyst { get; set; }
        public bool? IsHFAnalyst { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual ICollection<AnalystResearch> AnalystResearches { get; set; }
        public virtual ICollection<AnalystResearch> AnalystResearches1 { get; set; }
    }
}
