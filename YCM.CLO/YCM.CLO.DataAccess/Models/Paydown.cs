using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class Paydown
    {
        public int PaydownId { get; set; }
        public short PaydownObjectTypeId { get; set; }
        public int PaydownObjectId { get; set; }
        public string PaydownComments { get; set; }
        public string PaydownUser { get; set; }
		public int PaydownTypeId { get; set; }

		public DateTime? PaydownLastUpdatedOn { get; set; }

	    public override string ToString() =>
		    $"({PaydownId}) Obj Type: {PaydownObjectTypeId} Obj Id: {PaydownObjectId} User: {PaydownUser} Paydown Type: {PaydownTypeId}";
    }
}
