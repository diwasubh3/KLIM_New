using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class GicsToSnpMoodyIndustryMap
    {
        public int Id { get; set; }
        public short? SectorId { get; set; }
        public string GICSIndustryGroup { get; set; }
        public string GICSIndustry { get; set; }
        public string GICSIndustryGrpDesc { get; set; }
        public string GICSIndustryDesc { get; set; }
        public short? MappedSnPIndustryId { get; set; }
        public short? MappedMoodyIndustryId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
