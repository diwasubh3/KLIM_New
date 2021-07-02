using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class MoodyFacilityRatingAdjustedCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
           IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint,  int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {

            ParameterValue[] moodyRatings = parameterValues.Where
                (p => p.ParameterType.ParameterTypeName == "WARF Rating"
                      && !string.IsNullOrEmpty(p.ParameterValueText)
                      && p.ParameterValueText.StartsWith("Moody-")).OrderByDescending(p => p.ParameterMaxValueNumber).ToArray();

            if (marketData?.MoodyFacilityRatingId != null)
            {
                calculation.MoodyFacilityRatingAdjustedId = marketData.MoodyFacilityRatingId;
                
                if ((marketData.MoodyWatch == "-" || marketData.MoodyOutlook == "-" || marketData.MoodyWatch == "+" ) && marketData.MoodyFacilityRatingId != -1)
                {
                    var moodyFacilityRatingParam =
                            moodyRatings.FirstOrDefault(
                                m =>
                                    m.ParameterValueText.ToUpper() ==
                                    "MOODY-" + marketData.MoodyFacilityRating.ToUpper());

                    if (moodyFacilityRatingParam != null)
                    {
                        var index = Array.IndexOf(moodyRatings, moodyFacilityRatingParam);
                        if (marketData.MoodyWatch == "-")
                        {
                            index += -2;
                        }
                        else if (marketData.MoodyOutlook == "-")
                        {
                            index += -1;
                        }
                        else if (marketData.MoodyWatch == "+")
                        {
                            index += 1;
                        }

                        if (index >= moodyRatings.Length)
                        {
                            index = moodyRatings.Length - 1;
                        }
                        else if (index < 0)
                        {
                            index = 0;
                        }

                        var newRatingsText = moodyRatings[index].ParameterValueText;

                        var newRating =
                            cloContext.Ratings.FirstOrDefault(
                                r => "MOODY-" + r.RatingDesc.ToUpper() == newRatingsText);

                        if (newRating != null)
                        {
                            calculation.MoodyFacilityRatingAdjustedId = newRating.RatingId;
                        }
                    }
                }
            }


            return true;
        }
    }
}
