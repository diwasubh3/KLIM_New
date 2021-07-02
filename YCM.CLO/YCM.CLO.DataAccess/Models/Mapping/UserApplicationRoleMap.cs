using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class UserApplicationRoleMap: EntityTypeConfiguration<UserApplicationRole>
	{
		public UserApplicationRoleMap()
		{
			// Primary Key
			HasKey(t => t.UserApplicationRoleId);

			// Table & Column Mappings
			ToTable(nameof(UserApplicationRole), "CLO");
			Property(t => t.UserApplicationRoleId).HasColumnName(nameof(UserApplicationRole.UserApplicationRoleId));
			Property(t => t.UserId).HasColumnName(nameof(UserApplicationRole.UserId));
			Property(t => t.ApplicationRoleId).HasColumnName(nameof(UserApplicationRole.ApplicationRoleId));

			// Relationships
			HasRequired(t => t.ApplicationRole)
				.WithMany(t => t.UserApplicationRoles)
				.HasForeignKey(d => d.ApplicationRoleId);

		}
	}
}
