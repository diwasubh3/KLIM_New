using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class SecurityOverrideMap : EntityTypeConfiguration<SecurityOverride>
    {
        public SecurityOverrideMap()
        {
            // Primary Key
            this.HasKey(t => t.SecurityOverrideId);

            // Properties
            this.Property(t => t.ExistingValue)
                .HasMaxLength(100);

            this.Property(t => t.OverrideValue)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Comments)
                .HasMaxLength(1000);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("SecurityOverride", "CLO");
            this.Property(t => t.SecurityOverrideId).HasColumnName("SecurityOverrideId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.FieldId).HasColumnName("FieldId");
            this.Property(t => t.ExistingValue).HasColumnName("ExistingValue");
            this.Property(t => t.OverrideValue).HasColumnName("OverrideValue");
            this.Property(t => t.EffectiveFrom).HasColumnName("EffectiveFrom");
            this.Property(t => t.EffectiveTo).HasColumnName("EffectiveTo");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.IsConflict).HasColumnName("IsConflict");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasRequired(t => t.Field)
                .WithMany(t => t.SecurityOverrides)
                .HasForeignKey(d => d.FieldId);
            this.HasRequired(t => t.Security)
                .WithMany(t => t.SecurityOverrides)
                .HasForeignKey(d => d.SecurityId);

        }
    }
}
