CREATE VIEW [CLO].[vw_Report_Equities]
AS 
SELECT SecurityCode=ISNULL(iss.IssuerDesc,eo.SecurityCode),Notional = ISNULL(pos.Exposure,eo.Notional),Bid = ISNULL(mar.Bid, eo.Bid),eo.FundId FROM CLO.EquityOverride eo WITH(NOLOCK) 
LEFT JOIN CLO.Security sec WITH(NOLOCK) ON sec.SecurityCode = eo.SecurityCode
LEFT JOIN CLO.issuer iss WITH(NOLOCK) ON iss.IssuerId = sec.IssuerId
LEFT JOIN CLO.Position pos WITH(NOLOCK) ON pos.SecurityId = sec.SecurityId AND pos.FundId = eo.FundId
LEFT JOIN clo.MarketData mar WITH(NOLOCK) ON mar.SecurityId = sec.SecurityId AND mar.FundId = eo.FundId  
WHERE ISNULL(eo.IsDeleted,0) = 0 AND pos.DateId = clo.GetPrevDayDateId() AND mar.DateId = clo.GetPrevDayDateId()