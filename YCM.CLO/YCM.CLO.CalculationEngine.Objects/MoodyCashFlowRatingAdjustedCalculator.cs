using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class MoodyCashFlowRatingAdjustedCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
           IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint,  int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {
            var moodyCfrs = allMarketDatas.Where(m => m.MoodyCashFlowRatingAdjustedId.HasValue && m.MoodyCashFlowRatingAdjustedId.Value > 0).ToList();
            if (moodyCfrs.Count > 0)
            {
                calculation.MoodyCashFlowRatingAdjustedId = moodyCfrs.OrderByDescending(c => c.FundId).First().MoodyCashFlowRatingAdjustedId;
            }
            else
            {
                calculation.MoodyCashFlowRatingAdjustedId = -1;
            }
            return true;
        }
    }
}
