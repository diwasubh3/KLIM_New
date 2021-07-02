CREATE VIEW CLO.vw_PivotedPositionExposure
AS
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9]
	FROM (SELECT Exposure, FundCode, SecurityId
			FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
(MAX(Exposure) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9]) )AS pvt
GO
