using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class MarketDataMap : EntityTypeConfiguration<MarketData>
    {
        public MarketDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DateId, t.SecurityId, t.FundId });

            // Properties
            this.Property(t => t.MarketDataId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.MoodyOutlook)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.MoodyWatch)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.SnPWatch)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.AgentBank)
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("MarketData", "CLO");
            this.Property(t => t.MarketDataId).HasColumnName("MarketDataId");
            this.Property(t => t.DateId).HasColumnName("DateId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.FundId).HasColumnName("FundId");
            this.Property(t => t.Bid).HasColumnName("Bid");
            this.Property(t => t.Offer).HasColumnName("Offer");
            this.Property(t => t.Spread).HasColumnName("Spread");
            this.Property(t => t.LiborFloor).HasColumnName("LiborFloor");
            this.Property(t => t.MoodyCashFlowRatingId).HasColumnName("MoodyCashFlowRatingId");
            this.Property(t => t.MoodyCashFlowRatingAdjustedId).HasColumnName("MoodyCashFlowRatingAdjustedId");
            this.Property(t => t.MoodyFacilityRatingId).HasColumnName("MoodyFacilityRatingId");
            this.Property(t => t.MoodyRecovery).HasColumnName("MoodyRecovery");
            this.Property(t => t.SnPIssuerRatingId).HasColumnName("SnPIssuerRatingId");
            this.Property(t => t.SnPIssuerRatingAdjustedId).HasColumnName("SnPIssuerRatingAdjustedId");
            this.Property(t => t.SnPFacilityRatingId).HasColumnName("SnPFacilityRatingId");
            this.Property(t => t.SnPfacilityRatingAdjustedId).HasColumnName("SnPfacilityRatingAdjustedId");
            this.Property(t => t.SnPRecoveryRatingId).HasColumnName("SnPRecoveryRatingId");
            this.Property(t => t.MoodyOutlook).HasColumnName("MoodyOutlook");
            this.Property(t => t.MoodyWatch).HasColumnName("MoodyWatch");
            this.Property(t => t.SnPWatch).HasColumnName("SnPWatch");
            this.Property(t => t.NextReportingDate).HasColumnName("NextReportingDate");
            this.Property(t => t.FiscalYearEndDate).HasColumnName("FiscalYearEndDate");
            this.Property(t => t.AgentBank).HasColumnName("AgentBank");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasRequired(t => t.Fund)
                .WithMany(t => t.MarketDatas)
                .HasForeignKey(d => d.FundId);
            this.HasRequired(t => t.Rating)
                .WithMany(t => t.MarketDatas)
                .HasForeignKey(d => d.MoodyCashFlowRatingAdjustedId);
            this.HasOptional(t => t.Rating1)
                .WithMany(t => t.MarketDatas1)
                .HasForeignKey(d => d.MoodyFacilityRatingId);
            this.HasRequired(t => t.Rating3)
                .WithMany(t => t.MarketDatas3)
                .HasForeignKey(d => d.MoodyCashFlowRatingId);
            this.HasRequired(t => t.Security)
                .WithMany(t => t.MarketDatas)
                .HasForeignKey(d => d.SecurityId);
            this.HasOptional(t => t.Rating4)
                .WithMany(t => t.MarketDatas4)
                .HasForeignKey(d => d.SnPFacilityRatingId);
            this.HasOptional(t => t.Rating5)
                .WithMany(t => t.MarketDatas5)
                .HasForeignKey(d => d.SnPfacilityRatingAdjustedId);
            this.HasOptional(t => t.Rating6)
                .WithMany(t => t.MarketDatas6)
                .HasForeignKey(d => d.SnPIssuerRatingId);
            this.HasOptional(t => t.Rating7)
                .WithMany(t => t.MarketDatas7)
                .HasForeignKey(d => d.SnPIssuerRatingAdjustedId);
            this.HasOptional(t => t.Rating8)
                .WithMany(t => t.MarketDatas8)
                .HasForeignKey(d => d.SnPRecoveryRatingId);

        }
    }
}
