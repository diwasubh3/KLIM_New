using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace YCM.CLO.DataAccess.Models
{
    public partial class AnalystResearchImportLog : Entity
    {
	    public AnalystResearchImportLog(string fileName, string sheetName, string issuerName
			, DateTime createdOn, string createdBy, string importError = null, bool processed = false)
	    {
		    AnalystResearchFileName = fileName;
		    SheetName = sheetName;
		    IssuerName = issuerName;
		    CreatedOn = createdOn;
		    CreatedBy = createdBy;
		    ImportError = importError;
		    Processed = processed;
	    }

		[NotMapped]
		public override int Id
		{
			get { return AnalystResearchImportLogId; }
			set { AnalystResearchImportLogId = value; }
		}
		public int AnalystResearchImportLogId { get; set; }

        public string AnalystResearchFileName { get; set; }

        public string SheetName { get; set; }

        public string IssuerName { get; set; }
		public bool Processed { get; set; }
		public string ImportError { get; set; }

		[NotMapped]
		public new DateTime? LastUpdatedOn { get; set; }

		[NotMapped]
		public new string LastUpdatedBy { get; set; }

		public string ToHtmlTableRow()
		=> $"<tr><td>{Path.GetFileName(AnalystResearchFileName)}</td><td>{IssuerName}</td>"
			+ $"<td>{HttpUtility.HtmlEncode(SheetName)}</td><td>{ImportError ?? string.Empty}</td></tr>";

		public override string ToString()
			=> $"{AnalystResearchFileName}: {SheetName} - {Processed}: {ImportError ?? string.Empty}";

	}
}
