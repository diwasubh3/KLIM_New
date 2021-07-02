using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class Rule
    {
        public Rule()
        {
            this.RuleFields = new List<RuleField>();
        }

        public short RuleId { get; set; }
        public string RuleName { get; set; }
        public string ExecutionStoredProcedure { get; set; }
        public short? SortOrder { get; set; }
        public virtual ICollection<RuleField> RuleFields { get; set; }
    }
}
