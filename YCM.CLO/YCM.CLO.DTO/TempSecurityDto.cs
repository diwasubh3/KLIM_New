namespace YCM.CLO.DTO
{
	public class TempSecurityDto
    {
        public int? MoodyFacilityRatingId { get; set; }

        public int? MoodyCashFlowRatingId { get; set; }

        public int? SnPFacilityRatingId { get; set; }

        public int? SnPIssuerRatingId { get; set; }

        public int? MoodyIndustryId { get; set; }

        public int? SnPIndustryId { get; set; }

        public string GICSIndustry { get; set; }

        public int? MoodyRecovery { get; set; }

        public decimal? Spread { get; set; }

        public int? AdjustedWARF { get; set; }

        public string CallDate { get; set; }

        public string MaturityDate { get; set; }

        public LienTypeDto LienType { get; set; }

        public FacilityDto Facility { get; set; }

        public IssuerDto Issuer { get; set; }

        public string SecurityCode { get; set; }

        public string BBgId { get; set; }

    }
}
