using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class OperatorMap : EntityTypeConfiguration<Operator>
    {
        public OperatorMap()
        {
            // Primary Key
            this.HasKey(t => t.OperatorId);

            // Properties
            this.Property(t => t.OperatorCode)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.OperatorVal)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Operator", "CLO");
            this.Property(t => t.OperatorId).HasColumnName("OperatorId");
            this.Property(t => t.OperatorCode).HasColumnName("OperatorCode");
            this.Property(t => t.OperatorVal).HasColumnName("OperatorVal");
        }
    }
}
