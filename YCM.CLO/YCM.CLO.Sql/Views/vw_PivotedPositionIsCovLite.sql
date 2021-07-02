CREATE VIEW CLO.vw_PivotedPositionIsCovLite
AS
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8] , [CLO-9] 
	FROM (SELECT (CASE WHEN IsCovLite = 1 THEN 1 ELSE 0 END) IsCovLite
			, FundCode, SecurityId
			FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
(MAX(IsCovLite) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9] ) )AS pvt
GO
