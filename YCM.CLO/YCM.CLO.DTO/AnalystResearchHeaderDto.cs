using System;

namespace YCM.CLO.DTO
{
	public class AnalystResearchHeaderDto
	{
		public int AnalystResearchHeaderId { get; set; }
		public int IssuerId { get; set; }
		public string BusinessDescription { get; set; }
		public int? CLOAnalystId { get; set; }
		public int? HFAnalystId { get; set; }
		public decimal? CreditScore { get; set; }
		public string AgentBank { get; set; }
		public string CLOAnalyst { get; set; }
		public string HFAnalyst { get; set; }
		public string Issuer { get; set; }
		public DateTime LastUpdatedOn { get; set; }
		public string Sponsor { get; set; }
        public string LiborCategory { get; set; }

        public string LiborTransitionNote { get; set; }

        public override string ToString()
			=> $"{AnalystResearchHeaderId} - ({IssuerId}){Issuer} ({CLOAnalystId}){CLOAnalyst} ({HFAnalystId}){HFAnalyst}";
	}
}
