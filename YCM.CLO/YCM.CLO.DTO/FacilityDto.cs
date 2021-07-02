using System;

namespace YCM.CLO.DTO
{
	public class FacilityDto
    {
        public short FacilityId { get; set; }
        public string FacilityDesc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
