using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using YCM.CLO.DTO;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.Web.Models;
using AutoMapper;
using YCM.CLO.Web.Objects;
using YCM.CLO.DataAccess;
using OfficeOpenXml;
using System.IO;
using System.Reflection;
using log4net;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;


namespace YCM.CLO.Web.Controllers
{
    [NoCache]
    public class DataController : Controller
    {
        readonly IRepository _repository;
        private readonly ILog _logger;
        private readonly Dictionary<string, int> _resultsHeaderMap = new Dictionary<string, int>();

        private const string CLO_NAME = "CLO NAME";
        private const string TOTAL_PAR = "TOTAL PAR";
        private const string ASSET_PAR = "ASSET PAR";
        private const string SPREAD = "SPREAD";
        private const string TOTAL_COUPON = "TOTAL COUPON";
        private const string WARF = "WARF";
        private const string MOODYS_RECOVERY = "MOODY'S RECOVERY";
        private const string WA_LIFE = "WA LIFE";
        private const string WA_BID = "WA BID";
        private const string CLEAN_NAV = "CLEAN NAV";
        private const string PRINCIPAL_CASH = "PRINCIPAL CASH";
        private const string DIVERSITY = "DIVERSITY";

        //public DataController() : this(new Repository())
        //{

        //}

        public DataController(IRepository repository)
        {
            _repository = repository;
            _logger = LogManager.GetLogger(typeof(DataController));
            _resultsHeaderMap.Add(CLO_NAME, 1);
            _resultsHeaderMap.Add(TOTAL_PAR, 2);
            _resultsHeaderMap.Add(ASSET_PAR, 3);
            _resultsHeaderMap.Add(PRINCIPAL_CASH, 4);
            _resultsHeaderMap.Add(SPREAD, 5);
            _resultsHeaderMap.Add(TOTAL_COUPON, 6);
            _resultsHeaderMap.Add(WARF, 7);
            _resultsHeaderMap.Add(MOODYS_RECOVERY, 8);
            _resultsHeaderMap.Add(WA_LIFE, 9);
            _resultsHeaderMap.Add(WA_BID, 10);
            _resultsHeaderMap.Add(CLEAN_NAV, 11);
            _resultsHeaderMap.Add(DIVERSITY, 12);
        }

        public ActionResult DownloadLoanPositionsFile(int fundId)
        {
            var bits = new byte[0];
            var path = Path.GetTempPath();
            var fileName = GetLoanPositions(fundId, path);
            var filePath = Path.Combine(path, fileName);
            var result = new CustomFileResult(System.IO.File.ReadAllBytes(filePath),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = fileName, Inline = true };
            return result;
        }

        public ActionResult DownloadSummaries()
        {
            var bits = new byte[0];
            var path = Path.GetTempPath();
            var fileName = GetSummariesForDownload(path);
            var filePath = Path.Combine(path, fileName);
            var result = new CustomFileResult(System.IO.File.ReadAllBytes(filePath),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = fileName, Inline = true };
            return result;
        }

        public ActionResult DownloadReInvestCash(string filePath)
        {
            filePath = filePath.Replace("file:", "");
            var fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            var result = new CustomFileResult(ReadAllBytes(filePath),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = fileName, Inline = true };
            return result;
        }


        public JsonNetResult GetReportingData()
        {
            return new JsonNetResult()
            {
                Data = new ReportingData()
                {
                    AssetClasses = _repository.GetAssetClasses(),
                    FundAssetClasses = _repository.GetFundAssetClasses(),
                    Funds = Mapper.Map<IEnumerable<Fund>, IEnumerable<FundDto>>(_repository.GetFunds()),
                    Ratings = Mapper.Map<IEnumerable<Rating>, IEnumerable<RatingDto>>(_repository.GetRatings().OrderBy(r => r.Rank)).ToList(),
                    EquityOverrides = _repository.GetEquityOverrides(),
                    DefaultSecurities = _repository.GetDefaultSecurities(Helper.GetPrevDayDateId())
                }
            };
        }

