CREATE PROCEDURE [CLO].[spGetPriceMove] @fromDateId INT = 0, @toDateId INT = 0, @sectionName VARCHAR(100)
AS
SELECT m.[DateId], m.[SecurityId], COALESCE(MAX(currentupload.[Bid]), MAX(m.[Bid]), MAX(previousupload.[Bid])) AS [Bid]
	 , MAX(cm.[CostPrice]) AS [CostPrice], MAX(cm.[MoodyCashFlowRatingAdjusted]) AS [MoodyCFR]
INTO #MarketData1
FROM CLO.MarketData m WITH (NOLOCK) LEFT JOIN CLO.vw_SecurityMarket cm WITH (NOLOCK)
	ON cm.SecurityId = m.[SecurityId]
	LEFT JOIN [CLO].[vw_Price] currentupload WITH (NOLOCK)
	ON m.DateId = currentupload.DateId AND m.SecurityId = currentupload.SecurityId
	LEFT JOIN [CLO].[vw_Price] previousupload WITH (NOLOCK)
	ON m.DateId > currentupload.DateId AND m.SecurityId = currentupload.SecurityId
	INNER JOIN CLO.Fund F
	ON F.FundId = m.FundId AND (F.IsActive = 1 OR F.IsWarehouse = 1)
WHERE m.DateId = @fromDateId
GROUP BY m.DateId, m.SecurityId;

SELECT m.[DateId], m.[SecurityId], COALESCE(MAX(currentupload.[Bid]), MAX(m.[Bid]), MAX(previousupload.[Bid])) AS [Bid]
INTO #MarketData2
FROM CLO.MarketData m WITH (NOLOCK) LEFT JOIN [CLO].[vw_Price] currentupload WITH (NOLOCK)
	ON m.DateId = currentupload.DateId AND m.SecurityId = currentupload.SecurityId
	LEFT JOIN [CLO].[vw_Price] previousupload WITH (NOLOCK)
	ON m.DateId > currentupload.DateId AND m.SecurityId = currentupload.SecurityId
	INNER JOIN CLO.Fund F
	ON F.FundId = m.FundId AND (F.IsActive = 1 OR F.IsWarehouse = 1)
WHERE m.DateId = @toDateId
GROUP BY m.DateId, m.SecurityId;

SELECT s.Issuer Issuer, m1.Bid Bid, (m1.[Bid] - m2.[Bid]) PriceMove, SUM(p.Exposure) TotalPar
	 , CLO.SafeDivideBy(SUM(p.Exposure * m1.CostPrice), SUM(p.Exposure)) CostPrice, MAX(m1.[MoodyCFR]) [MoodyCFR]
INTO #Result
FROM [CLO].[vw_Security] s LEFT JOIN [CLO].Position p WITH (NOLOCK)
ON s.SecurityId = p.SecurityId
LEFT JOIN #MarketData1 m1 WITH (NOLOCK)
ON s.SecurityId = m1.SecurityId
LEFT JOIN #MarketData2 m2 WITH (NOLOCK)
ON s.SecurityId = m2.SecurityId
WHERE p.DateId = @fromDateId AND ISNULL(m1.Bid, 0) <> 0 AND ISNULL(m2.Bid, 0) <> 0 AND m1.Bid <> m2.Bid
GROUP BY s.Issuer, m1.[Bid], m2.[Bid];

SELECT TOP 15 *, ROW_NUMBER() OVER (ORDER BY CASE WHEN @sectionName = 'Bottom' THEN p.PriceMove
											END DESC, CASE WHEN @sectionName = 'Top' THEN p.PriceMove
													  END ASC) RowNum
FROM #Result p
WHERE ISNULL(p.PriceMove, 0) <> 0 AND ISNULL(p.TotalPar, 0) <> 0 AND p.TotalPar > 1
ORDER BY RowNum;

DROP TABLE #MarketData1;
DROP TABLE #MarketData2;
DROP TABLE #Result;

GO