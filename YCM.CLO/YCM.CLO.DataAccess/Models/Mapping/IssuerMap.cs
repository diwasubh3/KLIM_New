using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class IssuerMap : EntityTypeConfiguration<Issuer>
    {
        public IssuerMap()
        {
            // Primary Key
            this.HasKey(t => t.IssuerId);

            // Properties
            this.Property(t => t.IssuerDesc)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Issuer", "CLO");
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.IssuerDesc).HasColumnName("IssuerDesc");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
