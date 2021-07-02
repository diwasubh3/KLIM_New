USE Yoda
GO
IF NOT EXISTS(SELECT * FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund') AND name = 'IsWarehouse')
	ALTER TABLE CLO.Fund ADD IsWarehouse BIT NOT NULL DEFAULT(0)
ELSE
	PRINT 'IsWarehouse EXISTS!'
GO
UPDATE CLO.Fund
SET IsWarehouse = 1
WHERE FundId = 5
GO
IF OBJECT_ID(N'CLO.vw_PivotedFundTargetPar') IS NOT NULL
DROP VIEW CLO.vw_PivotedFundTargetPar
ELSE
	PRINT 'CLO.vw_PivotedFundTargetPar EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_PivotedTradeAllocation') IS NOT NULL
DROP VIEW CLO.vw_PivotedTradeAllocation
ELSE
	PRINT 'CLO.vw_PivotedTradeAllocation EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_PivotedTradeTotalAllocation') IS NOT NULL
DROP VIEW CLO.vw_PivotedTradeTotalAllocation
ELSE
	PRINT 'CLO.vw_PivotedTradeTotalAllocation EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_SecurityFund') IS NOT NULL
DROP VIEW CLO.vw_SecurityFund
ELSE
	PRINT 'CLO.vw_SecurityFund EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_Security_Watch') IS NOT NULL
DROP VIEW CLO.vw_Security_Watch
ELSE
	PRINT 'CLO.vw_Security_Watch EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_Security') IS NOT NULL
DROP VIEW CLO.vw_Security
ELSE
	PRINT 'CLO.vw_Security EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_SecurityMarketCalculation') IS NOT NULL
DROP VIEW CLO.vw_SecurityMarketCalculation
ELSE
	PRINT 'CLO.vw_SecurityMarketCalculation EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_MarketData') IS NOT NULL
DROP VIEW CLO.vw_MarketData
ELSE
	PRINT 'CLO.vw_MarketData EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_Calculation') IS NOT NULL
DROP VIEW CLO.vw_Calculation
ELSE
	PRINT 'CLO.vw_Calculation EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_PivotedPositionExposure') IS NOT NULL
DROP VIEW CLO.vw_PivotedPositionExposure
ELSE
	PRINT 'CLO.vw_PivotedPositionExposure EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_PivotedPositionIsCovLite') IS NOT NULL
DROP VIEW CLO.vw_PivotedPositionIsCovLite
ELSE
	PRINT 'CLO.vw_PivotedPositionIsCovLite EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_PivotedPositionCountry') IS NOT NULL
DROP VIEW CLO.vw_PivotedPositionCountry
ELSE
	PRINT 'CLO.vw_PivotedPositionCountry EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_PositionCountryFund') IS NOT NULL
DROP VIEW CLO.vw_PositionCountryFund
ELSE
	PRINT 'CLO.vw_PositionCountryFund EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_CalculationSecurity') IS NOT NULL
DROP VIEW CLO.vw_CalculationSecurity
ELSE
	PRINT 'CLO.vw_CalculationSecurity EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_MarketDataSecurity') IS NOT NULL
DROP VIEW CLO.vw_MarketDataSecurity
ELSE
	PRINT 'CLO.vw_MarketDataSecurity EXISTS!'
GO
IF OBJECT_ID(N'CLO.vw_PrevDayMarketData') IS NOT NULL
DROP VIEW CLO.vw_PrevDayMarketData
ELSE
	PRINT 'CLO.vw_PrevDayMarketData EXISTS!'
GO
IF OBJECT_ID(N'CLO.GetActiveTrades') IS NOT NULL
DROP FUNCTION CLO.GetActiveTrades
ELSE
	PRINT 'CLO.GetActiveTrades EXISTS!'
GO
CREATE FUNCTION CLO.GetActiveTrades ()
RETURNS @returntable TABLE
	(
		 SecurityId INT
	   , FundId INT
	   , Allocation NUMERIC(38, 10)
	   , TradedCash NUMERIC(38, 10)
	   , TradePrice NUMERIC(38, 4)
	   , TotalAllocation NUMERIC(38, 10)
	   , HasBuy BIT
	   , HasSell BIT
	   , FundCode VARCHAR(100)
	)
AS
	BEGIN

		DECLARE @tradeallocations TABLE
		(
			 SecurityId INT
		   , FundId INT
		   , Allocation NUMERIC(38, 10)
		   , TradedCash NUMERIC(38, 10)
		   , TradePrice NUMERIC(38, 4)
		   , HasBuy BIT
		   , FundCode VARCHAR(100)
		)
	
		DECLARE @prevDayDateId INT 
		SELECT @prevDayDateId = CLO.GetPrevDayDateId()

		INSERT INTO @tradeallocations (SecurityId, FundId, Allocation, TradedCash, TradePrice, HasBuy, FundCode)
		SELECT t.SecurityId, ta.FundId
				, SUM(CASE WHEN ISNULL(t.IsBuy, 0) = 0 THEN -1 * ISNULL(ta.NewAllocation, 0) ELSE ISNULL(ta.NewAllocation, 0) END) Allocation
				, SUM(CASE WHEN ISNULL(t.IsBuy, 0) = 0 THEN (ISNULL(ta.NewAllocation, 0) * (ISNULL(t.TradePrice, 0) * 0.01))
						 ELSE (-1 * ISNULL(ta.NewAllocation, 0) * (ISNULL(t.TradePrice, 0) * 0.01)) END) TradedCash
				, MAX(t.TradePrice), CAST(MAX(CASE WHEN t.IsBuy = 1 THEN 1 ELSE 0 END) AS BIT)
				, f.FundCode
		FROM CLO.Trade t WITH (NOLOCK)
		JOIN CLO.TradeAllocation ta WITH (NOLOCK) ON t.TradeId = ta.TradeId
		JOIN CLO.Fund f WITH (NOLOCK) ON ta.FundId = f.FundId
		WHERE (ISNULL(t.IsCancelled, 0) <> 1)
		AND (ISNULL(t.KeepOnBlotter, 0) = 1 OR t.DateId = @prevDayDateId)
		GROUP BY t.SecurityId, ta.FundId, f.FundCode
	
		INSERT INTO @returntable (SecurityId, FundId, Allocation, TradedCash, TotalAllocation, TradePrice, HasBuy, HasSell, FundCode)
		SELECT SecurityId, FundId, Allocation, TradedCash, SUM(Allocation) OVER (PARTITION BY SecurityId)
			, TradePrice, HasBuy, CASE WHEN HasBuy = 1 THEN 0 ELSE 1 END, FundCode
		FROM @tradeallocations

		RETURN
	END
