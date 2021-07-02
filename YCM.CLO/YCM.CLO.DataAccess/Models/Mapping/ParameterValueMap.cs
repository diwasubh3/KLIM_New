using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class ParameterValueMap : EntityTypeConfiguration<ParameterValue>
    {
        public ParameterValueMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ParameterValueText)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ParameterValue", "CLO");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParameterTypeId).HasColumnName("ParameterTypeId");
            this.Property(t => t.ParameterValueNumber).HasColumnName("ParameterValueNumber");
            this.Property(t => t.ParameterValueText).HasColumnName("ParameterValueText");
            this.Property(t => t.ParameterMinValueNumber).HasColumnName("ParameterMinValueNumber");
            this.Property(t => t.ParameterMaxValueNumber).HasColumnName("ParameterMaxValueNumber");

            // Relationships
            this.HasRequired(t => t.ParameterType)
                .WithMany(t => t.ParameterValues)
                .HasForeignKey(d => d.ParameterTypeId);

        }
    }
}
