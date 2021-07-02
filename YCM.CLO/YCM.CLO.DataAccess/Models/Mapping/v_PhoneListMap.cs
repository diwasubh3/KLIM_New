using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class v_PhoneListMap : EntityTypeConfiguration<v_PhoneList>
    {
        public v_PhoneListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.DisplayName, t.Email, t.NetworkId, t.FirstName, t.LastName });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.NetworkId)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("v_PhoneList");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.NetworkId).HasColumnName("NetworkId");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
        }
    }
}
