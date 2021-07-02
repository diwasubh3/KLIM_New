namespace YCM.CLO.DTO
{
	public class CustomViewFieldDto
	{
		public int CustomViewFieldId { get; set; }

		public int ViewId { get; set; }

		public short FieldId { get; set; }
		public string FieldTitle { get; set; }
		public int FieldGroupId { get; set; }

		public int SortOrder { get; set; }
		public bool IsHidden { get; set; }
	}
}
