using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{

	public partial class AnalystResearchHeader : Entity
	{
        public AnalystResearchHeader()
        {
            AnalystResearchDetails = new List<AnalystResearchDetail>();
        }

		[NotMapped]
		public override int Id
		{
			get { return AnalystResearchHeaderId; }
			set { AnalystResearchHeaderId = value; }
		}

		public int AnalystResearchHeaderId { get; set; }

        public int IssuerId { get; set; }

        public string BusinessDescription { get; set; }

        public int? CLOAnalystId { get; set; }

        public int? HFAnalystId { get; set; }

        public decimal? CreditScore { get; set; }

        public string AgentBank { get; set; }
		public string Sponsor { get; set; }

        public virtual Issuer Issuer { get; set; }

        public virtual ICollection<AnalystResearchDetail> AnalystResearchDetails { get; set; }

		public override string ToString()
			=> $"Id: {AnalystResearchHeaderId} Issuer Id: {IssuerId}";


        public string LiborCategory { get; set; }

        public string LiborTransitionNote { get; set; }

    }
}
