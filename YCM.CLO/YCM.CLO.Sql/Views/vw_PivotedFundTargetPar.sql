CREATE VIEW CLO.vw_PivotedFundTargetPar
	AS 

SELECT  [CLO-1]
      , [CLO-2]
      , [CLO-3]
      , [CLO-4]
      , [CLO-5]
      , [CLO-6]
      , [CLO-7]
	  , [CLO-8]
	  , [CLO-9]
FROM 
(
    SELECT 
    TargetPar = SUM(TargetPar),FundCode = [ParentFundCode]
    FROM CLO.Fund (NOLOCK)
	WHERE IsActive = 1
	GROUP BY [ParentFundCode]
) AS s
PIVOT
(
    MAX(TargetPar)
    FOR [FundCode] IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9]
,WH5, WH1)
) AS pvt
GO
