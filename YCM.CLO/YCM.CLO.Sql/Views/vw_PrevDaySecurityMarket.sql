﻿CREATE VIEW [CLO].[vw_PrevDaySecurityMarket]
AS 
SELECT 
p.SecurityId,
m.DateId,
AVG(COALESCE(currentupload.[Bid],m.[Bid],previousupload.[Bid])) AS [Bid],
AVG(COALESCE(currentupload.[Offer], m.[Offer],previousupload.[Offer])) AS [Offer],
CLO.SafeDivideBy(SUM(ISNULL(p.Exposure,0)*ISNULL(m.CostPrice,0)),SUM(ISNULL(p.Exposure,0))) CostPrice,
AVG(m.[Spread]) AS [Spread],
AVG(m.[LiborFloor]) AS [LiborFloor],
MAX(moodycashflowrating.RatingDesc) [MoodyCashFlowRating],
MAX(moodycashflowratingadjusted.RatingDesc) [MoodyCashFlowRatingAdjusted],
MAX(moodyfacilityrating.RatingDesc) [MoodyFacilityRating],
MAX(m.[MoodyFacilityRatingId])  MoodyFacilityRatingId,
MAX(m.[MoodyRecovery]) AS [MoodyRecovery],
MAX(snpissuerrating.RatingDesc) [SnPIssuerRating],
MAX(snpissuerratingadjusted.RatingDesc) [SnPIssuerRatingAdjusted],
MAX(snpfacilityrating.RatingDesc) [SnPFacilityRating],
MAX(snpfacilityratingadjusted.RatingDesc) [SnPfacilityRatingAdjusted],
MAX(snprecoveryrating.RatingDesc) [SnPRecoveryRating],
MAX(m.[MoodyOutlook]) AS [MoodyOutlook],
MAX(m.[MoodyWatch]) AS [MoodyWatch],
MAX(m.[SnPWatch]) AS [SnPWatch],
MAX(m.[NextReportingDate]) AS [NextReportingDate],
MAX(m.[FiscalYearEndDate]) AS [FiscalYearEndDate],
MAX(m.[AgentBank]) AS [AgentBank]

FROM CLO.MarketData m WITH(NOLOCK)
JOIN CLO.Position p WITH(NOLOCK) ON m.SecurityId = p.SecurityId AND m.FundId = p.FundId AND p.DateId = m.DateId
LEFT JOIN CLO.Rating moodycashflowrating  with(nolock)  on moodycashflowrating.RatingId = m.[MoodyCashFlowRatingId]
LEFT JOIN CLO.Rating moodycashflowratingadjusted  with(nolock)  on moodycashflowratingadjusted.RatingId = m.[MoodyCashFlowRatingAdjustedId]
LEFT JOIN CLO.Rating moodyfacilityrating  with(nolock)  on moodyfacilityrating.RatingId = m.[MoodyFacilityRatingId]
LEFT JOIN CLO.Rating snpissuerrating  with(nolock)  on snpissuerrating.RatingId = m.[SnPIssuerRatingId]
LEFT JOIN CLO.Rating snpissuerratingadjusted  with(nolock)  on snpissuerratingadjusted.RatingId = m.[SnPIssuerRatingAdjustedId]
LEFT JOIN CLO.Rating snpfacilityrating  with(nolock)  on snpfacilityrating.RatingId = m.[SnPFacilityRatingId]
LEFT JOIN CLO.Rating snpfacilityratingadjusted  with(nolock)  on snpfacilityratingadjusted.RatingId = m.[SnPfacilityRatingAdjustedId]
LEFT JOIN CLO.Rating snprecoveryrating  with(nolock)  on snprecoveryrating.RatingId = m.[SnPRecoveryRatingId]
LEFT JOIN [CLO].[vw_Price] currentupload WITH(NOLOCK) ON m.DateId =  currentupload.DateId AND m.SecurityId = currentupload.SecurityId
LEFT JOIN [CLO].[vw_Price] previousupload WITH(NOLOCK) ON m.DateId >  currentupload.DateId AND m.SecurityId = currentupload.SecurityId

WHERE p.DateId = [CLO].[GetPrevToPrevDayDateId]()
GROUP BY p.SecurityId,m.DateId