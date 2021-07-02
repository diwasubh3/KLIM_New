using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_AnalystResearch
    {
        public long AnalystResearchId { get; set; }
        public int IssuerId { get; set; }
        public int? CLOAnalystUserId { get; set; }
        public int? HFAnalystUserId { get; set; }
        public string CLOAnalyst { get; set; }
        public string HFAnalyst { get; set; }
        public DateTime? AsOfDate { get; set; }
        public decimal? CreditScore { get; set; }
        public decimal? OneLLeverage { get; set; }
        public decimal? TotalLeverage { get; set; }
        public decimal? EVMultiple { get; set; }
        public decimal? LTMRevenues { get; set; }
        public decimal? LTMEBITDA { get; set; }
        public decimal? FCF { get; set; }
        public string Comments { get; set; }

        public string BusinessDescription { get; set; }

        public string AgentBank { get; set; }

        public string IssuerDesc { get; set; }

        public string IssuerCode { get; set; }


    }
}
