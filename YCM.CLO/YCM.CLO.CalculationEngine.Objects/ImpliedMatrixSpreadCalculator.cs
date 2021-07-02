using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCM.CLO.CalculationEngine.Objects.Contracts;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.CalculationEngine.Objects
{
    public class ImpliedMatrixSpreadCalculator 
    {

        private static decimal GetWarfPlusRecovery(vw_MatrixData data, FundCalculation fundCalculation)
        {
            return data.Warf.Value - ((fundCalculation.MoodyRecovery.Value - 43) * data.WarfModifier.Value);
        }

        private static decimal? CalculateSlope(vw_MatrixData data1, vw_MatrixData data2, FundCalculation fundCalculation)
        {
            if (data1 != null && data2 != null)
            {
                return (GetWarfPlusRecovery(data1, fundCalculation)
                                                        - GetWarfPlusRecovery(data2, fundCalculation))
                                                        /
                                                        ((data1.Spread * 100) - (data2.Spread * 100));
            }
            else
            {
                return 0;
            }
        }

        public bool Calculate(int dateId, int fundId, string user, ILog _logger)
        {

            using (CLOContext context = new CLOContext())
            {
                var marketDatas = context.vw_MarketData.AsNoTracking().Where(d => d.DateId == dateId).AsNoTracking().ToList();

                context.Funds.AsNoTracking().Where(f => f.IsActive && (f.FundId == fundId || fundId == -1)).ToList().ForEach(fund =>
                {
                    var fundCalculation = context.FundCalculations.FirstOrDefault(f => f.FundId == fund.FundId && f.DateId == dateId);
                    if (fundCalculation != null)
                    {

                        var matrixPoint = context.MatrixPoints.AsNoTracking().Where(m => m.FundId == fund.FundId).OrderByDescending(f => f.Id).FirstOrDefault();
                        if (matrixPoint != null)
                        {

                            using (IRepository _repository = new Repository())
                            {
                                //  1   3
                                //
                                //    2
                                //  7 0 8
                                //    5
                                //
                                //  4   6

                                vw_MatrixData[] vw_MatrixDatas = new vw_MatrixData[9] { null, null, null, null, null, null, null,null,null };

                                vw_MatrixDatas[0] = _repository.GetMatrixData(fund.FundId,matrixPoint.Spread.Value,matrixPoint.Diversity.Value);

                                if(matrixPoint.TopSpread.HasValue)
                                {
                                    vw_MatrixDatas[2] = _repository.GetMatrixData(fund.FundId, matrixPoint.TopSpread.Value, matrixPoint.Diversity.Value);
                                }

                                if(matrixPoint.BottomSpread.HasValue)
                                {
                                    vw_MatrixDatas[5] = _repository.GetMatrixData(fund.FundId, matrixPoint.BottomSpread.Value, matrixPoint.Diversity.Value);
                                }

                                if(matrixPoint.TopMajorSpread.HasValue)
                                {
                                    if (matrixPoint.LeftMajorDiversity.HasValue)
                                    {
                                        vw_MatrixDatas[1] = _repository.GetMatrixData(fund.FundId, matrixPoint.TopMajorSpread.Value, matrixPoint.LeftMajorDiversity.Value);
                                    }

                                    if(matrixPoint.RightMajorDiversity.HasValue)
                                    {
                                        vw_MatrixDatas[3] = _repository.GetMatrixData(fund.FundId, matrixPoint.TopMajorSpread.Value, matrixPoint.RightMajorDiversity.Value);
                                    }
                                }

                                if(matrixPoint.BottomSpread.HasValue)
                                {
                                    if (matrixPoint.LeftMajorDiversity.HasValue)
                                    {
                                        vw_MatrixDatas[4] = _repository.GetMatrixData(fund.FundId, matrixPoint.BottomMajorSpread.Value, matrixPoint.LeftMajorDiversity.Value);
                                    }

                                    if (matrixPoint.RightMajorDiversity.HasValue)
                                    {
                                        vw_MatrixDatas[6] = _repository.GetMatrixData(fund.FundId, matrixPoint.BottomMajorSpread.Value, matrixPoint.RightMajorDiversity.Value);
                                    }
                                }
                                
                                if(matrixPoint.LeftDiversity.HasValue)
                                {
                                    vw_MatrixDatas[7] = _repository.GetMatrixData(fund.FundId, matrixPoint.Spread.Value, matrixPoint.LeftDiversity.Value);
                                }

                                if (matrixPoint.RightDiversity.HasValue)
                                {
                                    vw_MatrixDatas[8] = _repository.GetMatrixData(fund.FundId, matrixPoint.Spread.Value, matrixPoint.RightDiversity.Value);
                                }

                                decimal?[] slopes = new decimal?[6] { 0, 0, 0, 0, 0, 0 };
                                //  1   3
                                //
                                //    2
                                //  7 0 8
                                //    5
                                //
                                //  4   6

                                slopes[0] = CalculateSlope(vw_MatrixDatas[0], vw_MatrixDatas[2], fundCalculation);
                                slopes[1] = CalculateSlope(vw_MatrixDatas[0], vw_MatrixDatas[5], fundCalculation);

                                slopes[2] = CalculateSlope(vw_MatrixDatas[7], vw_MatrixDatas[1], fundCalculation);
                                slopes[3] = CalculateSlope(vw_MatrixDatas[7], vw_MatrixDatas[4], fundCalculation);

                                slopes[4] = CalculateSlope(vw_MatrixDatas[8], vw_MatrixDatas[3], fundCalculation);
                                slopes[5] = CalculateSlope(vw_MatrixDatas[8], vw_MatrixDatas[6], fundCalculation);


                                decimal? avgSlope = slopes.Where(s => s != 0).Average();

                                //Calculate b

                                decimal? yIntercept = GetWarfPlusRecovery(vw_MatrixDatas[0], fundCalculation) - (avgSlope.Value * (matrixPoint.Spread * 100));

                                context.Calculations.Where(p => p.FundId == fund.FundId && p.DateId == dateId).ToList().ForEach(calculation =>
                                {

                                    var marketData = marketDatas.FirstOrDefault(m => m.SecurityId == calculation.SecurityId && m.FundId==calculation.FundId);

                                    if(marketData == null)
                                    {
                                        marketData = marketDatas.FirstOrDefault(m => m.SecurityId == calculation.SecurityId);
                                    }

                                    calculation.MatrixWARFRecovery = (marketData != null ? calculation.WARF - ((marketData.MoodyRecovery - 43) * matrixPoint.WarfModifier) : 0);
                                    calculation.MatrixImpliedSpread = ((calculation.MatrixWARFRecovery - yIntercept) / avgSlope.Value);

                                    calculation.LastUpdatedBy = user;
                                    calculation.LastUpdatedOn = DateTime.Now;
                                    context.Entry(calculation).State = EntityState.Modified;
                                });

                                context.SaveChanges();
                            }
                        }
                    }
                });
                _logger.Info("Completed Frontier Calculation for FundId:" + fundId);
                EmailHelper.SendEmail("Completed Frontier Calculation for FundId:" + fundId, "CLO Frontier Calculation");
                return true;
            }
        }
    }
}
