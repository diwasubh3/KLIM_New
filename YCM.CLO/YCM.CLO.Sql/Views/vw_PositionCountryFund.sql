CREATE VIEW CLO.vw_PositionCountryFund
AS
	SELECT FundId = f.ParentFundId, p.SecurityId, p.DateId, Exposure = SUM(p.Exposure)
		, NULLIF(SUM(ISNULL(p.PctExposure,0)) * 100,0) PctExposure
		, PxPrice = AVG(p.PxPrice), CreatedOn = MAX(p.CreatedOn), CreatedBy = MAX(p.CreatedBy), LastUpdatedOn = MAX(p.LastUpdatedOn), LastUpdatedBy = MAX(p.LastUpdatedBy), p.IsCovLite, CountryDesc = MAX(c.CountryDesc), 
		FundCode = f.ParentFundCode
	FROM CLO.Position p WITH (NOLOCK)
	LEFT JOIN CLO.Country c WITH (NOLOCK) ON c.CountryId = p.CountryId
	JOIN CLO.Fund f WITH (NOLOCK) ON p.FundId = f.FundId
		AND ISNULL(ISNULL(NULLIF(f.IsStale, CAST(0 AS BIT)), f.IsPrincipalCashStale), 0) = ISNULL(p.IsStale, 0)
	WHERE DateId = CLO.GetPrevDayDateId() AND (f.IsActive = 1 OR f.IsWarehouse = 1)
	GROUP BY f.ParentFundId,p.SecurityId, p.DateId, p.IsCovLite,f.ParentFundCode
GO
