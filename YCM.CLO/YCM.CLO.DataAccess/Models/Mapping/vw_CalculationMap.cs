using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_CalculationMap : EntityTypeConfiguration<vw_Calculation>
    {
        public vw_CalculationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.CalculationId, t.SecurityId, t.FundId, t.DateId });

            // Properties
            this.Property(t => t.CalculationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("vw_Calculation", "CLO");
            this.Property(t => t.CalculationId).HasColumnName("CalculationId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.FundId).HasColumnName("FundId");
            this.Property(t => t.YieldBid).HasColumnName("YieldBid");
            this.Property(t => t.YieldOffer).HasColumnName("YieldOffer");
            this.Property(t => t.YieldMid).HasColumnName("YieldMid");
            this.Property(t => t.CappedYieldBid).HasColumnName("CappedYieldBid");
            this.Property(t => t.CappedYieldOffer).HasColumnName("CappedYieldOffer");
            this.Property(t => t.CappedYieldMid).HasColumnName("CappedYieldMid");
            this.Property(t => t.TargetYieldBid).HasColumnName("TargetYieldBid");
            this.Property(t => t.TargetYieldOffer).HasColumnName("TargetYieldOffer");
            this.Property(t => t.TargetYieldMid).HasColumnName("TargetYieldMid");
            this.Property(t => t.BetterWorseBid).HasColumnName("BetterWorseBid");
            this.Property(t => t.BetterWorseOffer).HasColumnName("BetterWorseOffer");
            this.Property(t => t.BetterWorseMid).HasColumnName("BetterWorseMid");
            this.Property(t => t.TotalCoupon).HasColumnName("TotalCoupon");
            this.Property(t => t.WARF).HasColumnName("WARF");
            this.Property(t => t.WARFRecovery).HasColumnName("WARFRecovery");
            this.Property(t => t.Life).HasColumnName("Life");
            this.Property(t => t.TotalPar).HasColumnName("TotalPar");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
            this.Property(t => t.DateId).HasColumnName("DateId");
        }
    }
}
