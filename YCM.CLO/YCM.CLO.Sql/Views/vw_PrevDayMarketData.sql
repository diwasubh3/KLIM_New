CREATE VIEW CLO.vw_PrevDayMarketData
AS
	SELECT m.DateId, m.MarketDataId MarketDataId, o.OverrideMarketDataId OverrideMarketDataId, m.SecurityId, m.FundId
			, COALESCE(o.Bid, currentupload.Bid, m.Bid, previousupload.Bid) AS Bid, COALESCE(o.Offer, currentupload.Offer, m.Offer, previousupload.Offer) AS Offer
			, COALESCE(o.CostPrice, m.CostPrice) AS CostPrice, ISNULL(o.Spread, m.Spread) AS Spread, ISNULL(o.LiborFloor, m.LiborFloor) AS LiborFloor
			, moodycashflowrating.RatingDesc AS MoodyCashFlowRating, moodycashflowratingadjusted.RatingDesc AS MoodyCashFlowRatingAdjusted
			, moodyfacilityrating.RatingDesc AS MoodyFacilityRating, ISNULL(o.MoodyFacilityRatingId, m.MoodyFacilityRatingId) AS MoodyFacilityRatingId
			, ISNULL(o.MoodyRecovery, m.MoodyRecovery) AS MoodyRecovery, snpissuerrating.RatingDesc AS SnPIssuerRating
			, snpissuerratingadjusted.RatingDesc AS SnPIssuerRatingAdjusted, snpfacilityrating.RatingDesc AS SnPFacilityRating
			, snpfacilityratingadjusted.RatingDesc AS SnPfacilityRatingAdjusted, snprecoveryrating.RatingDesc AS SnPRecoveryRating
			, ISNULL(o.MoodyOutlook, m.MoodyOutlook) AS MoodyOutlook, ISNULL(o.MoodyWatch, m.MoodyWatch) AS MoodyWatch, ISNULL(o.SnPWatch, m.SnPWatch) AS SnPWatch
			, ISNULL(o.NextReportingDate, m.NextReportingDate) AS NextReportingDate, ISNULL(o.FiscalYearEndDate, m.FiscalYearEndDate) AS FiscalYearEndDate
			, ISNULL(o.AgentBank, m.AgentBank) AS AgentBank, ROW_NUMBER() OVER (PARTITION BY m.SecurityId, m.FundId ORDER BY m.DateId DESC) AS MarketRowNum
	FROM CLO.MarketData m (NOLOCK)
	LEFT JOIN CLO.OverrideMarketData o (NOLOCK) ON m.DateId = o.DateId
														AND m.SecurityId = o.SecurityId
														AND m.FundId = o.FundId
	LEFT JOIN CLO.Rating moodycashflowrating (NOLOCK) ON moodycashflowrating.RatingId = ISNULL(o.MoodyCashFlowRatingId, m.MoodyCashFlowRatingId)
	LEFT JOIN CLO.Rating moodycashflowratingadjusted (NOLOCK) ON moodycashflowratingadjusted.RatingId = ISNULL(o.MoodyCashFlowRatingAdjustedId,
																													m.MoodyCashFlowRatingAdjustedId)
	LEFT JOIN CLO.Rating moodyfacilityrating (NOLOCK) ON moodyfacilityrating.RatingId = ISNULL(o.MoodyFacilityRatingId, m.MoodyFacilityRatingId)
	LEFT JOIN CLO.Rating snpissuerrating (NOLOCK) ON snpissuerrating.RatingId = ISNULL(o.SnPIssuerRatingId, m.SnPIssuerRatingId)
	LEFT JOIN CLO.Rating snpissuerratingadjusted (NOLOCK) ON snpissuerratingadjusted.RatingId = ISNULL(o.SnPIssuerRatingAdjustedId,
																											m.SnPIssuerRatingAdjustedId)
	LEFT JOIN CLO.Rating snpfacilityrating (NOLOCK) ON snpfacilityrating.RatingId = ISNULL(o.SnPFacilityRatingId, m.SnPFacilityRatingId)
	LEFT JOIN CLO.Rating snpfacilityratingadjusted (NOLOCK) ON snpfacilityratingadjusted.RatingId = ISNULL(o.SnPfacilityRatingAdjustedId,
																												m.SnPfacilityRatingAdjustedId)
	LEFT JOIN CLO.Rating snprecoveryrating (NOLOCK) ON snprecoveryrating.RatingId = ISNULL(o.SnPRecoveryRatingId, m.SnPRecoveryRatingId)
	LEFT JOIN CLO.vw_Price currentupload (NOLOCK) ON m.DateId = currentupload.DateId
														  AND m.SecurityId = currentupload.SecurityId
	LEFT JOIN CLO.vw_Price previousupload (NOLOCK) ON m.DateId > currentupload.DateId
														   AND m.SecurityId = currentupload.SecurityId
	WHERE m.DateId = CLO.GetPrevToPrevDayDateId()
GO
