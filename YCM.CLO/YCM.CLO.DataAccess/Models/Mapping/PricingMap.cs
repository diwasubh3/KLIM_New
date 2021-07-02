using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class PricingMap : EntityTypeConfiguration<Pricing>
    {
        public PricingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DateId, t.SecurityId });

            // Properties
            this.Property(t => t.PricingId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Pricing", "CLO");
            this.Property(t => t.PricingId).HasColumnName("PricingId");
            this.Property(t => t.DateId).HasColumnName("DateId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.Bid).HasColumnName("Bid");
            this.Property(t => t.Offer).HasColumnName("Offer");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasRequired(t => t.Security)
                .WithMany(t => t.Pricings)
                .HasForeignKey(d => d.SecurityId);

        }
    }
}
