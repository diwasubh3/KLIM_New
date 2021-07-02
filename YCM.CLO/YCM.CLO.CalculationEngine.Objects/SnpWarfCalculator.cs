using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
    public class SnpWarfCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData, IList<Position> positions, 
            vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {


            var snpCreditWatch = marketData.SnpCreditWatch;

            var ratingsOffset = snpCreditWatch == "+" ? 1 : (snpCreditWatch == "-" ? -1 : 0);

            var ratings = parameterValues
                .Where(v => 
                    v.ParameterType.ParameterTypeName == "WARF Rating" 
                    && v.ParameterValueText.StartsWith("SnP-", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(v => v.Id)
                .Select(v => v.ParameterValueText.Substring(4))
                .ToArray();

            var currentRatingIndex = Array.IndexOf(ratings, string.IsNullOrEmpty(marketData.SnPIssuerRating) || marketData.SnPIssuerRating == "***MISSING***"? marketData.SnPIssuerRatingAdjusted:  marketData.SnPIssuerRating);

            if(currentRatingIndex == -1)
            {
                calculation.SnpWarf = null;
                return true;
            }

            var updatedRatingsIndex = currentRatingIndex + ratingsOffset;
            
            var snpIssuerRatingAdjusted = updatedRatingsIndex > 0 && updatedRatingsIndex < ratings.Length
                ? ratings[updatedRatingsIndex]
                : ratings[currentRatingIndex];

            var snpWarf = parameterValues.SingleOrDefault(
                    v => v.ParameterType.ParameterTypeName == "SnpIssuerAdjusted"
                    && v.ParameterValueText == snpIssuerRatingAdjusted)
                ?.ParameterValueNumber;

            calculation.SnPIssuerRatingAdjusted = snpIssuerRatingAdjusted;            
            calculation.SnpWarf = snpWarf;

            var effectiveSnpAssetRecovery = int.TryParse(calculation.zSnPAssetRecoveryRating, out int result) ? result : (security.LienType == "First Lien" ? 60 : 25);

            calculation.SnpLgd = snpWarf * (1 - decimal.Divide(effectiveSnpAssetRecovery, 100));

            return true;
        }
    }
}
