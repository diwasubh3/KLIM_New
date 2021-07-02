namespace YCM.CLO.DataAccess.Models
{
	public partial class RuleField
    {
        public int RuleFieldId { get; set; }
        public short RuleId { get; set; }
        public short? RuleSectionTypeId { get; set; }
        public short FieldId { get; set; }
        public short? SortOrder { get; set; }
        public virtual Field Field { get; set; }
        public virtual Rule Rule { get; set; }
        public virtual RuleSectionType RuleSectionType { get; set; }
    }
}
