using System;
using System.Collections.Generic;
using System.Linq;
using Excel.FinancialFunctions;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.CalculationEngine.Objects.Contracts;

namespace YCM.CLO.CalculationEngine.Objects
{
	public class YieldCalculator : ICalculator
    {
        public bool Calculate(CLOContext cloContext, Calculation calculation, vw_MarketData marketData,
            IList<Position> positions, vw_SecurityFund security, IList<ParameterValue> parameterValues, MatrixPoint matrixPoint, int calculationDateId, IList<vw_MarketData> allMarketDatas)
        {

            var maxCappedYield = parameterValues.First(c => c.ParameterType.ParameterTypeName == "Capped Yield");


            if (marketData != null)
            {
                double bid = (double)(marketData.Bid ?? 0) ;
                double offer = (double)(marketData.Offer??0);
                double totalCoupon = (double)(marketData.Spread + marketData.LiborFloor);
                DateTime maturityDate = security.MaturityDate ?? DateTime.Now;
                DateTime priceDate = DateTime.Today;

                DateTime maturityOffer = maturityDate;
                if (offer > 100.5)
                {
                    int minDays = (int)Math.Min((double)365 * 1.5, (double)(maturityOffer - priceDate).TotalDays);
                    maturityOffer = priceDate.AddDays(minDays);
                }

                DateTime maturityBid = maturityDate;
                if (bid > 100.5)
                {
                    int minDays = (int)Math.Min((double)365 * 1.5, (double)(maturityBid - priceDate).TotalDays);
                    maturityBid = priceDate.AddDays(minDays);
                }

                double couponPct = (double)(totalCoupon / 100);

                if (offer > 0 && bid > 0 && maturityDate.Date > priceDate.Date) 
                {
                    decimal offerYield = Math.Round((decimal)Financial.Yield(priceDate, maturityOffer, couponPct > 0 ? couponPct : 0, offer, 100.0, Frequency.Annual, DayCountBasis.ActualActual) * 100, 2);
                    decimal bidYield = Math.Round((decimal)Financial.Yield(priceDate, maturityBid, couponPct > 0 ? couponPct : 0, bid, 100.0, Frequency.Annual, DayCountBasis.ActualActual) * 100, 2);
                    calculation.YieldBid = bidYield;
                    calculation.YieldOffer = offerYield;
                    calculation.YieldMid = Math.Round((bidYield + offerYield) / 2, 2);

                    calculation.CappedYieldBid = bidYield > (maxCappedYield.ParameterMaxValueNumber ?? 0)
                        ? maxCappedYield.ParameterMaxValueNumber
                        : bidYield;

                    calculation.CappedYieldOffer = offerYield > (maxCappedYield.ParameterMaxValueNumber ?? 0)
                        ? maxCappedYield.ParameterMaxValueNumber
                        : offerYield;
                }


                calculation.TotalCoupon = (decimal)totalCoupon;

                calculation.CappedYieldMid = calculation.YieldMid > (maxCappedYield.ParameterMaxValueNumber ?? 0)
                    ? maxCappedYield.ParameterMaxValueNumber
                    : calculation.YieldMid;

                calculation.TotalPar = positions.Where(p => p.SecurityId == marketData.SecurityId).Select(p => p.Exposure).Sum();
                calculation.Life = (decimal)maturityDate.Subtract(priceDate).TotalDays;
            }

            return true;
        }
    }
}
