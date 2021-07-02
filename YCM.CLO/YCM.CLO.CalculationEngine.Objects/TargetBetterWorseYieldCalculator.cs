using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class TargetBetterWorseYieldCalculator : ICalculator
    {

        private readonly TaregtBidRecovery _cappedBidYieldsRecovery;
        private readonly TaregtBidRecovery _cappedOfferYieldsRecovery;
        private readonly TaregtBidRecovery _cappedMidYieldsRecovery;
        

        public TargetBetterWorseYieldCalculator(int dateId, TaregtBidRecovery cappedBidYieldsRecovery, TaregtBidRecovery cappedOfferYieldsRecovery, TaregtBidRecovery cappedMidYieldsRecovery)
        {
            _cappedBidYieldsRecovery = cappedBidYieldsRecovery;
            _cappedOfferYieldsRecovery = cappedOfferYieldsRecovery;
            _cappedMidYieldsRecovery = cappedMidYieldsRecovery;
            
        }

        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
            IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint,
            int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {
            IList<double> knownWarfRecovery = new List<double>();
            knownWarfRecovery.Add((double)(calculation.WARFRecovery ?? 0));
            
            calculation.TargetYieldBid =
                (decimal) Growth(_cappedBidYieldsRecovery.Yields, _cappedBidYieldsRecovery.WarfRecoveries, knownWarfRecovery, true).First();

            calculation.TargetYieldOffer =
                (decimal) Growth(_cappedOfferYieldsRecovery.Yields, _cappedOfferYieldsRecovery.WarfRecoveries, knownWarfRecovery, true).First();

            calculation.TargetYieldMid =
                (decimal) Growth(_cappedMidYieldsRecovery.Yields, _cappedMidYieldsRecovery.WarfRecoveries, knownWarfRecovery, true).First();

            calculation.BetterWorseBid = (calculation.YieldBid ?? 0) - (calculation.TargetYieldBid ?? 0);
            calculation.BetterWorseOffer = (calculation.YieldOffer ?? 0) - (calculation.TargetYieldOffer ?? 0);
            calculation.BetterWorseMid = (calculation.YieldMid ?? 0) - (calculation.TargetYieldMid ?? 0);
            
            return true;
        }

        IEnumerable<double> Growth(IList<double> knownY, IList<double> knownX, IList<double> newX, bool useConst)
        {
            // Credits: Ilmari Karonen

            // Default values for optional parameters:
            if (knownY == null) return null;
            if (knownX == null)
            {
                knownX = new List<double>();
                for (var i = 0; i <= knownY.Count; i++)
                    knownX.Add(i++);
            }
            if (newX == null)
            {
                newX = new List<double>();
                for (var i = 0; i <= knownY.Count; i++)
                    newX.Add(i++);
            }

            int n = knownY.Count;
            double avg_x = 0.0;
            double avg_y = 0.0;
            double avg_xy = 0.0;
            double avg_xx = 0.0;
            double beta = 0.0;
            double alpha = 0.0;
            for (var i = 0; i < n; i++)
            {
                var x = knownX[i];
                var y = Math.Log(knownY[i]);
                avg_x += x;
                avg_y += y;
                avg_xy += x * y;
                avg_xx += x * x;
            }
            avg_x /= n;
            avg_y /= n;
            avg_xy /= n;
            avg_xx /= n;

            // Compute linear regression coefficients:
            if (useConst)
            {
                beta = (avg_xy - avg_x * avg_y) / (avg_xx - avg_x * avg_x);
                alpha = avg_y - beta * avg_x;
            }
            else
            {
                beta = avg_xy / avg_xx;
                alpha = 0.0;
            }

            // Compute and return result array:
            return newX.Select(t => Math.Exp(alpha + beta * t)).ToList();

        }
    }
}
