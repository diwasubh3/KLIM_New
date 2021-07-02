CREATE VIEW CLO.vw_PivotedTradeTotalAllocation
AS
	SELECT	SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9] 
	FROM	(SELECT	TotalAllocation, FundCode, SecurityId FROM CLO.GetActiveTrades()) AS s PIVOT
(MAX(TotalAllocation) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9] ) ) AS pvt
GO
