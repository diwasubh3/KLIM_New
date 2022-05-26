using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class TraderMap : EntityTypeConfiguration<Trader>
    {
        public TraderMap()
        {
            // Primary Key
            this.HasKey(t => t.TraderId);

            // Properties
            this.Property(t => t.TraderName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Trader", "CLO");
            this.Property(t => t.TraderId).HasColumnName("TraderId");
            this.Property(t => t.TraderName).HasColumnName("TraderName");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
