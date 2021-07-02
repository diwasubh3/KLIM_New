namespace YCM.CLO.DTO
{
	public class PricingDto
    {
        public long PricingId { get; set; }
        public int DateId { get; set; }
        public int SecurityId { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedByFullName { get; set; }
        public string CreatedBy { get; set; }
        public string SecurityCode { get; set; }
        public string Issuer { get; set; }
        public string Facility { get; set; }
        public string SearchText => SecurityCode + "|" + Issuer + "|" + Facility + "|" + Bid + "|" + Offer+ "|" + CreatedOn + "|" + CreatedBy;

    }
}
