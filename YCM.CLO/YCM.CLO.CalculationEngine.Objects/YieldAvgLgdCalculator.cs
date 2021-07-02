using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
    public class YieldAvgLgdCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData, IList<Position> positions, 
            vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {
            var cappedYieldOfferRatio = calculation.CappedYieldOffer / 100;
            var averageLgd = (DefaultIfNull(calculation.SnpLgd) + DefaultIfNull(calculation.MoodysLgd)) / 2;

            calculation.YieldAvgLgd = SafeDivide(cappedYieldOfferRatio - .015m, averageLgd);

            return true;
        }

        private decimal DefaultIfNull(decimal? d) => d.HasValue ? d.Value : 0;
        private decimal? SafeDivide(decimal? num, decimal? dem) => dem.HasValue && dem.Value != 0 ? num / dem : null; 
    }
}
