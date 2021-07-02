using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class CustomViewFieldMap : EntityTypeConfiguration<CustomViewField>
	{
		public CustomViewFieldMap()
		{
			// Primary Key
			HasKey(t => t.CustomViewFieldId);

			// Table & Column Mappings
			ToTable(nameof(CustomViewField), "CLO");
			Property(t => t.CustomViewFieldId).HasColumnName(nameof(CustomViewField.CustomViewFieldId));
			Property(t => t.ViewId).HasColumnName(nameof(CustomViewField.ViewId));
			Property(t => t.FieldId).HasColumnName(nameof(CustomViewField.FieldId));
			Property(t => t.SortOrder).HasColumnName(nameof(CustomViewField.SortOrder));

			// Relationships
			HasRequired(t => t.CustomView)
				.WithMany(t => t.CustomViewFields)
				.HasForeignKey(d => d.ViewId);

			//HasRequired(t => t.Field)
			//	.WithMany(t => t.CustomViewFields)
			//	.HasForeignKey(d => d.FieldId);

		}
	}
}
