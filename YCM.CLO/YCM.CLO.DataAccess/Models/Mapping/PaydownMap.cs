using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class PaydownMap : EntityTypeConfiguration<Paydown>
    {
        public PaydownMap()
        {
            // Primary Key
            this.HasKey(t => t.PaydownId);

            // Properties
            this.Property(t => t.PaydownComments)
                .HasMaxLength(500);

            this.Property(t => t.PaydownUser)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Paydown", "CLO");
            this.Property(t => t.PaydownId).HasColumnName("PaydownId");
            this.Property(t => t.PaydownObjectTypeId).HasColumnName("PaydownObjectTypeId");
            this.Property(t => t.PaydownObjectId).HasColumnName("PaydownObjectId");
            this.Property(t => t.PaydownComments).HasColumnName("PaydownComments");
            this.Property(t => t.PaydownUser).HasColumnName("PaydownUser");
            this.Property(t => t.PaydownLastUpdatedOn).HasColumnName("PaydownLastUpdatedOn");
        }
    }
}
