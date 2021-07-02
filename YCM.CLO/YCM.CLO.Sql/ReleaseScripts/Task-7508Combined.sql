USE Yoda
GO
IF OBJECT_ID(N'CLO.CreditScore') IS NULL
	CREATE TABLE CLO.CreditScore
	(
		Id INT NOT NULL IDENTITY(1, 1)
		, ScoreDescription VARCHAR(2000) NOT NULL 
	)
ELSE
	PRINT 'CreditScore exists!'
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
	  ,WH5
FROM (
    SELECT 
        TargetPar,FundCode
    FROM [CLO].[Fund] with(nolock)
) as s
PIVOT
(
    max(TargetPar)
    FOR FundCode IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],WH5)
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
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], WH5
	FROM (SELECT (CASE WHEN IsCovLite = 1 THEN 1 ELSE 0 END) IsCovLite
			, FundCode, SecurityId
			FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
(MAX(IsCovLite) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], WH5) )AS pvt
GO
CREATE VIEW CLO.vw_PivotedPositionExposure
AS
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH5
	FROM (SELECT Exposure, FundCode, SecurityId
			FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
(MAX(Exposure) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH5) )AS pvt
GO
CREATE VIEW CLO.vw_PivotedPositionCountry
AS
	SELECT	SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], WH5
	FROM	(SELECT	CountryDesc, FundCode, SecurityId
				FROM CLO.vw_PositionCountryFund WITH (NOLOCK)) AS s PIVOT
( MAX(CountryDesc) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], WH5) )AS pvt
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
	SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH5
	FROM (SELECT	Allocation, FundCode, SecurityId FROM CLO.GetActiveTrades()) AS s PIVOT
(MAX(Allocation) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH5)) AS pvt
GO
CREATE VIEW CLO.vw_PivotedTradeTotalAllocation
AS
	SELECT	SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH5
	FROM	(SELECT	TotalAllocation, FundCode, SecurityId FROM CLO.GetActiveTrades()) AS s PIVOT
(MAX(TotalAllocation) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], WH5)) AS pvt
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
		  , NULLIF((ISNULL(MAX(pos.[WH5]),0) + ISNULL(MAX(allocation.[WH5]),0)),0)		TRSNumExposure

		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0), '#,###')	CLO1Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0), '#,###')	CLO2Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0), '#,###')	CLO3Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0), '#,###')	CLO4Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0), '#,###')	CLO5Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[WH5]),0) + ISNULL(MAX(allocation.[WH5]),0)),0), '#,###')		TRSExposure

		  , FORMAT(((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0))/ISNULL(MAX(targetPar.[CLO-1]),1)) , 'p')  CLO1PctExposure   
		  , FORMAT(((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0))/ISNULL(MAX(targetPar.[CLO-2]),1)) , 'p')  CLO2PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0))/ISNULL(MAX(targetPar.[CLO-3]),1)) , 'p')  CLO3PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0))/ISNULL(MAX(targetPar.[CLO-4]),1)) , 'p')  CLO4PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0))/ISNULL(MAX(targetPar.[CLO-5]),1)) , 'p')  CLO5PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[WH5]),0) + ISNULL(MAX(allocation.[WH5]),0))/ISNULL(MAX(targetPar.[WH5]),1)) , 'p')		  TRSPctExposure 

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
		  CROSS JOIN CLO.vw_PivotedFundTargetPar	targetpar
	group by S.SecurityId,s.IsOnWatch
GO
IF NOT EXISTS(SELECT * FROM CLO.Field WHERE FieldGroupId = 5 AND (FieldName = 'TRSExposure' OR FieldName = 'TRSPctExposure'))
BEGIN
	INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, JsonFormatString, DisplayWidth, IsPercentage, SortOrder, FieldType,
							HeaderCellClass, CellClass, CellTemplate, Hidden, PinnedLeft, IsSecurityOverride, ShowInFilter, FilterOrder)
	SELECT FieldGroupId, REPLACE(FieldName, 'CLO4', 'TRS') FieldName, REPLACE(JsonPropertyName, 'clo4', 'trs') JsonPropertyName
	, REPLACE(FieldTitle, 'CLO-4', 'WH') FieldTitle, JsonFormatString, DisplayWidth, IsPercentage, SortOrder + 20 SortOrder, FieldType,
							HeaderCellClass, CellClass
		, REPLACE(CellTemplate, 'clo4', 'trs') CellTemplate
		, Hidden, PinnedLeft, IsSecurityOverride, ShowInFilter, FilterOrder
	FROM CLO.Field
	WHERE FieldGroupId = 5
	AND FieldId IN (114, 115)
END
ELSE
	PRINT 'CLO 5 exists!'
