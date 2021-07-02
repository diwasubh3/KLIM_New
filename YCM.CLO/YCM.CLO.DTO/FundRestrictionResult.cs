namespace YCM.CLO.DTO
{
	public class FundRestrictionResult
	{
		public int Id { get; set; }
		public int FundId { get; set; }
		public decimal RestrictionValue { get; set; }
		public short OperatorId { get; set; }
		public string FieldName { get; set; }
		public string DisplayColor { get; set; }
		public short FundRestrictionTypeId { get; set; }
		public string FundRestrictionTypeName { get; set; }
		public string FundRestrictionToolTip { get; set; }
		public string FieldTitle { get; set; }
		public short FieldId { get; set; }
		public string OperatorCode { get; set; }
		public string OperatorVal { get; set; }
		public string JsonPropertyName { get; set; }
		public short? SortOrder { get; set; }
		public bool? IsPercentage { get; set; }
		public bool IsDifferenceOverThreshold { get; set; }
        public bool IsDisabled { get; set; }
		public override string ToString()
			=> $"Fund Id: {FundId} Field Name: {FieldName} Rest Type: {FundRestrictionTypeName} Value: {RestrictionValue:C0}";
	}
}