GO
CREATE VIEW CLO.vw_PivotedFundTargetPar
	AS 

SELECT [CLO-1]
      ,[CLO-2]
      ,[CLO-3]
      ,[CLO-4]
      ,[CLO-5]
	  ,WH
FROM (
    SELECT 
        TargetPar,FundCode
    FROM [CLO].[Fund] with(nolock)
) as s
PIVOT
(
    max(TargetPar)
    FOR FundCode IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],WH)
)AS pvt
GO
CREATE VIEW CLO.vw_PositionCountryFund
AS
	SELECT p.PositionId, p.FundId, p.SecurityId, p.DateId, p.Exposure
		, CASE	WHEN p.PctExposure IS NOT NULL THEN p.PctExposure * 100 ELSE NULL END PctExposure
		, p.PxPrice, p.CreatedOn, p.CreatedBy, p.LastUpdatedOn, p.LastUpdatedBy, p.IsCovLite, c.CountryDesc, f.FundCode
		, ROW_NUMBER() OVER (PARTITION BY p.FundId, p.SecurityId ORDER BY p.DateId DESC) AS RowNumber
	FROM CLO.Position p WITH (NOLOCK)
	LEFT JOIN CLO.Country c WITH (NOLOCK) ON c.CountryId = p.CountryId
	JOIN CLO.Fund f WITH (NOLOCK) ON p.FundId = f.FundId
		AND ISNULL(ISNULL(NULLIF(f.IsStale, CAST(0 AS BIT)), f.IsPrincipalCashStale), 0) = ISNULL(p.IsStale, 0)
	WHERE DateId = CLO.GetPrevDayDateId()
GO
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
CREATE VIEW CLO.vw_MarketDataSecurity
AS
	WITH marketdata_cfe
	AS (
		SELECT m.DateId, m.MarketDataId MarketDataId, o.OverrideMarketDataId OverrideMarketDataId, m.SecurityId
			, COALESCE(o.Bid, currentupload.Bid, m.Bid, previousupload.Bid) AS Bid
			, COALESCE(o.Offer, currentupload.Offer, m.Offer, previousupload.Offer) AS Offer, COALESCE(o.CostPrice, m.CostPrice) AS CostPrice
			, ISNULL(o.Spread, m.Spread) AS Spread, ISNULL(o.LiborFloor, m.LiborFloor) AS LiborFloor
			, moodycashflowrating.RatingDesc AS MoodyCashFlowRating, moodycashflowratingadjusted.RatingDesc AS MoodyCashFlowRatingAdjusted
			, moodyfacilityrating.RatingDesc AS MoodyFacilityRating, ISNULL(o.MoodyFacilityRatingId, m.MoodyFacilityRatingId) AS MoodyFacilityRatingId
			, ISNULL(o.MoodyRecovery, m.MoodyRecovery) AS MoodyRecovery, snpissuerrating.RatingDesc AS SnPIssuerRating
			, snpissuerratingadjusted.RatingDesc AS SnPIssuerRatingAdjusted, snpfacilityrating.RatingDesc AS SnPFacilityRating
			, snpfacilityratingadjusted.RatingDesc AS SnPfacilityRatingAdjusted, snprecoveryrating.RatingDesc AS SnPRecoveryRating
			, ISNULL(o.MoodyOutlook, m.MoodyOutlook) AS MoodyOutlook, ISNULL(o.MoodyWatch, m.MoodyWatch) AS MoodyWatch
			, ISNULL(o.SnPWatch, m.SnPWatch) AS SnPWatch, ISNULL(o.NextReportingDate, m.NextReportingDate) AS NextReportingDate
			, ISNULL(o.FiscalYearEndDate, m.FiscalYearEndDate) AS FiscalYearEndDate, ISNULL(o.AgentBank, m.AgentBank) AS AgentBank
			, ROW_NUMBER() OVER (PARTITION BY m.SecurityId ORDER BY m.DateId DESC) AS MarketRowNum
		FROM      CLO.MarketData m WITH (NOLOCK)
		LEFT JOIN CLO.OverrideMarketData o WITH (NOLOCK) ON m.DateId = o.DateId
															AND m.SecurityId = o.SecurityId
															AND m.FundId = o.FundId
		LEFT JOIN CLO.Rating moodycashflowrating WITH (NOLOCK) ON moodycashflowrating.RatingId = ISNULL(o.MoodyCashFlowRatingId,
																										m.MoodyCashFlowRatingId)
		LEFT JOIN CLO.Rating moodycashflowratingadjusted WITH (NOLOCK) ON moodycashflowratingadjusted.RatingId = ISNULL(o.MoodyCashFlowRatingAdjustedId,
																														m.MoodyCashFlowRatingAdjustedId)
		LEFT JOIN CLO.Rating moodyfacilityrating WITH (NOLOCK) ON moodyfacilityrating.RatingId = ISNULL(o.MoodyFacilityRatingId,
																										m.MoodyFacilityRatingId)
		LEFT JOIN CLO.Rating snpissuerrating WITH (NOLOCK) ON snpissuerrating.RatingId = ISNULL(o.SnPIssuerRatingId, m.SnPIssuerRatingId)
		LEFT JOIN CLO.Rating snpissuerratingadjusted WITH (NOLOCK) ON snpissuerratingadjusted.RatingId = ISNULL(o.SnPIssuerRatingAdjustedId,
																												m.SnPIssuerRatingAdjustedId)
		LEFT JOIN CLO.Rating snpfacilityrating WITH (NOLOCK) ON snpfacilityrating.RatingId = ISNULL(o.SnPFacilityRatingId, m.SnPFacilityRatingId)
		LEFT JOIN CLO.Rating snpfacilityratingadjusted WITH (NOLOCK) ON snpfacilityratingadjusted.RatingId = ISNULL(o.SnPfacilityRatingAdjustedId,
																													m.SnPfacilityRatingAdjustedId)
		LEFT JOIN CLO.Rating snprecoveryrating WITH (NOLOCK) ON snprecoveryrating.RatingId = ISNULL(o.SnPRecoveryRatingId, m.SnPRecoveryRatingId)
		LEFT JOIN CLO.vw_Price currentupload WITH (NOLOCK) ON m.DateId = currentupload.DateId
															AND m.SecurityId = currentupload.SecurityId
		LEFT JOIN CLO.vw_Price previousupload WITH (NOLOCK) ON m.DateId > currentupload.DateId
																AND m.SecurityId = currentupload.SecurityId
		WHERE     m.DateId = CLO.GetPrevDayDateId()
		)
	SELECT DateId, MarketDataId, OverrideMarketDataId, SecurityId, Bid, Offer, CostPrice, Spread, LiborFloor, MoodyCashFlowRating, MoodyCashFlowRatingAdjusted
		, MoodyFacilityRating, MoodyFacilityRatingId, MoodyRecovery, SnPIssuerRating, SnPIssuerRatingAdjusted, SnPFacilityRating, SnPfacilityRatingAdjusted
		, SnPRecoveryRating, MoodyOutlook, MoodyWatch, SnPWatch, NextReportingDate, FiscalYearEndDate, AgentBank, MarketRowNum
	FROM marketdata_cfe
	WHERE marketdata_cfe.MarketRowNum = 1
