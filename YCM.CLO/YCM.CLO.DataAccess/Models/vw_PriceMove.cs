namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_PriceMove
    {
        public string Issuer { get; set; }
        public decimal? TotalPar { get; set; }
        public decimal? PriceMove { get; set; }
        public decimal? Bid   { get; set; }
        public decimal? CostPrice { get; set; }
        public string MoodyCFR { get; set; }
    }
}
