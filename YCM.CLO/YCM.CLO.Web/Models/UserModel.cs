namespace YCM.CLO.Web.Models
{
	public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SearchText { get{return FirstName+LastName;}}
    }
}