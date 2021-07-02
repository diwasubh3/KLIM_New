using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class GicsToSnpMoodyIndustryMapMap : EntityTypeConfiguration<GicsToSnpMoodyIndustryMap>
    {
        public GicsToSnpMoodyIndustryMapMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.GICSIndustryGroup)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.GICSIndustry)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.GICSIndustryGrpDesc)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.GICSIndustryDesc)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("GicsToSnpMoodyIndustryMap", "CLO");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SectorId).HasColumnName("SectorId");
            this.Property(t => t.GICSIndustryGroup).HasColumnName("GICSIndustryGroup");
            this.Property(t => t.GICSIndustry).HasColumnName("GICSIndustry");
            this.Property(t => t.GICSIndustryGrpDesc).HasColumnName("GICSIndustryGrpDesc");
            this.Property(t => t.GICSIndustryDesc).HasColumnName("GICSIndustryDesc");
            this.Property(t => t.MappedSnPIndustryId).HasColumnName("MappedSnPIndustryId");
            this.Property(t => t.MappedMoodyIndustryId).HasColumnName("MappedMoodyIndustryId");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
