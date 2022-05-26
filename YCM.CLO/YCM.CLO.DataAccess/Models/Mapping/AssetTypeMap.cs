using System.Data.Entity.ModelConfiguration; 

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class AssetTypeMap : EntityTypeConfiguration<AssetType>
    {
        public AssetTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.AssetId);

            // Properties
            this.Property(t => t.AssetName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("AssetType", "CLO");
            this.Property(t => t.AssetId).HasColumnName("AssetId");
            this.Property(t => t.AssetName).HasColumnName("AssetName");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