GO
CREATE VIEW CLO.vw_CalculationSecurity
AS
	SELECT	CalculationId, DateId, SecurityId, YieldBid, YieldOffer, YieldMid
		, CappedYieldBid, CappedYieldOffer, CappedYieldMid, TargetYieldBid, TargetYieldOffer
		, TargetYieldMid, BetterWorseBid, BetterWorseOffer, BetterWorseMid, TotalCoupon, WARF
		, WARFRecovery, Life, TotalPar, moodyfacilityratingadjusted.RatingDesc AS MoodyFacilityRatingAdjusted
		, c.CreatedOn, c.CreatedBy, c.LastUpdatedOn, c.LastUpdatedBy, ROW_NUMBER() OVER (PARTITION BY SecurityId ORDER BY DateId DESC) AS RowNumber
	FROM	CLO.Calculation c WITH (NOLOCK)
	LEFT JOIN CLO.Rating moodyfacilityratingadjusted WITH (NOLOCK) ON moodyfacilityratingadjusted.RatingId = c.MoodyFacilityRatingAdjustedId
	WHERE	DateId = CLO.GetPrevDayDateId();
GO
CREATE VIEW CLO.vw_PivotedPositionIsCovLite
AS
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], WH
	FROM (SELECT (CASE WHEN IsCovLite = 1 THEN 1 ELSE 0 END) IsCovLite
			, FundCode, SecurityId
			FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
(MAX(IsCovLite) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], WH) )AS pvt
GO
CREATE VIEW CLO.vw_PivotedPositionExposure
AS
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH
	FROM (SELECT Exposure, FundCode, SecurityId
			FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
(MAX(Exposure) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH) )AS pvt
GO
CREATE VIEW CLO.vw_PivotedPositionCountry
AS
	SELECT	SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], TRS
	FROM	(SELECT	CountryDesc, FundCode, SecurityId
				FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
( MAX(CountryDesc) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], TRS) )AS pvt
GO
CREATE VIEW CLO.vw_Calculation
	AS 
	SELECT 
	  CalculationId
	, DateId
	, SecurityId
	, FundId
	, YieldBid
	, YieldOffer
	, YieldMid
	, CappedYieldBid
	, CappedYieldOffer
	, CappedYieldMid
	, TargetYieldBid
	, TargetYieldOffer
	, TargetYieldMid
	, BetterWorseBid
	, BetterWorseOffer
	, BetterWorseMid
	, TotalCoupon
	, WARF
	, WARFRecovery
	, Life
	, TotalPar
	, moodyfacilityratingadjusted.RatingDesc as MoodyFacilityRatingAdjusted
	, c.CreatedOn
	, c.CreatedBy
	, c.LastUpdatedOn
	, c.LastUpdatedBy
	, ROW_NUMBER() OVER (PARTITION BY SecurityId, FundId
			ORDER BY DateId DESC) AS RowNumber
	FROM CLO.Calculation c (NOLOCK)
	LEFT JOIN CLO.Rating moodyfacilityratingadjusted (NOLOCK) ON moodyfacilityratingadjusted.RatingId = c.MoodyFacilityRatingAdjustedId
	WHERE DateId = CLO.GetPrevDayDateId()
