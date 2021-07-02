using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class AnalystResearchHeaderHistory : Entity
	{
		public AnalystResearchHeaderHistory(AnalystResearchHeader header
			, DatabaseOperation operation)
		{
			AnalystResearchHeaderId = header.AnalystResearchHeaderId;
			IssuerId = header.IssuerId;
			BusinessDescription = header.BusinessDescription;
			CLOAnalystId = header.CLOAnalystId;
			HFAnalystId = header.HFAnalystId;
			CreditScore = header.CreditScore;
			AgentBank = header.AgentBank;
			CreatedOn = header.CreatedOn;
			CreatedBy = header.CreatedBy;
			LastUpdatedOn = header.LastUpdatedOn;
			LastUpdatedBy = header.LastUpdatedBy;
			Operation = operation.ToString();
		}
		[NotMapped]
		public override int Id
		{
			get { return AnalystResearchHeaderHistoryId; }
			set { AnalystResearchHeaderHistoryId = value; }
		}

		public int AnalystResearchHeaderHistoryId { get; set; }
		public int AnalystResearchHeaderId { get; set; }

		public int IssuerId { get; set; }

		public string BusinessDescription { get; set; }

		public int? CLOAnalystId { get; set; }

		public int? HFAnalystId { get; set; }

		public decimal? CreditScore { get; set; }

		public string AgentBank { get; set; }

		public string Operation { get; set; }
		
        public override string ToString()
			=> $"Id: {AnalystResearchHeaderId} Issuer Id: {IssuerId}";

        public string LiborCategory { get; set; }

        public string LiborTransitionNote { get; set; }

    }
}
