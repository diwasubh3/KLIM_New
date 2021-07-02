using System;
using System.IO;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using static YCM.CLO.AnalystLinkage.Constants;
using static YCM.Common.Utils.EmailHandler;
using System.Collections.Generic;
using YCM.CLO.DataAccess.Models;
using System.Linq;
using log4net;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using System.Security.Cryptography;
using YCM.Common.Utils.ExtensionMethods;
using YCM.Common.Utils.ExcelUtilities;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace YCM.CLO.AnalystLinkage
{
	class Program
	{
		private static ILog _logger;
		private static readonly IRepository _repo = new Repository();
		private static readonly Dictionary<string, int> _headerRowDictionary
			= new Dictionary<string, int>();
		private static readonly Dictionary<string, int> _detailRowDictionary
			= new Dictionary<string, int>();
		private static List<AnalystResearchImportLog> _importFailures
			= new List<AnalystResearchImportLog>();
		private static readonly List<AnalystResearchImportLog> _importSuccesses
			= new List<AnalystResearchImportLog>();
		private static List<AnalystResearchRowLocation> _rowLocations;
		private static List<vw_YorkCoreGenevaAnalyst> _users;
		private static List<Issuer> _issuers;
		private static List<AnalystResearchFile> _analystResearchFiles;
		private static List<AnalystResearchHeader> _headers;
		private static List<AnalystResearchDetail> _details;
		private static int _inserts;
		private static int _updates;
		private static int _skips;
		private static int _filesSkipped;

		private static readonly List<string> _sheetExclusionList
			= SheetExclusionList.Select(x => x.ToLower()).ToList();

		static void Main(string[] args)
		{
			log4net.Config.XmlConfigurator.Configure();
			_logger = LogManager.GetLogger(typeof(Program));
			var exitCode = Run();
			Environment.Exit(exitCode);
		}

		private static int Run()
		{
			var exitCode = 0;
			try
			{
				_logger.Info("Loading data from the database...");
				_logger.Info("Getting users...");
				_users = _repo.GetAnalysts().ToList();
				_logger.Info("Getting issuers...");
				_issuers = _repo.GetIssuers().ToList();
				_logger.Info("Getting headers...");
				_headers = _repo.GetAnalystResearchHeaders().ToList();
				_logger.Info("Getting details...");
				_details = _repo.GetAnalystResearchDetails().ToList();
				_logger.Info("Getting cell locations...");
				_rowLocations = _repo.GetAnalystResearchRowLocations().ToList();
				_logger.Info("Getting file load data...");
				_analystResearchFiles = _repo.GetAnalystResearchFiles().ToList();
				_logger.Info($"Reading files from {FilePath}...");
				//_importFailures = _repo.GetAnalystResearchImportLogs().ToList();
				//TestEmail(FilePath);
				LoadExcel(FilePath);
				_logger.Info($"Finished processing files from {FilePath}.");
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				exitCode = 1;
			}
			finally
			{
				_repo.Dispose();
			}
			_logger.Info($"Exiting with the following code: {exitCode}.");
			return exitCode;
		}

		private static void TestEmail(string path)
		{
			try
			{
				var startTime = DateTime.Now;
				var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
				.Where(x => ExcelExtensionNames.Any(y => x.EndsWith(y))).ToList();
				var endTime = DateTime.Now;
				SendFailureEmail(startTime, endTime, files.Count);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}
		}

		private static void LoadExcel(string path)
		{
			try
			{
				var fileAccessFailures = new List<string>();
				var startTime = DateTime.Now;
				var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
				.Where(x => ExcelExtensionNames.Any(y => x.EndsWith(y))).ToList();
				if (files == null || files.Count() == 0)
				{
					_logger.Info($"Unable to find files with the extensions ({string.Join(",", ExcelExtensionNames)} in {path}.)");
					return;
				}
				_logger.Info($"Found {files.Count()} file(s) with the extensions ({string.Join(",", ExcelExtensionNames)}) in {path}.");
				foreach (var file in files)
				{
					try
					{
						var fileInfo = new FileInfo(file);
						if (!FileWasUpdated(fileInfo))
						{
							_logger.Info($"No changes detected in {file}.  Processing the next file.");
							_filesSkipped++;
							continue;
						}
						_logger.Info($"Processing file {file}...");
						using (var pkg = new ExcelPackage(fileInfo))
						{
							foreach (var sheet in pkg.Workbook.Worksheets)
							{
								//skip sheets in the exclusion list.  The check below recognizes * as a wildcard.
								if (_sheetExclusionList.Any(x => LikeOperator.LikeString(sheet.Name, x, CompareMethod.Text)))
								{
									_logger.Info($"Skipping sheet: {sheet.Name}");
									_skips++;
									continue;
								}
								SaveRowLocations(sheet);
								BuildDictionaries(sheet);
								_logger.Info($"Processing sheet {sheet.Name} in {file}...");
								ProcessSheet(sheet, file);
							}
						}
						SaveFileInfo(fileInfo);
					}
					catch (Exception ex)
					{
						_logger.Error(ex);
						fileAccessFailures.Add(file);
					}
				}
				var endTime = DateTime.Now;
				SendFailureEmail(startTime, endTime, files.Count, fileAccessFailures);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}
		}

		private static void SendFailureEmail(DateTime processingStartTime, DateTime processingEndTime
			, int numberOfFilesProcessed, List<string> fileAccessFailures = null)
		{
			var body = new StringBuilder();
			var errors = false;
			body.Append("<style>");
			body.Append("table{border-collapse: collapse;font-family:Calibri;}");
			body.Append("td{padding: 2px;}");
			body.Append(".div_text{font-family:Calibri;font-size:18px;}");
			body.Append("</style>");
			body.Append(@"<table>");
			body.Append(ConvertToHtmlTableRow("Processing Start Time", processingStartTime));
			body.Append(ConvertToHtmlTableRow("Processing End Time", processingEndTime));
			body.Append(ConvertToHtmlTableRow("Files Processed", numberOfFilesProcessed));
			body.Append(ConvertToHtmlTableRow("Files Skipped", _filesSkipped));
			body.Append(ConvertToHtmlTableRow("Issuers/Tabs Skipped", _skips));
			body.Append(ConvertToHtmlTableRow("Issuers/Tabs Processed Successfully", _importSuccesses.Count));
			body.Append(ConvertToHtmlTableRow("Rows Inserted", _inserts));
			body.Append(ConvertToHtmlTableRow("Rows Updated", _updates));
			body.Append(ConvertToHtmlTableRow("Issuers/Tabs Failed Processing", _importFailures.Count));
			body.Append("</table>");
			if(_importFailures.Count > 0)
			{
				body.Append("<br/><br/>");
				body.Append(@"<div class=""div_text"">");
				body.Append("Issuers that failed processing:<br/><br/>");
				body.Append("</div>");
				body.Append(@"<table border=""1"">");
				body.Append($"<tr><th>File Name</th><th>Issuer Code</th><th>Sheet Name</th><th>Error</th></tr>");
				body.Append(string.Join(string.Empty, _importFailures.Select(x => x.ToHtmlTableRow())));
				body.Append("</table>");
				errors = true;
			}
			if((fileAccessFailures?.Count ?? 0) > 0)
			{
				body.Append("<br/><br/>");
				body.Append(@"<div class=""div_text"">");
				body.Append("Files that failed processing:<br/><br/>");
				body.Append(string.Join("<br/>", fileAccessFailures));
				body.Append("</div>");
				errors = true;
			}
			var subject = $"{(errors ? "Processing Errors - " : string.Empty)}{EmailSubject}";
			SendHtmlEmailWithoutCredentials(EmailFrom, EmailTo, subject, body.ToString());
		}

		private static string ConvertToHtmlTableRow<T>(string rowLabel, T rowValue, string horizontalAlignment = "right")
			=> $@"<tr><td>{rowLabel}</td><td align=""{horizontalAlignment}"">{rowValue}</td></tr>";

		private static bool FileWasUpdated(FileInfo fileInfo)
		{
			var file = _analystResearchFiles.FirstOrDefault(x => x.AnalystResearchFileName == fileInfo.Name);
			//TODO - maybe add hash for better update check
			var fileWasUpdated = file == null
				|| file.LastFileUpdate.GetDateTimeWithoutMilliseconds()
				!= fileInfo.LastWriteTime.GetDateTimeWithoutMilliseconds();
			return fileWasUpdated;
		}

		private static void SaveFileInfo(FileInfo fileInfo)
		{
			var file = _analystResearchFiles.FirstOrDefault(x => x.AnalystResearchFileName == fileInfo.Name);
			if(file == null)
				file = new AnalystResearchFile
				{
					  AnalystResearchFileName = fileInfo.Name
					, CreatedBy = LastUpdatedBy
					, CreatedOn = DateTime.Now
				};
			file.LastFileUpdate = fileInfo.LastWriteTime;
			file.LastUpdatedBy = LastUpdatedBy;
			file.LastUpdatedOn = DateTime.Now;
			_repo.SaveEntity(file);
		}

		private static void BuildDictionaries(ExcelWorksheet sheet)
		{
			if (_headerRowDictionary.Count == 0)
			{
				BuildPropertyDictionary(nameof(AnalystResearchHeader), _headerRowDictionary);
			}
			if (_detailRowDictionary.Count == 0)
			{
				BuildPropertyDictionary(nameof(AnalystResearchDetail), _detailRowDictionary);
			}
		}

		//TODO - find better way to send back successes and failures
		private static void ProcessSheet(ExcelWorksheet sheet, string fileName)
		{
			try
			{
				var header = GetHeader(sheet, _headerRowDictionary);
				var name = GetIssuerName(sheet, _headerRowDictionary);
				if (header.IssuerId == default(int))
				{
					_logger.Error($"Unable to find issuer id for issuer name {name} from worksheet {sheet.Name}.");
					var failureEntry = new AnalystResearchImportLog(fileName, sheet.Name, name, DateTime.Now, LastUpdatedBy
						, "Unable to find issuer id.");
					_importFailures.Add(failureEntry);
					_repo.SaveEntity(failureEntry);
					//we don't want to process this sheet so return right away
					return;
				}
				var details = GetDetails(sheet, _detailRowDictionary);
				var updated = SaveAnalystResearch(new List<AnalystResearchHeader> { header }, details);
				if (updated)
				{
					var logEntry = new AnalystResearchImportLog(fileName, sheet.Name, name, DateTime.Now, LastUpdatedBy, null, true);
					_importSuccesses.Add(logEntry);
					_repo.SaveEntity(logEntry);
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Unable to process {sheet.Name} in {fileName}.", ex);
			}
		}

		private static bool SaveAnalystResearch(List<AnalystResearchHeader> headers, List<AnalystResearchDetail> details)
		{
			var updated = false;
			foreach (var header in headers)
			{
				var headerId = default(int);
				var existing = _headers.Any(x => x.IssuerId == header.IssuerId);
				try
				{
					var result = InsertOrUpdateHeader(_headers, header);
					if (!updated && result.Success)
						updated = true;
					LogUpdateError("Header update failure.", result);
					headerId = result.Id;
				}
				catch (Exception ex)
				{
					_logger.Error($"Unable to save data for issuer id: {header.IssuerId}", ex);
				}
				if (headerId == default(int))
					continue;
				var researchDetails = !existing
					? details.OrderByDescending(x => x.AsOfDate)
					.ToList()
					: details.OrderByDescending(x => x.AsOfDate)
					.Take(NumberOfRelevantPeriods)//we only care about last x periods for updates
					.ToList();
				try
				{
					var detailsToUpdate = GetDetailsToUpdate(_details, researchDetails
						, headerId);
					if (detailsToUpdate.Count > 0)
					{
						var result = _repo.SaveAnalystResearchDetails(_details, detailsToUpdate);
						if(result.Success)
							_logger.Info($"Successfully saved details for header id: {headerId}.");
						else
						{
							if (result.OperationException != null)
								_logger.Error($"Failed to save detail id; {headerId}.", result.OperationException);
						}
					}
					else
						_logger.Info($"No detail data to save for header id: {headerId}.");
				}
				catch (Exception ex)
				{
					_logger.Error($"Unable to save detail for header id: {header.AnalystResearchHeaderId}", ex);
				}
			}
			return updated;
		}

		private static List<AnalystResearchDetail> GetDetailsToUpdate(List<AnalystResearchDetail> existingDetails
			, List<AnalystResearchDetail> details, int headerId)
		{
			var detailsToUpdate = new List<AnalystResearchDetail>();
			foreach (var detail in details)
			{
				detail.AnalystResearchHeaderId = headerId;
				var existing = existingDetails.FirstOrDefault(x => x.AnalystResearchHeaderId == headerId
				&& x.AsOfDate == detail.AsOfDate);
				if (DetailUpdateRequired(existing, detail))
				{
					UpdateEntity(existing, detail, LastUpdatedBy);
				}
				else if (existing != null)
					continue;
				detailsToUpdate.Add(detail);
			}
			return detailsToUpdate;
		}

		private static void LogUpdateError(string message, DatabaseEntityOperationResult result)
		{
			if (result != null && !result.Success && result.OperationException != null)
				_logger.Error(message, result.OperationException);
		}

		private static DatabaseEntityOperationResult InsertOrUpdateHeader(List<AnalystResearchHeader> existingHeaders, AnalystResearchHeader header)
		{
			var existing = existingHeaders.FirstOrDefault(x => x.IssuerId == header.IssuerId);
			if (HeaderUpdateRequired(existing, header))
			{
				UpdateEntity(existing, header, LastUpdatedBy);
				_updates++;
			}
			else if (existing != null)
			{
				_skips++;
				return new DatabaseEntityOperationResult(existing.AnalystResearchHeaderId
					, false, null);
			}
			var result = _repo.SaveAnalystResearchHeader(existing, header);
			if (existing == null)
			{
				existingHeaders.Add(header);
				_inserts++;
			}
			return result;
		}

		private static bool DetailUpdateRequired(AnalystResearchDetail existing, AnalystResearchDetail detail)
			=> existing != null && (Math.Truncate(existing.Revenues.GetValueOrDefault() * MultiplierForComparison)
			!= Math.Truncate(detail.Revenues.GetValueOrDefault() * MultiplierForComparison)
							|| Math.Truncate(existing.TotalLeverage.GetValueOrDefault() * MultiplierForComparison)
			!= Math.Truncate(detail.TotalLeverage.GetValueOrDefault() * MultiplierForComparison));

		private static bool HeaderUpdateRequired(AnalystResearchHeader existing, AnalystResearchHeader header)
			=> existing != null && existing.IssuerId == header.IssuerId
			&& (existing.BusinessDescription != header.BusinessDescription
			|| existing.Sponsor != header.Sponsor
			|| existing.AgentBank != header.AgentBank
			|| existing.CLOAnalystId != header.CLOAnalystId
			|| existing.CreditScore.GetValueOrDefault() != header.CreditScore.GetValueOrDefault()
			|| existing.HFAnalystId != header.HFAnalystId
            || existing.LiborCategory != header.LiborCategory
            || existing.LiborTransitionNote != header.LiborTransitionNote
            );

		private static void UpdateEntity<T>(T existing, T entity, string lastUpdatedBy) where T:Entity
		{
			entity.Id = existing.Id;
			entity.CreatedBy = existing.CreatedBy;
			entity.CreatedOn = existing.CreatedOn;
			entity.LastUpdatedOn = DateTime.Now;
			entity.LastUpdatedBy = lastUpdatedBy;
		}


        private static List<AnalystResearchDetail> GetDetails(ExcelWorksheet sheet
			, Dictionary<string, int> rowDictionary)
		{
			var details = new List<AnalystResearchDetail>();
			for (var column = DetailFromColumn; column <= DetailToColumn; column++)
			{
				if (!ColumnHasData(sheet, _detailRowDictionary, column))
					continue;
				var detail = GetDetail(sheet, _detailRowDictionary, column);
				details.Add(detail);
			}
			return details;
		}

		private static bool ColumnHasData(ExcelWorksheet sheet, Dictionary<string, int> rowDictionary
			, int column)
		{
			var asOfDateCell = sheet.Cells[rowDictionary[nameof(AnalystResearchDetail.AsOfDate)], column];
			DateTime test;
			var hasData = DateTime.TryParse(asOfDateCell.Value?.ToString(), out test);
			return hasData;
		}

		private static AnalystResearchHeader GetHeader(ExcelWorksheet sheet
			, Dictionary<string, int> rowDictionary)
		{
			var cells = sheet.Cells[HeaderFromRow, HeaderColumn, HeaderToRow, HeaderColumn];
			var header = new AnalystResearchHeader();
			header.AgentBank = cells.GetStringValueFromCell(rowDictionary
				, nameof(AnalystResearchHeader.AgentBank), HeaderColumn);
			header.Sponsor = cells.GetStringValueFromCell(rowDictionary
				, nameof(AnalystResearchHeader.Sponsor), HeaderColumn);
			header.BusinessDescription = cells.GetStringValueFromCell(rowDictionary
				, nameof(AnalystResearchHeader.BusinessDescription), HeaderColumn);
			var name = cells.GetStringValueFromCell(rowDictionary
				, nameof(AnalystResearchHeader.CLOAnalystId), HeaderColumn);
			header.CLOAnalystId = GetUserId(name);
			header.CreditScore = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchHeader.CreditScore), HeaderColumn);

            header.LiborCategory = cells.GetStringValueFromCell(rowDictionary
    , nameof(AnalystResearchHeader.LiborCategory), HeaderColumn);

            header.LiborTransitionNote = cells.GetStringValueFromCell(rowDictionary
, nameof(AnalystResearchHeader.LiborTransitionNote), HeaderColumn);

            name = cells.GetStringValueFromCell(rowDictionary
				, nameof(AnalystResearchHeader.HFAnalystId), HeaderColumn);




			header.HFAnalystId = GetUserId(name);
			name = GetIssuerName(sheet, rowDictionary);
			header.IssuerId = GetIssuerId(name);
			header.CreatedBy = LastUpdatedBy;
			header.CreatedOn = DateTime.Now;
			header.LastUpdatedBy = header.CreatedBy;
			header.LastUpdatedOn = header.CreatedOn;
            
			return header;
		}

		private static string GetIssuerName(ExcelWorksheet sheet
			, Dictionary<string, int> rowDictionary)
			=> sheet.Cells[HeaderFromRow, HeaderColumn, HeaderToRow, HeaderColumn]
			.GetStringValueFromCell(_headerRowDictionary
				, nameof(AnalystResearchHeader.IssuerId), HeaderColumn);

		private static int? GetUserId(string userName)
		{
			if (string.IsNullOrEmpty(userName))
				return null;
			var users = _users.Where(x => x.AnalystDesc.ToLower()
			.Contains(userName.ToLower())).ToList();
			var id = users.Count > 0 ? users.Max(x => x.AnalystId) : (int?)null;
			if (users.Count > 1)
				_logger.Error($"Multiple ids found for username {userName}!");
			return id;
		}

		private static int GetIssuerId(string issuerName)
		{
			var id = default(int);
			try
			{
				//use single so the statement throws an error if there are dupes
				var issuer = _issuers.Single(x => !string.IsNullOrEmpty(x.IssuerCode)
					&& x.IssuerCode.Equals(issuerName, StringComparison.CurrentCultureIgnoreCase));
				id = issuer.IssuerId;
			}
			catch (Exception ex)
			{
				_logger.Error($"Unable to find issuer id for issuer name {issuerName}", ex);
			}
			return id;
		}

		private static AnalystResearchDetail GetDetail(ExcelWorksheet sheet
			, Dictionary<string, int> rowDictionary, int column)
		{
			var detailFromRow = rowDictionary.Min(x => x.Value);
			var detailToRow = rowDictionary.Max(x => x.Value);
			var cells = sheet.Cells[detailFromRow, column, detailToRow, column];
			var detail = new AnalystResearchDetail();
			detail.AsOfDate = cells.GetDateTimeValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.AsOfDate), column).Date;
			detail.ABLRCF = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.ABLRCF), column);
			detail.CapitalExpenditures = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.CapitalExpenditures), column);
			detail.Cash = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.Cash), column);
			detail.CashEBITDA = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.CashEBITDA), column);
			detail.CashTaxes = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.CashTaxes), column);
			detail.Comments = cells.GetStringValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.Comments), column);
			detail.CovenantEBITDA = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.CovenantEBITDA), column);
			detail.EnterpriseValue = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.EnterpriseValue), column);
			detail.EquityMarketCap = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.EquityMarketCap), column);
			detail.EstimatedEnterpriseValue = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.EstimatedEnterpriseValue), column);
			detail.FCF = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.FCF), column);
			detail.FCFDebt = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.FCFDebt), column);
			detail.FirstLienDebt = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.FirstLienDebt), column);
			detail.Interest = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.Interest), column);
			detail.LTMEBITDA = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.LTMEBITDA), column);
			detail.LTMFCF = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.LTMFCF), column);
			detail.LTMPFEBITDA = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.LTMPFEBITDA), column);
			detail.LTMRevenues = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.LTMRevenues), column);
			detail.Margin = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.Margin), column);
			detail.NetTotalLeverage = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.NetTotalLeverage), column);
			detail.OCF = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.OCF), column);
			detail.OrganicGrowth = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.OrganicGrowth), column);
			detail.Other1 = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.Other1), column);
			detail.Other2 = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.Other2), column);
			detail.PFAcquisitionAdjustment = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.PFAcquisitionAdjustment), column);
			detail.PFCostSaves = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.PFCostSaves), column);
			detail.PFEBITDA = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.PFEBITDA), column);
			detail.RestructuringAndIntegration = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.RestructuringAndIntegration), column);
			detail.RestructuringOneTime = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.RestructuringOneTime), column);
			detail.Revenues = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.Revenues), column);
			detail.SeniorLeverage = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.SeniorLeverage), column);
			detail.TotalDebt = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.TotalDebt), column);
			detail.TotalLeverage = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.TotalLeverage), column);
			detail.TransactionExpenses = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.TransactionExpenses), column);
			detail.WorkingCapital = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.WorkingCapital), column);
			detail.YoYGrowth = cells.GetNullableDecimalValueFromCell(rowDictionary
				, nameof(AnalystResearchDetail.YoYGrowth), column);
			detail.CreatedOn = DateTime.Now;
			detail.CreatedBy = LastUpdatedBy;
			detail.LastUpdatedOn = detail.CreatedOn;
			detail.LastUpdatedBy = detail.CreatedBy;
			return detail;
		}

		private static void BuildPropertyDictionary(string className
			, Dictionary<string, int> dictionary)
		{
			var locations = _rowLocations.Where(x => x.ClassName == className)
				.ToList();
			foreach (var location in locations)
			{
				dictionary[location.PropertyName] = location.RowIndex;
			}
		}

		private static void BuildPropertyDictionary(ExcelWorksheet sheet
			, int fromRow, int fromColumn, int toRow, int toColumn
			, Dictionary<string, int> dictionary)
		{
			var excelRange = sheet.Cells[fromRow, fromColumn, toRow, toColumn];
			foreach (var cell in excelRange)
			{
				var key = Regex.Replace(cell.Value.ToString()
					.Replace("&", "And")
					, @"[^a-zA-Z]+", string.Empty);
				if (key.Equals("other", StringComparison.CurrentCultureIgnoreCase))
				{
					dictionary["Other1"] = 20;
					key = "Other2";
				}
				dictionary[key] = cell.Start.Row;
			}
		}

		private static void SaveRowLocations(ExcelWorksheet sheet)
		{
			if (_rowLocations.Count != 0)
				return;
			_logger.Info($"Saving row locations.  Using cells from {sheet.Name}.");
			if (_headerRowDictionary.Count == 0)
			{
				BuildPropertyDictionary(sheet, HeaderFromRow, LabelColumn
					, HeaderToRow, LabelColumn, _headerRowDictionary);
				_headerRowDictionary.UpdateKey(IssuerName, IssuerNameProperty);
				_headerRowDictionary.UpdateKey(CLOAnalyst, CLOAnalystProperty);
				_headerRowDictionary.UpdateKey(HFAnalyst, HFAnalystProperty);
			}
			if (_detailRowDictionary.Count == 0)
			{
				BuildPropertyDictionary(sheet, DetailFromRow, LabelColumn
					, DetailToRow, LabelColumn, _detailRowDictionary);
				_detailRowDictionary.UpdateKey(QuarterEnded, QuarterEndedProperty);
			}
			SaveMap();
			_rowLocations = _repo.GetAnalystResearchRowLocations().ToList();
		}

		private static void SaveMap()
		{
			try
			{
				foreach (var key in _headerRowDictionary.Keys)
				{
					var location = new AnalystResearchRowLocation
					{
						ClassName = "AnalystResearchHeader", CreatedBy = LastUpdatedBy, CreatedOn = DateTime.Now
						, LastUpdatedBy = LastUpdatedBy, LastUpdatedOn = DateTime.Now
						, PropertyName = key, RowIndex = _headerRowDictionary[key]
					};
					_repo.SaveEntity(location);
				}
				foreach (var key in _detailRowDictionary.Keys)
				{
					var location = new AnalystResearchRowLocation
					{
						ClassName = "AnalystResearchDetail", CreatedBy = LastUpdatedBy, CreatedOn = DateTime.Now
						, LastUpdatedBy = LastUpdatedBy, LastUpdatedOn = DateTime.Now
						, PropertyName = key, RowIndex = _detailRowDictionary[key]
					};
					_repo.SaveEntity(location);
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}
		}

		private static byte[] GetFileHash(string fileName)
		{
			var sha1 = HashAlgorithm.Create();
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				return sha1.ComputeHash(stream);
		}

	}
}
