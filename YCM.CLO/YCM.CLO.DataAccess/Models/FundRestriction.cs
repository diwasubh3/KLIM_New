using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class FundRestriction
    {
        public int Id { get; set; }
        public int FundId { get; set; }
        public short FundRestrictionTypeId { get; set; }
        public short FieldId { get; set; }
        public short OperatorId { get; set; }
        public decimal RestrictionValue { get; set; }
        public virtual Field Field { get; set; }
        public virtual Fund Fund { get; set; }
        public virtual FundRestrictionType FundRestrictionType { get; set; }
        public virtual Operator Operator { get; set; }
		[NotMapped]
		public decimal RestrictionValueCurrent { get; set; }
		[NotMapped]
		public decimal RestrictionValuePrevious { get; set; }

	    public override string ToString()
		    => $"{Fund?.FundCode} {Field?.FieldName} {FundRestrictionType?.FundRestrictionTypeName} {RestrictionValue}";

    }
}
