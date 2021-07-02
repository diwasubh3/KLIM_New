using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class UserDefaultCustomViewMap : EntityTypeConfiguration<UserDefaultCustomView>
	{
		public UserDefaultCustomViewMap()
		{
			ToTable(nameof(UserDefaultCustomView), "CLO");
		}
	}
}
