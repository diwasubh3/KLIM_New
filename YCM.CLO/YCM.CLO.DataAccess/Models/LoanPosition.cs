using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public class LoanPosition
	{
		public int FundId { get; set; }
		public string FundCode { get; set; }
		public int SecurityId { get; set; }
		[DisplayName("Loanx")]
		public string SecurityCode { get; set; }
		public int IssuerId { get; set; }
		public string Issuer { get; set; }
		public string Facility { get; set; }
		[DisplayName("Current Par")]
		public decimal Exposure { get; set; }
		public decimal PriorExposure { get; set; }
		[NotMapped]
		public decimal Difference => Exposure - PriorExposure;
		public override string ToString() => $"{SecurityCode} - {Issuer} - {Facility} - {Exposure:C0} - {PriorExposure:C0}";
	}
}
