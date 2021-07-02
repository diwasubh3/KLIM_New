using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class UserDefaultCustomView : Entity
    {
		[NotMapped]
	    public override int Id
	    {
		    get { return UserDefaultCustomViewId; }
			set { UserDefaultCustomViewId = value; }
		}
		public int UserDefaultCustomViewId { get; set; }

        public int UserId { get; set; }

        public int ViewId { get; set; }

	    public override string ToString() => $"Id: {UserDefaultCustomViewId} User: {UserId} View: {ViewId}";

    }
}
