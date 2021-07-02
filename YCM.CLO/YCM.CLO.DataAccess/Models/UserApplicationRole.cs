namespace YCM.CLO.DataAccess.Models
{
	public partial class UserApplicationRole
    {
        public int UserApplicationRoleId { get; set; }

        public int UserId { get; set; }

        public int ApplicationRoleId { get; set; }

        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}
