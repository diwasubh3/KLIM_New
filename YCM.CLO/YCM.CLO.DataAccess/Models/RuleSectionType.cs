using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class RuleSectionType
    {
        public RuleSectionType()
        {
            this.RuleFields = new List<RuleField>();
        }

        public short RuleSectionTypeId { get; set; }
        public string RuleSectionName { get; set; }
        public virtual ICollection<RuleField> RuleFields { get; set; }
    }
}
