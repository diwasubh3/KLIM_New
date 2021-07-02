using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class TradeSwapSnapshotMap : EntityTypeConfiguration<TradeSwapSnapshot>
    {
        public TradeSwapSnapshotMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TradeSwapId, t.SellSecurityId, t.SellFundId, t.BuySecurityId });

            // Properties
            this.Property(t => t.TradeSwapSnapshotId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.TradeSwapId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellSecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellFundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellIssuer)
                .HasMaxLength(100);

            this.Property(t => t.SellFacility)
                .HasMaxLength(100);

            this.Property(t => t.BuySecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BuyIssuer)
                .HasMaxLength(100);

            this.Property(t => t.BuyFacility)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TradeSwapSnapshot", "CLO");
            this.Property(t => t.TradeSwapSnapshotId).HasColumnName("TradeSwapSnapshotId");
            this.Property(t => t.TradeSwapId).HasColumnName("TradeSwapId");
            this.Property(t => t.SellSecurityId).HasColumnName("SellSecurityId");
            this.Property(t => t.SellFundId).HasColumnName("SellFundId");
            this.Property(t => t.SellExposure).HasColumnName("SellExposure");
            this.Property(t => t.SellTotalExposure).HasColumnName("SellTotalExposure");
            this.Property(t => t.SellSecurityBidPrice).HasColumnName("SellSecurityBidPrice");
            this.Property(t => t.SellPctPosition).HasColumnName("SellPctPosition");
            this.Property(t => t.SellSpread).HasColumnName("SellSpread");
            this.Property(t => t.SellLiquidityScore).HasColumnName("SellLiquidityScore");
            this.Property(t => t.SellMaturityDate).HasColumnName("SellMaturityDate");
            this.Property(t => t.SellIssuer).HasColumnName("SellIssuer");
            this.Property(t => t.SellFacility).HasColumnName("SellFacility");
            this.Property(t => t.BuySecurityId).HasColumnName("BuySecurityId");
            this.Property(t => t.BuySecurityOfferPrice).HasColumnName("BuySecurityOfferPrice");
            this.Property(t => t.BuyFundId).HasColumnName("BuyFundId");
            this.Property(t => t.BuyExposure).HasColumnName("BuyExposure");
            this.Property(t => t.BuyTotalExposure).HasColumnName("BuyTotalExposure");
            this.Property(t => t.BuyPctPosition).HasColumnName("BuyPctPosition");
            this.Property(t => t.BuySpread).HasColumnName("BuySpread");
            this.Property(t => t.BuyLiquidityScore).HasColumnName("BuyLiquidityScore");
            this.Property(t => t.BuyMaturityDate).HasColumnName("BuyMaturityDate");
            this.Property(t => t.BuyIssuer).HasColumnName("BuyIssuer");
            this.Property(t => t.BuyFacility).HasColumnName("BuyFacility");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");

            // Relationships
            this.HasRequired(t => t.BuyFund)
                .WithMany(t => t.BuyTradeSwappingSnapshots)
                .HasForeignKey(d => d.BuyFundId);
            this.HasRequired(t => t.SellFund)
                .WithMany(t => t.SellTradeSwappingSnapshots)
                .HasForeignKey(d => d.SellFundId);
            this.HasRequired(t => t.BuySecurity)
                .WithMany(t => t.BuyTradeSwappingSnapshots)
                .HasForeignKey(d => d.BuySecurityId);
            this.HasRequired(t => t.SellSecurity)
                .WithMany(t => t.SellTradeSwappingSnapshots)
                .HasForeignKey(d => d.SellSecurityId);
            this.HasRequired(t => t.TradeSwap)
                .WithMany(t => t.TradeSwappingSnapshots)
                .HasForeignKey(d => d.TradeSwapId);

        }
    }
}
