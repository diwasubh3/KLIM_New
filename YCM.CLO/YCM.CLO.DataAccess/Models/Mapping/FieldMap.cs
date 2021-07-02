using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class FieldMap : EntityTypeConfiguration<Field>
    {
        public FieldMap()
        {
            // Primary Key
            this.HasKey(t => t.FieldId);

            // Properties
            this.Property(t => t.FieldName)
                .HasMaxLength(100);

            this.Property(t => t.JsonPropertyName)
                .HasMaxLength(100);

            this.Property(t => t.FieldTitle)
                .HasMaxLength(100);

            this.Property(t => t.JsonFormatString)
                .HasMaxLength(200);

            this.Property(t => t.HeaderCellClass)
                .HasMaxLength(100);

            this.Property(t => t.CellClass)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Field", "CLO");
            this.Property(t => t.FieldId).HasColumnName("FieldId");
            this.Property(t => t.FieldGroupId).HasColumnName("FieldGroupId");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.JsonPropertyName).HasColumnName("JsonPropertyName");
            this.Property(t => t.FieldTitle).HasColumnName("FieldTitle");
            this.Property(t => t.JsonFormatString).HasColumnName("JsonFormatString");
            this.Property(t => t.DisplayWidth).HasColumnName("DisplayWidth");
            this.Property(t => t.IsPercentage).HasColumnName("IsPercentage");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.FieldType).HasColumnName("FieldType");
            this.Property(t => t.HeaderCellClass).HasColumnName("HeaderCellClass");
            this.Property(t => t.CellClass).HasColumnName("CellClass");
            this.Property(t => t.CellTemplate).HasColumnName("CellTemplate");
            this.Property(t => t.Hidden).HasColumnName("Hidden");
            this.Property(t => t.PinnedLeft).HasColumnName("PinnedLeft");
            this.Property(t => t.IsSecurityOverride).HasColumnName("IsSecurityOverride");

            // Relationships
            this.HasRequired(t => t.FieldGroup)
                .WithMany(t => t.Fields)
                .HasForeignKey(d => d.FieldGroupId);

        }
    }
}
