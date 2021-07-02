CREATE VIEW [clo].[vw_Report_Funds]
AS 

SELECT f.FundCode, f.TargetPar,f.EquityPar,f.MgmtFees,f.OperatingExpenses,f.BloombergCode,f.PricingDate,f.ClosingDate,f.NonCallEndsDate,f.ReInvestEndDate,f.FinalMaturity,f.ProjectedEquityDistribtion,
fc.TimeToNonCallEnd,fc.TimeToReinvest,fc.Spread,fc.WACostOfDebt,fc.TotalManagementFees,Net = NULLIF((ISNULL(fc.Net,0)/100),0),fc.EquityLeverage,fc.AnnualExcessSpreadToEquity,fc.ClassDMVOC,fc.ClassEMVOC,fc.ClassFMVOC,fc.EquityNav,
fc.FundId,fc.DateId,fc.TotalDebt
FROM CLO.vw_Fund f WITH(NOLOCK)
JOIN CLO.FundCalculation fc WITH(NOLOCK) ON fc.FundId = f.FundId 
WHERE fc.DateId = CLO.GetPrevDayDateId()