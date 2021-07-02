using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_CurrentActiveSecurityOverridesMap : EntityTypeConfiguration<vw_CurrentActiveSecurityOverrides>
    {
        public vw_CurrentActiveSecurityOverridesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SecurityId, t.OverrideValue });

            // Properties
            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OverrideValue)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.FieldName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("vw_CurrentActiveSecurityOverrides", "CLO");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.OverrideValue).HasColumnName("OverrideValue");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
        }
    }
}
