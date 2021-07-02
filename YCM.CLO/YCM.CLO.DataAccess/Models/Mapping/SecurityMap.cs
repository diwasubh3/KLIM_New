using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class SecurityMap : EntityTypeConfiguration<Security>
    {
        public SecurityMap()
        {
            // Primary Key
            this.HasKey(t => t.SecurityId);

            // Properties
            this.Property(t => t.SecurityCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SecurityDesc)
                .HasMaxLength(500);

            this.Property(t => t.BBGId)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.GICSIndustry)
                .HasMaxLength(500);

            this.Property(t => t.ISIN)
                .HasMaxLength(20);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Security", "CLO");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.SecurityCode).HasColumnName("SecurityCode");
            this.Property(t => t.SecurityDesc).HasColumnName("SecurityDesc");
            this.Property(t => t.BBGId).HasColumnName("BBGId");
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.FacilityId).HasColumnName("FacilityId");
            this.Property(t => t.CallDate).HasColumnName("CallDate");
            this.Property(t => t.MaturityDate).HasColumnName("MaturityDate");
            this.Property(t => t.GICSIndustry).HasColumnName("GICSIndustry");
            this.Property(t => t.SnPIndustryId).HasColumnName("SnPIndustryId");
            this.Property(t => t.MoodyIndustryId).HasColumnName("MoodyIndustryId");
            this.Property(t => t.IsFloating).HasColumnName("IsFloating");
            this.Property(t => t.LienTypeId).HasColumnName("LienTypeId");
            
            this.Property(t => t.ISIN).HasColumnName("ISIN");
            this.Property(t => t.SourceId).HasColumnName("SourceId");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasRequired(t => t.Facility)
                .WithMany(t => t.Securities)
                .HasForeignKey(d => d.FacilityId);
            this.HasRequired(t => t.Industry)
                .WithMany(t => t.Securities)
                .HasForeignKey(d => d.MoodyIndustryId);
            this.HasRequired(t => t.Industry1)
                .WithMany(t => t.Securities1)
                .HasForeignKey(d => d.SnPIndustryId);
            this.HasRequired(t => t.Issuer)
                .WithMany(t => t.Securities)
                .HasForeignKey(d => d.IssuerId);
            this.HasRequired(t => t.LienType)
                .WithMany(t => t.Securities)
                .HasForeignKey(d => d.LienTypeId);

        }
    }
}
