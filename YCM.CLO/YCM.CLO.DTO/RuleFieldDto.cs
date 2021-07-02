namespace YCM.CLO.DTO
{
	public class RuleFieldDto
    {

        public int RuleFieldId { get; set; }
        public short RuleId { get; set; }
        public short? RuleSectionTypeId { get; set; }
        public short FieldId { get; set; }
        public short? SortOrder { get; set; }
        public virtual FieldDto Field { get; set; }
        //public virtual RuleDto Rule { get; set; }
        public virtual RuleSectionTypeDto RuleSectionType { get; set; }
    }
}
