using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class CustomViewMap : EntityTypeConfiguration<CustomView>
	{
		public CustomViewMap()
		{
			// Primary Key
			HasKey(t => t.ViewId);

			// Table & Column Mappings
			ToTable(nameof(CustomView), "CLO");
			Property(t => t.ViewId).HasColumnName(nameof(CustomView.ViewId));
			Property(t => t.ViewName).HasColumnName(nameof(CustomView.ViewName));
			Property(t => t.UserId).HasColumnName(nameof(CustomView.UserId));
			//Property(t => t.IsDefault).HasColumnName(nameof(CustomView.IsDefault));
			Property(t => t.IsPublic).HasColumnName(nameof(CustomView.IsPublic));
		}
	}
}
