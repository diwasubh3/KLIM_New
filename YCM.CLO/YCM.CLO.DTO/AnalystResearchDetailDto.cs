using System;

namespace YCM.CLO.DTO
{
	public class AnalystResearchDetailDto
	{

		//public int AnalystResearchDetailId { get; set; }

		//public int AnalystResearchHeaderId { get; set; }

		public string AsOfDate { get; set; }

		public string Spacer1 => string.Empty;
		public string SeniorLeverage { get; set; }

		public string TotalLeverage { get; set; }

		public string NetTotalLeverage { get; set; }

		public string FCFDebt { get; set; }

		//public string EstimatedEnterpriseValue { get; set; }

		public string EnterpriseValue { get; set; }
		public string Spacer2 => string.Empty;

		public string LTMRevenues { get; set; }

		public string LTMEBITDA { get; set; }

		public string LTMFCF { get; set; }

		public string Spacer3 => string.Empty;
		public string Revenues { get; set; }

		public string YoYGrowth { get; set; }

		public string OrganicGrowth { get; set; }

		public string Spacer4 => string.Empty;
		public string CashEBITDA { get; set; }

		public string Margin { get; set; }

		public string Spacer5 => string.Empty;
		public string TransactionExpenses { get; set; }

		public string RestructuringAndIntegration { get; set; }

		public string Other1 { get; set; }

		public string PFEBITDA { get; set; }

		public string Spacer6 => string.Empty;
		public string LTMPFEBITDA { get; set; }

		public string PFCostSaves { get; set; }

		public string PFAcquisitionAdjustment { get; set; }

		public string CovenantEBITDA { get; set; }

		public string Spacer7 => string.Empty;
		public string Interest { get; set; }

		public string CashTaxes { get; set; }

		public string WorkingCapital { get; set; }

		public string RestructuringOneTime { get; set; }

		public string Other2 { get; set; }

		public string OCF { get; set; }

		public string CapitalExpenditures { get; set; }

		public string FCF { get; set; }

		public string ABLRCF { get; set; }

		public string FirstLienDebt { get; set; }

		public string TotalDebt { get; set; }

		public string EquityMarketCap { get; set; }

		public string Cash { get; set; }

		public string Comments { get; set; }

		public DateTime LastUpdatedOn { get; set; }
		public override string ToString()
			=> $"As Of: {AsOfDate:dd MMM yyyy} Revenues: {Revenues} Total Leverage: {TotalLeverage}";
		//=> $"Id: {AnalystResearchDetailId} Header Id: {AnalystResearchHeaderId} As Of: {AsOfDate:dd MMM yyyy} Revenues: {Revenues} Total Leverage: {TotalLeverage}";
	}
}