GO
CREATE VIEW CLO.vw_MarketData
AS
	SELECT m.DateId, m.MarketDataId MarketDataId, o.OverrideMarketDataId OverrideMarketDataId
		, m.SecurityId, m.FundId, COALESCE(o.Bid, currentupload.Bid, m.Bid, previousupload.Bid) AS Bid
		, COALESCE(o.Offer, currentupload.Offer, m.Offer, previousupload.Offer) AS Offer
		, COALESCE(o.CostPrice, m.CostPrice) AS CostPrice, ISNULL(o.Spread, m.Spread) AS Spread
		, ISNULL(o.LiborFloor, m.LiborFloor) AS LiborFloor, moodycashflowrating.RatingDesc AS MoodyCashFlowRating
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
	FROM CLO.MarketData m WITH (NOLOCK)
	LEFT JOIN CLO.OverrideMarketData o WITH (NOLOCK) ON m.DateId = o.DateId
											AND m.SecurityId = o.SecurityId
											AND m.FundId = o.FundId
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
GO
CREATE VIEW CLO.vw_SecurityMarketCalculation
AS
	WITH	PivotedSecurityOverrides_cfe
				AS (
						SELECT SecurityId, SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate
							, MaturityDate, SnpIndustry, MoodyIndustry, IsCovLite, IsFloating, LienType
							, MoodyFacilityRatingAdjusted, MoodyCashFlowRatingAdjusted
						FROM CLO.vw_CurrentActiveSecurityOverrides
						PIVOT ( MAX(OverrideValue) FOR FieldName IN (SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate
								, MaturityDate, SnpIndustry, MoodyIndustry, IsCovLite, IsFloating, LienType
								, MoodyFacilityRatingAdjusted, MoodyCashFlowRatingAdjusted) ) AS AvgIncomePerDay
					)

	SELECT	s.SecurityId, ISNULL(os.SecurityCode, s.SecurityCode) SecurityCode
		, ISNULL(os.SecurityDesc, s.SecurityDesc) SecurityDesc, ISNULL(os.BBGId, s.BBGId) BBGId
		, ISNULL(os.Issuer, i.IssuerDesc) Issuer, ISNULL(os.Facility, f.FacilityDesc) Facility
		, ISNULL(CAST(os.CallDate AS DATE), s.CallDate) CallDate
		, ISNULL(CAST(os.MaturityDate AS DATE), s.MaturityDate) MaturityDate
		, ISNULL(os.SnpIndustry, snpindustry.IndustryDesc) SnpIndustry
		, ISNULL(os.MoodyIndustry, moodyindustry.IndustryDesc) MoodyIndustry
		, ISNULL(CAST(CASE	WHEN os.IsFloating = 'Fixed' THEN 0 WHEN os.IsFloating = 'Floating' THEN 1 END AS BIT), s.IsFloating) IsFloating
		, ISNULL(os.LienType, lientype.LienTypeDesc) LienType, s.IssuerId, w.WatchId, s.MoodyIndustryId
		, CAST(CASE	WHEN w.WatchId IS NULL THEN 0 ELSE 1 END AS BIT) IsOnWatch
		, w.WatchObjectTypeId, w.WatchObjectId, w.WatchComments
		, FORMAT(w.WatchLastUpdatedOn, 'MM/dd/yyyy hh:mm tt') WatchLastUpdatedOn
		, w.WatchUser, s.SecurityCode OrigSecurityCode, s.SecurityDesc OrigSecurityDesc
		, s.BBGId OrigBBGId, i.IssuerDesc OrigIssuer, s.GICSIndustry
		, LTRIM(f.FacilityDesc) OrigFacility
		, CASE WHEN s.CallDate <> '1900-01-01' THEN CONVERT(VARCHAR(10), s.CallDate, 101) ELSE NULL END OrigCallDate
		, CONVERT(VARCHAR(10), s.MaturityDate, 101) OrigMaturityDate, snpindustry.IndustryDesc OrigSnpIndustry
		, moodyindustry.IndustryDesc OrigMoodyIndustry
		, CASE WHEN (s.IsFloating = 1) THEN 'Floating' ELSE 'Fixed' END OrigIsFloating
		, lientype.LienTypeDesc OrigLienType, c.MoodyFacilityRatingAdjusted AS OrigMoodyFacilityRatingAdjusted
		, m.MoodyCashFlowRatingAdjusted AS OrigMoodyCashFlowRatingAdjusted, s.LastUpdatedOn SecurityLastUpdatedOn
		, s.LastUpdatedBy SecurityLastUpdatedBy, s.CreatedOn SecurityCreatedOn
		, s.CreatedBy SecurityCreatedBy, s.SourceId
		, ISNULL(os.MoodyFacilityRatingAdjusted, c.MoodyFacilityRatingAdjusted) MoodyFacilityRatingAdjusted
		, ISNULL(os.MoodyCashFlowRatingAdjusted, m.MoodyCashFlowRatingAdjusted) MoodyCashFlowRatingAdjusted
		, CAST(NULL AS BIT) HasPositions
	FROM	CLO.Security s WITH (NOLOCK)
	LEFT JOIN CLO.Issuer i WITH (NOLOCK) ON i.IssuerId = s.IssuerId
	LEFT JOIN CLO.Facility f WITH (NOLOCK) ON f.FacilityId = s.FacilityId
	LEFT JOIN CLO.Industry snpindustry WITH (NOLOCK) ON snpindustry.IndustryId = s.SnPIndustryId
														AND snpindustry.IsSnP = 1
	LEFT JOIN CLO.Industry moodyindustry WITH (NOLOCK) ON moodyindustry.IndustryId = s.MoodyIndustryId
														AND moodyindustry.IsMoody = 1
	LEFT JOIN CLO.LienType lientype WITH (NOLOCK) ON lientype.LienTypeId = s.LienTypeId
	LEFT JOIN CLO.Watch w ON (w.WatchObjectTypeId = 1
								AND w.WatchObjectId = s.SecurityId)
								OR (w.WatchObjectId = s.IssuerId
								AND w.WatchObjectTypeId = 2)
	LEFT JOIN PivotedSecurityOverrides_cfe os ON os.SecurityId = s.SecurityId
	LEFT JOIN CLO.vw_MarketData m ON s.SecurityId = m.SecurityId
	LEFT JOIN CLO.vw_CalculationSecurity c ON s.SecurityId = c.SecurityId
	WHERE	ISNULL(IsDeleted, 0) = 0
