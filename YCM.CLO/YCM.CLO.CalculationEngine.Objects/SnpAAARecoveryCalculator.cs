using System.Collections.Generic;
using System.Linq;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
    public class SnpAAARecoveryCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData, IList<Position> positions, 
            vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {
            var snpAAARecoveryMappings = parameterValues
                .Where(v => v.ParameterType.ParameterTypeName == "AAARecoveryMapping")
                .ToDictionary(p => p.ParameterValueText, p => p.ParameterValueNumber);

            var snpAAARecoveryLienTypeMapping = parameterValues
                .Where(v => v.ParameterType.ParameterTypeName == "AAARecoveryLienType")
                .ToDictionary(p => p.ParameterValueText, p => p.ParameterValueNumber);

            if (snpAAARecoveryMappings.ContainsKey(calculation.zSnPAssetRecoveryRating ?? string.Empty))
                calculation.SnpAAARecovery = snpAAARecoveryMappings[calculation.zSnPAssetRecoveryRating];
            else if (snpAAARecoveryLienTypeMapping.ContainsKey(security.LienType))
                calculation.SnpAAARecovery = snpAAARecoveryLienTypeMapping[security.LienType];

            return true;
        }
    }
}