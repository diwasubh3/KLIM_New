﻿CREATE VIEW [CLO].[vw_MarketData]
AS
	SELECT m.DateId, m.MarketDataId MarketDataId, o.OverrideMarketDataId OverrideMarketDataId
		, m.SecurityId, m.FundId, COALESCE(o.Bid, currentupload.Bid, m.Bid, previousupload.Bid) AS Bid
		, COALESCE(o.Offer, currentupload.Offer, m.Offer, previousupload.Offer) AS Offer
		, COALESCE(o.CostPrice, m.CostPrice) AS CostPrice, ISNULL(o.Spread, m.Spread) AS Spread
		, ISNULL(o.LiborFloor, m.LiborFloor) AS LiborFloor, moodysdprating.RatingDesc AS MoodyDPRating
		, moodycashflowrating.RatingDesc AS MoodyCashFlowRating
		, moodycashflowratingadjusted.RatingDesc AS MoodyCashFlowRatingAdjusted
		, moodyfacilityrating.RatingDesc AS MoodyFacilityRating
		, ISNULL(o.MoodyFacilityRatingId, m.MoodyFacilityRatingId) AS MoodyFacilityRatingId
		, ISNULL(o.MoodyRecovery, m.MoodyRecovery) AS MoodyRecovery
		, snpissuerrating.RatingDesc AS SnPIssuerRating, snpissuerratingadjusted.RatingDesc AS SnPIssuerRatingAdjusted
		, snpfacilityrating.RatingDesc AS SnPFacilityRating, snpfacilityratingadjusted.RatingDesc AS SnPfacilityRatingAdjusted
		, snprecoveryrating.RatingDesc AS SnPRecoveryRating, ISNULL(o.MoodyOutlook, m.MoodyOutlook) AS MoodyOutlook
		, ISNULL(o.MoodyWatch, m.MoodyWatch) AS MoodyWatch, ISNULL(o.SnPWatch, m.SnPWatch) AS SnPWatch
		, ISNULL(o.NextReportingDate, m.NextReportingDate) AS NextReportingDate
		, ISNULL(o.FiscalYearEndDate, m.FiscalYearEndDate) AS FiscalYearEndDate
		, ISNULL(o.AgentBank, m.AgentBank) AS AgentBank
		, m.SnpCreditWatch
		, ISNULL(o.[LiborBaseRate], m.[LiborBaseRate]) AS [LiborBaseRate]
		, MoodyCashFlowRatingAdjustedId = ISNULL(o.MoodyCashFlowRatingAdjustedId, m.MoodyCashFlowRatingAdjustedId)
	FROM CLO.MarketData m WITH (NOLOCK)
	LEFT JOIN CLO.OverrideMarketData o WITH (NOLOCK) ON m.DateId = o.DateId
											AND m.SecurityId = o.SecurityId
											AND m.FundId = o.FundId
	LEFT JOIN CLO.Rating moodysdprating WITH (NOLOCK) ON moodysdprating.RatingId = ISNULL(o.[MoodyDPRatingId], m.[MoodyDPRatingId])
	LEFT JOIN CLO.Rating moodycashflowrating WITH (NOLOCK) ON moodycashflowrating.RatingId = ISNULL(o.MoodyCashFlowRatingId, m.MoodyCashFlowRatingId)
	LEFT JOIN CLO.Rating moodycashflowratingadjusted WITH (NOLOCK)
		ON moodycashflowratingadjusted.RatingId = ISNULL(o.MoodyCashFlowRatingAdjustedId, m.MoodyCashFlowRatingAdjustedId)
	LEFT JOIN CLO.Rating moodyfacilityrating WITH (NOLOCK) ON moodyfacilityrating.RatingId = ISNULL(o.MoodyFacilityRatingId, m.MoodyFacilityRatingId)
	LEFT JOIN CLO.Rating snpissuerrating WITH (NOLOCK) ON snpissuerrating.RatingId = ISNULL(o.SnPIssuerRatingId, m.SnPIssuerRatingId)
	LEFT JOIN CLO.Rating snpissuerratingadjusted WITH (NOLOCK)
		ON snpissuerratingadjusted.RatingId = ISNULL(o.SnPIssuerRatingAdjustedId, m.SnPIssuerRatingAdjustedId)
	LEFT JOIN CLO.Rating snpfacilityrating WITH (NOLOCK) ON snpfacilityrating.RatingId = ISNULL(o.SnPFacilityRatingId, m.SnPFacilityRatingId)
	LEFT JOIN CLO.Rating snpfacilityratingadjusted WITH (NOLOCK)
		ON snpfacilityratingadjusted.RatingId = ISNULL(o.SnPfacilityRatingAdjustedId, m.SnPfacilityRatingAdjustedId)
	LEFT JOIN CLO.Rating snprecoveryrating WITH (NOLOCK) ON snprecoveryrating.RatingId = ISNULL(o.SnPRecoveryRatingId, m.SnPRecoveryRatingId)
	LEFT JOIN CLO.vw_Price currentupload WITH (NOLOCK) ON m.DateId = currentupload.DateId
											AND m.SecurityId = currentupload.SecurityId
	LEFT JOIN CLO.vw_Price previousupload WITH (NOLOCK) ON m.DateId > currentupload.DateId
											AND m.SecurityId = currentupload.SecurityId
	JOIN CLO.Position p WITH (NOLOCK) ON m.SecurityId = p.SecurityId
											AND m.FundId = p.FundId
											AND m.DateId = p.DateId
	WHERE m.DateId = CLO.GetPrevDayDateId()