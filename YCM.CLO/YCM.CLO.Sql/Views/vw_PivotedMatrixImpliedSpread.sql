CREATE VIEW [CLO].[vw_PivotedMatrixImpliedSpread]
	AS 
SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7]
	FROM (SELECT c.[MatrixImpliedSpread], f.FundCode, c.SecurityId
			FROM CLO.vw_Calculation c WITH (NOLOCK)
			JOIN CLO.Fund f WITH(NOLOCK) ON f.FundId = c.FundId
			WHERE c.DateId = CLO.GetPrevDayDateId()
			) AS s PIVOT
(MAX([MatrixImpliedSpread]) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7]) )AS pvt
