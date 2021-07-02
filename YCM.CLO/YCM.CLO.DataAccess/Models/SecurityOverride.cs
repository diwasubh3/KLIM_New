using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class SecurityOverride
    {
        public int SecurityOverrideId { get; set; }
        public int SecurityId { get; set; }
        public short FieldId { get; set; }
        public string ExistingValue { get; set; }
        public string OverrideValue { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Comments { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsConflict { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual Field Field { get; set; }
        public virtual Security Security { get; set; }
    }
}