GO
ALTER VIEW [CLO].[vw_CLOSummary]
	AS 


	WITH traded_cfe AS 
	(
		SELECT FundId,SUM(TradedCash) TradedCash FROM [CLO].[GetActiveTrades]()
		GROUP BY FundID
	)


	SELECT 
	f.FundCode,
	ISNULL(NULLIF(f.IsStale,CAST(0 AS BIT)),f.[IsPrincipalCashStale]) IsStale,
	MAX(p.PositionDateId) DateId,
	(SUM(ISNULL(p.NumExposure,0)) + (ISNULL(MAX(p.PrincipalCash),0) + ISNULL(MAX(tc.TradedCash),0)) - SUM(p.CapitalizedInterestOrig)) Par,
	(SUM(ISNULL(p.BODExposure,0)) + (ISNULL(MAX(p.PrincipalCash),0))  - SUM(p.CapitalizedInterestOrig)) BODPar,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(p.Spread,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END Spread,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(p.Spread,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODSpread,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(p.TotalCoupon,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END  TotalCoupon,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(p.TotalCoupon,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODTotalCoupon,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(p.WARF,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END   WARF,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(p.WARF,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END   BODWARF,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(p.MoodyRecovery,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END   MoodyRecovery,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(p.MoodyRecovery,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODMoodyRecovery,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(p.Bid,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END  Bid,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(p.Bid,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODBid,
	ISNULL(MAX(p.PrincipalCash),0) + ISNULL(MAX(tc.TradedCash),0)  PrincipalCash,
	ISNULL(MAX(p.PrincipalCash),0)  BODPrincipalCash,
	f.FundId,
	CAST(NULL as numeric(38,10)) Diversity,
	CAST(NULL as numeric(38,10)) BODDiversity,
	
	CASE WHEN ISNULL(MAX(p.EquityPar),0) <> 0 then ((SUM((ISNULL(p.NumExposure,0) * coalesce(p.Bid,p.tradeprice,0))/100) + (ISNULL(MAX(p.PrincipalCash),0) + ISNULL(MAX(tc.TradedCash),0)) - ISNULL(MAX(p.LiabilityPar),0))/ISNULL(MAX(p.EquityPar),0))*100 ELSE NULL END   CleanNav ,
	CASE WHEN ISNULL(MAX(p.EquityPar),0) <> 0 THEN ((SUM((ISNULL(p.BODExposure,0) * ISNULL(p.Bid,0))/100) + ISNULL(MAX(p.PrincipalCash),0) - ISNULL(MAX(p.LiabilityPar),0))/ ISNULL(MAX(p.EquityPar),0))*100 ELSE NULL END  BODCleanNav,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 THEN (((SUM(ISNULL(p.MaturityDays,0)*ISNULL(p.NumExposure,0))/SUM(CASE WHEN p.MaturityDays IS NOT NULL then ISNULL(p.NumExposure,0) ELSE 0 end))/365)+ISNULL(MAX(p.WALifeAdjustment),0)) ELSE NULL END WAMaturityDays,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 THEN (((SUM(ISNULL(p.MaturityDays,0)*ISNULL(p.BODExposure,0))/SUM(CASE WHEN p.MaturityDays IS NOT NULL then ISNULL(p.BODExposure,0) ELSE 0 end))/365)+ISNULL(MAX(p.WALifeAdjustment),0)) ELSE NULL END BODWAMaturityDays,
	CAST(NULL as numeric(38,4)) AssetPar,
	CAST(NULL as numeric(38,4)) PriorDayExposure,
	CAST(NULL as numeric(38,10)) PriorDayPrincipalCash
	, MAX(f.WSOSpread) WSOSpread
	, MAX(f.WSOWARF) WSOWARF
	, MAX(f.WSOMoodyRecovery) WSOMoodyRecovery
	, MAX(f.WSOWALife) WSOWALife
	, MAX(f.WSODiversity) WSODiversity
	, f.IsWarehouse

	FROM CLO.vw_Position p with(nolock)
	LEFT JOIN traded_cfe tc ON tc.FundId = p.FundId
	RIGHT JOIN [clo].Fund f WITH(NOLOCK) ON f.FundId = p.FundId

	where f.IsActive = 1 

	GROUP BY f.FundCode,f.FundId,f.IsStale,f.[IsPrincipalCashStale], f.IsWarehouse
GO
ALTER VIEW [CLO].[vw_PivotedPositionIsCovLite]
as

SELECT [SecurityId]
      ,[CLO-1]
      ,[CLO-2]
      ,[CLO-3]
      ,[CLO-4]
      ,[CLO-5]
	  ,WH5
FROM (
    SELECT 
      (case when IsCovLite = 1 then 1 else 0 end) IsCovLite,FundCode,SecurityId
    FROM [CLO].[vw_PositionCountryFund] with(nolock)
) as s
PIVOT
(
    max(IsCovLite)
    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],[WH5])
)AS pvt
GO
ALTER PROCEDURE CLO.spGetAssetPar
    @dateId AS INT
AS
DECLARE @prevDateId INT
SET @prevDateId = CLO.GetPrevDayDateId()
IF @dateId = @prevDateId
	SELECT F.FundId, SUM(P.Exposure) AS AssetPar, MAX(ISNULL(F.PrincipalCash, 0)) AS PrincipalCash
	FROM CLO.vw_SecurityFund F INNER JOIN CLO.Position P ON P.FundId = F.FundId
	AND P.SecurityId = F.SecurityId
	WHERE P.DateId = @dateId
	GROUP BY F.FundId, F.FundCode
ELSE
BEGIN
	WITH priorSnap AS 
	(
		SELECT FundId, DateId, MAX(PrincipalCash) AS PrincipalCash FROM CLO.FundDailySnapshot
		WHERE DateId = @prevDateId
		GROUP BY FundId, DateId
	)
	SELECT F.FundId, SUM(P.Exposure) AS AssetPar, MAX(ISNULL(S.PrincipalCash, 0)) AS PrincipalCash
	FROM CLO.vw_SecurityFund F INNER JOIN CLO.Position P ON P.FundId = F.FundId
	AND P.SecurityId = F.SecurityId
	INNER JOIN priorSnap S
	ON S.FundId = F.FundId
	WHERE P.DateId = @dateId
	GROUP BY F.FundId, F.FundCode
END
GO
UPDATE CLO.Fund
SET IsStale = 0, IsActive = 1, DisplayText = 'WH', FundCode = 'WH'
WHERE FundId = 5
GO
ALTER PROCEDURE [CLO].[spGetSummaryData]
	@dateId INT = 0
AS

DECLARE @priorDay TABLE(FundId INT, Exposure NUMERIC(38, 4), PrincipalCash NUMERIC(38, 10))
DECLARE @currentDay TABLE(FundId INT, Exposure NUMERIC(38, 4), PrincipalCash NUMERIC(38, 10))

CREATE TABLE #fundDiversities 
(
	FundId int,
	Diversity numeric(38,10),
	BODDiversity numeric(38,10)
)

CREATE TABLE #ExposureBy_Issuer_MoodyIndustry_Portfolio(
       FundCode VARCHAR(100), 
       FundId INT,
       IssuerDesc VARCHAR(1000),
       IssuerId INT, 
       IndustryDesc VARCHAR(1000),
       MoodyIndustryId INT, 
       Exposure NUMERIC(28,2),
	   BODExposure NUMERIC(28,2),
       DiversityUnit NUMERIC(28,8), 
       Diveristy NUMERIC(28,8),
       BODDiversityUnit NUMERIC(28,8), 
       BODDiveristy NUMERIC(28,8)
)

CREATE TABLE #AVG_Fund_Exposure (FundId VARCHAR(100), AvgExposure NUMERIC(38,10), BODAvgExposure NUMERIC(38,10))

INSERT INTO #ExposureBy_Issuer_MoodyIndustry_Portfolio
SELECT p.FundCode , p.FundId, p.Issuer,p.IssuerId, p.MoodyIndustry,
p.MoodyIndustryId, SUM(ISNULL(P.NumExposure,0)) Exposure  ,SUM(ISNULL(P.BODExposure,0)) BODExposure,  
NULL AS DiversityUnit, NULL AS Diveristy,NULL AS BODDiversityUnit, NULL AS BODDiveristy
FROM CLO.vw_Position p WITH(NOLOCK) 
where  p.PositionDateId is not null
GROUP BY p.IssuerId,p.Issuer, p.MoodyIndustryId,p.MoodyIndustry,p.FundCode,p.FundId
ORDER by p.Issuer,p.MoodyIndustry,p.FundCode


INSERT INTO #AVG_Fund_Exposure
        ( FundId, AvgExposure,BODAvgExposure )
SELECT FundId, AVG(Exposure) AvgExposure, AVG(BODExposure) BODAvgExposure
FROM #ExposureBy_Issuer_MoodyIndustry_Portfolio WITH(NOLOCK)
WHERE Exposure <> 0
GROUP BY FundId


UPDATE #ExposureBy_Issuer_MoodyIndustry_Portfolio
SET DiversityUnit = CASE WHEN AvgExposure = 0.0000 THEN 0.0000 ELSE (Exposure/AvgExposure) END ,
BODDiversityUnit = CASE WHEN BODAvgExposure = 0.0000 THEN 0.0000 ELSE (BODExposure/BODAvgExposure) END 
FROM #AVG_Fund_Exposure i  WITH(NOLOCK) WHERE i.FundId = [#ExposureBy_Issuer_MoodyIndustry_Portfolio].FundId


UPDATE #ExposureBy_Issuer_MoodyIndustry_Portfolio
SET DiversityUnit = 1.0000 
WHERE DiversityUnit > 1.0

UPDATE #ExposureBy_Issuer_MoodyIndustry_Portfolio
SET BODDiversityUnit = 1.0000 
WHERE BODDiversityUnit > 1.0


CREATE TABLE #Diverisity (FundId int, MoodyIndustryId INT, IndustryDesc VARCHAR(1000),
TotalDiversityUnit NUMERIC(28,4), DiversityValue NUMERIC(38,4),BODTotalDiversityUnit NUMERIC(28,4), BODDiversityValue NUMERIC(38,4) )

INSERT INTO #Diverisity
SELECT FundId, MoodyIndustryId, MAX(IndustryDesc) IndustryDesc,  
SUM(DiversityUnit) TotalDiversityUnit, NULL AS DiversityValue ,
SUM(BODDiversityUnit) BODTotalDiversityUnit, NULL AS BODDiversityValue 
FROM #ExposureBy_Issuer_MoodyIndustry_Portfolio  WITH(NOLOCK)
GROUP BY MoodyIndustryId, FundId 

UPDATE #Diverisity
SET DiversityValue = (SELECT TOP 1 [ParameterMaxValueNumber]  FROM 
[CLO].ParameterValue p  WITH(NOLOCK) WHERE p.ParameterTypeId =5 AND ParameterValueNumber <= TotalDiversityUnit
ORDER BY p.ParameterValueNumber DESC)


UPDATE #Diverisity
SET BODDiversityValue = (SELECT TOP 1 [ParameterMaxValueNumber] FROM 
[CLO].ParameterValue p  WITH(NOLOCK) WHERE p.ParameterTypeId =5 AND ParameterValueNumber <= BODTotalDiversityUnit
ORDER BY p.ParameterValueNumber DESC)


INSERT #fundDiversities
SELECT FundId, SUM(DiversityValue) AS Diversity,SUM(BODDiversityValue) AS BODDiversity  FROM #Diverisity WITH(NOLOCK) GROUP BY FundId

--get the current day exposure and cash
INSERT INTO @currentDay (FundId, Exposure, PrincipalCash)
SELECT F.FundId, SUM(P.Exposure), MAX(F.PrincipalCash)
FROM CLO.vw_SecurityFund F INNER JOIN CLO.Position P WITH(NOLOCK)
ON P.FundId = F.FundId
AND P.SecurityId = F.SecurityId
WHERE P.DateId = @dateId
GROUP BY F.FundId, F.FundCode

SELECT FundId, FundCode, SUM(Exposure) AS Exposure
INTO #assetPar
FROM #ExposureBy_Issuer_MoodyIndustry_Portfolio
GROUP BY FundId, FundCode

SELECT summary.[FundCode]
	  ,summary.IsStale
	  ,summary.DateId	
      ,summary.[Par]
	  ,summary.[BODPar]
      ,summary.[Spread]
	  ,summary.[BODSpread]
      ,summary.[TotalCoupon]
	  ,summary.[BODTotalCoupon]
      ,summary.[WARF]
	  ,summary.[BODWARF]
      ,summary.[MoodyRecovery]
	  ,summary.[BODMoodyRecovery]
      ,summary.[Bid]
	  ,summary.[BODBid]
      ,summary.[PrincipalCash]
	  ,summary.[BODPrincipalCash]
      ,summary.[FundId]
	  ,d.[Diversity]
	  ,d.[BODDiversity]
	  ,summary.[CleanNav]
	  ,summary.[BODCleanNav]
	  ,summary.[WAMaturityDays]
	  ,summary.[BODWAMaturityDays]
	  ,CAST(NULL As numeric(30,10)) AS PriorDayExposure
	  ,CAST(NULL As numeric(30,10))  AS PriorDayPrincipalCash
	  , p.Exposure AS AssetPar
	  , summary.WSOSpread
	  , summary.WSOWARF
	  , summary.WSOMoodyRecovery
	  , summary.WSOWALife
	  , summary.WSODiversity
FROM
CLO.vw_CLOSummary summary WITH(NOLOCK)
LEFT JOIN #fundDiversities d WITH(NOLOCK) ON d.FundId = summary.FundId
LEFT OUTER JOIN @currentDay c ON summary.FundId = c.FundId
LEFT OUTER JOIN #assetPar p ON p.FundId = summary.FundId
WHERE summary.DateId = @dateId

DROP TABLE #ExposureBy_Issuer_MoodyIndustry_Portfolio
DROP TABLE #AVG_Fund_Exposure
DROP TABLE #fundDiversities
DROP TABLE #Diverisity
DROP TABLE #assetPar
GO
IF NOT EXISTS(SELECT * FROM CLO.Field WHERE FieldName = 'BBGId')
	INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, JsonFormatString, DisplayWidth, IsPercentage, SortOrder, FieldType,
							HeaderCellClass, CellClass, CellTemplate, Hidden, PinnedLeft, IsSecurityOverride, ShowInFilter, FilterOrder)
	SELECT FieldGroupId, 'BBGId' FieldName, 'bbgId' JsonPropertyName, 'BLOOMBERG' FieldTitle, JsonFormatString, DisplayWidth + 100 DisplayWidth, IsPercentage, SortOrder + 10 AS SortOrder
	, FieldType, HeaderCellClass, CellClass
	, '<div class="ui-grid-cell-contents pull-left" uib-tooltip="{{row.entity.bbgId}}" tooltip-append-to-body="true" style="cursor:pointer" context-menu="grid.appScope.showBbgMenu(row.entity)"<span>{{row.entity.bbgId}}</span></div>' AS CellTemplate
	, Hidden, PinnedLeft, NULL IsSecurityOverride, ShowInFilter, FilterOrder
	FROM CLO.Field
	WHERE FieldId = 8
ELSE
	PRINT 'BBGId EXISTS!'
GO
IF NOT EXISTS(SELECT TOP 1 * FROM CLO.CreditScore)
BEGIN
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Very strong operating performance and credit metrics.  Cash substitute.  Very strong, sleeper credit.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Stable operating performance and credit metrics.  Buying decision is primarily an exercise in checking metrics.  Strong credit.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Average performance and credit metrics.  Some credit metrics may be stretched.  Has the potential to disappoint on news and for loan to trade off.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Risky credit with some negative history/headwinds, but not necessarily stressed just yet. These credits should be trading at above market yields.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Below average, stressed credit but with a clear path to recovery. Some principal loss is possible but not likely.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Underperforming, stressed credit with non-trivial probabilities of both recovery and default.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Very weak credit.  Some principal loss expected.  Good chance of default.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Impaired asset.  Significant principal loss likely.')
END
ELSE
	PRINT 'Legends exist!'
GO
IF NOT EXISTS(SELECT * FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund') AND name = 'PortfolioName')
	ALTER TABLE CLO.Fund ADD PortfolioName VARCHAR(255)
ELSE
	PRINT 'PortfolioName EXISTS!'
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-1 Ltd.'
WHERE FundId = 1
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-2 Ltd.'
WHERE FundId = 2
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-3 Ltd.'
WHERE FundId = 3
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-4 Ltd.'
WHERE FundId = 4
GO
IF OBJECT_ID(N'CLO.CreditScore') IS NULL
	CREATE TABLE CLO.CreditScore
	(
		Id INT NOT NULL IDENTITY(1, 1)
		, ScoreDescription VARCHAR(2000) NOT NULL 
	)
ELSE
	PRINT 'CreditScore exists!'
GO
IF NOT EXISTS(SELECT TOP 1 * FROM CLO.CreditScore)
BEGIN
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Very strong operating performance and credit metrics.  Cash substitute.  Very strong, sleeper credit.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Stable operating performance and credit metrics.  Buying decision is primarily an exercise in checking metrics.  Strong credit.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Average performance and credit metrics.  Some credit metrics may be stretched.  Has the potential to disappoint on news and for loan to trade off.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Risky credit with some negative history/headwinds, but not necessarily stressed just yet. These credits should be trading at above market yields.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Below average, stressed credit but with a clear path to recovery. Some principal loss is possible but not likely.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Underperforming, stressed credit with non-trivial probabilities of both recovery and default.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Very weak credit.  Some principal loss expected.  Good chance of default.')
	INSERT INTO CLO.CreditScore (ScoreDescription)
	VALUES('Impaired asset.  Significant principal loss likely.')
END
ELSE
	PRINT 'Legends exist!'
GO
IF NOT EXISTS(SELECT * FROM CLO.Field WHERE FieldName = 'GlobalAmountString')
	INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, JsonFormatString, DisplayWidth, IsPercentage, SortOrder, FieldType, HeaderCellClass, CellClass, CellTemplate, Hidden, PinnedLeft, IsSecurityOverride, ShowInFilter, FilterOrder)
	SELECT FieldGroupId, 'GlobalAmountString' FieldName, 'globalAmountString' JsonPropertyName
	, 'GLOBAL AMOUNT' FieldTitle, JsonFormatString, 100 AS DisplayWidth, IsPercentage, SortOrder + 10 AS SortOrder, FieldType, HeaderCellClass
	, 'text-right' CellClass
	--, '<div class="ui-grid-cell-contents"  uib-tooltip="{{row.entity.globalAmount| currency : '' : 2}}" style="cursor:pointer;" tooltip-append-to-body="true"><span>{{row.entity.globalAmountString}} </span></div>' CellTemplate
	, NULL AS CellTemplate
	, Hidden, PinnedLeft, IsSecurityOverride, ShowInFilter, FilterOrder
	FROM CLO.Field
	WHERE FieldId = 130
ELSE
	PRINT 'GlobalAmount exists!'
GO
ALTER VIEW CLO.vw_AggregatePosition
AS
		WITH wsodata AS
		(
			SELECT SecurityID AS SecurityCode, MIN(BankDeal_GlobalAmount) AS MinIssueSize, MAX(BankDeal_GlobalAmount) AS MaxIssueSize
			FROM CLO.WsoExtractAssets
			WHERE DateId = [CLO].[GetPrevDayDateId]()
			GROUP BY SecurityID
		),
		loancontract AS
		(
			SELECT SecurityCode, AVG(C.GlobalAmount) AS Average, MIN(C.GlobalAmount) MinAmt, MAX(C.GlobalAmount) AS MaxAmt
			FROM (
			SELECT AssetLoanXIDAssetIDName AS SecurityCode, F.FundId, SUM(C.ContractGlobalAmount) AS GlobalAmount FROM Yoda.CLO.LoanContract C INNER JOIN Yoda.CLO.Fund F
			ON F.PortfolioName = C.PortfolioName
			WHERE C.AsOfDate = DataMarts.dbo.GetDateFromDateId(Yoda.CLO.GetPrevDayDateId())
			GROUP BY C.AssetLoanXIDAssetIDName, F.FundId) C
			GROUP BY C.SecurityCode
		),
		totalPar AS
		(
			SELECT F.SecurityId, SUM(F.Exposure) AS TotalExposure
			FROM CLO.vw_PositionCountryFund F
			WHERE DateId = CLO.GetPrevDayDateId()
			GROUP BY F.SecurityId
		)

SELECT 
		  CLO.GetPrevDayDateId() PositionDateId
		  , NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0)	CLO1NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0)	CLO2NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0)	CLO3NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0)	CLO4NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0)	CLO5NumExposure
		  , NULLIF((ISNULL(MAX(pos.[WH5]),0) + ISNULL(MAX(allocation.WH5),0)),0)		TRSNumExposure

		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0), '#,###')	CLO1Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0), '#,###')	CLO2Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0), '#,###')	CLO3Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0), '#,###')	CLO4Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0), '#,###')	CLO5Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.WH5),0) + ISNULL(MAX(allocation.WH5),0)),0), '#,###')		TRSExposure

		  , FORMAT(((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0))/ISNULL(MAX(targetPar.[CLO-1]),1)) , 'p')  CLO1PctExposure   
		  , FORMAT(((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0))/ISNULL(MAX(targetPar.[CLO-2]),1)) , 'p')  CLO2PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0))/ISNULL(MAX(targetPar.[CLO-3]),1)) , 'p')  CLO3PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0))/ISNULL(MAX(targetPar.[CLO-4]),1)) , 'p')  CLO4PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0))/ISNULL(MAX(targetPar.[CLO-5]),1)) , 'p')  CLO5PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[WH5]),0) + ISNULL(MAX(allocation.[WH5]),0))/ISNULL(MAX(targetPar.[WH5]),1)) , 'p')		  TRSPctExposure 

		  , MAX(targetPar.[CLO-1]) CLO1TargetPar
		  , MAX(targetPar.[CLO-2]) CLO2TargetPar
		  , MAX(targetPar.[CLO-3]) CLO3TargetPar
		  , MAX(targetPar.[CLO-4]) CLO4TargetPar
		  , MAX(targetPar.[CLO-5]) CLO5TargetPar
		  , MAX(targetPar.[WH5]) TRSTargetPar

		  , s.SecurityId
		  , MAX(s.SecurityCode		)		SecurityCode
		  , MAX(s.SecurityDesc		)		SecurityDesc
		  , MAX(s.BBGId			)		BBGId
		  , MAX(s.Issuer			)		Issuer
		  , MAX(s.IssuerId			)		IssuerId
		  , MAX(s.IssuerDesc			)	IssuerDesc
		  , MAX(LTRIM(s.Facility)) Facility
		  , MAX(case when s.CallDate <> '1900-01-01' then  CONVERT(VARCHAR(10), s.CallDate, 101) else null END) CallDate
		  , COALESCE(MAX(country.[CLO-1]), MAX(country.[CLO-2]), MAX(country.[CLO-3]), MAX(country.[CLO-4]), MAX(country.WH5))  CountryDesc
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
		  , FORMAT(NULLIF(ISNULL(MAX(TP.TotalExposure), 0),0), '#,###') TotalPar
		  , ISNULL(MAX(TP.TotalExposure), 0) TotalParNum
		  --, FORMAT(NULLIF((ISNULL(MAX(c.TotalPar),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0), '#,###')	TotalPar
		  --, NULLIF((ISNULL(MAX(c.TotalPar),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0) TotalParNum
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
		  , ISNULL(MAX(lc.MaxAmt), MAX(wso.MaxIssueSize)) GlobalAmount

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
		  LEFT OUTER JOIN wsodata wso ON wso.SecurityCode = s.SecurityCode
		  LEFT OUTER JOIN loancontract lc ON lc.SecurityCode = s.SecurityCode
		  LEFT OUTER JOIN totalPar TP ON TP.SecurityId = s.SecurityId
		  CROSS JOIN CLO.vw_PivotedFundTargetPar	targetpar
	group by S.SecurityId,s.IsOnWatch
GO
UPDATE CLO.Field
SET CellTemplate = '<div class="ui-grid-cell-contents"  uib-tooltip="{{row.entity.creditScore + '' | '' + row.entity.scoreDescription}}" style="cursor:pointer;" tooltip-enable="{{row.entity.creditScore}}" tooltip-append-to-body="true"><span>{{row.entity.creditScore}} </span></div>'
WHERE FieldId = 48
GO
ALTER FUNCTION CLO.GetActiveTrades ()
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

		DECLARE @exposure TABLE
		(
			  FundId INT
			, SecurityId INT
			, Exposure DECIMAL(38, 4)
		)

		DECLARE @exclusions TABLE
		(
			  FundId INT
			, SecurityId INT
		)

		DECLARE @prevDayDateId INT 
		SELECT @prevDayDateId = CLO.GetPrevDayDateId()

		INSERT INTO @exposure (FundId, SecurityId, Exposure)
		SELECT FundId, SecurityId, SUM(Exposure) AS Exposure
		FROM CLO.Position (NOLOCK)
		WHERE DateId = @prevDayDateId
		GROUP BY FundId, SecurityId

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

		INSERT INTO @exposure (FundId, SecurityId, Exposure)
		SELECT FundId, SecurityId, SUM(Allocation)
		FROM @tradeallocations
		GROUP BY FundId, SecurityId

		INSERT INTO @exclusions (FundId, SecurityId)
		SELECT FundId, SecurityId
		FROM @exposure
		GROUP BY FundId, SecurityId
		HAVING SUM(Exposure) < 0

		INSERT INTO @returntable (SecurityId, FundId, Allocation, TradedCash, TotalAllocation, TradePrice, HasBuy, HasSell, FundCode)
		SELECT SecurityId, FundId, Allocation, TradedCash, SUM(Allocation) OVER (PARTITION BY SecurityId)
			, TradePrice, HasBuy, CASE WHEN HasBuy = 1 THEN 0 ELSE 1 END, FundCode
		FROM @tradeallocations A
		WHERE NOT EXISTS (SELECT * FROM @exclusions WHERE A.SecurityId = SecurityId AND A.FundId = FundId)

		RETURN
	END
GO
UPDATE CLO.Fund
SET DisplayText = 'W5', FundCode = 'WH5'
WHERE FundId = 5
GO
UPDATE CLO.Field
SET FieldTitle = 'WH5'
WHERE FieldId = 128
GO
UPDATE CLO.Field
SET FieldTitle = 'WH5 %'
WHERE FieldId = 129
GO
ALTER PROCEDURE CLO.LoadWso_MarketData
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
    --DECLARE @DatasetKeys TABLE (DatasetKey INT)
    --INSERT INTO @DatasetKeys
    --(
    --    DatasetKey
    --)
    --SELECT (1997)
    --UNION
    --SELECT (1998)
    --UNION
    --SELECT (1999)
    --UNION
    --SELECT (2000)

    BEGIN TRANSACTION

    DELETE FROM CLO.MarketData
    WHERE DateId = @asOfDateId
	AND FundId IN (SELECT DISTINCT D.DealId
		FROM [CLO].[WsoExtractAssets] EA
        JOIN @DatasetKeys DK
            ON DK.DatasetKey = EA.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON DK.DatasetKey = D.DatasetKey)

    DELETE FROM CLO.WSOMarketData
    WHERE DateId = @asOfDateId
	AND FundId IN (SELECT DISTINCT D.DealId
		FROM [CLO].[WsoExtractAssets] EA
        JOIN @DatasetKeys DK
            ON DK.DatasetKey = EA.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON DK.DatasetKey = D.DatasetKey)

    INSERT INTO CLO.MarketData
    (
        FundId,
		DateId,
        SecurityId,
        Bid,
        Offer,
		CostPrice,
        Spread,
        LiborFloor,
        MoodyCashFlowRatingId,
        MoodyCashFlowRatingAdjustedId,
        MoodyFacilityRatingId,
        MoodyRecovery,
        SnPIssuerRatingId,
        SnPIssuerRatingAdjustedId,
        SnPFacilityRatingId,
        SnPfacilityRatingAdjustedId,
        SnPRecoveryRatingId,
        MoodyOutlook,
        MoodyWatch,
        SnPWatch,
        NextReportingDate,
        FiscalYearEndDate,
        AgentBank,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy
    )
    SELECT 
		D.DealID AS FundId,
		@asOfDateId,
        (
            SELECT TOP 1
                SecurityId
            FROM CLO.Security
            WHERE SecurityCode = EA.SecurityId
        ) AS SecurityId,
        CAST(EA.MarkPrice_BidPrice AS NUMERIC(38, 10)) AS Bid, --NEED TO MAKE SURE ABOUT SCALE
        CAST(EA.MarkPrice_AskPrice AS NUMERIC(38, 10)) AS Offer,                                           --'***MISSING***' 
		CAST(EA.[CostPrice] AS NUMERIC(38, 10)) AS CostPrice,
        EA.WeightedAvgSpread * 100 AS Spread,
        ([Adjusted WAC] - [Adjusted WAS]) * 100 AS LiborFloor, --CALC PER WENDY
		--4)	Floor –it’s working in such a way that it’s showing up as a negative number on our delayed draw loans, which isn’t right… using Spectrum Delayed Draw, as an example, it’s technically earning a ticking fee, not interest. So to translate that, we would probably expect to see 1% spread and 0% floor… you show 1% spread and -2.25% floor. In looking at the assets datasheet, you can fix this by calculating the all-in-rate minus the zAdjustedWAS, rather than subtracting the Spread Libor.
		--EA.LiborBaseRateFloor * 100 AS LiborFloor,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.[Moody's CF Rating] = R.RatingDesc
        ),
                  -1
              ) MoodyCashFlowRatingId,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.[Moody's DP Rating - WARF (ADJ)] = R.RatingDesc
        ),
                  -1
              ) AS MoodyCashFlowRatingAdjustedId,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.RatingMoodyIssuance = R.RatingDesc
        ),
                  -1
              ) AS MoodyFacilityRatingId,
        
        CAST(EA.RecoveryRateMoody AS NUMERIC(18, 4)) * 100 AS MoodyRecovery,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.[S&P Issuer Rating] = R.RatingDesc
        ),
                  -1
              ) AS SnPIssuerRatingId,
        ISNULL(
        (
            SELECT TOP 1 R.RatingId FROM CLO.Rating R WHERE EA.RatingSP = R.RatingDesc
        ),
                  -1
              ) AS SnPIssuerRatingAdjustedId,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.RatingSPIssue = R.RatingDesc
        ),
                  -1
              ) AS SnPFacilityRatingId,
        -1 AS SnPfacilityRatingAdjustedId,                     --SP NOT ADJUSTED PER WENDY
        -1 AS SnPRecoveryRatingId,
        CASE EA.[Moody's Outlook]
            WHEN 'NEG' THEN
                '-'
            WHEN 'POS' THEN
                '+'
            ELSE
                '0'
        END AS MoodyOutlook,
		CASE EA.[Moody's Credit Watch]
            WHEN 'On Watch for upgrade' THEN
                '+'
            WHEN 'On Watch for downgrade' THEN
                '-'
            ELSE
                'X'
        END AS MoodyWatch,
        CASE EA.RatingSPIssueCreditWatch
            WHEN 'NEG' THEN
                '-'
            WHEN 'POS' THEN
                '+'
            ELSE
                '0'
        END AS SnPWatch,
        '1/1/1900' AS NextReportingDate,                       --'***NOT NEEDED PER WILL***'
        '1/1/1900' AS FiscalYearEndDate,                       --'***NOT NEEDED PER WILL***'
        '***MISSING***' AS AgentBank,                          --'BLOOMBERG: ln_agent'
        GETDATE(),                                             -- CreatedOn - datetime
        'WSO',                                                 -- CreatedBy - varchar(100)
        GETDATE(),                                             -- LastUpdatedOn - datetime
        'WSO'                                                  -- LastUpdatedBy - varchar(100)
    FROM CLO.WsoExtractAssets EA
        JOIN @DatasetKeys DK
            ON DK.DatasetKey = EA.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON DK.DatasetKey = D.DatasetKey
    --WHERE EA.PrincipalBalance <> 0
    --GROUP BY
    --    EA.SecurityID,
    --    EA.RatingSP,
    --    EA.RatingSPIssue,
    --    EA.RatingSPIssueCreditWatch,
    --    EA.[S&P Issuer Rating],
    --    EA.MarkPrice_BidPrice,
    --    EA.SpreadLibor,
    --    EA.LiborBaseRateFloor,
    --    [Moody's CF Rating],
    --    [Moody's DP Rating - WARF (ADJ)],
    --    EA.RatingMoodyIssuance,
    --    EA.RecoveryRateMoody,
    --    EA.[Moody's Outlook]
    --ORDER BY EA.SecurityID
    --SELECT * FROM dbo.WSOExtractAssets WHERE SecurityID = 'LX133675' ORDER BY 1 desc
    COMMIT TRANSACTION
GO
