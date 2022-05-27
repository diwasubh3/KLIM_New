using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class SettleMethodsMap : EntityTypeConfiguration<SettleMethods>
    {
        public SettleMethodsMap()
        {
            // Primary Key
            this.HasKey(t => t.MethodId);

            // Properties
            this.Property(t => t.MethodName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("SettleMethods", "CLO");
            this.Property(t => t.MethodId).HasColumnName("MethodId");
            this.Property(t => t.MethodName).HasColumnName("MethodName");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
