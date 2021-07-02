using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class FacilityMap : EntityTypeConfiguration<Facility>
    {
        public FacilityMap()
        {
            // Primary Key
            this.HasKey(t => t.FacilityId);

            // Properties
            this.Property(t => t.FacilityDesc)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Facility", "CLO");
            this.Property(t => t.FacilityId).HasColumnName("FacilityId");
            this.Property(t => t.FacilityDesc).HasColumnName("FacilityDesc");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
