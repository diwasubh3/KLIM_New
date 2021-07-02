namespace YCM.CLO.DTO
{
	public class FundRestrictionDto
    {
        public int Id { get; set; }
        public int FundId { get; set; }
        public short FundRestrictionTypeId { get; set; }
        public short FieldId { get; set; }
        public short OperatorId { get; set; }
        public decimal RestrictionValue { get; set; }
		public decimal RestrictionValueCurrent { get; set; }
		public decimal RestrictionValuePrevious { get; set; }
		public virtual FundRestrictionTypeDto FundRestrictionType { get; set; }
        public virtual OperatorDto Operator { get; set; }
    }
}
