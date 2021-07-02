using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_MarketDataMap : EntityTypeConfiguration<vw_MarketData>
    {
        public vw_MarketDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DateId, t.MarketDataId, t.SecurityId, t.FundId, });

            // Properties
            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.MarketDataId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Bid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Offer)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Spread)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LiborFloor)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.MoodyCashFlowRating)
                .HasMaxLength(100);

            this.Property(t => t.MoodyCashFlowRatingAdjusted)
                .HasMaxLength(100);

            this.Property(t => t.MoodyFacilityRating)
                .HasMaxLength(100);

            //this.Property(t => t.MoodyFacilityRatingAdjusted)
            //    .HasMaxLength(100);

            this.Property(t => t.MoodyRecovery)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SnPIssuerRating)
                .HasMaxLength(100);

            this.Property(t => t.SnPIssuerRatingAdjusted)
                .HasMaxLength(100);

            this.Property(t => t.SnPFacilityRating)
                .HasMaxLength(100);

            this.Property(t => t.SnPfacilityRatingAdjusted)
                .HasMaxLength(100);

            this.Property(t => t.SnPRecoveryRating)
                .HasMaxLength(100);

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



            // Table & Column Mappings
            this.ToTable("vw_MarketData", "CLO");
            this.Property(t => t.DateId).HasColumnName("DateId");
            this.Property(t => t.MarketDataId).HasColumnName("MarketDataId");
            this.Property(t => t.OverrideMarketDataId).HasColumnName("OverrideMarketDataId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.FundId).HasColumnName("FundId");
            this.Property(t => t.Bid).HasColumnName("Bid");
            this.Property(t => t.Offer).HasColumnName("Offer");
            this.Property(t => t.Spread).HasColumnName("Spread");
            this.Property(t => t.LiborFloor).HasColumnName("LiborFloor");
            this.Property(t => t.MoodyCashFlowRating).HasColumnName("MoodyCashFlowRating");
            this.Property(t => t.MoodyCashFlowRatingAdjusted).HasColumnName("MoodyCashFlowRatingAdjusted");
            this.Property(t => t.MoodyFacilityRating).HasColumnName("MoodyFacilityRating");
            //this.Property(t => t.MoodyFacilityRatingAdjusted).HasColumnName("MoodyFacilityRatingAdjusted");
            this.Property(t => t.MoodyRecovery).HasColumnName("MoodyRecovery");
            this.Property(t => t.SnPIssuerRating).HasColumnName("SnPIssuerRating");
            this.Property(t => t.SnPIssuerRatingAdjusted).HasColumnName("SnPIssuerRatingAdjusted");
            this.Property(t => t.SnPFacilityRating).HasColumnName("SnPFacilityRating");
            this.Property(t => t.SnPfacilityRatingAdjusted).HasColumnName("SnPfacilityRatingAdjusted");
            this.Property(t => t.SnPRecoveryRating).HasColumnName("SnPRecoveryRating");
            this.Property(t => t.MoodyOutlook).HasColumnName("MoodyOutlook");
            this.Property(t => t.MoodyWatch).HasColumnName("MoodyWatch");
            this.Property(t => t.SnPWatch).HasColumnName("SnPWatch");
            this.Property(t => t.NextReportingDate).HasColumnName("NextReportingDate");
            this.Property(t => t.FiscalYearEndDate).HasColumnName("FiscalYearEndDate");
            this.Property(t => t.AgentBank).HasColumnName("AgentBank");
            this.Property(t => t.MoodyFacilityRatingId).HasColumnName("MoodyFacilityRatingId");
            
        }
    }
}
