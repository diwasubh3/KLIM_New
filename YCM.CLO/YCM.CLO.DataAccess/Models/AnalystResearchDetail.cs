using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class AnalystResearchDetail : Entity
    {
	    [NotMapped]
	    public override int Id
	    {
			get { return AnalystResearchDetailId; }
			set { AnalystResearchDetailId = value; }
	    }

		public int AnalystResearchDetailId { get; set; }

        public int AnalystResearchHeaderId { get; set; }

        public DateTime AsOfDate { get; set; }

        public decimal? Revenues { get; set; }

        public decimal? YoYGrowth { get; set; }

        public decimal? OrganicGrowth { get; set; }

        public decimal? CashEBITDA { get; set; }

        public decimal? Margin { get; set; }

        public decimal? TransactionExpenses { get; set; }

        public decimal? RestructuringAndIntegration { get; set; }

        public decimal? Other1 { get; set; }

		public decimal? Other2 { get; set; }

		public decimal? PFEBITDA { get; set; }

        public decimal? LTMPFEBITDA { get; set; }

        public decimal? PFCostSaves { get; set; }

        public decimal? PFAcquisitionAdjustment { get; set; }

        public decimal? CovenantEBITDA { get; set; }

        public decimal? Interest { get; set; }

        public decimal? CashTaxes { get; set; }

        public decimal? WorkingCapital { get; set; }

        public decimal? RestructuringOneTime { get; set; }

        public decimal? OCF { get; set; }

        public decimal? CapitalExpenditures { get; set; }

        public decimal? FCF { get; set; }

        public decimal? ABLRCF { get; set; }

        public decimal? FirstLienDebt { get; set; }

        public decimal? TotalDebt { get; set; }

        public decimal? EquityMarketCap { get; set; }

        public decimal? Cash { get; set; }

        public decimal? LTMRevenues { get; set; }

        public decimal? LTMEBITDA { get; set; }

        public decimal? LTMFCF { get; set; }

        public decimal? SeniorLeverage { get; set; }

        public decimal? TotalLeverage { get; set; }

        public decimal? NetTotalLeverage { get; set; }

        public decimal? FCFDebt { get; set; }

        public decimal? EstimatedEnterpriseValue { get; set; }

        public decimal? EnterpriseValue { get; set; }

        public string Comments { get; set; }

        public virtual AnalystResearchHeader AnalystResearchHeader { get; set; }

		public override string ToString()
		=> $"Id: {AnalystResearchDetailId} Header Id: {AnalystResearchHeaderId} As Of: {AsOfDate:dd MMM yyyy} Revenues: {Revenues} Total Leverage: {TotalLeverage}";
	}
}
