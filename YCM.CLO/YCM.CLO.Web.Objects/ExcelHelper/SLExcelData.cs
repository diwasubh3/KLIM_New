using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;

namespace YCM.CLO.Web.Objects.ExcelHelper
{
    public class SlExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    public class SlExcelData
    {
        public SlExcelStatus Status { get; set; }
        public Columns ColumnConfigurations { get; set; }
        public List<string> PreHeaders { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        public SlExcelData()
        {
            Status      = new SlExcelStatus();
            PreHeaders  = new List<string>();
            Headers     = new List<string>();
            DataRows    = new List<List<string>>();
        }
    }
}