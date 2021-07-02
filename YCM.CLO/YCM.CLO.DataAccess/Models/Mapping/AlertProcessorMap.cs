using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AlertProcessorMap : EntityTypeConfiguration<AlertProcessor>
    {
        public AlertProcessorMap()
        {
            // Primary Key
            this.HasKey(t => t.AlertId);

            // Properties
            this.Property(t => t.AlertProcessorClassName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("AlertProcessor", "CLO");
            this.Property(t => t.AlertId).HasColumnName("AlertId");
            this.Property(t => t.AlertProcessorClassName).HasColumnName("AlertProcessorClassName");
            this.Property(t => t.ParameterTypeId).HasColumnName("ParameterTypeId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.ParameterType)
                .WithMany(t => t.AlertProcessors)
                .HasForeignKey(d => d.ParameterTypeId);

        }
    }
}
