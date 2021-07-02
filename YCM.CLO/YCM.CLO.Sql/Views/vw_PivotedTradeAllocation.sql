CREATE VIEW CLO.vw_PivotedTradeAllocation
AS
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9] 
	FROM (SELECT	Allocation, FundCode, SecurityId FROM CLO.GetActiveTrades()) AS s PIVOT
(MAX(Allocation) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9] )) AS pvt
GO