        private byte[] ReadAllBytes(string pathSource)
        {
            byte[] bytes = new byte[0];
            try
            {
                using (FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {

                    // Read the source file into a byte array.
                    bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                }
            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            return bytes;
        }


        [System.Web.Http.HttpPost]
        public JsonNetResult SaveReportingData([System.Web.Http.FromBody] ReportingData reportingData)
        {
            var funds = Mapper.Map<IEnumerable<FundDto>, IEnumerable<Fund>>(reportingData.Funds);
            funds.ToList().ForEach(fund => _repository.SaveFund(fund));

            if (reportingData.FundAssetClasses != null)
            {
                reportingData.FundAssetClasses.ToList().ForEach(fa => _repository.SaveFundAssetClass(fa, User.Identity.Name));
            }

            if (reportingData.EquityOverrides != null)
            {
                reportingData.EquityOverrides.ToList().ForEach(eo => _repository.SaveEquityOverride(eo, User.Identity.Name));
            }

            return GetReportingData();
        }

        private string GetSummariesForDownload(string fileLocation)
        {
            try
            {
                _logger.Info("Getting dateId...");
                var dateId = Helper.GetPrevDayDateId();
                _logger.Info($"Date Id: {dateId}");

                _logger.Info($"_repository is null? {_repository == null}");
                var data = CLOCache.GetSummaries();
                var restrictions = _repository.GetFundRestrictions(null);
                _logger.Info($"data is null? {data == null}");
                if (data != null)
                    _logger.Info($"data count: {data.Count}");
                var sheetName = "TestResults";
                var fileName = $"{sheetName}-{DateTime.Now:yyyyMMddhhmmss}.xlsx";
                var filePath = Path.Combine(fileLocation, fileName);
                var pkg = new ExcelPackage();
                var sheet = pkg.Workbook.Worksheets.Add(sheetName);
                PopulateAndFormatWorksheet(sheet, data);

                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                sheet.PrinterSettings.Orientation = eOrientation.Landscape;
                sheet.PrinterSettings.FitToPage = true;
                sheet.PrinterSettings.FitToHeight = 1;
                sheet.PrinterSettings.FooterMargin = 1M;
                sheet.PrinterSettings.TopMargin = 1M;
                sheet.PrinterSettings.LeftMargin = 1M;
                sheet.PrinterSettings.RightMargin = 1M;
                pkg.SaveAs(new FileInfo(filePath));
                return fileName;
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception has occurred during CLO Calculation");
                throw;
            }
        }

        private void PopulateAndFormatWorksheet(ExcelWorksheet sheet, List<vw_CLOSummary> summaries)
        {
            foreach (var map in _resultsHeaderMap)
            {
                sheet.Cells[1, map.Value].Value = map.Key;
            }

            var header = sheet.Cells[1, _resultsHeaderMap.Min(x => x.Value), 1, _resultsHeaderMap.Max(x => x.Value)];
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            header.Style.Font.Bold = true;

            var rowIdx = 1;
            var plainNumberWithDecimals = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
            var plainNumber = "_(* #,##0_);_(* (#,##0);_(* \"-\"??_);_(@_)";

            foreach (var summary in summaries)
            {
                try
                {
                    rowIdx++;
                    sheet.Cells[rowIdx, _resultsHeaderMap[CLO_NAME]].Value = summary.FundCode;
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[TOTAL_PAR], summary.FundId, summary.Par, 40);
                    sheet.Cells[rowIdx, _resultsHeaderMap[TOTAL_PAR]].Style.Numberformat.Format = plainNumber;
                    sheet.Cells[rowIdx, _resultsHeaderMap[ASSET_PAR]].Value = summary.AssetPar;
                    sheet.Cells[rowIdx, _resultsHeaderMap[ASSET_PAR]].Style.Numberformat.Format = plainNumber;
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[PRINCIPAL_CASH], summary.FundId, summary.PrincipalCash, 45);
                    sheet.Cells[rowIdx, _resultsHeaderMap[PRINCIPAL_CASH]].Style.Numberformat.Format = plainNumberWithDecimals;
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[SPREAD], summary.FundId, summary.WSOSpread, 41, true);
                    sheet.Cells[rowIdx, _resultsHeaderMap[SPREAD]].Style.Numberformat.Format = "#.00%";
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[TOTAL_COUPON], summary.FundId, summary.TotalCoupon, 42, true);
                    sheet.Cells[rowIdx, _resultsHeaderMap[TOTAL_COUPON]].Style.Numberformat.Format = "#.00%";
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[WARF], summary.FundId, summary.WSOWARF, 43);
                    sheet.Cells[rowIdx, _resultsHeaderMap[WARF]].Style.Numberformat.Format = plainNumber;
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[MOODYS_RECOVERY], summary.FundId, summary.WSOMoodyRecovery, 44);
                    sheet.Cells[rowIdx, _resultsHeaderMap[MOODYS_RECOVERY]].Style.Numberformat.Format = plainNumberWithDecimals;
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[WA_LIFE], summary.FundId, summary.WSOWALife, 107);
                    sheet.Cells[rowIdx, _resultsHeaderMap[WA_LIFE]].Style.Numberformat.Format = plainNumberWithDecimals;
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[WA_BID], summary.FundId, summary.Bid, 46);
                    sheet.Cells[rowIdx, _resultsHeaderMap[WA_BID]].Style.Numberformat.Format = plainNumberWithDecimals;
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[CLEAN_NAV], summary.FundId, summary.CleanNav, 46, true);
                    sheet.Cells[rowIdx, _resultsHeaderMap[CLEAN_NAV]].Style.Numberformat.Format = "#.00%";
                    SetValueAndFormatBackground(sheet, rowIdx, _resultsHeaderMap[DIVERSITY], summary.FundId, summary.WSODiversity, 47);
                    sheet.Cells[rowIdx, _resultsHeaderMap[DIVERSITY]].Style.Numberformat.Format = plainNumber;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void SetValueAndFormatBackground(ExcelWorksheet sheet, int rowIdx, int colIdx, int fundId, decimal? valueToTest, int fieldId
            , bool divideByOneHundred = false)
        {
            if (valueToTest.HasValue)
            {
                var decimalValue = valueToTest.Value;
                var cell = sheet.Cells[rowIdx, colIdx];
                cell.Value = divideByOneHundred ? decimalValue / 100 : decimalValue;
                var bgd = GetTestBackgroundColor(fundId, decimalValue, fieldId);
                try
                {
                    if (!string.IsNullOrEmpty(bgd))
                    {
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        var color = GetColor(bgd);
                        cell.Style.Fill.BackgroundColor.SetColor(color);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private string GetTestBackgroundColor(int fundId, decimal valueToTest, int fieldId)
        {
            var restrictions = _repository.GetFundRestrictions(fundId);
            var restriction = restrictions.FirstOrDefault(x => x.FundId == fundId && x.FieldId == fieldId
                                                                                  && x.FundRestrictionTypeId == 2);
            var background = string.Empty;
            if (restriction != null)
            {
                var isInvalid = ValueIsInvalid(fundId, restriction, valueToTest);
                if (isInvalid)
                    background = restriction.FundRestrictionType.DisplayColor;
                else
                {
                    restriction = restrictions.FirstOrDefault(x => x.FundId == fundId && x.FieldId == fieldId
                                                                                          && x.FundRestrictionTypeId == 1);
                    isInvalid = ValueIsInvalid(fundId, restriction, valueToTest);
                    if (isInvalid)
                        background = restriction.FundRestrictionType.DisplayColor;
                }
            }

            return background;
        }

        private Color GetColor(string colorName)
        {
            var color = Color.Transparent;
            try
            {
                if (!string.IsNullOrWhiteSpace(colorName))
                {
                    var properName = colorName[0].ToString().ToUpper() + colorName.ToLower().Substring(1);
                    color = Color.FromName(properName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return color;
        }

        private bool ValueIsInvalid(int fundId, FundRestriction restriction, decimal valueToTest)
        {
            if (restriction == null)
                return false;
            var operatorId = restriction.OperatorId;
            /*
1	=
2	>
3	>=
4	<
5	<=			 */
            switch (operatorId)
            {
                case 1:
                    return valueToTest == restriction.RestrictionValue;
                case 2:
                    return valueToTest > restriction.RestrictionValue;
                case 3:
                    return valueToTest >= restriction.RestrictionValue;
                case 4:
                    return valueToTest < restriction.RestrictionValue;
                case 5:
                    return valueToTest <= restriction.RestrictionValue;
                default:
                    return false;
            }
        }

        private string GetLoanPositions(int fundId, string fileLocation)
        {
            try
            {
                var dateId = Helper.GetPrevDayDateId();
                var prevDateId = Helper.GetPrevToPrevDayDateId();
                var data = _repository
                    .GetLoanPositions(dateId, prevDateId, fundId)
                    .Where(x => x.Difference != 0).ToList()
                    .OrderByDescending(a => Math.Abs(a.Difference))
                    .ThenBy(x => x.Issuer)
                    .ToList();
                var sheetName = data.Count > 0
                    ? data.First().FundCode
                    : "positions";
                var fileName = $"{sheetName}-{DateTime.Now:yyyyMMddhhmmss}.xlsx";
                var filePath = Path.Combine(fileLocation, fileName);
                var pkg = new ExcelPackage();
                var sheet = pkg.Workbook.Worksheets.Add(sheetName);
                var mi = typeof(LoanPosition)
                    .GetProperties()
                    .Where(pi => pi.Name != nameof(LoanPosition.FundId)
                                 && pi.Name != nameof(LoanPosition.FundCode)
                                 && pi.Name != nameof(LoanPosition.SecurityId)
                                 && pi.Name != nameof(LoanPosition.IssuerId))
                    .Select(pi => (MemberInfo)pi)
                    .ToArray();
                sheet.Cells[1, 1].LoadFromCollection(data, true, TableStyles.None, BindingFlags.Public, mi);
                var prevDate = DateTime.ParseExact(prevDateId.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                sheet.Cells[1, 5].Value = $"{prevDate:d} Par";
                FormatCurrencyColumns(sheet, new List<int> { 4, 5, 6 });
                sheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
                AddTotal(sheet);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                pkg.SaveAs(new FileInfo(filePath));
                return fileName;
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception has occurred during CLO Calculation");
                throw;
            }
        }

        private void FormatCurrencyColumns(ExcelWorksheet sheet, List<int> indices)
        {
            foreach (var index in indices)
            {
                sheet.Column(index).Style.Numberformat.Format = "#,##0";
            }
        }

        private void AddTotal(ExcelWorksheet sheet)
        {
            var endRow = sheet.Dimension.End.Row;
            var totalRow = endRow + 2;
            sheet.Cells[totalRow, 1].Value = "TOTAL";
            sheet.Cells[totalRow, 4].Formula = $"SUM(D2:D{endRow})";
            sheet.Cells[totalRow, 5].Formula = $"SUM(E2:E{endRow})";
            sheet.Cells[totalRow, 6].Formula = $"SUM(F2:F{endRow})";
            sheet.Cells[totalRow, 1, totalRow, 6].Style.Font.Bold = true;
        }

        public JsonNetResult GetAnalystResearchIssuerIds()
        {
            var ids = _repository.GetAnalystResearchHeaders()
                .Select(x => x.IssuerId);
            return new JsonNetResult { Data = ids };
        }

        public JsonNetResult GetAnalystResearchDetails(int headerId)
        {
            var data = Mapper.Map<List<AnalystResearchDetail>, List<AnalystResearchDetailDto>>(_repository.GetAnalystResearchDetails(headerId)
            .OrderByDescending(x => x.AsOfDate).ToList());
            return new JsonNetResult { Data = data };
        }

        public JsonNetResult GetAnalystResearchHeader(int issuerId)
        {
            var dto = GetAnalystResearchHeaderInternal(issuerId);
            return new JsonNetResult { Data = dto };
        }

        private AnalystResearchHeaderDto GetAnalystResearchHeaderInternal(int issuerId)
        {
            var data = _repository.GetAnalystResearchHeader(issuerId);
            var dto = data != null ? Mapper.Map<AnalystResearchHeader, AnalystResearchHeaderDto>(data) : null;
            var analysts = _repository.GetAnalysts();
            if (dto != null)
            {
                dto.CLOAnalyst = GetAnalystDesc(dto.CLOAnalystId, analysts);
                dto.HFAnalyst = GetAnalystDesc(dto.HFAnalystId, analysts);
            }
            return dto;
        }

        private string GetAnalystDesc(int? id, IEnumerable<vw_YorkCoreGenevaAnalyst> analysts)
            => analysts.FirstOrDefault(x => x.AnalystId == id.GetValueOrDefault())?.AnalystDesc;

        public JsonNetResult GetSummaries()
        {
            try
            {
                _logger.Info("Getting dateId...");
                var dateId = Helper.GetPrevDayDateId();

                _logger.Info($"_repository is null? {_repository == null}");
                var data = CLOCache.GetSummaries();//_repository.GetSummaries(dateId).ToList();

                if (data != null && data.Any())
                {
                    _logger.Info($"Fund Summary data available... ");
                    _logger.Info($"Start Call to Get Reinvest Details... ");
                    var reinvest = _repository.GetReinvestDetails();
                    if (reinvest != null && reinvest.Any())
                    {
                        _logger.Info($"Found {reinvest.Count()} Reinvest Details... ");
                        foreach (var rFile in reinvest)
                        {
                            var rData = data.Where(w => w.FundCode == rFile.FundCode).FirstOrDefault();
                            rData.reInvestCashFilepath = new Uri(Path.Combine(rFile.FilePath, rFile.FileName)).AbsoluteUri;
                            rData.reInvestCash = ReadReinvestData(rFile.FilePath, rFile.FileName, rFile.SheetName, rFile.FieldLocation);
                        }
                    }
                    else _logger.Info($"No Details found for Reinvest... ");
                }
                else _logger.Info($"No Fund Summary data available... ");

                _logger.Info($"data is null? {data == null}");
                if (data != null)
                    _logger.Info($"data count: {data.Count}");
                return new JsonNetResult()
                {
                    Data = data
                };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "CLO:" + ConfigurationManager.AppSettings["Environment"] + ":" + "Exception occurred in GetSummaries");
                throw;
            }
        }


        public JsonNetResult GetTestResults()
        {
            try
            {
                _logger.Info("Getting dateId...");
                var dateId = Helper.GetPrevDayDateId();

                _logger.Info($"_repository is null? {_repository == null}");
                var data = CLOCache.GetTestResults();//_repository.GetSummaries(dateId).ToList();				

                _logger.Info($"data is null? {data == null}");
                if (data != null)
                    _logger.Info($"data count: {data.Count}");
                return new JsonNetResult()
                {
                    Data = data
                };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "CLO:" + ConfigurationManager.AppSettings["Environment"] + ":" + "Exception occurred in GetTestResults");
                throw;
            }
        }


        private decimal ReadReinvestData(string filePath, string fileName, string sheetName, string fieldLocation)
        {
            decimal reinvestValue = 0;
            try
            {
                _logger.Info($"Checking if File Directory {filePath} exist...");
                if (Directory.Exists(filePath))
                {
                    _logger.Info($"File Directory {filePath} exist...");
                    var path = Path.Combine(filePath, fileName);
                    var fileInfo = new FileInfo(path);
                    _logger.Info($"Opening excel file using ExcelPackage...");
                    using (var pkg = new ExcelPackage(fileInfo))
                    {
                        var sheet1 = pkg.Workbook.Worksheets.Cast<ExcelWorksheet>().FirstOrDefault(worksheet => worksheet.Name == sheetName);
                        var sheet = pkg.Workbook.Worksheets[sheetName];
                        if (sheet == null)
                        {
                            _logger.Info($"Sheet with name {sheetName} not exist in workbook {path}...");
                            reinvestValue = 0;
                        }
                        else reinvestValue = Convert.ToDecimal(sheet.Cells[fieldLocation].Value);
                    }
                }
                else _logger.Info($"File Directory {filePath} not exist and no reinvest value found...");
            }
            catch (Exception ex)
            {
                reinvestValue = 0;
                _logger.Error(ex);
            }
            return reinvestValue;
        }


        public JsonNetResult GetMoodyRatings()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Rating>, IEnumerable<RatingDto>>(_repository.GetMoodyRatings()) };
        }

        public JsonNetResult GetRatings()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Rating>, IEnumerable<RatingDto>>(_repository.GetRatings()) };
        }

        public JsonNetResult GetRules()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Rule>, IEnumerable<RuleDto>>(_repository.GetRules()).OrderBy(r => r.SortOrder) };
        }

        public JsonNetResult GetOperators()
        {
            return new JsonNetResult() { Data = _repository.GetOperators().Select(o => new { o.OperatorId, o.OperatorCode, o.OperatorVal }) };
        }

        public JsonNetResult GetFunds()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Fund>, IEnumerable<FundDto>>(_repository.GetFunds()) };
        }

        public JsonNetResult UserIsASuperUser()
        {
            _logger.Info($"Getting permissions for {User.Identity.Name}.");

            string name = System.Web.HttpContext.Current.User.Identity.Name;
            var permissions = _repository.GetPermission(name.Substring(name.IndexOf("\\") + 1).ToLower()).ToList();
            //var permissions = objhm.GetRolesPermissions(User.Identity.Name);
            _logger.Info($"Permissions for {User.Identity.Name}: {permissions}");
            var isSuperUser = permissions.Contains("Admin");
            _logger.Info($"{User.Identity.Name} is a super user: {isSuperUser}.");

            return new JsonNetResult() { Data = isSuperUser };
        }

        public JsonNetResult UserIsAnAdmin()
        {
            string name = System.Web.HttpContext.Current.User.Identity.Name;
            var permissions = _repository.GetPermission(name.Substring(name.IndexOf("\\") + 1).ToLower()).ToList();
            var isAdmin = permissions.Contains("Admin");

            return new JsonNetResult() { Data = isAdmin };
        }

        public JsonNetResult GetCustomViews()
        {
            try
            {
                var views = _repository.GetCustomViews().Where(x => x.IsPublic || x.UserId == UserId || x.UserId == 0).ToList();
                var def = _repository.GetUserDefaultCustomViews().FirstOrDefault(x => x.UserId == UserId);
                foreach (var view in views)
                {
                    view.IsDefault = false;
                }

                if (def != null)
                {
                    var defView = views.FirstOrDefault(x => x.ViewId == def.ViewId);
                    if (defView != null)
                        defView.IsDefault = true;
                }

                var result = new JsonNetResult { Data = views };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public JsonNetResult GetCustomView(int viewId)
        {
            var views = _repository.GetCustomViews().ToList();
            var view = views.FirstOrDefault(x => x.ViewId == viewId);
            //var def = _repository.GetUserDefaultCustomViews().FirstOrDefault(x => x.UserId == UserId);
            //view.IsDefault = def != null && view != null && def.ViewId == view.ViewId;

            var result = new JsonNetResult { Data = view };
            return result;
        }

        public JsonNetResult GetPerson()
        {
            var perDetails = _repository.GetPerson(User.Identity.Name);
            var result = new JsonNetResult { Data = perDetails };
            return result;
        }

        private User Person => _repository.GetPerson(User.Identity.Name);
        private int UserId => Person.UserId;

        public JsonNetResult ViewNameIsTaken(string viewName)
        {
            var isTaken = _repository.ViewNameIsTaken(UserId, viewName);
            var result = new JsonNetResult { Data = isTaken };
            return result;
        }

        [System.Web.Http.HttpPost]
        public JsonNetResult SaveCustomView(CustomView view)
        {
            view.UserId = UserId;
            var data = _repository.AddOrUpdateCustomView(view, Person.FullName);
            return new JsonNetResult() { Data = data };
        }

        [HttpPost]
        public JsonNetResult DeleteCustomView(CustomView view)
        {
            view.UserId = UserId;
            var data = _repository.DeleteCustomView(view);
            return new JsonNetResult() { Data = data };
        }

        [HttpPost]
        public JsonNetResult SaveFund(Fund fund)
        {
            var savedFund = Mapper.Map<Fund, FundDto>(_repository.SaveFund(fund));
            _repository.UpdateFundTriggersForMatrixPoint(savedFund.FundId);
            Calculate(_repository.GetPrevDayDateId());
            return new JsonNetResult() { Data = savedFund };
        }

        public JsonNetResult GetIssuers()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<Issuer>, IEnumerable<IssuerDto>>(_repository.GetIssuers().Where(i => i.IssuerId > 0).OrderBy(i => i.IssuerDesc))
            };
        }

        public JsonNetResult Calculate(int dateId)
        {
            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            return new JsonNetResult() { Data = new { status = calculationEngineClient.Calculate(dateId, User.Identity.Name) } };
        }


        public JsonNetResult GetTrends(int trendTypeId, DateTime startDate, DateTime endDate, int periodId)
        {
            try
            {
                _logger.Info("Getting dateId...");
                var dateId = Helper.GetPrevDayDateId();

                _logger.Info($"_repository is null? {_repository == null}");
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Trends>, IEnumerable<Trends>>(_repository.GetTrends(trendTypeId, startDate, endDate, periodId)) };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception occurred in GetTrends");
                throw;
            }
        }

        public JsonNetResult GetTrendTypes()
        {
            try
            {
                _logger.Info($"_repository is null? {_repository == null}");
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<TrendType>, IEnumerable<TrendType>>(_repository.GetTrendTypes()) };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception occurred in GetTrendTypes");
                throw;
            }
        }

        public JsonNetResult GetTrendPeriod()
        {
            try
            {
                _logger.Info($"_repository is null? {_repository == null}");
                var check = new JsonNetResult() { Data = Mapper.Map<IEnumerable<TrendPeriod>, IEnumerable<TrendPeriod>>(_repository.GetTrendPeriod()) };
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<TrendPeriod>, IEnumerable<TrendPeriod>>(_repository.GetTrendPeriod()) };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception occurred in GetTrendPeriod");
                throw;
            }
        }

        public JsonNetResult GetRatingChanges(int? startDateId = null, int? endDateId = null)
        {
            try
            {
                if (startDateId == null)
                {
                    var today = DateTime.Today;

                    if (today.DayOfWeek == DayOfWeek.Monday)
                    {
                        startDateId = Helper.GetDateId(today.AddDays(-4));
                    }
                    else
                    {
                        startDateId = Helper.GetDateId(today.AddDays(-2));
                    }
                }

                if (endDateId == null)
                {
                    var today = DateTime.Today;
                    endDateId = Helper.GetDateId(today.AddDays(-1));
                }
                _logger.Info($"_repository is null? {_repository == null}");
                //var check = new JsonNetResult() { Data = Mapper.Map<IEnumerable<RatingChange>, IEnumerable<RatingChange>>(_repository.GetRatingChanges(startDateId.Value, endDateId.Value)) };
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<RatingChange>, IEnumerable<RatingChange>>(_repository.GetRatingChanges(startDateId.Value, endDateId.Value)) };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception occurred in GetRatingChanges");
                throw;
            }
        }

        public JsonNetResult GetTotalParDifferenceforUI(int? startDateId = null, int? endDateId = null)
        {
            try
            {
                TotalParChanges totalParChanges = new TotalParChanges();
                int fromDateId = Helper.GetPrevDayDateId();
                int toDateId;

                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                {
                    toDateId = Helper.GetDateId(Helper.GetPrevBusinessDay(3));
                }
                else
                {
                    toDateId = Helper.GetDateId(Helper.GetPrevBusinessDay(1));
                }

                DateTime Prev5BusinessDay = new DateTime();
                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                {
                    Prev5BusinessDay = Helper.GetPrevBusinessDay(7);
                }
                else
                {
                    Prev5BusinessDay = Helper.GetPrevBusinessDay(5);
                }

                int to5DateId = Helper.GetDateId(Prev5BusinessDay);
                totalParChanges.PrevTotalParChange = _repository.GetTotalParDifferenceforUI(fromDateId, toDateId);
                totalParChanges.PrevTotal = (totalParChanges.PrevTotalParChange.Count() > 0);
                totalParChanges.Prev5TotalParChange = _repository.GetTotalParDifferenceforUI(fromDateId, to5DateId);
                totalParChanges.Prev5Total = (totalParChanges.Prev5TotalParChange.Count() > 0);

                return new JsonNetResult() { Data = totalParChanges };

                //if (endDateId == null)
                //{
                //    var today = DateTime.Today;

                //    if (today.DayOfWeek == DayOfWeek.Monday)
                //    {
                //        endDateId = Helper.GetDateId(today.AddDays(-4));
                //    }
                //    else
                //    {
                //        endDateId = Helper.GetDateId(today.AddDays(-2));
                //    }
                //}

                //if (startDateId == null)
                //{
                //    var today = DateTime.Today;
                //    startDateId = Helper.GetDateId(today.AddDays(-1));
                //}
                //_logger.Info($"_repository is null? {_repository == null}");
                ////var check = new JsonNetResult() { Data = Mapper.Map<IEnumerable<TotalParChange>, IEnumerable<TotalParChange>>(_repository.GetTotalParChange(startDateId.Value, endDateId.Value)) };
                //return new JsonNetResult() { Data = Mapper.Map<IEnumerable<TotalParChange>, IEnumerable<TotalParChange>>(_repository.GetTotalParChange(startDateId.Value, endDateId.Value)) };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception occurred in GetTotalParChanges");
                throw;
            }
        }

        public JsonNetResult GetMoodyRecoveryDateChanges(int? startDateId = null, int? endDateId = null)
        {
            try
            {
                if (endDateId == null)
                {
                    var today = DateTime.Today;

                    if (today.DayOfWeek == DayOfWeek.Monday)
                    {
                        endDateId = Helper.GetDateId(today.AddDays(-4));
                    }
                    else
                    {
                        endDateId = Helper.GetDateId(today.AddDays(-2));
                    }
                }

                if (startDateId == null)
                {
                    var today = DateTime.Today;
                    startDateId = Helper.GetDateId(today.AddDays(-1));
                }
                _logger.Info($"_repository is null? {_repository == null}");
                //var check = new JsonNetResult() { Data = Mapper.Map<IEnumerable<MoodyRecoveryChange>, IEnumerable<MoodyRecoveryChange>>(_repository.GetMoodyRecoveryChange(startDateId.Value, endDateId.Value)) };
                return new JsonNetResult() { Data = Mapper.Map<IEnumerable<MoodyRecoveryChange>, IEnumerable<MoodyRecoveryChange>>(_repository.GetMoodyRecoveryChange(startDateId.Value, endDateId.Value)) };
            }
            catch (Exception exception)
            {
                EmailHelper.SendEmail(exception.ToString(), "Exception occurred in GetMoodyRecoveryDateChanges");
                throw;
            }
        }

        public JsonNetResult GetTopBottomPriceMovers()
        {
            TopBottomPriceMovers priceMovers = new TopBottomPriceMovers();
            int fromDateId = Helper.GetPrevDayDateId();
            int toDateId;

            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                toDateId = Helper.GetDateId(Helper.GetPrevBusinessDay(3));
            }
            else
            {
                toDateId = Helper.GetDateId(Helper.GetPrevBusinessDay(1));
            }

            var funds = _repository.GetFunds();

            DateTime Prev5BusinessDay = new DateTime();
            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                Prev5BusinessDay = Helper.GetPrevBusinessDay(7);
            }
            else
            {
                Prev5BusinessDay = Helper.GetPrevBusinessDay(5);
            }

            int to5DateId = Helper.GetDateId(Prev5BusinessDay);

            priceMovers.PrevDayTop = _repository.GetPriceMove("Top", fromDateId, toDateId);
            priceMovers.PrevDayBottom = _repository.GetPriceMove("Bottom", fromDateId, toDateId);
            priceMovers.IsStale = funds.Any(f => ((f.IsStale.HasValue && f.IsStale.Value) || (f.IsPrincipalCashStale.HasValue && f.IsPrincipalCashStale.Value)));

            priceMovers.Prev5BusinessDay = Prev5BusinessDay.ToShortDateString();
            priceMovers.Prev5DayTop = _repository.GetPriceMove("Top", fromDateId, to5DateId);
            priceMovers.Prev5DayBottom = _repository.GetPriceMove("Bottom", fromDateId, to5DateId);

            return new JsonNetResult() { Data = priceMovers };
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
