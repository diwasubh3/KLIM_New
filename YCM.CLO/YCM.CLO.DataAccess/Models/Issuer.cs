using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Issuer : Entity
    {
        public Issuer()
        {
            this.AnalystResearches = new List<AnalystResearch>();
            this.Securities = new List<Security>();
			AnalystResearchHeaders = new List<AnalystResearchHeader>();
        }

        public int IssuerId { get; set; }
        public string IssuerDesc { get; set; }
        public string IssuerCode { get; set; }
		public bool IsPrivate { get; set; }
        public override DateTime? CreatedOn { get; set; }
        public override string CreatedBy { get; set; }
        public override DateTime? LastUpdatedOn { get; set; }
        public override string LastUpdatedBy { get; set; }
        public virtual ICollection<AnalystResearch> AnalystResearches { get; set; }
        public virtual ICollection<AnalystResearchHeader> AnalystResearchHeaders { get; set; }
        public virtual ICollection<Security> Securities { get; set; }
		[NotMapped]
		public override int Id { get => IssuerId; set => IssuerId = value; }

		public override string ToString()
			=> $"{IssuerId} - {IssuerCode} - Desc: {IssuerDesc}";
	}
}
