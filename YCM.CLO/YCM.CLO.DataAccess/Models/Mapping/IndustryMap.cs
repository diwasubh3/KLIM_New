using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class IndustryMap : EntityTypeConfiguration<Industry>
    {
        public IndustryMap()
        {
            // Primary Key
            this.HasKey(t => t.IndustryId);

            // Properties
            this.Property(t => t.IndustryDesc)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Industry", "CLO");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.IndustryDesc).HasColumnName("IndustryDesc");
            this.Property(t => t.IsSnP).HasColumnName("IsSnP");
            this.Property(t => t.IsMoody).HasColumnName("IsMoody");
            this.Property(t => t.MappedSnPIndustryId).HasColumnName("MappedSnPIndustryId");
            this.Property(t => t.MappedMoodyIndustryId).HasColumnName("MappedMoodyIndustryId");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