GO
CREATE VIEW CLO.vw_Security
AS
	WITH PivotedSecurityOverrides_cfe
		AS (SELECT	SecurityId, SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate, MaturityDate, SnpIndustry
				, MoodyIndustry, IsCovLite, IsFloating, LienType, MoodyFacilityRatingAdjusted, MoodyCashFlowRatingAdjusted
			FROM CLO.vw_CurrentActiveSecurityOverrides WITH (NOLOCK)
			PIVOT (MAX(OverrideValue) FOR FieldName IN (SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate
				, MaturityDate, SnpIndustry, MoodyIndustry, IsCovLite, IsFloating, LienType, MoodyFacilityRatingAdjusted
				, MoodyCashFlowRatingAdjusted) ) AS AvgIncomePerDay)

	SELECT s.SecurityId, ISNULL(os.SecurityCode, s.SecurityCode) SecurityCode
		, ISNULL(os.SecurityDesc, s.SecurityDesc) SecurityDesc, ISNULL(os.BBGId, s.BBGId) BBGId
		, COALESCE(os.Issuer, i.IssuerCode, i.IssuerDesc) Issuer, i.IssuerDesc IssuerDesc
		, ISNULL(os.Facility, f.FacilityDesc) Facility, ISNULL(CAST(os.CallDate AS DATE), s.CallDate) CallDate
		, ISNULL(CAST(os.MaturityDate AS DATE), s.MaturityDate) MaturityDate
		, ISNULL(os.SnpIndustry, snpindustry.IndustryDesc) SnpIndustry
		, ISNULL(os.MoodyIndustry, moodyindustry.IndustryDesc) MoodyIndustry
		, ISNULL(CAST(CASE	WHEN os.IsFloating = 'Fixed' THEN 0 WHEN os.IsFloating = 'Floating' THEN 1 END AS BIT), s.IsFloating) IsFloating
		, ISNULL(os.LienType, lientype.LienTypeDesc) LienType, s.IssuerId, ISNULL(wi.WatchId, ws.WatchId) WatchId, s.MoodyIndustryId
		, CAST (CASE	WHEN ISNULL(wi.WatchId, ws.WatchId) IS NULL THEN 0 ELSE 1 END AS BIT) IsOnWatch
		, ISNULL(wi.WatchObjectTypeId, ws.WatchObjectTypeId) WatchObjectTypeId
		, ISNULL(wi.WatchObjectId, ws.WatchObjectId) WatchObjectId, ISNULL(wi.WatchComments, ws.WatchComments) WatchComments
		, FORMAT(ISNULL(wi.WatchLastUpdatedOn, ws.WatchLastUpdatedOn), 'MM/dd/yyyy hh:mm tt') WatchLastUpdatedOn
		, ISNULL(wi.WatchUser, ws.WatchUser) WatchUser, s.SecurityCode OrigSecurityCode, s.SecurityDesc OrigSecurityDesc
		, s.BBGId OrigBBGId, ISNULL(i.IssuerCode, i.IssuerDesc) OrigIssuer, s.GICSIndustry, LTRIM(f.FacilityDesc) OrigFacility
		, CASE WHEN s.CallDate <> '1900-01-01' THEN CONVERT(VARCHAR(10), s.CallDate, 101) ELSE NULL END OrigCallDate
		, CONVERT(VARCHAR(10), s.MaturityDate, 101) OrigMaturityDate, snpindustry.IndustryDesc OrigSnpIndustry
		, moodyindustry.IndustryDesc OrigMoodyIndustry, CASE	WHEN (s.IsFloating = 1) THEN 'Floating' ELSE 'Fixed' END OrigIsFloating
		, lientype.LienTypeDesc OrigLienType, NULL OrigMoodyFacilityRatingAdjusted, NULL OrigMoodyCashFlowRatingAdjusted
		, s.LastUpdatedOn SecurityLastUpdatedOn, s.LastUpdatedBy SecurityLastUpdatedBy, s.CreatedOn SecurityCreatedOn
		, s.CreatedBy SecurityCreatedBy, s.SourceId, os.MoodyFacilityRatingAdjusted, os.MoodyCashFlowRatingAdjusted
		, CAST(NULL AS BIT) HasPositions
		, lientype.LienTypeId
	FROM CLO.Security s WITH (NOLOCK)
	LEFT JOIN CLO.Issuer i WITH (NOLOCK) ON i.IssuerId = s.IssuerId
	LEFT JOIN CLO.Facility f WITH (NOLOCK) ON f.FacilityId = s.FacilityId
	LEFT JOIN CLO.Industry snpindustry WITH (NOLOCK) ON snpindustry.IndustryId = s.SnPIndustryId
														AND snpindustry.IsSnP = 1
	LEFT JOIN CLO.Industry moodyindustry WITH (NOLOCK) ON moodyindustry.IndustryId = s.MoodyIndustryId
														AND moodyindustry.IsMoody = 1
	LEFT JOIN CLO.LienType lientype WITH (NOLOCK) ON lientype.LienTypeId = s.LienTypeId
	LEFT JOIN CLO.Watch ws WITH (NOLOCK) ON (ws.WatchObjectTypeId = 1
														AND ws.WatchObjectId = s.SecurityId)
	LEFT JOIN CLO.Watch wi WITH (NOLOCK) ON (wi.WatchObjectId = s.IssuerId
														AND wi.WatchObjectTypeId = 2)
	LEFT JOIN PivotedSecurityOverrides_cfe os ON os.SecurityId = s.SecurityId
	WHERE ISNULL(IsDeleted, 0) = 0
