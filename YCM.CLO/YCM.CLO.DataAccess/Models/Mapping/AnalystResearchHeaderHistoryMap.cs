using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AnalystResearchHeaderHistoryMap : EntityTypeConfiguration<AnalystResearchHeaderHistory>
	{
		public AnalystResearchHeaderHistoryMap()
		{
			// Primary Key
			HasKey(t => t.AnalystResearchHeaderHistoryId);

			// Properties
			Property(t => t.CreatedBy)
				.HasMaxLength(100);

			Property(t => t.LastUpdatedBy)
				.HasMaxLength(100);

			// Table & Column Mappings
			ToTable(nameof(AnalystResearchHeaderHistory), "CLO");
			Property(t => t.AnalystResearchHeaderHistoryId)
				.HasColumnName(nameof(AnalystResearchHeaderHistory.AnalystResearchHeaderHistoryId));
			Property(t => t.AnalystResearchHeaderId)
				.HasColumnName(nameof(AnalystResearchHeaderHistory.AnalystResearchHeaderId));
			Property(t => t.IssuerId).HasColumnName(nameof(AnalystResearchHeaderHistory.IssuerId));
			Property(t => t.CLOAnalystId).HasColumnName(nameof(AnalystResearchHeaderHistory.CLOAnalystId));
			Property(t => t.HFAnalystId).HasColumnName(nameof(AnalystResearchHeaderHistory.HFAnalystId));
			Property(t => t.CreditScore).HasColumnName(nameof(AnalystResearchHeaderHistory.CreditScore));
			Property(t => t.AgentBank).HasColumnName(nameof(AnalystResearchHeaderHistory.AgentBank));
			Property(t => t.BusinessDescription).HasColumnName(nameof(AnalystResearchHeaderHistory.BusinessDescription));
			Property(t => t.CreatedOn).HasColumnName(nameof(AnalystResearchHeaderHistory.CreatedOn));
			Property(t => t.CreatedBy).HasColumnName(nameof(AnalystResearchHeaderHistory.CreatedBy));
			Property(t => t.LastUpdatedOn).HasColumnName(nameof(AnalystResearchHeaderHistory.LastUpdatedOn));
			Property(t => t.LastUpdatedBy).HasColumnName(nameof(AnalystResearchHeaderHistory.LastUpdatedBy));
			Property(t => t.Operation).HasColumnName(nameof(AnalystResearchHeaderHistory.Operation));

		}
	}
}
