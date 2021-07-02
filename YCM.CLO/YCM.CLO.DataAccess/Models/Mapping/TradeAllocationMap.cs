using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class TradeAllocationMap : EntityTypeConfiguration<TradeAllocation>
    {
        public TradeAllocationMap()
        {
            // Primary Key
            this.HasKey(t => t.TradeAllocationId);

            // Properties
            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TradeAllocation", "CLO");
            this.Property(t => t.TradeAllocationId).HasColumnName("TradeAllocationId");
            this.Property(t => t.TradeId).HasColumnName("TradeId");
            this.Property(t => t.CurrentAllocation).HasColumnName("CurrentAllocation");
            this.Property(t => t.NewAllocation).HasColumnName("NewAllocation");
            this.Property(t => t.FundId).HasColumnName("FundId");
            this.Property(t => t.FinalAllocation).HasColumnName("FinalAllocation");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasRequired(t => t.Fund)
                .WithMany(t => t.TradeAllocations)
                .HasForeignKey(d => d.FundId);
            this.HasRequired(t => t.Trade)
                .WithMany(t => t.TradeAllocations)
                .HasForeignKey(d => d.TradeId);

        }
    }
}
