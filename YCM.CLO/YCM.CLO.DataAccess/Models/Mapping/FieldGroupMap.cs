using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class FieldGroupMap : EntityTypeConfiguration<FieldGroup>
    {
        public FieldGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.FieldGroupId);

            // Properties
            this.Property(t => t.FieldGroupName)
                .HasMaxLength(100);

            this.Property(t => t.DisplayIcon)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FieldGroup", "CLO");
            this.Property(t => t.FieldGroupId).HasColumnName("FieldGroupId");
            this.Property(t => t.FieldGroupName).HasColumnName("FieldGroupName");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.DisplayIcon).HasColumnName("DisplayIcon");
            this.Property(t => t.ShowOnPositions).HasColumnName("ShowOnPositions");
        }
    }
}
