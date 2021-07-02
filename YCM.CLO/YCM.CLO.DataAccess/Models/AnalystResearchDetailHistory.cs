using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class AnalystResearchDetailHistory : Entity
	{
		public AnalystResearchDetailHistory(AnalystResearchDetail detail, DatabaseOperation operation)
		{
			AnalystResearchDetailId = detail.AnalystResearchDetailId;
			AnalystResearchHeaderId = detail.AnalystResearchHeaderId;
			AsOfDate = detail.AsOfDate;
			Revenues = detail.Revenues;
			YoYGrowth = detail.YoYGrowth;
			OrganicGrowth = detail.OrganicGrowth;
			CashEBITDA = detail.CashEBITDA;
			Margin = detail.Margin;
			TransactionExpenses = detail.TransactionExpenses;
			RestructuringAndIntegration = detail.RestructuringAndIntegration;
			Other1 = detail.Other1;
			Other2 = detail.Other2;
			PFEBITDA = detail.PFEBITDA;
			LTMPFEBITDA = detail.LTMPFEBITDA;
			PFCostSaves = detail.PFCostSaves;
			PFAcquisitionAdjustment = detail.PFAcquisitionAdjustment;
			CovenantEBITDA = detail.CovenantEBITDA;
			Interest = detail.Interest;
			CashTaxes = detail.CashTaxes;
			WorkingCapital = detail.WorkingCapital;
			RestructuringOneTime = detail.RestructuringOneTime;
			OCF = detail.OCF;
			CapitalExpenditures = detail.CapitalExpenditures;
			FCF = detail.FCF;
			ABLRCF = detail.ABLRCF;
			FirstLienDebt = detail.FirstLienDebt;
			TotalDebt = detail.TotalDebt;
			EquityMarketCap = detail.EquityMarketCap;
			Cash = detail.Cash;
			LTMRevenues = detail.LTMRevenues;
			LTMEBITDA = detail.LTMEBITDA;
			LTMFCF = detail.LTMFCF;
			SeniorLeverage = detail.SeniorLeverage;
			TotalLeverage = detail.TotalLeverage;
			NetTotalLeverage = detail.NetTotalLeverage;
			FCFDebt = detail.FCFDebt;
			EstimatedEnterpriseValue = detail.EstimatedEnterpriseValue;
			EnterpriseValue = detail.EnterpriseValue;
			Comments = detail.Comments;
			CreatedOn = detail.CreatedOn;
			CreatedBy = detail.CreatedBy;
			LastUpdatedOn = detail.LastUpdatedOn;
			LastUpdatedBy = detail.LastUpdatedBy;
			Operation = operation.ToString();
		}

		[NotMapped]
		public override int Id
		{
			get { return AnalystResearchDetailHistoryId; }
			set { AnalystResearchDetailHistoryId = value; }
		}

		public int AnalystResearchDetailHistoryId { get; set; }
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

		public string Operation { get; set; }
		public override string ToString()
		=> $"Header Id: {AnalystResearchHeaderId} Op: {Operation} As Of: {AsOfDate:dd MMM yyyy} Revenues: {Revenues} Total Leverage: {TotalLeverage}";
	}
}
