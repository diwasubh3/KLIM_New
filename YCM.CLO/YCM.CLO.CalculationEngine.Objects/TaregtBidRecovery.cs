using System.Collections.Generic;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class TaregtBidRecovery
    {
        public IList<double> Yields { get; set; }
        public IList<double> WarfRecoveries { get; set; }

        public TaregtBidRecovery()
        {
            Yields = new List<double>();
            WarfRecoveries = new List<double>();
        }
    }
}
