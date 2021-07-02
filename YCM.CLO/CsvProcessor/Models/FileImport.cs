namespace CsvProcessor.Models
{
	public class FileImport
	{
		public string FileNameMask { get; set; }
		public string TableName { get; set; }
		public string FileLocation { get; set; }
		public string DeleteWhereClause { get; set; }
		public bool HasHeader { get; set; }
		public bool UseDateIdOnFileMask { get; set; }
		public bool UseDateIdOnDeleteClauseMask { get; set; }

		public override string ToString()
			=> $"Name: {TableName} Mask: {FileNameMask} Location: {FileLocation} Has Header: {HasHeader} Where: {DeleteWhereClause} File Mask Date Id: {UseDateIdOnFileMask}";
	}
}
