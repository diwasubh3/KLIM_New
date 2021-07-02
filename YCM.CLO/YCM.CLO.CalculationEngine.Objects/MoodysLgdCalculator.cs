using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
    public class MoodysLgdCalculator : ICalculator
    {

        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData, IList<Position> positions, 
            vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {
            var moodyRecovery = allMarketDatas.Max(m => m.MoodyRecovery);

            calculation.MoodysLgd = calculation.WARF.HasValue
                ? (calculation.WARF.Value / 10000) *  ((100-moodyRecovery)/100)
                : (decimal?)null;

            return true;            
        }
    }
}