GO
CREATE VIEW CLO.vw_SecurityFund
AS 
SELECT
	s.SecurityId
	, s.SecurityCode
	, s.SecurityDesc
	, s.BBGId
	, s.Issuer
	, s.IssuerDesc
	, s.Facility
	, s.CallDate
	, s.MaturityDate
	, s.SnpIndustry
	, s.MoodyIndustry
	, s.IsFloating
	, s.LienType
	, s.IssuerId
	, s.WatchId
	, s.MoodyIndustryId
	, s.IsOnWatch
	, s.WatchObjectTypeId
	, s.WatchObjectId
	, s.WatchComments
	, s.WatchLastUpdatedOn
	, s.WatchUser
	, s.OrigSecurityCode
	, s.OrigSecurityDesc
	, s.OrigBBGId
	, s.OrigIssuer
	, s.GICSIndustry
	, s.OrigFacility
	, s.OrigCallDate
	, s.OrigMaturityDate
	, s.OrigSnpIndustry
	, s.OrigMoodyIndustry
	, s.OrigIsFloating
	, s.OrigLienType
	, s.OrigMoodyFacilityRatingAdjusted
	, s.OrigMoodyCashFlowRatingAdjusted
	, s.SecurityLastUpdatedOn
	, s.SecurityLastUpdatedBy
	, s.SecurityCreatedOn
	, s.SecurityCreatedBy
	, s.SourceId
	, s.MoodyFacilityRatingAdjusted
	, s.MoodyCashFlowRatingAdjusted
	, s.HasPositions
	, f.FundId
	, f.FundCode
	, f.FundDesc
	, f.PrincipalCash
	, f.EquityPar
	, f.LiabilityPar 
	, f.TargetPar
	, f.WALifeAdjustment
	, ISNULL(NULLIF(f.IsStale,CAST(0 AS BIT)),f.IsPrincipalCashStale) IsStale
	, ta.Allocation Allocation
	, ta.HasBuy HasBuy
	, ta.TotalAllocation  TotalAllocation
	, ta.TradePrice
	, s.LienTypeId
FROM CLO.vw_security s WITH(NOLOCK)
CROSS JOIN CLO.Fund f WITH(NOLOCK)
LEFT JOIN  CLO.GetActiveTrades() ta ON s.SecurityId = ta.SecurityId AND f.FundId = ta.FundId
WHERE f.IsActive = 1 OR f.IsWarehouse = 1
GO
CREATE VIEW CLO.vw_Security_Watch
AS
	SELECT s.SecurityId, s.SecurityCode, s.SecurityDesc, s.Issuer, s.IssuerId, s.IsOnWatch
		, s.WatchObjectTypeId, s.WatchObjectId, s.WatchId, s.WatchComments, s.WatchLastUpdatedOn, s.WatchUser
	FROM CLO.vw_Security s WITH (NOLOCK)
GO
CREATE VIEW CLO.vw_PivotedTradeAllocation
AS
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH
	FROM (SELECT	Allocation, FundCode, SecurityId FROM CLO.GetActiveTrades()) AS s PIVOT
(MAX(Allocation) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH)) AS pvt
GO
CREATE VIEW CLO.vw_PivotedTradeTotalAllocation
AS
	SELECT	SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH
	FROM	(SELECT	TotalAllocation, FundCode, SecurityId FROM CLO.GetActiveTrades()) AS s PIVOT
