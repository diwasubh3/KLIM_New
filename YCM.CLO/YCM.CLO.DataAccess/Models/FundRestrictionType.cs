using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class FundRestrictionType
    {
        public FundRestrictionType()
        {
            this.FundRestrictions = new List<FundRestriction>();
        }

        public short FundRestrictionTypeId { get; set; }
        public string FundRestrictionTypeName { get; set; }
		[NotMapped]
		public string FundRestrictionToolTip { get; set; }
		public string DisplayColor { get; set; }
        public short? SortOrder { get; set; }
        public virtual ICollection<FundRestriction> FundRestrictions { get; set; }
    }
}
