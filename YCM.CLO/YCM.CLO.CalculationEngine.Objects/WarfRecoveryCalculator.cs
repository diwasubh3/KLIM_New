using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.CalculationEngine.Objects.Contracts;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class WarfRecoveryCalculator : ICalculator
    {

        //public static List<int> MoodyBasedWarfCalculationFundIds = System.Configuration.ConfigurationManager.AppSettings["MoodyBasedWarfCalculationFundIds"].Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();

        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
            IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {
            var recoveryParams      = parameterValues.Where(p => p.ParameterType.ParameterTypeName == "Recovery").ToList();
            var recoveryMultiplier  = recoveryParams.First(r => r.ParameterValueText == "Multiplier");
            var recoverySubtracter  = recoveryParams.First(r => r.ParameterValueText == "Subtracter");
            var position = positions.FirstOrDefault(p => p.FundId == calculation.FundId);
            var warfMoodyRatingsParameters =
                parameterValues.Where(
                    c =>
                        c.ParameterType.ParameterTypeName == "WARF Rating" &&
                        c.ParameterValueText.Contains("Moody-")).ToList();

            if (marketData != null && position != null)
            {
                var warfRating =
                    warfMoodyRatingsParameters.FirstOrDefault(
                        w => w.ParameterValueText == "Moody-" + marketData.MoodyDPRating);

                if (warfRating != null)
                {
                    calculation.WARF = warfRating.ParameterMaxValueNumber;
                    calculation.WARFRecovery = calculation.WARF -
                                               ((marketData.MoodyRecovery - recoverySubtracter.ParameterMaxValueNumber ??
                                                 43)*recoveryMultiplier.ParameterMaxValueNumber ?? 50);
                }

                //if(MoodyBasedWarfCalculationFundIds.Contains(calculation.FundId))
                //{
                //    var warfDPRating =
                //    warfMoodyRatingsParameters.FirstOrDefault(
                //        w => w.ParameterValueText == "Moody-" + marketData.MoodyDPRating);

                //    if (warfDPRating != null)
                //    {
                //        calculation.WARF = warfDPRating.ParameterMaxValueNumber;
                //    }
                //}
            }

            return true;
        }
    }
}
