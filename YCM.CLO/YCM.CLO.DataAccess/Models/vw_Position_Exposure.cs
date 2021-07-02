namespace YCM.CLO.DataAccess.Models
{
	public class vw_Position_Exposure
    {
        public long? PositionId { get; set; }
        public string Exposure { get; set; }
        public decimal? NumExposure { get; set; }
        public int? FundId { get; set; }
        public string FundCode { get; set; }
        public string FundDesc { get; set; }
        public int? SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string Bid { get; set; }
        public string Offer { get; set; }
        public decimal? BidNum { get; set; }
        public decimal? OfferNum { get; set; }
        public string TotalPar { get; set; }
        public decimal? TotalParNum { get; set; }

    }
}
