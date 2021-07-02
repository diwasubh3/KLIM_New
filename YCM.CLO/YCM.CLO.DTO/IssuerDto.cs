using System;

namespace YCM.CLO.DTO
{
    public class IssuerDto
    {
        public int IssuerId { get; set; }
        public string IssuerDesc { get; set; }
		public bool IsPrivate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
