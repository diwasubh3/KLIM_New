using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public partial class Operator
    {
        public Operator()
        {
            this.FundRestrictions = new List<FundRestriction>();
        }

        public short OperatorId { get; set; }
        public string OperatorCode { get; set; }
        public string OperatorVal { get; set; }
        public virtual ICollection<FundRestriction> FundRestrictions { get; set; }
    }
}
