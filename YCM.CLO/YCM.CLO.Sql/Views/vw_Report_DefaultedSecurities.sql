CREATE VIEW [clo].[vw_Report_DefaultedSecurities]
AS 
SELECT sec.SecurityDesc,ds.Exposure,ds.Bid,ds.DefaultDate,ds.FundId,ds.DateId FROM CLO.vw_DefaultSecurities ds
JOIN clo.Security sec ON sec.SecurityId = ds.SecurityId
WHERE ds.DateId = CLO.GetPrevDayDateId()
