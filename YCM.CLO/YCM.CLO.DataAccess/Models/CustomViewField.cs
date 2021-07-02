using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class CustomViewField : Entity
    {
	    [NotMapped]
	    public override int Id
	    {
		    get { return CustomViewFieldId; }
		    set { CustomViewFieldId = value; }
	    }
        public int CustomViewFieldId { get; set; }

        public int ViewId { get; set; }

        public short FieldId { get; set; }

        public int SortOrder { get; set; }
		public bool IsPinned { get; set; }

		public virtual CustomView CustomView { get; set; }
	    [NotMapped]
	    public override DateTime? CreatedOn { get; set; }

	    [NotMapped]
	    public override string CreatedBy { get; set; }

	    [NotMapped]
	    public override DateTime? LastUpdatedOn { get; set; }

	    [NotMapped]
	    public override string LastUpdatedBy { get; set; }

	    public override string ToString() =>
		    $"Id: {CustomViewFieldId} View Id: {ViewId} Field Id: {FieldId} Sort: {SortOrder} Pinned: {IsPinned}";
    }
}
