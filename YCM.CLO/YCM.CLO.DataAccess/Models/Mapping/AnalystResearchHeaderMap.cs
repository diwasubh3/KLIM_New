using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class AnalystResearchHeaderMap : EntityTypeConfiguration<AnalystResearchHeader>
    {
        public AnalystResearchHeaderMap()
        {
            // Primary Key
            HasKey(t => t.AnalystResearchHeaderId);

            // Properties
            Property(t => t.CreatedBy)
                .HasMaxLength(100);

            Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable(nameof(AnalystResearchHeader), "CLO");
            Property(t => t.AnalystResearchHeaderId)
                .HasColumnName(nameof(AnalystResearchHeader.AnalystResearchHeaderId));
            Property(t => t.IssuerId).HasColumnName(nameof(AnalystResearchHeader.IssuerId));
            Property(t => t.CLOAnalystId).HasColumnName(nameof(AnalystResearchHeader.CLOAnalystId));
            Property(t => t.HFAnalystId).HasColumnName(nameof(AnalystResearchHeader.HFAnalystId));
            Property(t => t.CreditScore).HasColumnName(nameof(AnalystResearchHeader.CreditScore));
            Property(t => t.AgentBank).HasColumnName(nameof(AnalystResearchHeader.AgentBank));
            Property(t => t.BusinessDescription).HasColumnName(nameof(AnalystResearchHeader.BusinessDescription));
            Property(t => t.CreatedOn).HasColumnName(nameof(AnalystResearchHeader.CreatedOn));
            Property(t => t.CreatedBy).HasColumnName(nameof(AnalystResearchHeader.CreatedBy));
            Property(t => t.LastUpdatedOn).HasColumnName(nameof(AnalystResearchHeader.LastUpdatedOn));
            Property(t => t.LastUpdatedBy).HasColumnName(nameof(AnalystResearchHeader.LastUpdatedBy));

            // Relationships
            HasRequired(t => t.Issuer)
                .WithMany(t => t.AnalystResearchHeaders)
                .HasForeignKey(d => d.IssuerId);

        }
    }
}
