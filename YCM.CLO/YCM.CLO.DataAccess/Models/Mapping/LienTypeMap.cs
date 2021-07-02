using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class LienTypeMap : EntityTypeConfiguration<LienType>
    {
        public LienTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.LienTypeId);

            // Properties
            this.Property(t => t.LienTypeDesc)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("LienType", "CLO");
            this.Property(t => t.LienTypeId).HasColumnName("LienTypeId");
            this.Property(t => t.LienTypeDesc).HasColumnName("LienTypeDesc");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
