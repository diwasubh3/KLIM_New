namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_Security_Paydown
    {
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        
        public string Issuer { get; set; }
        
        public int IssuerId { get; set; }
        public int? PaydownId { get; set; }
        public bool? IsOnPaydown { get; set; }
        public short? PaydownObjectTypeId { get; set; }
        public int? PaydownObjectId { get; set; }
        public string PaydownComments { get; set; }
        public string PaydownLastUpdatedOn { get; set; }
        public string PaydownUser { get; set; }
        
    }
}
