using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class ApplicationRoleMap : EntityTypeConfiguration<ApplicationRole>
	{
		public ApplicationRoleMap()
		{
			// Primary Key
			HasKey(t => t.ApplicationRoleId);

			// Table & Column Mappings
			ToTable(nameof(ApplicationRole), "CLO");
			Property(t => t.ApplicationRoleId).HasColumnName(nameof(ApplicationRole.ApplicationRoleId));
			Property(t => t.RoleDescription).HasColumnName(nameof(ApplicationRole.RoleDescription));
		}
	}
}
