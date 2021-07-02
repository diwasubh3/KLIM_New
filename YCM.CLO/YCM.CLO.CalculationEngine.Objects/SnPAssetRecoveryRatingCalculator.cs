using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess.Models;
using System.Globalization;
using YCM.CLO.CalculationEngine.Objects.Contracts;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class SnPAssetRecoveryRatingCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
            IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {
            string snpAssetRecoveryRating = positions.Max(p => p.SnPAssetRecoveryRating);
            if(!string.IsNullOrEmpty(snpAssetRecoveryRating))
            {
               snpAssetRecoveryRating = snpAssetRecoveryRating.Substring(snpAssetRecoveryRating.IndexOf("(") + 1, 2).Replace("%", "");
            }

            calculation.zSnPAssetRecoveryRating = snpAssetRecoveryRating;

            return true;
        }
    }
}
