namespace YCM.CLO.DTO
{
	public class AnalystResearchDto
    {
        public long AnalystId { get; set; }
        public int IssuerId { get; set; }
        public int SecurityId { get; set; }
        public string CLOAnalyst { get; set; }
        public string HFAnalyst { get; set; }
        public string AsOfDate { get; set; }
        public decimal? CreditScore { get; set; }
        public decimal? OneLLeverage { get; set; }
        public decimal? TotalLeverage { get; set; }
        public decimal? EVMultiple { get; set; }
        public decimal? LTMRevenues { get; set; }
        public decimal? LTMEBITDA { get; set; }
        public decimal? FCF { get; set; }
        public string Comments { get; set; }

        public string BusinessDescription { get; set; }
        public string Issuer { get; set; }
        public int CLOAnalystUserId { get; set; }
        public int? HFAnalystUserId { get; set; }
        public string LastUpdatedOn => AsOfDate;
        public int AnalystResearchId { get; set; }
        public string AgentBank { get; set; }
        public bool IsVisible =>  true;
        public string SearchText => (CLOAnalyst + "|" + HFAnalyst?.ToString() + "|" + AsOfDate
                                    + "|" + CreditScore?.ToString() + "|" + OneLLeverage?.ToString() +
                                    "|" + TotalLeverage?.ToString() + "|" + EVMultiple?.ToString() +
                                    "|" + LTMRevenues?.ToString() + "|" + LTMEBITDA?.ToString()
                                    + "|" + AgentBank?.ToString()
                                    + "|" + FCF?.ToString() + "|" + Comments?.ToString() + "|" + BusinessDescription?.ToString() + "|" + Issuer).ToLower();


    }  
}
