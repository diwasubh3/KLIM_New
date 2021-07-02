using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AnalystResearchDetailHistoryMap : EntityTypeConfiguration<AnalystResearchDetailHistory>
	{
		public AnalystResearchDetailHistoryMap()
		{
			// Primary Key
			HasKey(t => t.AnalystResearchDetailHistoryId);

			// Properties
			Property(t => t.CreatedBy)
				.HasMaxLength(100);

			Property(t => t.LastUpdatedBy)
				.HasMaxLength(100);

			// Table & Column Mappings
			ToTable(nameof(AnalystResearchDetailHistory), "CLO");
			Property(t => t.AnalystResearchDetailHistoryId)
				.HasColumnName(nameof(AnalystResearchDetailHistory.AnalystResearchDetailHistoryId));
			Property(t => t.AnalystResearchDetailId)
				.HasColumnName(nameof(AnalystResearchDetailHistory.AnalystResearchDetailId));
			Property(t => t.AnalystResearchHeaderId).HasColumnName(nameof(AnalystResearchDetailHistory.AnalystResearchHeaderId));
			Property(t => t.AsOfDate).HasColumnName(nameof(AnalystResearchDetailHistory.AsOfDate));
			Property(t => t.Revenues).HasColumnName(nameof(AnalystResearchDetailHistory.Revenues)).HasPrecision(38, 4);
			Property(t => t.YoYGrowth).HasColumnName(nameof(AnalystResearchDetailHistory.YoYGrowth)).HasPrecision(10, 4);
			Property(t => t.OrganicGrowth).HasColumnName(nameof(AnalystResearchDetailHistory.OrganicGrowth)).HasPrecision(10, 4);
			Property(t => t.CashEBITDA).HasColumnName(nameof(AnalystResearchDetailHistory.CashEBITDA)).HasPrecision(38, 4);
			Property(t => t.Margin).HasColumnName(nameof(AnalystResearchDetailHistory.Margin)).HasPrecision(10, 4);
			Property(t => t.TransactionExpenses).HasColumnName(nameof(AnalystResearchDetailHistory.TransactionExpenses)).HasPrecision(10, 4);
			Property(t => t.RestructuringAndIntegration).HasColumnName(nameof(AnalystResearchDetailHistory.RestructuringAndIntegration)).HasPrecision(10, 4);
			Property(t => t.Other1).HasColumnName(nameof(AnalystResearchDetailHistory.Other1)).HasPrecision(10, 4);
			Property(t => t.Other2).HasColumnName(nameof(AnalystResearchDetailHistory.Other2)).HasPrecision(10, 4);
			Property(t => t.PFEBITDA).HasColumnName(nameof(AnalystResearchDetailHistory.PFEBITDA)).HasPrecision(38, 4);
			Property(t => t.LTMPFEBITDA).HasColumnName(nameof(AnalystResearchDetailHistory.LTMPFEBITDA)).HasPrecision(38, 4);
			Property(t => t.PFCostSaves).HasColumnName(nameof(AnalystResearchDetailHistory.PFCostSaves)).HasPrecision(10, 4);
			Property(t => t.PFAcquisitionAdjustment).HasColumnName(nameof(AnalystResearchDetailHistory.PFAcquisitionAdjustment)).HasPrecision(10, 4);
			Property(t => t.CovenantEBITDA).HasColumnName(nameof(AnalystResearchDetailHistory.CovenantEBITDA)).HasPrecision(38, 4);
			Property(t => t.Interest).HasColumnName(nameof(AnalystResearchDetailHistory.Interest)).HasPrecision(10, 4);
			Property(t => t.CashTaxes).HasColumnName(nameof(AnalystResearchDetailHistory.CashTaxes)).HasPrecision(10, 4);
			Property(t => t.WorkingCapital).HasColumnName(nameof(AnalystResearchDetailHistory.WorkingCapital)).HasPrecision(38, 4);
			Property(t => t.RestructuringOneTime).HasColumnName(nameof(AnalystResearchDetailHistory.RestructuringOneTime)).HasPrecision(10, 4);
			Property(t => t.OCF).HasColumnName(nameof(AnalystResearchDetailHistory.OCF)).HasPrecision(38, 4);
			Property(t => t.CapitalExpenditures).HasColumnName(nameof(AnalystResearchDetailHistory.CapitalExpenditures)).HasPrecision(38, 4);
			Property(t => t.FCF).HasColumnName(nameof(AnalystResearchDetailHistory.FCF)).HasPrecision(38, 4);
			Property(t => t.ABLRCF).HasColumnName(nameof(AnalystResearchDetailHistory.ABLRCF)).HasPrecision(38, 4);
			Property(t => t.FirstLienDebt).HasColumnName(nameof(AnalystResearchDetailHistory.FirstLienDebt)).HasPrecision(38, 4);
			Property(t => t.TotalDebt).HasColumnName(nameof(AnalystResearchDetailHistory.TotalDebt)).HasPrecision(38, 4);
			Property(t => t.EquityMarketCap).HasColumnName(nameof(AnalystResearchDetailHistory.EquityMarketCap)).HasPrecision(38, 4);
			Property(t => t.Cash).HasColumnName(nameof(AnalystResearchDetailHistory.Cash)).HasPrecision(38, 4);
			Property(t => t.LTMRevenues).HasColumnName(nameof(AnalystResearchDetailHistory.LTMRevenues)).HasPrecision(38, 4);
			Property(t => t.LTMEBITDA).HasColumnName(nameof(AnalystResearchDetailHistory.LTMEBITDA)).HasPrecision(38, 4);
			Property(t => t.LTMFCF).HasColumnName(nameof(AnalystResearchDetailHistory.LTMFCF)).HasPrecision(38, 4);
			Property(t => t.SeniorLeverage).HasColumnName(nameof(AnalystResearchDetailHistory.SeniorLeverage)).HasPrecision(10, 4);
			Property(t => t.TotalLeverage).HasColumnName(nameof(AnalystResearchDetailHistory.TotalLeverage)).HasPrecision(10, 4);
			Property(t => t.NetTotalLeverage).HasColumnName(nameof(AnalystResearchDetailHistory.NetTotalLeverage)).HasPrecision(10, 4);
			Property(t => t.FCFDebt).HasColumnName(nameof(AnalystResearchDetailHistory.FCFDebt)).HasPrecision(10, 4);
			Property(t => t.EstimatedEnterpriseValue).HasColumnName(nameof(AnalystResearchDetailHistory.EstimatedEnterpriseValue)).HasPrecision(10, 4);
			Property(t => t.EnterpriseValue).HasColumnName(nameof(AnalystResearchDetailHistory.EnterpriseValue)).HasPrecision(10, 4);
			Property(t => t.Comments).HasColumnName(nameof(AnalystResearchDetailHistory.Comments));
			Property(t => t.CreatedOn).HasColumnName(nameof(AnalystResearchDetailHistory.CreatedOn));
			Property(t => t.CreatedBy).HasColumnName(nameof(AnalystResearchDetailHistory.CreatedBy));
			Property(t => t.LastUpdatedOn).HasColumnName(nameof(AnalystResearchDetailHistory.LastUpdatedOn));
			Property(t => t.LastUpdatedBy).HasColumnName(nameof(AnalystResearchDetailHistory.LastUpdatedBy));
			Property(t => t.Operation).HasColumnName(nameof(AnalystResearchDetailHistory.Operation));

		}
	}
}
