using System.Data;
using static ExportCsv.ExtensionMethods.IDataRecordExtensionMethods;

namespace ExportCsv.Models
{
	public class FileImportColumnMap
	{
		public static FileImportColumnMap CreateNewColumnMap(IDataRecord record)
		=> new FileImportColumnMap
		{
			Id = record.GetIntValueOrDefault(nameof(Id))
			, FileImportId = record.GetIntValueOrDefault(nameof(FileImportId))
			, TableColumnName = record[nameof(TableColumnName)].ToString()
			, FileColumnName = record[nameof(FileColumnName)].ToString()
			, FileColumnIndex = record.GetIntValueOrDefault(nameof(FileColumnIndex))
			, FileImportColumnTypeId = record.GetIntValueOrDefault(nameof(FileImportColumnTypeId))
		};

		public int Id { get; set; }
		public int FileImportId { get; set; }
		public string TableColumnName { get; set; }
		public string FileColumnName { get; set; }
		public int? FileColumnIndex { get; set; }
		public int FileImportColumnTypeId { get; set; }

		public override string ToString()
			=> $"Import Id: {FileImportId} Name: {TableColumnName} Type: {FileImportColumnTypeId} Col Idx: {FileColumnIndex} Col Name: {FileColumnName}";
	}
}
