using System.Collections.Generic;

namespace YCM.CLO.DTO
{
	public class RuleDto
    {
        public short RuleId { get; set; }
        public string RuleName { get; set; }
        public short? SortOrder { get; set; }
        public virtual ICollection<RuleFieldDto> RuleFields { get; set; }
    }
}
