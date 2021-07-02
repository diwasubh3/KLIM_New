using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class Field
    {
        public Field()
        {
            this.FundRestrictions = new List<FundRestriction>();
            this.RuleFields = new List<RuleField>();
            this.SecurityOverrides = new List<SecurityOverride>();
			CustomViewFields = new List<CustomViewField>();
        }

        public short FieldId { get; set; }
        public short FieldGroupId { get; set; }
        public string FieldName { get; set; }
        public string JsonPropertyName { get; set; }
        public string FieldTitle { get; set; }
        public string JsonFormatString { get; set; }
        public int? DisplayWidth { get; set; }
        public bool? IsPercentage { get; set; }
        public decimal? SortOrder { get; set; }
        public short? FieldType { get; set; }
        public string HeaderCellClass { get; set; }
        public string CellClass { get; set; }
        public string CellTemplate { get; set; }
        public bool? Hidden { get; set; }
        public bool? PinnedLeft { get; set; }
        public bool? IsSecurityOverride { get; set; }
        public bool? ShowInFilter { get; set; }
        public decimal? FilterOrder { get; set; }

        public virtual FieldGroup FieldGroup { get; set; }
        public virtual ICollection<FundRestriction> FundRestrictions { get; set; }
        public virtual ICollection<RuleField> RuleFields { get; set; }
        public virtual ICollection<SecurityOverride> SecurityOverrides { get; set; }
	    public virtual ICollection<CustomViewField> CustomViewFields { get; set; }

	    public override string ToString()
		    =>
			    $"Id: {FieldId} Name: {FieldName} Grp: {FieldGroupId} Sort: {SortOrder} Hide: {Hidden.GetValueOrDefault()} Pin: {PinnedLeft}";
    }
}
