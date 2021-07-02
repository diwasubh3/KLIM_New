namespace YCM.CLO.DataAccess.Models
{
	public partial class Summary
    {
        public string SecurityCode { get; set; }
        public decimal Par { get; set; }
        public decimal? Spread { get; set; }
        public decimal? TotalCoupon { get; set; }
        public decimal? WARF { get; set; }
        public decimal? MoodyRecovery { get; set; }
        public decimal? Bid { get; set; }
        public decimal PrincipalCash { get; set; }
    }
}
