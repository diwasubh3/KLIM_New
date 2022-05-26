using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class TradeReasonMap : EntityTypeConfiguration<TradeReason>
    {
        public TradeReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.TradeReasonId);

            // Properties
            this.Property(t => t.TradeReasonDesc)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TradeReason", "CLO");
            this.Property(t => t.TradeReasonId).HasColumnName("TradeReasonId");
            this.Property(t => t.TradeReasonDesc).HasColumnName("TradeReasonDesc");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