(MAX(TotalAllocation) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH)) AS pvt
GO
ALTER VIEW CLO.vw_AggregatePosition
AS
SELECT 
		  CLO.GetPrevDayDateId() PositionDateId
		  , NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0)	CLO1NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0)	CLO2NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0)	CLO3NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0)	CLO4NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0)	CLO5NumExposure
		  , NULLIF((ISNULL(MAX(pos.[WH]),0) + ISNULL(MAX(allocation.[WH]),0)),0)		TRSNumExposure

		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0), '#,###')	CLO1Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0), '#,###')	CLO2Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0), '#,###')	CLO3Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0), '#,###')	CLO4Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0), '#,###')	CLO5Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[WH]),0) + ISNULL(MAX(allocation.[WH]),0)),0), '#,###')		TRSExposure

		  , FORMAT(((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0))/ISNULL(MAX(targetPar.[CLO-1]),1)) , 'p')  CLO1PctExposure   
		  , FORMAT(((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0))/ISNULL(MAX(targetPar.[CLO-2]),1)) , 'p')  CLO2PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0))/ISNULL(MAX(targetPar.[CLO-3]),1)) , 'p')  CLO3PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0))/ISNULL(MAX(targetPar.[CLO-4]),1)) , 'p')  CLO4PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0))/ISNULL(MAX(targetPar.[CLO-5]),1)) , 'p')  CLO5PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[WH]),0) + ISNULL(MAX(allocation.[WH]),0))/ISNULL(MAX(targetPar.[WH]),1)) , 'p')		  TRSPctExposure 

		  , s.SecurityId
		  , MAX(s.SecurityCode		)		SecurityCode
		  , MAX(s.SecurityDesc		)		SecurityDesc
		  , MAX(s.BBGId			)		BBGId
		  , MAX(s.Issuer			)		Issuer
		  , MAX(s.IssuerId			)		IssuerId
		  , MAX(s.IssuerDesc			)	IssuerDesc
		  , MAX(LTRIM(s.Facility)) Facility
		  , MAX(case when s.CallDate <> '1900-01-01' then  CONVERT(VARCHAR(10), s.CallDate, 101) else null END) CallDate
		  , COALESCE(MAX(country.[CLO-1]), MAX(country.[CLO-2]), MAX(country.[CLO-3]), MAX(country.[CLO-4]))  CountryDesc
		  , MAX(CONVERT(VARCHAR(10), s.MaturityDate, 101)) MaturityDate
		  , MAX(s.SnpIndustry)				SnpIndustry
		  , MAX(s.MoodyIndustry)			MoodyIndustry
		  , COALESCE(MAX(covlite.[CLO-1]), MAX(covlite.[CLO-2]), MAX(covlite.[CLO-3]), MAX(covlite.[CLO-4]))  IsCovLite
		  , MAX(case when (s.IsFloating = 1) then 'Floating' else 'Fixed' END) IsFloating
		  , MAX(s.LienType						)		LienType
		  , s.IsOnWatch					
		  , MAX(s.WatchObjectTypeId				)		WatchObjectTypeId
		  , MAX(s.WatchObjectId					)		WatchObjectId					
		  , MAX(s.WatchId						)		WatchId						
		  , MAX(s.WatchComments				)		WatchComments				
		  , MAX(s.WatchLastUpdatedOn				)		WatchLastUpdatedOn				
		  , MAX(s.WatchUser						)		WatchUser						
		  , MAX(s.OrigSecurityCode				)		OrigSecurityCode				
		  , MAX(s.OrigSecurityDesc				)		OrigSecurityDesc				
		  , MAX(s.OrigBBGId					)		OrigBBGId					
		  , MAX(s.OrigIssuer					)		OrigIssuer					
		  , MAX(s.OrigFacility					)		OrigFacility					
		  , MAX(s.OrigCallDate 				)		OrigCallDate 				
		  , MAX(s.OrigMaturityDate				)		OrigMaturityDate				
		  , MAX(s.OrigSnpIndustry				)		OrigSnpIndustry				
		  , MAX(s.OrigMoodyIndustry			)		OrigMoodyIndustry			
		  , MAX(s.OrigIsFloating 				)		OrigIsFloating 				
		  , MAX(s.OrigLienType					)		OrigLienType					
		  , MAX(c.MoodyFacilityRatingAdjusted	)	AS OrigMoodyFacilityRatingAdjusted
		  , MAX(m.MoodyCashFlowRatingAdjusted	)	AS OrigMoodyCashFlowRatingAdjusted
		  , CONVERT(varchar, CAST(AVG(m.Bid) AS MONEY),1)  Bid
		  , CONVERT(varchar, CAST(AVG(m.Offer)AS MONEY),1) Offer
		  , MAX(m.Bid) BidNum
		  , MAX(m.Offer) OfferNum
		  , CAST(MAX(m.Spread) as numeric(20,2)) Spread	
		  , CAST(MAX(m.LiborFloor) as numeric(20,2))	LiborFloor
		  , MAX(m.MoodyCashFlowRating) MoodyCashFlowRating
		  , ISNULL(MAX(s.MoodyCashFlowRatingAdjusted), MAX(m.MoodyCashFlowRatingAdjusted)) MoodyCashFlowRatingAdjusted
		  , MAX(m.MoodyFacilityRating)		MoodyFacilityRating
		  , MAX(m.MoodyRecovery				)		MoodyRecovery
		  , MAX(m.SnPIssuerRating				)		SnPIssuerRating				
		  , MAX(m.SnPIssuerRatingAdjusted		)		SnPIssuerRatingAdjusted		
		  , MAX(m.SnPFacilityRating			)		SnPFacilityRating			
		  , MAX(m.SnPfacilityRatingAdjusted	)		SnPfacilityRatingAdjusted	
		  , MAX(m.SnPRecoveryRating			)		SnPRecoveryRating			
		  , MAX(m.MoodyOutlook					)		MoodyOutlook					
		  , MAX(m.MoodyWatch					)		MoodyWatch					
		  , MAX(m.SnPWatch						)		SnPWatch						
		  , CONVERT(VARCHAR(10), MAX(m.NextReportingDate), 101) NextReportingDate
		  , CONVERT(VARCHAR(10), MAX(m.FiscalYearEndDate), 101) FiscalYearEndDate
		  
		  
		  , CAST(AVG(c.YieldBid			)as numeric(20,2))			YieldBid				
		  , CAST(AVG(c.YieldOffer			)as numeric(20,2))			YieldOffer			
		  , CAST(AVG(c.YieldMid			)as numeric(20,2))			YieldMid				
		  , CAST(AVG(c.CappedYieldBid		)as numeric(20,2))			CappedYieldBid		
		  , CAST(AVG(c.CappedYieldOffer	)as numeric(20,2))			CappedYieldOffer		
		  , CAST(AVG(c.CappedYieldMid		)as numeric(20,2))			CappedYieldMid		
		  , CAST(AVG(c.TargetYieldBid		)as numeric(20,2))			TargetYieldBid		
		  , CAST(AVG(c.TargetYieldOffer	)as numeric(20,2))			TargetYieldOffer		
		  , CAST(AVG(c.TargetYieldMid		)as numeric(20,2))			TargetYieldMid		
		  , CAST(AVG(c.BetterWorseBid		)as numeric(20,2))			BetterWorseBid		
		  , CAST(AVG(c.BetterWorseOffer	)as numeric(20,2))			BetterWorseOffer		
		  , CAST(AVG(c.BetterWorseMid		)as numeric(20,2))			BetterWorseMid		
		  , CAST(AVG(c.TotalCoupon			)as numeric(20,2))			TotalCoupon			
		  , CAST(AVG(c.WARF				)as numeric(20,2))			WARF					
		  , CAST(AVG(c.WARFRecovery		)as numeric(20,2))			WARFRecovery			
		  , CAST(AVG(c.Life				)as numeric(20,2))			Life				
		  , FORMAT(NULLIF((ISNULL(MAX(c.TotalPar),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0), '#,###')	TotalPar
		  , NULLIF((ISNULL(MAX(c.TotalPar),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0) TotalParNum
		  , (ISNULL(MAX(c.TotalPar),0)) BODTotalPar
		  , ISNULL(MAX(s.MoodyFacilityRatingAdjusted),  MAX(c.MoodyFacilityRatingAdjusted)) MoodyFacilityRatingAdjusted
		  , MAX(a.CLOAnalyst)			CLOAnalyst
		  , MAX(a.HFAnalyst)				HFAnalyst
		  , CONVERT(VARCHAR(10), MAX(a.AsOfDate), 101) AsOfDate
		  , MAX(a.CreditScore		)	CreditScore	
		  , MAX(a.LiquidityScore		)	LiquidityScore		
		  , MAX(a.OneLLeverage		)	OneLLeverage		
		  , CONVERT(varchar, CAST(MAX(a.TotalLeverage) AS money), 1) TotalLeverage
		  , CONVERT(varchar, CAST(MAX(a.EVMultiple) AS money), 1) EVMultiple
		  , CONVERT(varchar, CAST(MAX(a.LTMRevenues) AS money), 1) LTMRevenues
		  , CONVERT(varchar, CAST(MAX(a.LTMEBITDA )AS money), 1) LTMEBITDA
		  , CONVERT(varchar, CAST(MAX(a.FCF) AS money), 1) FCF
		  , MAX(a.Comments)						Comments	
		  , MAX(a.BusinessDescription)			BusinessDescription	
		  , MAX(a.AgentBank			)			AgentBank			
		  , MAX(s.MaturityDate) SecurityMaturityDate
		  , CAST(0 as bit) as IsOnAlert
		  , CAST(MAX(a.LTMFCF) AS NUMERIC(38,2)) LTMFCF
		  , CAST(MAX(a.EnterpriseValue) AS NUMERIC(10,2)) EnterpriseValue
		  , CAST(MAX(a.SeniorLeverage) AS NUMERIC(10,2)) SeniorLeverage
		  

		  , CONVERT(varchar, CAST(MAX(m.CostPrice) AS money), 1) CostPrice
		  , MAX(m.CostPrice) CostPriceNum
		  , MAX(pm.Bid)	PrevDayBidNum
		  , MAX(pm.Offer)	PrevDayOfferNum
		  , CONVERT(varchar, CAST(MAX(pm.Bid) AS money), 1) PrevDayBid
		  , CONVERT(varchar, CAST(MAX(pm.Offer) AS money), 1) PrevDayOffer
		  , CASE WHEN ISNULL(MAX(m.Bid),0) <> 0 AND ISNULL(MAX(pm.Bid),0) <> 0 THEN (MAX(m.Bid) - MAX(pm.Bid)) ELSE NULL END PriceMove
		  , NULL As SearchText
		  , MAX(s.LienTypeId) LienTypeId
		  , MAX(cs.ScoreDescription) ScoreDescription

    FROM  CLO.vw_Security s  WITH (NOLOCK)
	      LEFT  JOIN CLO.vw_SecurityMarket m WITH (NOLOCK) ON s.SecurityId			= m.SecurityId 
		  LEFT  JOIN CLO.vw_PrevDaySecurityMarket pm WITH (NOLOCK) ON s.SecurityId	= pm.SecurityId 
		  LEFT  JOIN CLO.vw_CurrentAnalystResearch a WITH (NOLOCK) ON a.IssuerId	= s.IssuerId
		  LEFT  JOIN CLO.vw_Calculation c WITH (NOLOCK) ON s.SecurityId			= c.SecurityId 
		  LEFT	JOIN CLO.Rating cfra WITH(NOLOCK) ON cfra.RatingDesc			= ISNULL(s.MoodyCashFlowRatingAdjusted, m.MoodyCashFlowRatingAdjusted)
		  LEFT	JOIN CLO.Rating fra WITH(NOLOCK) ON fra.RatingDesc				= ISNULL(s.MoodyFacilityRatingAdjusted,  c.MoodyFacilityRatingAdjusted)
		  LEFT  JOIN CLO.vw_PivotedPositionExposure		pos  WITH (NOLOCK) on pos.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.vw_PivotedPositionCountry		country  WITH (NOLOCK) on country.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.vw_PivotedPositionIsCovLite	covlite  WITH (NOLOCK) on covlite.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.vw_PivotedTradeTotalAllocation	totalallocation  WITH (NOLOCK) on totalallocation.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.vw_PivotedTradeAllocation	allocation  WITH (NOLOCK) on allocation.SecurityId = s.SecurityId
		  LEFT OUTER JOIN CLO.CreditScore cs ON CAST(a.CreditScore AS INT) = cs.Id
		  CROSS JOIN CLO.vw_PivotedFundTargetPar	targetpar
	group by S.SecurityId,s.IsOnWatch
GO
