using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AnalystResearchMap : EntityTypeConfiguration<AnalystResearch>
    {
        public AnalystResearchMap()
        {
            // Primary Key
            this.HasKey(t => t.AnalystResearchId);

            // Properties
            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("AnalystResearch", "CLO");
            this.Property(t => t.AnalystResearchId).HasColumnName("AnalystResearchId");
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.CLOAnalystUserId).HasColumnName("CLOAnalystUserId");
            this.Property(t => t.HFAnalystUserId).HasColumnName("HFAnalystUserId");
            this.Property(t => t.AsOfDate).HasColumnName("AsOfDate");
            this.Property(t => t.CreditScore).HasColumnName("CreditScore");
            this.Property(t => t.OneLLeverage).HasColumnName("OneLLeverage");
            this.Property(t => t.TotalLeverage).HasColumnName("TotalLeverage");
            this.Property(t => t.EVMultiple).HasColumnName("EVMultiple");
            this.Property(t => t.LTMRevenues).HasColumnName("LTMRevenues");
            this.Property(t => t.LTMEBITDA).HasColumnName("LTMEBITDA");
            this.Property(t => t.FCF).HasColumnName("FCF");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.BusinessDescription).HasColumnName("BusinessDescription");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.AnalystResearches)
                .HasForeignKey(d => d.CLOAnalystUserId);
            this.HasOptional(t => t.User1)
                .WithMany(t => t.AnalystResearches1)
                .HasForeignKey(d => d.HFAnalystUserId);
            this.HasRequired(t => t.Issuer)
                .WithMany(t => t.AnalystResearches)
                .HasForeignKey(d => d.IssuerId);

        }
    }
}
