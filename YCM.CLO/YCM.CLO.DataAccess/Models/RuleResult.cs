using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
	public class RuleResult
    {
        public IList<vw_AggregatePosition> TopPositions { get; set; }
        public IList<vw_AggregatePosition> BottomPositions { get; set; }
    }
}
