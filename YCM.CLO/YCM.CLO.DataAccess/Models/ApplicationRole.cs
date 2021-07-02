using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class ApplicationRole
    {
        public ApplicationRole()
        {
            UserApplicationRoles = new List<UserApplicationRole>();
        }

        public int ApplicationRoleId { get; set; }

        public string RoleDescription { get; set; }

        public virtual ICollection<UserApplicationRole> UserApplicationRoles { get; set; }
    }
}
