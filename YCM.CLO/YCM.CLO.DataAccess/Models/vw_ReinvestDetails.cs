namespace YCM.CLO.DataAccess.Models
{
    public class vw_ReinvestDetails
	{
		public int id { get; set; }
		public int FundId { get; set; }
		public string FundCode { get; set; }
		public string FilePath { get; set; }
		public string FileName { get; set; }
		public string SheetName { get; set; }
		public string FieldLocation { get; set; }
	}
}
