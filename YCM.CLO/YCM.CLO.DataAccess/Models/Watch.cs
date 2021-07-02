using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class Watch
    {
        public int WatchId { get; set; }
        public short WatchObjectTypeId { get; set; }
        public int WatchObjectId { get; set; }
        public string WatchComments { get; set; }
        public string WatchUser { get; set; }
		public int WatchTypeId { get; set; }

		public DateTime? WatchLastUpdatedOn { get; set; }

	    public override string ToString() =>
		    $"({WatchId}) Obj Type: {WatchObjectTypeId} Obj Id: {WatchObjectId} User: {WatchUser} Watch Type: {WatchTypeId}";
    }
}
