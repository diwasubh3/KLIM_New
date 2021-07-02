namespace YCM.CLO.DTO
{
	public class SecurityDto
    {
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        public string BBGId { get; set; }
        public int IssuerId { get; set; }
        public string Issuer { get; set; }
        public short FacilityId { get; set; }
        public string Facility { get; set; }
        public string CallDate { get; set; }
        public short? CountryId    { get; set; }
        public string CountryDesc  { get; set; }
        public string MaturityDate { get; set; }
        public string GICSIndustry { get; set; }
        public short SnPIndustryId { get; set; }
        public string SnpIndustry  { get; set; }
        public short MoodyIndustryId { get; set; }
        public string MoodyIndustry { get; set; }
        public string IsCovLite { get; set; }
        public string IsFloating { get; set; }
        public short LienTypeId { get; set; }
        public string LienType { get; set; }
        
        public string SecurityName { get; set; }
        public string OrigSecurityCode { get; set; }
        public string OrigSecurityDesc { get; set; }
        public string OrigBBGId { get; set; }
        public string OrigIssuer { get; set; }
        public string OrigFacility { get; set; }
        public string OrigCallDate { get; set; }
        public string OrigCountryDesc { get; set; }
        public string OrigMaturityDate { get; set; }
        public string OrigSnpIndustry { get; set; }
        public string OrigMoodyIndustry { get; set; }
        public string OrigIsCovLite { get; set; }
        public string OrigIsFloating { get; set; }
        public string OrigLienType { get; set; }
	}
}
