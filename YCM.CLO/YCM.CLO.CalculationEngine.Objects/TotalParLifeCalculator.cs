using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess.Models;
using System.Globalization;
using YCM.CLO.CalculationEngine.Objects.Contracts;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class TotalParLifeCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
            IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {

                DateTime today = DateTime.ParseExact(calculationDateId.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime maturityDate = security.MaturityDate ?? DateTime.Now;
                calculation.TotalPar = positions.Select(p => p.Exposure.HasValue? p.Exposure : 0).Sum();
                calculation.Life = (decimal)maturityDate.Subtract(today).TotalDays;

            return true;
        }
    }
}
