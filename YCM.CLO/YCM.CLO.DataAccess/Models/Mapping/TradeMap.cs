using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class TradeMap : EntityTypeConfiguration<Trade>
    {
        public TradeMap()
        {
            // Primary Key
            this.HasKey(t => t.TradeId);

            // Properties
            this.Property(t => t.Comments)
                .HasMaxLength(1000);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Trade", "CLO");
            this.Property(t => t.TradeId).HasColumnName("TradeId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.DateId).HasColumnName("DateId");
            this.Property(t => t.IsBuy).HasColumnName("IsBuy");
            this.Property(t => t.TradeAmount).HasColumnName("TradeAmount");
            this.Property(t => t.TradePrice).HasColumnName("TradePrice");
            this.Property(t => t.SellAll).HasColumnName("SellAll");
            this.Property(t => t.KeepOnBlotter).HasColumnName("KeepOnBlotter");
            this.Property(t => t.BidOfferPrice).HasColumnName("BidOfferPrice");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.FinalAllocation).HasColumnName("FinalAllocation");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasRequired(t => t.Security)
                .WithMany(t => t.Trades)
                .HasForeignKey(d => d.SecurityId);

        }
    }
}
