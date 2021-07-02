namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_Security_Watch
    {
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        
        public string Issuer { get; set; }
        
        public int IssuerId { get; set; }
        public int? WatchId { get; set; }
        public bool? IsOnWatch { get; set; }
        public short? WatchObjectTypeId { get; set; }
        public int? WatchObjectId { get; set; }
        public string WatchComments { get; set; }
        public string WatchLastUpdatedOn { get; set; }
        public string WatchUser { get; set; }
        
    }
}
