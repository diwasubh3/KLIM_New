using System.Collections.Generic;

namespace YCM.CLO.DTO
{
	public class FieldGroupDto
    {
        public short FieldGroupId { get; set; }
        public string FieldGroupName { get; set; }
        public short? SortOrder { get; set; }
        public string DisplayIcon { get; set; }
        public bool? ShowOnPositions { get; set; }
        public virtual ICollection<FieldDto> Fields { get; set; }
    }
}
