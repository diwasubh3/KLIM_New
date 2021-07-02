using System;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace YCM.CLO.Web.Models
{
	public class CustomFileResult : FileContentResult
	{
		public CustomFileResult(byte[] fileContents, string contentType) : base(fileContents, contentType)
		{
		}

		public bool Inline { get; set; }

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = ContentType;
			if (!string.IsNullOrEmpty(FileDownloadName))
			{
				string str = new ContentDisposition { FileName = this.FileDownloadName, Inline = Inline }.ToString();
				context.HttpContext.Response.AddHeader("Content-Disposition", str);
			}
			WriteFile(response);
		}
	}
}