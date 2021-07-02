namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_Price
    {
        
        public int DateId { get; set; }
        public int SecurityId { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
    }
}
