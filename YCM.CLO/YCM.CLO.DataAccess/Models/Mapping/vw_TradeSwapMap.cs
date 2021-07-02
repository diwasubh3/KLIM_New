using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class vw_TradeSwapMap : EntityTypeConfiguration<vw_TradeSwap>
    {
        public vw_TradeSwapMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TradeSwapSnapshotId, t.TradeSwapId, t.SellSecurityId, t.SellFundId, t.BuySecurityId, t.BuyFundId, t.BuySecurityCode, t.SellSecurityCode });

            // Properties
            this.Property(t => t.TradeSwapSnapshotId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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

            this.Property(t => t.BuyFundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BuyIssuer)
                .HasMaxLength(100);

            this.Property(t => t.BuyFacility)
                .HasMaxLength(100);

            this.Property(t => t.BuySecurityCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SellSecurityCode)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("vw_TradeSwap", "CLO");
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
            this.Property(t => t.BuySecurityCode).HasColumnName("BuySecurityCode");
            this.Property(t => t.SellSecurityCode).HasColumnName("SellSecurityCode");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
        }
    }
}
