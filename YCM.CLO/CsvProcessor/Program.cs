using CsvProcessor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using static CsvProcessor.ConfigurationConstants;
using static CsvProcessor.ExtensionMethods.DateTimeExtensionMethods;
using static CsvProcessor.ExtensionMethods.IDataReaderExtensionMethods;
using static CsvProcessor.ExtensionMethods.IDataRecordExtensionMethods;
using static CsvProcessor.Utilities;
using System.IO.Compression;

namespace CsvProcessor
{
	class Program
	{
		private readonly static NLog.ILogger _logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

		private static ImportArgument _argument;

		static int Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.Error.WriteLine("Import Id parameter expected.");
				return Environment.ExitCode = (int)ExitCode.MissingParameter;
			}
			_argument = new ImportArgument(args);
			_logger.Info(_argument);

			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Yoda"].ConnectionString;

			Process(_argument, connectionString);
			return (int)ExitCode.Success;
		}

		private static void Process(ImportArgument argument, string connectionString)
		{
			if (argument.OpType == OperationType.MetadataLoad)
				PopulateMetaData(_argument, connectionString);
			else if (argument.OpType == OperationType.DataLoad)
				ImportData(_argument, connectionString);
		}

		/// <summary>
		/// Create a ZIP file of the files provided.
		/// </summary>
		/// <param name="fileName">The full path and name to store the ZIP file at.</param>
		/// <param name="files">The list of files to be added.</param>
		public static void CreateZipFile(string fileName, List<string> files)
		{
			if (files == null || files.Count == 0)
			{
				_logger.Info($"Nothing to archive.  Files == null? {files == null} File count: {files?.Count}");
				return;
			}
			// Create and open a new ZIP file
			var archiveMode = File.Exists(fileName) ? ZipArchiveMode.Update : ZipArchiveMode.Create;
			_logger.Info($"Archive mode: {archiveMode}");
			using (var zip = ZipFile.Open(fileName, archiveMode))
				foreach (var file in files)
				{
					if (archiveMode == ZipArchiveMode.Create || !zip.Entries.Any(x => x.Name == Path.GetFileName(file)))
					{
						_logger.Info($"Adding {file} to {fileName}...");
						zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
						_logger.Info($"{file} added.");
					}
					else
						_logger.Info($"File already in the archive? {!zip.Entries.Any(x => x.Name == Path.GetFileName(file))}");
					_logger.Info($"Deleting file {file}...");
					File.Delete(file);
					_logger.Info($"{file} deleted.");
				}
		}

		private static void ArchiveFiles(string folder)
		{
			if (!ConfigurationConstants.ArchiveFiles)
				return;
			_logger.Info("Starting archiving process.");
			var today = DateTime.Now;
			var currentMonthStartDate = CurrentMonthStartDate;
			var currentMonthEndDate = CurrentMonthEndDate;
			var lastMonthEndDate = PreviousMonthEndDate;
			var archiveFolder = Path.Combine(@folder, "Archive");
			_logger.Info($"Saving zip file(s) to {archiveFolder}");
			_logger.Info($"Archiving files from last month. Previous month end date: {lastMonthEndDate}");
			GetOldFilesAndZip(folder, archiveFolder, currentMonthStartDate, lastMonthEndDate);
			_logger.Info($"Archiving files from current month. Previous week end date: {today.Date.AddDays(-1 * AgeOfFilesToArchive)}");
			GetOldFilesAndZip(folder, archiveFolder, today.Date.AddDays(-1 * AgeOfFilesToArchive), currentMonthEndDate);
		}

		private static void GetOldFilesAndZip(string sourceFolder, string archiveFolder
			, DateTime endDate, DateTime archiveDateStamp)
		{
			var oldFileInfo = new DirectoryInfo(sourceFolder)
				.EnumerateFiles()
				.Select(x =>
				{
					x.Refresh();
					return x;
				})
				.Where(x => x.Name.ToUpper().Contains(".CSV") && x.CreationTime < endDate)
				.ToList();
			var fileName = $"Archive{archiveDateStamp:yyyyMMdd}.zip";
			CreateZipFile(Path.Combine(archiveFolder, fileName), oldFileInfo.Select(x => x.FullName).ToList());
		}

		private static void ImportData(ImportArgument argument, string connectionString)
		{
			var maps = GetMaps(connectionString, argument.ImportId);

			var date = argument.ImportDate;

			var import = GetFileImport(connectionString, argument.ImportId);
			_logger.Info($"File import properties: {import}");
			var fileName = GetFileName(import, argument);
			var data = ProcessCsv(fileName, import.HasHeader);
			if (data.Item2.Count == 0)
			{
				_logger.Info("No data to import. Exiting.");
				return;
			}
			var columnNames = GetColumnNames(data.Item1);
			var map = GetTableFileColumnMap(maps, columnNames);
			var dateFilterString = GetDateFilterString(date, import);
			var whereClause = string.Format(import.DeleteWhereClause, dateFilterString);
			InsertData(map, import.TableName, columnNames, data.Item2, connectionString, date, whereClause);
			ArchiveFiles(import.FileLocation);
		}

		private static string GetDateFilterString(DateTime date, FileImport import)
			=> import.UseDateIdOnDeleteClauseMask ? GetDateId(date).ToString() : $"'{date}'";

		private static void PopulateMetaData(ImportArgument argument, string connectionString)
		{
			try
			{
				var import = GetFileImport(connectionString, argument.ImportId);
				var fileName = GetFileName(import, argument);
				var data = ProcessCsv(fileName);
				InsertMap(data.Item1, data.Item2[0], connectionString);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				throw;
			}
		}

		private static Dictionary<string, int> GetTableFileColumnMap(List<FileImportColumnMap> columnMaps
			, List<string> headers)
		{
			var maps = columnMaps.ToDictionary(k => k.TableColumnName, v => v.FileColumnIndex ?? -1);
			if (columnMaps.Any(x => !x.FileColumnIndex.HasValue))
			{
				var blanks = columnMaps.Where(x => !x.FileColumnIndex.HasValue).ToList();
				foreach (var blank in blanks)
				{
					if (headers.Contains(blank.TableColumnName))
						maps[blank.TableColumnName] = headers.IndexOf(blank.TableColumnName);
				}
			}
			return maps;
		}

		private static List<FileImportColumnMap> GetMaps(string connectionString, int importId)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (var cmd = new SqlCommand("SELECT Id, FileImportId, TableColumnName, FileColumnName, FileColumnIndex"
					+ $", FileImportColumnTypeId FROM CLO.FileImportColumnMap WHERE FileImportId = {importId}", connection))
				{
					using (var reader = cmd.ExecuteReader())
					{
						var maps = new List<FileImportColumnMap>();
						try
						{
							maps = reader.ToEnumerable(FileImportColumnMap.CreateNewColumnMap).ToList();
						}
						catch (Exception e)
						{
							_logger.Error(e);
						}
						return maps;
					}
				}
			}
		}

		private static void InsertData(Dictionary<string, int> map, string sqlTableName, List<string> columnNames
			, List<string[]> data, string connectionString, DateTime date, string whereClause = null)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				_logger.Info($"Opening connection to {connection.Database} on {connection.DataSource}.");
				connection.Open();
				using (var tran = connection.BeginTransaction())
				{
					try
					{
						using (var cmd = new SqlCommand(GetDeleteCommand(sqlTableName, whereClause), connection))
						{
							_logger.Info($"Preparing to delete old data. {cmd.CommandText}");
							cmd.Transaction = tran;
							var rowsAffected = cmd.ExecuteNonQuery();
							_logger.Info($"Deleted {rowsAffected} rows.");
						}
						using (var bulkCopy = GetSqlBulkCopy(connection, tran, sqlTableName, map))
						{
							var table = GetDataTableWithData(columnNames, data);
							_logger.Info($"Preparing to bulk insert {table.Rows.Count} rows.");
							InsertDataTable(bulkCopy, table);
							_logger.Info("Bulk insert done.");
						}
						tran.Commit();
					}
					catch (Exception e)
					{
						_logger.Error(e);
						_logger.Info("Rolling transaction back.");
						tran.Rollback();
						_logger.Info("Transaction rolled back.");
						Console.WriteLine(e);
					}
				}
			}
		}

		private static DataTable GetDataTableWithData(List<string> columnNames
			, List<string[]> data)
		{
			var table = GetDataTable("table", columnNames);
			data.ForEach(x => table.Rows.Add(x));
			return table;
		}

		private static string GetDeleteCommand(string tableName, string whereClause = null)
			=> $"DELETE {tableName} {whereClause ?? string.Empty}";

		private static SqlBulkCopy GetSqlBulkCopy(SqlConnection connection, SqlTransaction transaction, string tableName
			, Dictionary<string, int> map)
		{
			var copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
			copy.DestinationTableName = tableName;
			map.Keys.ToList().ForEach(x => copy.ColumnMappings.Add(x, map[x]));
			return copy;
		}

		private static DataTable GetDataTable(string tableName, List<string> columnNames)
		{
			var table = new DataTable(tableName);
			table.Columns.AddRange(columnNames.Select(x => new DataColumn(x)).ToArray());
			return table;
		}

		private static List<string> GetColumnNames(List<string> headers)
			=> headers.Select(x => RemoveSpecialCharacters(x)).ToList();

		private static FileImport GetFileImport(string connectionString, int importId)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (var cmd = new SqlCommand("SELECT FileNameMask, TableName, FileLocation, DeleteWhereClause"
				+ $", HasHeader, UseDateIdOnFileMask, UseDateIdOnDeleteClauseMask FROM CLO.FileImport WHERE Id = {importId}"
					, connection))
				{
					using (var reader = cmd.ExecuteReader())
					{
						reader.Read();
						var imp = new FileImport
						{
							FileLocation = reader[nameof(FileImport.FileLocation)].ToString()
							, FileNameMask = reader[nameof(FileImport.FileNameMask)].ToString()
							, TableName = reader[nameof(FileImport.TableName)].ToString()
							, DeleteWhereClause = reader[nameof(FileImport.DeleteWhereClause)].ToString()
							, HasHeader = reader.GetBoolValueOrDefault(nameof(FileImport.HasHeader))
							, UseDateIdOnFileMask = reader.GetBoolValueOrDefault(nameof(FileImport.UseDateIdOnFileMask))
							, UseDateIdOnDeleteClauseMask = reader.GetBoolValueOrDefault(nameof(FileImport.UseDateIdOnDeleteClauseMask))
						};
						return imp;
					}
				}
			}
		}

		private static string GetFileName(FileImport import, ImportArgument argument)
		{
			var val = import.UseDateIdOnFileMask
				? argument.DateId.ToString()
				: string.Empty;
			var fileName = string.Format(Path.Combine(import.FileLocation, import.FileNameMask), val);
			return fileName;
		}

		private static void InsertDataTable(SqlBulkCopy sqlBulkCopy, DataTable dataTable)
		{
			sqlBulkCopy.WriteToServer(dataTable);

			dataTable.Rows.Clear();
		}

		public static Tuple<List<string>, List<string[]>> ProcessCsv(string fileName, bool hasHeader = true)
		{
			var headers = new List<string>();
			var rows = new List<string[]>();
			var data = new Tuple<List<string>, List<string[]>>(headers, rows);
			if (!File.Exists(fileName))
			{
				_logger.Info($"{fileName} doesn't exist!");
				return data;
			}
			try
			{
				_logger.Info($"Processing file {fileName}.");
				using (var parser = new TextFieldParser(fileName))
				{
					parser.Delimiters = new string[] { "," };
					// Skip the header line if it exists
					if (hasHeader && !parser.EndOfData)
						headers.AddRange(parser.ReadFields());

					while (!parser.EndOfData)
					{
						var csvLine = parser.ReadFields();
						rows.Add(csvLine.Select(x => string.IsNullOrEmpty(x) ? null : x).ToArray());
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				_logger.Error(e);
			}

			return data;
		}

	}
}
