using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class ParameterTypeMap : EntityTypeConfiguration<ParameterType>
    {
        public ParameterTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ParameterTypeId);

            // Properties
            this.Property(t => t.ParameterTypeName)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ParameterType", "CLO");
            this.Property(t => t.ParameterTypeId).HasColumnName("ParameterTypeId");
            this.Property(t => t.ParameterTypeName).HasColumnName("ParameterTypeName");
        }
    }
}
