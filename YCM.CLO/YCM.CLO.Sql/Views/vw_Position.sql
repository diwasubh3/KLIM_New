USE [Yoda]
GO

/****** Object:  View [CLO].[vw_Position]    Script Date: 1/4/2021 1:10:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [CLO].[vw_Position]
AS
WITH prev_position
AS	(
			SELECT PositionId, f.ParentFundId FundId, SecurityId, DateId, MarketValue, Exposure, PctExposure, PxPrice, IsCovLite, CountryId
			, p.CreatedOn, p.CreatedBy, p.LastUpdatedOn, p.LastUpdatedBy, p.IsStale, p.CapitalizedInterestOrig,p.[SnPAssetRecoveryRating]
		FROM CLO.Position p WITH (NOLOCK) 
		JOIN CLO.Fund f WITH(NOLOCK)  ON p.FundId = f.FundId
		WHERE p.DateId = CLO.GetPrevDayDateId() OR p.PositionId IS NULL
	)
SELECT p.PositionId, p.DateId PositionDateId
	, FORMAT(NULLIF((ISNULL(p.Exposure, 0) + ISNULL(s.Allocation, 0)), 0), '#,###') Exposure
	, (ISNULL(p.Exposure, 0) + ISNULL(s.Allocation, 0)) NumExposure, ISNULL(p.Exposure, 0) BODExposure
	, ISNULL(p.Exposure, 0) + ISNULL((CASE WHEN s.HasBuy = 1 THEN s.Allocation ELSE 0 END), 0) BODwithBuyExposure
	, FORMAT(((ISNULL(p.Exposure, 0) + ISNULL(s.Allocation, 0)) / s.TargetPar), 'p') PctExposure
	, ISNULL(p.CapitalizedInterestOrig, 0) CapitalizedInterestOrig,
	p.[SnPAssetRecoveryRating],
	p.PxPrice
	, ((ISNULL(p.Exposure, 0) + ISNULL(s.Allocation, 0)) / s.TargetPar) PctExposureNum, s.FundId, s.FundCode
	, s.FundDesc, s.SecurityId, s.SecurityCode, s.SecurityDesc, s.BBGId, s.Issuer, s.IssuerId
	, s.IssuerDesc, LTRIM(s.Facility) Facility
	, CASE WHEN s.CallDate <> '1900-01-01' THEN CONVERT(VARCHAR(10), s.CallDate, 101) ELSE NULL END CallDate
	, country.CountryDesc, CONVERT(VARCHAR(10), s.MaturityDate, 101) MaturityDate
	, s.SnpIndustry, s.MoodyIndustry, CASE WHEN (p.IsCovLite = 1) THEN 'Y' ELSE 'N' END IsCovLite
	, CASE WHEN (s.IsFloating = 1) THEN 'Floating' ELSE 'Fixed' END IsFloating
	, s.LienType

	, s.IsOnWatch, s.WatchObjectTypeId, s.WatchObjectId, s.WatchId, s.WatchComments, s.WatchLastUpdatedOn, s.WatchUser

	, s.SellCandidateObjectTypeId, s.SellCandidateObjectId, s.SellCandidateId, s.SellCandidateComments , s.SellCandidateLastUpdatedOn, s.SellCandidateUser

	, s.OrigSecurityCode, s.OrigSecurityDesc, s.OrigBBGId
	, s.OrigIssuer, s.OrigFacility, s.OrigCallDate, s.OrigMaturityDate, s.OrigSnpIndustry
	, s.OrigMoodyIndustry, s.OrigIsFloating, s.OrigLienType
	, c.MoodyFacilityRatingAdjusted AS OrigMoodyFacilityRatingAdjusted
	, m.MoodyCashFlowRatingAdjusted AS OrigMoodyCashFlowRatingAdjusted, s.PrincipalCash, s.LiabilityPar, s.EquityPar
	, s.MoodyIndustryId, s.SecurityLastUpdatedOn, s.SecurityLastUpdatedBy, s.SecurityCreatedOn
	, s.SecurityCreatedBy, m.DateId MarketDateId, m.MarketDataId, m.OverrideMarketDataId
	, CONVERT(VARCHAR, CAST(m.Bid AS MONEY), 1) Bid, CONVERT(VARCHAR, CAST(m.Offer AS MONEY), 1) Offer
	, CONVERT(VARCHAR, CAST(pm.Bid AS MONEY), 1) PrevDayBid
	, CONVERT(VARCHAR, CAST(pm.Offer AS MONEY), 1) PrevDayOffer
	, CONVERT(VARCHAR, CAST(m.CostPrice AS MONEY), 1) CostPrice, m.CostPrice CostPriceNum, m.Bid BidNum
	, m.Offer OfferNum
	, CASE WHEN ISNULL(m.Bid, 0) <> 0 AND ISNULL(pm.Bid, 0) <> 0 THEN FORMAT(((m.Bid - pm.Bid) / m.Bid), 'p') ELSE NULL END PctBidDiff
	, CASE WHEN ISNULL(m.Bid, 0) <> 0 AND ISNULL(pm.Bid, 0) <> 0 THEN ((m.Bid - pm.Bid) / m.Bid) ELSE NULL END PctBidDiffNum
	, CASE WHEN ISNULL(m.Offer, 0) <> 0 AND ISNULL(pm.Offer, 0) <> 0 THEN FORMAT(((m.Offer - pm.Offer) / m.Offer), 'p') ELSE NULL END PctOfferDiff
	, CASE WHEN ISNULL(m.Offer, 0) <> 0 AND ISNULL(pm.Offer, 0) <> 0 THEN ((m.Offer - pm.Offer) / m.Offer) ELSE NULL END PctOfferDiffNum
	, CASE WHEN ISNULL(m.Bid, 0) <> 0 AND ISNULL(pm.Bid, 0) <> 0 THEN (m.Bid - pm.Bid) ELSE NULL END PriceMove
	, pm.Bid PrevDayBidNum, pm.Offer PrevDayOfferNum
	, CAST(m.Spread AS NUMERIC(20, 2)) Spread, CAST(m.LiborFloor AS NUMERIC(20, 2)) LiborFloor
	, m.MoodyCashFlowRating
	, ISNULL(s.MoodyCashFlowRatingAdjusted, m.MoodyCashFlowRatingAdjusted) MoodyCashFlowRatingAdjusted
	, m.MoodyFacilityRating, m.MoodyRecovery, m.SnPIssuerRating, m.SnPIssuerRatingAdjusted
	, m.SnPFacilityRating, m.SnPfacilityRatingAdjusted, m.SnPRecoveryRating, m.MoodyOutlook, m.MoodyWatch
	, m.SnPWatch, CONVERT(VARCHAR(10), m.NextReportingDate, 101) NextReportingDate
	, CONVERT(VARCHAR(10), m.FiscalYearEndDate, 101) FiscalYearEndDate, c.CalculationId, c.YieldBid
	, c.YieldOffer, CAST(c.YieldMid AS NUMERIC(20, 2)) YieldMid
	, CAST(c.CappedYieldBid AS NUMERIC(20, 2)) CappedYieldBid
	, CAST(c.CappedYieldOffer AS NUMERIC(20, 2)) CappedYieldOffer
	, CAST(c.CappedYieldMid AS NUMERIC(20, 2)) CappedYieldMid
	, CAST(c.TargetYieldBid AS NUMERIC(20, 2)) TargetYieldBid
	, CAST(c.TargetYieldOffer AS NUMERIC(20, 2)) TargetYieldOffer
	, CAST(c.TargetYieldMid AS NUMERIC(20, 2)) TargetYieldMid
	, CAST(c.BetterWorseBid AS NUMERIC(20, 2)) BetterWorseBid
	, CAST(c.BetterWorseOffer AS NUMERIC(20, 2)) BetterWorseOffer
	, CAST(c.BetterWorseMid AS NUMERIC(20, 2)) BetterWorseMid
	, CAST(c.TotalCoupon AS NUMERIC(20, 2)) TotalCoupon, CAST(c.WARF AS NUMERIC(20, 2)) WARF
	, CAST(c.WARFRecovery AS NUMERIC(20, 2)) WARFRecovery, CAST(c.Life AS NUMERIC(20, 2)) Life
	, c.DateId CalculationDateId
	, FORMAT(NULLIF((ISNULL(c.TotalPar, 0) + ISNULL(s.TotalAllocation, 0)), 0), '#,###') TotalPar
	, NULLIF((ISNULL(c.TotalPar, 0) + ISNULL(s.TotalAllocation, 0)), 0) TotalParNum
	, (ISNULL(c.TotalPar, 0)) BODTotalPar
	, ISNULL(s.MoodyFacilityRatingAdjusted, c.MoodyFacilityRatingAdjusted) MoodyFacilityRatingAdjusted
	, ISNULL(a.AnalystResearchId, 0) AnalystResearchId, a.CLOAnalyst, a.HFAnalyst
	, CONVERT(VARCHAR(10), a.AsOfDate, 101) AsOfDate, a.CreditScore, a.LiquidityScore
	, a.OneLLeverage, CONVERT(VARCHAR, CAST(a.TotalLeverage AS MONEY), 1) TotalLeverage
	, CONVERT(VARCHAR, CAST(a.EVMultiple AS MONEY), 1) EVMultiple
	, CONVERT(VARCHAR, CAST(a.LTMRevenues AS MONEY), 1) LTMRevenues
	, CONVERT(VARCHAR, CAST(a.LTMEBITDA AS MONEY), 1) LTMEBITDA
	, CONVERT(VARCHAR, CAST(a.FCF AS MONEY), 1) FCF, CAST(a.LTMFCF AS MONEY) LTMFCF
	, CAST(a.EnterpriseValue AS MONEY) EnterpriseValue, CAST(a.SeniorLeverage AS MONEY) SeniorLeverage, a.Comments
	, a.BusinessDescription, a.AgentBank, cfra.Rank MoodyCashFlowRatingAdjustedRank
	, fra.Rank MoodyFacilityRatingAdjustedRank, s.MaturityDate SecurityMaturityDate
	, CASE WHEN s.MaturityDate IS NOT NULL THEN DATEDIFF(DAY, GETDATE(), s.MaturityDate) ELSE NULL END MaturityDays
	, s.WALifeAdjustment, s.TradePrice, CAST(0 AS BIT) AS IsOnAlert, ISNULL(s.IsStale, 0) IsStale
	, NULL AS SearchText
	, ROW_NUMBER() OVER (PARTITION BY p.PositionId, p.FundId, p.SecurityId ORDER BY p.DateId DESC) AS POSROWNUM
	, cs.ScoreDescription
	, a.Sponsor
	, MatrixImpliedSpread = c.MatrixImpliedSpread
	, DifferentialSpread = ISNULL(c.MatrixImpliedSpread,0)-(m.Spread)
		,a.[LiborCategory]
	,a.[LiborTransitionNote]
	,s.IsInDefault
	,s.DefaultDate
FROM CLO.vw_SecurityFund s
	LEFT JOIN prev_position p WITH (NOLOCK)
	ON s.SecurityId = p.SecurityId AND s.FundId = p.FundId AND ISNULL(s.IsStale, 0) = ISNULL(p.IsStale, 0)
	LEFT JOIN CLO.Country country WITH (NOLOCK)
	ON country.CountryId = p.CountryId
	LEFT JOIN CLO.vw_MarketData m WITH (NOLOCK)
	ON s.SecurityId = m.SecurityId AND s.FundId = m.FundId
	LEFT JOIN CLO.vw_PrevDayMarketData pm WITH (NOLOCK)
	ON s.SecurityId = pm.SecurityId AND s.FundId = pm.FundId
	LEFT JOIN CLO.vw_CurrentAnalystResearch a WITH (NOLOCK)
	ON a.IssuerId = s.IssuerId
	LEFT JOIN CLO.vw_Calculation c WITH (NOLOCK)
	ON s.SecurityId = c.SecurityId AND s.FundId = c.FundId
	LEFT JOIN CLO.Rating cfra WITH (NOLOCK)
	ON cfra.RatingDesc = ISNULL(s.MoodyCashFlowRatingAdjusted, m.MoodyCashFlowRatingAdjusted)
	LEFT JOIN CLO.Rating fra WITH (NOLOCK)
	ON fra.RatingDesc = ISNULL(s.MoodyFacilityRatingAdjusted, c.MoodyFacilityRatingAdjusted)
	LEFT OUTER JOIN CLO.CreditScore cs
	ON CAST(a.CreditScore AS INT) = cs.Id;
GO


