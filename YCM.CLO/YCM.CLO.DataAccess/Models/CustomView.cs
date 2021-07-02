using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class CustomView : Entity
    {
	    public CustomView()
	    {
		    CustomViewFields = new List<CustomViewField>();
	    }
	    [NotMapped]
	    public override int Id
	    {
		    get { return ViewId; }
		    set { ViewId = value; }
	    }

        public int ViewId { get; set; }
		public string ViewName { get; set; }
		public int UserId { get; set; }

        public bool IsPublic { get; set; }

	    [NotMapped]
        public bool IsDefault { get; set; }
		public int SortOrder { get; set; }
	    public virtual ICollection<CustomViewField> CustomViewFields { get; set; }

	    [NotMapped]
	    public override DateTime? CreatedOn { get; set; }

	    [NotMapped]
	    public override string CreatedBy { get; set; }

	    [NotMapped]
	    public override DateTime? LastUpdatedOn { get; set; }

	    [NotMapped]
	    public override string LastUpdatedBy { get; set; }

	    public override string ToString() =>
		    $"Id: {ViewId} Name: {ViewName} User: {UserId} Public: {IsPublic} Default: {IsDefault}";
    }
}
