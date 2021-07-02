using System.Collections.Generic;
using YCM.CLO.DataAccess.Models;



namespace YCM.CLO.CalculationEngine.Objects.Contracts
{
	interface ICalculator
    {
        bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
            IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas);
    }
}
