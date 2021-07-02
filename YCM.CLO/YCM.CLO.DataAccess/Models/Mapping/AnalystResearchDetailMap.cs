using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AnalystResearchDetailMap : EntityTypeConfiguration<AnalystResearchDetail>
	{
		public AnalystResearchDetailMap()
		{
			// Primary Key
			HasKey(t => t.AnalystResearchDetailId);

			// Properties
			Property(t => t.CreatedBy)
				.HasMaxLength(100);

			Property(t => t.LastUpdatedBy)
				.HasMaxLength(100);

			// Table & Column Mappings
			ToTable(nameof(AnalystResearchDetail), "CLO");
			Property(t => t.AnalystResearchDetailId)
				.HasColumnName(nameof(AnalystResearchDetail.AnalystResearchDetailId));
			Property(t => t.AnalystResearchHeaderId).HasColumnName(nameof(AnalystResearchDetail.AnalystResearchHeaderId));
			Property(t => t.AsOfDate).HasColumnName(nameof(AnalystResearchDetail.AsOfDate));
			Property(t => t.Revenues).HasColumnName(nameof(AnalystResearchDetail.Revenues)).HasPrecision(38, 4);
			Property(t => t.YoYGrowth).HasColumnName(nameof(AnalystResearchDetail.YoYGrowth)).HasPrecision(10, 4);
			Property(t => t.OrganicGrowth).HasColumnName(nameof(AnalystResearchDetail.OrganicGrowth)).HasPrecision(10, 4);
			Property(t => t.CashEBITDA).HasColumnName(nameof(AnalystResearchDetail.CashEBITDA)).HasPrecision(38, 4);
			Property(t => t.Margin).HasColumnName(nameof(AnalystResearchDetail.Margin)).HasPrecision(10, 4);
			Property(t => t.TransactionExpenses).HasColumnName(nameof(AnalystResearchDetail.TransactionExpenses)).HasPrecision(10, 4);
			Property(t => t.RestructuringAndIntegration).HasColumnName(nameof(AnalystResearchDetail.RestructuringAndIntegration)).HasPrecision(10, 4);
			Property(t => t.Other1).HasColumnName(nameof(AnalystResearchDetail.Other1)).HasPrecision(10, 4);
			Property(t => t.Other2).HasColumnName(nameof(AnalystResearchDetail.Other2)).HasPrecision(10, 4);
			Property(t => t.PFEBITDA).HasColumnName(nameof(AnalystResearchDetail.PFEBITDA)).HasPrecision(38, 4);
			Property(t => t.LTMPFEBITDA).HasColumnName(nameof(AnalystResearchDetail.LTMPFEBITDA)).HasPrecision(38, 4);
			Property(t => t.PFCostSaves).HasColumnName(nameof(AnalystResearchDetail.PFCostSaves)).HasPrecision(10, 4);
			Property(t => t.PFAcquisitionAdjustment).HasColumnName(nameof(AnalystResearchDetail.PFAcquisitionAdjustment)).HasPrecision(10, 4);
			Property(t => t.CovenantEBITDA).HasColumnName(nameof(AnalystResearchDetail.CovenantEBITDA)).HasPrecision(38, 4);
			Property(t => t.Interest).HasColumnName(nameof(AnalystResearchDetail.Interest)).HasPrecision(10, 4);
			Property(t => t.CashTaxes).HasColumnName(nameof(AnalystResearchDetail.CashTaxes)).HasPrecision(10, 4);
			Property(t => t.WorkingCapital).HasColumnName(nameof(AnalystResearchDetail.WorkingCapital)).HasPrecision(38, 4);
			Property(t => t.RestructuringOneTime).HasColumnName(nameof(AnalystResearchDetail.RestructuringOneTime)).HasPrecision(10, 4);
			Property(t => t.OCF).HasColumnName(nameof(AnalystResearchDetail.OCF)).HasPrecision(38, 4);
			Property(t => t.CapitalExpenditures).HasColumnName(nameof(AnalystResearchDetail.CapitalExpenditures)).HasPrecision(38, 4);
			Property(t => t.FCF).HasColumnName(nameof(AnalystResearchDetail.FCF)).HasPrecision(38, 4);
			Property(t => t.ABLRCF).HasColumnName(nameof(AnalystResearchDetail.ABLRCF)).HasPrecision(38, 4);
			Property(t => t.FirstLienDebt).HasColumnName(nameof(AnalystResearchDetail.FirstLienDebt)).HasPrecision(38, 4);
			Property(t => t.TotalDebt).HasColumnName(nameof(AnalystResearchDetail.TotalDebt)).HasPrecision(38, 4);
			Property(t => t.EquityMarketCap).HasColumnName(nameof(AnalystResearchDetail.EquityMarketCap)).HasPrecision(38, 4);
			Property(t => t.Cash).HasColumnName(nameof(AnalystResearchDetail.Cash)).HasPrecision(38, 4);
			Property(t => t.LTMRevenues).HasColumnName(nameof(AnalystResearchDetail.LTMRevenues)).HasPrecision(38, 4);
			Property(t => t.LTMEBITDA).HasColumnName(nameof(AnalystResearchDetail.LTMEBITDA)).HasPrecision(38, 4);
			Property(t => t.LTMFCF).HasColumnName(nameof(AnalystResearchDetail.LTMFCF)).HasPrecision(38, 4);
			Property(t => t.SeniorLeverage).HasColumnName(nameof(AnalystResearchDetail.SeniorLeverage)).HasPrecision(10, 4);
			Property(t => t.TotalLeverage).HasColumnName(nameof(AnalystResearchDetail.TotalLeverage)).HasPrecision(10, 4);
			Property(t => t.NetTotalLeverage).HasColumnName(nameof(AnalystResearchDetail.NetTotalLeverage)).HasPrecision(10, 4);
			Property(t => t.FCFDebt).HasColumnName(nameof(AnalystResearchDetail.FCFDebt)).HasPrecision(10, 4);
			Property(t => t.EstimatedEnterpriseValue).HasColumnName(nameof(AnalystResearchDetail.EstimatedEnterpriseValue)).HasPrecision(10, 4);
			Property(t => t.EnterpriseValue).HasColumnName(nameof(AnalystResearchDetail.EnterpriseValue)).HasPrecision(10, 4);
			Property(t => t.Comments).HasColumnName(nameof(AnalystResearchDetail.Comments));
			Property(t => t.CreatedOn).HasColumnName(nameof(AnalystResearchDetail.CreatedOn));
			Property(t => t.CreatedBy).HasColumnName(nameof(AnalystResearchDetail.CreatedBy));
			Property(t => t.LastUpdatedOn).HasColumnName(nameof(AnalystResearchDetail.LastUpdatedOn));
			Property(t => t.LastUpdatedBy).HasColumnName(nameof(AnalystResearchDetail.LastUpdatedBy));

			// Relationships
			HasRequired(t => t.AnalystResearchHeader)
				.WithMany(t => t.AnalystResearchDetails)
				.HasForeignKey(d => d.AnalystResearchHeaderId);

		}
	}
}
