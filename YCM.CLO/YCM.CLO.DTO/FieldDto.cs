namespace YCM.CLO.DTO
{
	public class FieldDto
    {
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
        
        public string FieldGroupName { get; set; }
        public bool? IsSecurityOverride { get; set; }

        public bool? ShowInFilter { get; set; }
        public decimal? FilterOrder { get; set; }

		public override string ToString()
			=> $"{FieldName} {Hidden.GetValueOrDefault()} {JsonPropertyName}";
	}
}
