using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_SecurityWatchMap : EntityTypeConfiguration<vw_Security_Watch>
    {
        public vw_SecurityWatchMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SecurityId, t.SecurityCode});

            // Properties
            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecurityCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SecurityDesc)
                .HasMaxLength(100);

            this.Property(t => t.Issuer)
                .HasMaxLength(100);

            this.Property(t => t.IssuerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.WatchComments)
                .HasMaxLength(500);

            this.Property(t => t.WatchLastUpdatedOn)
                .HasMaxLength(4000);

            this.Property(t => t.WatchUser)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("vw_Security_Watch", "CLO");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.SecurityCode).HasColumnName("SecurityCode");
            this.Property(t => t.SecurityDesc).HasColumnName("SecurityDesc");
            
            this.Property(t => t.Issuer).HasColumnName("Issuer");
            
            
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.WatchId).HasColumnName("WatchId");
            this.Property(t => t.IsOnWatch).HasColumnName("IsOnWatch");
            this.Property(t => t.WatchObjectTypeId).HasColumnName("WatchObjectTypeId");
            this.Property(t => t.WatchObjectId).HasColumnName("WatchObjectId");
            this.Property(t => t.WatchComments).HasColumnName("WatchComments");
            this.Property(t => t.WatchLastUpdatedOn).HasColumnName("WatchLastUpdatedOn");
            this.Property(t => t.WatchUser).HasColumnName("WatchUser");
        }
    }
}
