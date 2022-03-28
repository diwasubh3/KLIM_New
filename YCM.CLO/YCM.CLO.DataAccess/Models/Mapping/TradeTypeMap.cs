using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class TradeTypeMap: EntityTypeConfiguration<TradeType>
    {
        public TradeTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.TradeTypeId);

            // Properties
            this.Property(t => t.TradeTypeDesc)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TradeType", "CLO");
            this.Property(t => t.TradeTypeId).HasColumnName("TradeTypeId");
            this.Property(t => t.TradeTypeDesc).HasColumnName("TradeType");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
