namespace YCM.CLO.DTO
{
	public class SecurityReconDto
    {
        public string Source { get; set; }
        public string Code { get; set; }

        public string Facility { get; set; }

        public string LoanDesc { get; set; }
        public int SecurityId { get; set; }
        public string Issuer { get; set; }
        public  string MaturityDate { get; set; }
        public string CreditScore { get; set; }
        public string SearchText => Source + "|" + Code + "|" + Issuer  + "|" + Facility + "|" + MaturityDate + "|" + CreditScore;
    }
}
