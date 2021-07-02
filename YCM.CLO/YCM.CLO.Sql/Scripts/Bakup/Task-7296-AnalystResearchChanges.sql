UPDATE CLO.Field
SET CellTemplate = '<div class="pull-left ui-grid-cell-contents"  uib-tooltip="Issuer: {{row.entity.issuerDesc}}" tooltip-append-to-body="true" style="cursor:pointer;max-width:150px" context-menu="grid.appScope.showMenu(row.entity,row.entity.isOnWatch)" > <span ng-hide="grid.appScope.researchExists(row.entity.issuerId)">{{row.entity.issuer}} </span><span ng-show="grid.appScope.researchExists(row.entity.issuerId)"><a ng-click="grid.appScope.showAnalystResearch(row.entity.issuerId)">{{row.entity.issuer}} </a></span></div><div class="pull-right"  uib-tooltip="{{''Original Value : '' + (row.entity[''orig''+col.colDef.field.capitalizeFirstLetter()]?row.entity[''orig''+col.colDef.field.capitalizeFirstLetter()]:'''')}}" ng-if="row.entity[''orig''+col.colDef.field.capitalizeFirstLetter()] != row.entity[col.colDef.field]" tooltip-append-to-body="true"><img class="i-c" src="CLO/Content/Images/Comment.png"></div>'
WHERE FieldTitle = 'ISSUER'
GO
IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund')
AND name = 'WSOSpread')
	ALTER TABLE CLO.Fund
	ADD WSOSpread DECIMAL(38, 6)
ELSE
	PRINT 'WSOSpread Column exists!'
GO
IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund')
AND name = 'WSOWARF')
	ALTER TABLE CLO.Fund
	ADD WSOWARF DECIMAL(38, 6)
ELSE
	PRINT 'WSOWARF Column exists!'
GO
IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund')
AND name = 'WSOMoodyRecovery')
	ALTER TABLE CLO.Fund
	ADD WSOMoodyRecovery DECIMAL(38, 6)
ELSE
	PRINT 'WSOMoodyRecovery Column exists!'
GO
IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund')
AND name = 'WSOWALife')
	ALTER TABLE CLO.Fund
	ADD WSOWALife DECIMAL(38, 6)
ELSE
	PRINT 'WSOWALife Column exists!'
GO
IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund')
AND name = 'WSODiversity')
	ALTER TABLE CLO.Fund
	ADD WSODiversity DECIMAL(38, 6)
ELSE
	PRINT 'WSODiversity Column exists!'
GO
IF OBJECT_ID(N'CLO.vw_CLOSummary') IS NOT NULL
	DROP VIEW CLO.vw_CLOSummary
ELSE
	PRINT 'CLO.vw_CLOSummary does not exist!'
GO
IF OBJECT_ID(N'CLO.vw_AggregatePosition') IS NOT NULL
	DROP VIEW CLO.vw_AggregatePosition
ELSE
	PRINT 'CLO.vw_AggregatePosition does not exist!'
GO
IF OBJECT_ID(N'CLO.vw_Position') IS NOT NULL
	DROP VIEW CLO.vw_Position
ELSE
	PRINT 'CLO.vw_Position does not exist!'
GO
IF OBJECT_ID(N'CLO.vw_CurrentAnalystResearch') IS NOT NULL
	DROP VIEW CLO.vw_CurrentAnalystResearch
ELSE
	PRINT 'CLO.vw_CurrentAnalystResearch does not exist!'
GO
CREATE VIEW CLO.vw_CurrentAnalystResearch
AS
    WITH    analystrefresh_cfe
              AS (
                  SELECT    D.AnalystResearchDetailId AS AnalystResearchId, IssuerId, H.CLOAnalystId AS CLOAnalystUserId
					, H.HFAnalystId AS HFAnalystUserId, AsOfDate, CreditScore, CAST(NULL AS DECIMAL(10, 4)) LiquidityScore
					, CAST(NULL AS DECIMAL(10, 4)) AS OneLLeverage, D.SeniorLeverage
					, D.EnterpriseValue, D.LTMFCF, TotalLeverage, CAST(NULL AS DECIMAL(10, 4)) AS EVMultiple
					, LTMRevenues, LTMEBITDA, FCF, Comments, BusinessDescription, GETDATE() AS CreatedOn, 'YodaUser' AS CreatedBy
					, GETDATE() AS LastUpdatedOn, 'YodaUser' AS LastUpdatedBy, AgentBank
					, ROW_NUMBER() OVER (PARTITION BY IssuerId ORDER BY AsOfDate DESC) AS ROWNUM
                  FROM      CLO.AnalystResearchDetail D WITH (NOLOCK) INNER JOIN CLO.AnalystResearchHeader H WITH (NOLOCK)
				  ON H.AnalystResearchHeaderId = D.AnalystResearchHeaderId
                  WHERE     AsOfDate IS NULL
                            OR AsOfDate <= CONVERT(DATE, GETDATE())
                 )
    SELECT  AnalystResearchId, a.IssuerId, a.CLOAnalystUserId, a.HFAnalystUserId, cloanalyst.AnalystDesc CLOAnalyst
		, hfanalyst.AnalystDesc HFAnalyst, AsOfDate, CreditScore, LiquidityScore, OneLLeverage, TotalLeverage
		, EVMultiple, LTMRevenues, LTMEBITDA, FCF, Comments, issuer.IssuerDesc, a.BusinessDescription, a.AgentBank
		, SeniorLeverage, EnterpriseValue, LTMFCF
    FROM    analystrefresh_cfe a WITH (NOLOCK)
    LEFT JOIN CLO.vw_YorkCoreGenevaAnalyst cloanalyst WITH (NOLOCK) ON cloanalyst.AnalystId = a.CLOAnalystUserId
    LEFT JOIN CLO.vw_YorkCoreGenevaAnalyst hfanalyst WITH (NOLOCK) ON hfanalyst.AnalystId = a.HFAnalystUserId
    JOIN    CLO.Issuer issuer WITH (NOLOCK) ON a.IssuerId = issuer.IssuerId
    WHERE   ROWNUM = 1; 
GO
CREATE VIEW [CLO].[vw_Position]-- WITH SCHEMABINDING
AS
	WITH prev_position AS
		(
			SELECT PositionId, FundId, SecurityId, DateId, MarketValue, Exposure, PctExposure, PxPrice, IsCovLite, CountryId, CreatedOn, CreatedBy, LastUpdatedOn,
				 LastUpdatedBy, IsStale, CapitalizedInterestOrig
			FROM CLO.Position WITH (NOLOCK) WHERE DateId = [CLO].[GetPrevDayDateId]() OR PositionId IS NULL
		)

	SELECT 
		   p.[PositionId] 
		  ,p.DateId PositionDateId
		  ,FORMAT(NULLIF((ISNULL(p.[Exposure],0) + ISNULL(s.Allocation,0)),0) , '#,###') [Exposure]
		  ,(ISNULL(p.[Exposure],0) + ISNULL(s.Allocation,0))  [NumExposure]
		  ,ISNULL(p.[Exposure],0) [BODExposure]
		  ,ISNULL(p.[Exposure],0) + ISNULL((CASE WHEN s.HasBuy =1 then s.Allocation ELSE 0 END),0) [BODwithBuyExposure]
		  ,FORMAT(((ISNULL(p.[Exposure],0) + ISNULL(s.Allocation,0))/s.TargetPar) , 'p')  [PctExposure]  
		  ,ISNULL(p.[CapitalizedInterestOrig],0)  [CapitalizedInterestOrig]


          ,p.[PxPrice] 
		  ,((ISNULL(p.[Exposure],0) + ISNULL(s.Allocation,0))/s.TargetPar) PctExposureNum
		  ,s.FundId
		  ,s.FundCode
		  ,s.FundDesc
		  ,s.SecurityId
		  ,s.[SecurityCode]
		  ,s.[SecurityDesc]
		  ,s.[BBGId]
		  ,s.[Issuer]
		  ,s.[IssuerId]
		  ,s.[IssuerDesc]
		  ,LTRIM(s.[Facility]) [Facility]
		  ,case when s.CallDate <> '1900-01-01' then  CONVERT(VARCHAR(10), s.[CallDate], 101) else null end [CallDate]
		  ,country.[CountryDesc]
		  ,CONVERT(VARCHAR(10), s.[MaturityDate], 101) [MaturityDate]
		  ,s.[SnpIndustry]
		  ,s.[MoodyIndustry]
		  ,case when (p.[IsCovLite] =1 ) then 'Y' else 'N' end [IsCovLite]
		  ,case when (s.[IsFloating] = 1) then 'Floating' else 'Fixed' end [IsFloating]
		  ,s.[LienType]
		  
		  ,s.[IsOnWatch]
		  ,s.WatchObjectTypeId
		  ,s.WatchObjectId
		  ,s.[WatchId]
		  ,s.[WatchComments]
		  ,s.WatchLastUpdatedOn
		  ,s.WatchUser
		  
		  ,s.[OrigSecurityCode]
		  ,s.[OrigSecurityDesc]
		  ,s.[OrigBBGId]
		  ,s.[OrigIssuer]
		  ,s.[OrigFacility]
		  ,s.[OrigCallDate] 
		  ,s.[OrigMaturityDate]
		  ,s.[OrigSnpIndustry]
		  ,s.[OrigMoodyIndustry]
		  ,s.[OrigIsFloating] 
		  ,s.[OrigLienType]
		  ,c.[MoodyFacilityRatingAdjusted] AS OrigMoodyFacilityRatingAdjusted
		  ,m.[MoodyCashFlowRatingAdjusted] AS OrigMoodyCashFlowRatingAdjusted

		  ,s.PrincipalCash
		  ,s.LiabilityPar
		  ,s.EquityPar
		  ,s.[MoodyIndustryId]
		  ,s.[SecurityLastUpdatedOn]
		  ,s.[SecurityLastUpdatedBy]
	      ,s.[SecurityCreatedOn]
		  ,s.[SecurityCreatedBy]

		  ,m.[DateId] MarketDateId
		  ,m.[MarketDataId]
		  ,m.[OverrideMarketDataId]
		  ,CONVERT(varchar, CAST(m.[Bid] AS money), 1) [Bid]
		  ,CONVERT(varchar, CAST(m.[Offer] AS money), 1) [Offer]
		  ,CONVERT(varchar, CAST(pm.[Bid] AS money), 1) [PrevDayBid]
		  ,CONVERT(varchar, CAST(pm.[Offer] AS money), 1) [PrevDayOffer]
		  ,CONVERT(varchar, CAST(m.[CostPrice] AS money), 1) [CostPrice]
		  ,m.[CostPrice] [CostPriceNum]
		  ,m.[Bid] BidNum
		  ,m.[Offer] OfferNum
		  ,CASE WHEN ISNULL(m.[Bid],0) <> 0 AND ISNULL(pm.[Bid],0) <> 0 THEN FORMAT(((m.Bid - pm.Bid)/m.Bid),'p') ELSE NULL END PctBidDiff
		  ,CASE WHEN ISNULL(m.[Bid],0) <> 0 AND ISNULL(pm.[Bid],0) <> 0 THEN ((m.Bid - pm.Bid)/m.Bid) ELSE NULL END PctBidDiffNum
		  ,CASE WHEN ISNULL(m.[Offer],0) <> 0 AND ISNULL(pm.[Offer],0) <> 0 THEN FORMAT(((m.Offer - pm.Offer)/m.Offer),'p') ELSE NULL END PctOfferDiff
		  ,CASE WHEN ISNULL(m.[Offer],0) <> 0 AND ISNULL(pm.[Offer],0) <> 0 THEN ((m.Offer - pm.Offer)/m.Offer) ELSE NULL END PctOfferDiffNum
		  ,CASE WHEN ISNULL(m.[Bid],0) <> 0 AND ISNULL(pm.[Bid],0) <> 0 THEN (m.Bid - pm.Bid) ELSE NULL END PriceMove
		  ,pm.[Bid] PrevDayBidNum
		  ,pm.[Offer] PrevDayOfferNum
		  ,cast(m.[Spread] as numeric(20,2)) [Spread]
		  ,cast(m.[LiborFloor] as numeric(20,2)) [LiborFloor]
		  ,m.[MoodyCashFlowRating]
		  ,ISNULL(s.[MoodyCashFlowRatingAdjusted], m.[MoodyCashFlowRatingAdjusted]) [MoodyCashFlowRatingAdjusted]
		  ,m.[MoodyFacilityRating]
		  ,m.[MoodyRecovery]
		  ,m.[SnPIssuerRating]
		  ,m.[SnPIssuerRatingAdjusted]
		  ,m.[SnPFacilityRating]
		  ,m.[SnPfacilityRatingAdjusted]
		  ,m.[SnPRecoveryRating]
		  ,m.[MoodyOutlook]
		  ,m.[MoodyWatch]
		  ,m.[SnPWatch]
		  ,CONVERT(VARCHAR(10), m.[NextReportingDate], 101) [NextReportingDate]
		  ,CONVERT(VARCHAR(10), m.[FiscalYearEndDate], 101) [FiscalYearEndDate]
		  
		  ,c.[CalculationId]
		  ,c.[YieldBid]
		  ,c.[YieldOffer]
		  ,cast(c.[YieldMid] as numeric(20,2)) [YieldMid]
		  ,cast(c.[CappedYieldBid] as numeric(20,2)) [CappedYieldBid]
		  ,cast(c.[CappedYieldOffer] as numeric(20,2)) [CappedYieldOffer]
		  ,cast(c.[CappedYieldMid] as numeric(20,2)) [CappedYieldMid]
		  ,cast(c.[TargetYieldBid] as numeric(20,2)) [TargetYieldBid]
		  ,cast(c.[TargetYieldOffer] as numeric(20,2)) [TargetYieldOffer]
		  ,cast(c.[TargetYieldMid] as numeric(20,2)) [TargetYieldMid]
		  ,cast(c.[BetterWorseBid] as numeric(20,2)) [BetterWorseBid]
		  ,cast(c.[BetterWorseOffer] as numeric(20,2)) [BetterWorseOffer]
		  ,cast(c.[BetterWorseMid] as numeric(20,2)) [BetterWorseMid]
		  ,cast(c.[TotalCoupon] as numeric(20,2)) [TotalCoupon]
		  ,cast(c.[WARF] as numeric(20,2)) [WARF]
		  ,cast(c.[WARFRecovery] as numeric(20,2)) [WARFRecovery]
		  ,cast(c.[Life] as numeric(20,2)) [Life]
		  ,c.DateId CalculationDateId
		  ,FORMAT(NULLIF((ISNULL(c.[TotalPar],0) + ISNULL(s.TotalAllocation,0)),0), '#,###') [TotalPar]
		  ,NULLIF((ISNULL(c.[TotalPar],0) + ISNULL(s.TotalAllocation,0)),0) [TotalParNum]
		  ,(ISNULL(c.[TotalPar],0)) [BODTotalPar]
		  ,ISNULL(s.[MoodyFacilityRatingAdjusted],  c.[MoodyFacilityRatingAdjusted]) [MoodyFacilityRatingAdjusted]

		  ,[a].AnalystResearchId
		  ,[a].[CLOAnalyst]
		  ,[a].[HFAnalyst]
		  ,CONVERT(VARCHAR(10), [a].[AsOfDate], 101) [AsOfDate]
		  ,[a].[CreditScore]
		  ,[a].[LiquidityScore]
		  ,[a].[OneLLeverage]
		  ,CONVERT(varchar, CAST([a].[TotalLeverage] AS money), 1) [TotalLeverage]
		  ,CONVERT(varchar, CAST([a].[EVMultiple] AS money), 1) [EVMultiple]
		  ,CONVERT(varchar, CAST([a].[LTMRevenues] AS money), 1) [LTMRevenues]
		  ,CONVERT(varchar, CAST([a].[LTMEBITDA] AS money), 1) [LTMEBITDA]
		  ,CONVERT(varchar, CAST([a].[FCF] AS money), 1) [FCF]
		  ,CAST(a.LTMFCF AS money) LTMFCF
		  ,CAST(a.EnterpriseValue AS money) EnterpriseValue
		  ,CAST(a.SeniorLeverage AS money) SeniorLeverage
		  ,[a].[Comments]            
		  ,[a].[BusinessDescription]
		  ,[a].AgentBank
		  ,cfra.Rank MoodyCashFlowRatingAdjustedRank
		  ,fra.Rank MoodyFacilityRatingAdjustedRank
		  ,s.[MaturityDate] SecurityMaturityDate
		  ,CASE WHEN s.MaturityDate IS NOT NULL THEN DATEDIFF(DAY, GETDATE(), s.MaturityDate) ELSE NULL END MaturityDays
		  ,s.WALifeAdjustment
		  ,s.TradePrice
		  ,Cast(0 as bit) as IsOnAlert
		  ,ISNULL(s.IsStale,0) IsStale
		  ,NULL As SearchText
		  ,ROW_NUMBER() OVER ( PARTITION BY p.PositionId,p.[FundId],p.[SecurityId]
                                                ORDER BY p.DateId DESC ) AS POSROWNUM

    FROM  [CLO].[vw_SecurityFund] s
		  LEFT  JOIN prev_position p WITH (NOLOCK) on s.SecurityId = p.SecurityId and s.FundId = p.FundId AND ISNULL(s.IsStale,0) = ISNULL(p.IsStale,0)
		  LEFT	JOIN [CLO].Country country   WITH(NOLOCK) on country.CountryId = p.CountryId
          LEFT  JOIN [CLO].vw_MarketData m WITH (NOLOCK) ON s.SecurityId			= m.SecurityId and s.FundId = m.FundId
		  LEFT  JOIN [CLO].vw_PrevDayMarketData pm WITH (NOLOCK) ON s.SecurityId	= pm.SecurityId and s.FundId = pm.FundId
		  LEFT  JOIN [CLO].vw_CurrentAnalystResearch a WITH (NOLOCK) ON a.IssuerId	= s.IssuerId
		  LEFT  JOIN [CLO].vw_Calculation c WITH (NOLOCK) ON s.SecurityId			= c.SecurityId and s.FundId = c.FundId
		  LEFT	JOIN [CLO].Rating cfra WITH(NOLOCK) ON cfra.[RatingDesc]			= ISNULL(s.[MoodyCashFlowRatingAdjusted], m.[MoodyCashFlowRatingAdjusted])
		  LEFT	JOIN [CLO].Rating fra WITH(NOLOCK) ON fra.[RatingDesc]				= ISNULL(s.[MoodyFacilityRatingAdjusted],  c.[MoodyFacilityRatingAdjusted])
GO
CREATE VIEW [CLO].[vw_CLOSummary]
	--WITH SCHEMABINDING 
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

	FROM CLO.vw_Position p with(nolock)
	LEFT JOIN traded_cfe tc ON tc.FundId = p.FundId
	RIGHT JOIN [clo].Fund f WITH(NOLOCK) ON f.FundId = p.FundId

	where f.IsActive = 1 

	GROUP BY f.FundCode,f.FundId,f.IsStale,f.[IsPrincipalCashStale]
GO
IF OBJECT_ID(N'CLO.spGetSummaryData') IS NOT NULL
	DROP PROCEDURE CLO.spGetSummaryData
ELSE
	PRINT 'CLO.spGetSummaryData does not exist!'
GO
CREATE PROCEDURE [CLO].[spGetSummaryData]
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
	  ,c.Exposure AS AssetPar
	  , summary.WSOSpread
	  , summary.WSOWARF
	  , summary.WSOMoodyRecovery
	  , summary.WSOWALife
	  , summary.WSODiversity
FROM
CLO.vw_CLOSummary summary WITH(NOLOCK)
LEFT JOIN #fundDiversities d WITH(NOLOCK) ON d.FundId = summary.FundId
LEFT OUTER JOIN @currentDay c ON summary.FundId = c.FundId
WHERE summary.DateId = @dateId

DROP TABLE #ExposureBy_Issuer_MoodyIndustry_Portfolio
DROP TABLE #AVG_Fund_Exposure
DROP TABLE #fundDiversities
DROP TABLE #Diverisity
GO
CREATE VIEW [CLO].[vw_AggregatePosition]
AS
SELECT 
		  [CLO].[GetPrevDayDateId]() PositionDateId
		  ,NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0)	[CLO1NumExposure]
		  ,NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0)	[CLO2NumExposure]
		  ,NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0)	[CLO3NumExposure]
		  ,NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0)	[CLO4NumExposure]
		  ,NULLIF((ISNULL(MAX(pos.[TRS]),0) + ISNULL(MAX(allocation.[TRS]),0)),0)		[TRSNumExposure]

		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0), '#,###')	[CLO1Exposure]
		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0), '#,###')	[CLO2Exposure]
		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0), '#,###')	[CLO3Exposure]
		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0), '#,###')	[CLO4Exposure]
		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[TRS]),0) + ISNULL(MAX(allocation.[TRS]),0)),0), '#,###')		[TRSExposure]

		  ,FORMAT(((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0))/ISNULL(MAX(targetPar.[CLO-1]),1)) , 'p')  [CLO1PctExposure]   
		  ,FORMAT(((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0))/ISNULL(MAX(targetPar.[CLO-2]),1)) , 'p')  [CLO2PctExposure] 
		  ,FORMAT(((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0))/ISNULL(MAX(targetPar.[CLO-3]),1)) , 'p')  [CLO3PctExposure] 
		  ,FORMAT(((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0))/ISNULL(MAX(targetPar.[CLO-4]),1)) , 'p')  [CLO4PctExposure] 
		  ,FORMAT(((ISNULL(MAX(pos.[TRS]),0) + ISNULL(MAX(allocation.[TRS]),0))/ISNULL(MAX(targetPar.[TRS]),1)) , 'p')		  [TRSPctExposure] 

		  ,s.SecurityId
		  ,MAX(s.[SecurityCode]		)		[SecurityCode]
		  ,MAX(s.[SecurityDesc]		)		[SecurityDesc]
		  ,MAX(s.[BBGId]			)		[BBGId]
		  ,MAX(s.[Issuer]			)		[Issuer]
		  ,MAX(s.[IssuerId]			)		[IssuerId]
		  ,MAX(s.[IssuerDesc]			)	[IssuerDesc]
		  ,MAX(LTRIM(s.[Facility])) [Facility]
		  ,MAX(case when s.CallDate <> '1900-01-01' then  CONVERT(VARCHAR(10), s.[CallDate], 101) else null END) [CallDate]
		  ,COALESCE(MAX(country.[CLO-1]),MAX(country.[CLO-2]),MAX(country.[CLO-3]),MAX(country.[CLO-4]))  [CountryDesc]
		  ,MAX(CONVERT(VARCHAR(10), s.[MaturityDate], 101)) [MaturityDate]
		  ,MAX(s.[SnpIndustry])				[SnpIndustry]
		  ,MAX(s.[MoodyIndustry])			[MoodyIndustry]
		  ,COALESCE(MAX(covlite.[CLO-1]),MAX(covlite.[CLO-2]),MAX(covlite.[CLO-3]),MAX(covlite.[CLO-4]))  [IsCovLite]
		  ,MAX(case when (s.[IsFloating] = 1) then 'Floating' else 'Fixed' END) [IsFloating]
		  ,MAX(s.[LienType]						)		[LienType]
		  ,s.[IsOnWatch]					
		  ,MAX(s.WatchObjectTypeId				)		WatchObjectTypeId
		  ,MAX(s.WatchObjectId					)		WatchObjectId					
		  ,MAX(s.[WatchId]						)		[WatchId]						
		  ,MAX(s.[WatchComments]				)		[WatchComments]				
		  ,MAX(s.WatchLastUpdatedOn				)		WatchLastUpdatedOn				
		  ,MAX(s.WatchUser						)		WatchUser						
		  ,MAX(s.[OrigSecurityCode]				)		[OrigSecurityCode]				
		  ,MAX(s.[OrigSecurityDesc]				)		[OrigSecurityDesc]				
		  ,MAX(s.[OrigBBGId]					)		[OrigBBGId]					
		  ,MAX(s.[OrigIssuer]					)		[OrigIssuer]					
		  ,MAX(s.[OrigFacility]					)		[OrigFacility]					
		  ,MAX(s.[OrigCallDate] 				)		[OrigCallDate] 				
		  ,MAX(s.[OrigMaturityDate]				)		[OrigMaturityDate]				
		  ,MAX(s.[OrigSnpIndustry]				)		[OrigSnpIndustry]				
		  ,MAX(s.[OrigMoodyIndustry]			)		[OrigMoodyIndustry]			
		  ,MAX(s.[OrigIsFloating] 				)		[OrigIsFloating] 				
		  ,MAX(s.[OrigLienType]					)		[OrigLienType]					
		  ,MAX(c.[MoodyFacilityRatingAdjusted]	)	AS OrigMoodyFacilityRatingAdjusted
		  ,MAX(m.[MoodyCashFlowRatingAdjusted]	)	AS OrigMoodyCashFlowRatingAdjusted
		  ,CONVERT(varchar, CAST(AVG(m.[Bid]) AS MONEY),1)  [Bid]
		  ,CONVERT(varchar, CAST(AVG(m.[Offer])AS MONEY),1) [Offer]
		  ,MAX(m.[Bid]) BidNum
		  ,MAX(m.[Offer]) OfferNum
		  ,cast(MAX(m.[Spread]) as numeric(20,2)) [Spread]	
		  ,cast(MAX(m.[LiborFloor]) as numeric(20,2))	[LiborFloor]
		  ,MAX(m.[MoodyCashFlowRating]) [MoodyCashFlowRating]
		  ,ISNULL(MAX(s.[MoodyCashFlowRatingAdjusted]), MAX(m.[MoodyCashFlowRatingAdjusted])) [MoodyCashFlowRatingAdjusted]
		  ,MAX(m.[MoodyFacilityRating])		[MoodyFacilityRating]
		  ,MAX(m.[MoodyRecovery]				)		[MoodyRecovery]
		  ,MAX(m.[SnPIssuerRating]				)		[SnPIssuerRating]				
		  ,MAX(m.[SnPIssuerRatingAdjusted]		)		[SnPIssuerRatingAdjusted]		
		  ,MAX(m.[SnPFacilityRating]			)		[SnPFacilityRating]			
		  ,MAX(m.[SnPfacilityRatingAdjusted]	)		[SnPfacilityRatingAdjusted]	
		  ,MAX(m.[SnPRecoveryRating]			)		[SnPRecoveryRating]			
		  ,MAX(m.[MoodyOutlook]					)		[MoodyOutlook]					
		  ,MAX(m.[MoodyWatch]					)		[MoodyWatch]					
		  ,MAX(m.[SnPWatch]						)		[SnPWatch]						
		  ,CONVERT(VARCHAR(10), MAX(m.[NextReportingDate]), 101) [NextReportingDate]
		  ,CONVERT(VARCHAR(10), MAX(m.[FiscalYearEndDate]), 101) [FiscalYearEndDate]
		  
		  
		  ,CAST(AVG(c.[YieldBid]			)as numeric(20,2))			[YieldBid]				
		  ,CAST(AVG(c.[YieldOffer]			)as numeric(20,2))			[YieldOffer]			
		  ,CAST(AVG(c.[YieldMid]			)as numeric(20,2))			[YieldMid]				
		  ,CAST(AVG(c.[CappedYieldBid]		)as numeric(20,2))			[CappedYieldBid]		
		  ,CAST(AVG(c.[CappedYieldOffer]	)as numeric(20,2))			[CappedYieldOffer]		
		  ,CAST(AVG(c.[CappedYieldMid]		)as numeric(20,2))			[CappedYieldMid]		
		  ,CAST(AVG(c.[TargetYieldBid]		)as numeric(20,2))			[TargetYieldBid]		
		  ,CAST(AVG(c.[TargetYieldOffer]	)as numeric(20,2))			[TargetYieldOffer]		
		  ,CAST(AVG(c.[TargetYieldMid]		)as numeric(20,2))			[TargetYieldMid]		
		  ,CAST(AVG(c.[BetterWorseBid]		)as numeric(20,2))			[BetterWorseBid]		
		  ,CAST(AVG(c.[BetterWorseOffer]	)as numeric(20,2))			[BetterWorseOffer]		
		  ,CAST(AVG(c.[BetterWorseMid]		)as numeric(20,2))			[BetterWorseMid]		
		  ,CAST(AVG(c.[TotalCoupon]			)as numeric(20,2))			[TotalCoupon]			
		  ,CAST(AVG(c.[WARF]				)as numeric(20,2))			[WARF]					
		  ,CAST(AVG(c.[WARFRecovery]		)as numeric(20,2))			[WARFRecovery]			
		  ,CAST(AVG(c.[Life]				)as numeric(20,2))			[Life]				
		  ,FORMAT(NULLIF((ISNULL(MAX(c.[TotalPar]),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0), '#,###')	[TotalPar]
		  ,NULLIF((ISNULL(MAX(c.[TotalPar]),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0) [TotalParNum]
		  ,(ISNULL(MAX(c.[TotalPar]),0)) [BODTotalPar]
		  ,ISNULL(MAX(s.[MoodyFacilityRatingAdjusted]),  MAX(c.[MoodyFacilityRatingAdjusted])) [MoodyFacilityRatingAdjusted]
		  ,MAX([a].[CLOAnalyst])			[CLOAnalyst]
		  ,MAX([a].[HFAnalyst])				[HFAnalyst]
		  ,CONVERT(VARCHAR(10), MAX([a].[AsOfDate]), 101) [AsOfDate]
		  ,MAX([a].[CreditScore]		)	[CreditScore]	
		  ,MAX([a].[LiquidityScore]		)	[LiquidityScore]		
		  ,MAX([a].[OneLLeverage]		)	[OneLLeverage]		
		  ,CONVERT(varchar, CAST(MAX([a].[TotalLeverage]) AS money), 1) [TotalLeverage]
		  ,CONVERT(varchar, CAST(MAX([a].[EVMultiple]) AS money), 1) [EVMultiple]
		  ,CONVERT(varchar, CAST(MAX([a].[LTMRevenues]) AS money), 1) [LTMRevenues]
		  ,CONVERT(varchar, CAST(MAX([a].[LTMEBITDA] )AS money), 1) [LTMEBITDA]
		  ,CONVERT(varchar, CAST(MAX([a].[FCF]) AS money), 1) [FCF]
		  ,MAX([a].[Comments])						[Comments]	
		  ,MAX([a].[BusinessDescription])			[BusinessDescription]	
		  ,MAX(a.[AgentBank]			)			[AgentBank]			
		  ,MAX(s.[MaturityDate]) SecurityMaturityDate
		  ,Cast(0 as bit) as IsOnAlert
		  ,CAST(MAX(a.LTMFCF) AS NUMERIC(38,2)) LTMFCF
		  ,CAST(MAX(a.EnterpriseValue) AS NUMERIC(10,2)) EnterpriseValue
		  ,CAST(MAX(a.SeniorLeverage) AS NUMERIC(10,2)) SeniorLeverage
		  

		  ,CONVERT(varchar, CAST(MAX(m.[CostPrice]) AS money), 1) [CostPrice]
		  ,MAX(m.[CostPrice]) [CostPriceNum]
		  ,MAX(pm.[Bid])	PrevDayBidNum
		  ,MAX(pm.[Offer])	PrevDayOfferNum
		  ,CONVERT(varchar, CAST(MAX(pm.[Bid]) AS money), 1) [PrevDayBid]
		  ,CONVERT(varchar, CAST(MAX(pm.[Offer]) AS money), 1) [PrevDayOffer]
		  ,CASE WHEN ISNULL(MAX(m.[Bid]),0) <> 0 AND ISNULL(MAX(pm.[Bid]),0) <> 0 THEN (MAX(m.Bid) - MAX(pm.Bid)) ELSE NULL END PriceMove

		  ,NULL As SearchText

    FROM  [CLO].[vw_Security] s  WITH (NOLOCK)
	      LEFT  JOIN [CLO].vw_SecurityMarket m WITH (NOLOCK) ON s.SecurityId			= m.SecurityId 
		  LEFT  JOIN [CLO].vw_PrevDaySecurityMarket pm WITH (NOLOCK) ON s.SecurityId	= pm.SecurityId 
		  LEFT  JOIN [CLO].vw_CurrentAnalystResearch a WITH (NOLOCK) ON a.IssuerId	= s.IssuerId
		  LEFT  JOIN [CLO].vw_Calculation c WITH (NOLOCK) ON s.SecurityId			= c.SecurityId 
		  LEFT	JOIN [CLO].Rating cfra WITH(NOLOCK) ON cfra.[RatingDesc]			= ISNULL(s.[MoodyCashFlowRatingAdjusted], m.[MoodyCashFlowRatingAdjusted])
		  LEFT	JOIN [CLO].Rating fra WITH(NOLOCK) ON fra.[RatingDesc]				= ISNULL(s.[MoodyFacilityRatingAdjusted],  c.[MoodyFacilityRatingAdjusted])
		  LEFT  JOIN [CLO].[vw_PivotedPositionExposure]		pos  WITH (NOLOCK) on pos.SecurityId = s.SecurityId
		  LEFT  JOIN [CLO].[vw_PivotedPositionCountry]		country  WITH (NOLOCK) on country.SecurityId = s.SecurityId
		  LEFT  JOIN [CLO].[vw_PivotedPositionIsCovLite]	covlite  WITH (NOLOCK) on covlite.SecurityId = s.SecurityId
		  LEFT  JOIN [CLO].[vw_PivotedTradeTotalAllocation]	totalallocation  WITH (NOLOCK) on totalallocation.SecurityId = s.SecurityId
		  LEFT  JOIN [CLO].[vw_PivotedTradeAllocation]	allocation  WITH (NOLOCK) on allocation.SecurityId = s.SecurityId
		  CROSS JOIN [CLO].[vw_PivotedFundTargetPar]	targetpar
	group by S.SecurityId,s.[IsOnWatch]
GO
IF NOT EXISTS (SELECT * FROM CLO.Field WHERE FieldGroupId = 4 AND FieldId > 122)
BEGIN
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, JsonFormatString, DisplayWidth, IsPercentage, SortOrder
				, FieldType, HeaderCellClass, CellClass, CellTemplate, [Hidden], PinnedLeft, IsSecurityOverride, ShowInFilter, FilterOrder)
			SELECT FieldGroupId, FieldName, JsonPropertyName, FieldTitle, JsonFormatString, DisplayWidth, IsPercentage, 1000 + SortOrder AS SortOrder
				, FieldType, HeaderCellClass, CellClass, CellTemplate, [Hidden], PinnedLeft, IsSecurityOverride, ShowInFilter, FilterOrder
			FROM CLO.Field
			WHERE FieldId IN (41, 43, 44, 107, 47)

			UPDATE CLO.Field
			SET FieldName = 'WSOSpread', JsonPropertyName = 'wsoSpread'
			WHERE FieldId = 41

			UPDATE CLO.Field
			SET FieldName = 'WSOWARF', JsonPropertyName = 'wsowarf'
			WHERE FieldId = 43

			UPDATE CLO.Field
			SET FieldName = 'WSOMoodyRecovery', JsonPropertyName = 'wsoMoodyRecovery'
			WHERE FieldId = 44

			UPDATE CLO.Field
			SET FieldName = 'WSODiversity', JsonPropertyName = 'wsoDiversity'
			WHERE FieldId = 47

			UPDATE CLO.Field
			SET FieldName = 'WSOWALife', JsonPropertyName = 'wsowaLife'
			WHERE FieldId = 107

			COMMIT
    END TRY
	BEGIN CATCH
		ROLLBACK
	END CATCH

END
GO
IF NOT EXISTS(SELECT 1 FROM CLO.Field WHERE FieldGroupId = 3 AND FieldName = 'SeniorLeverage')
	UPDATE CLO.Field SET FieldName = 'SeniorLeverage', JsonPropertyName = 'seniorLeverage', FieldTitle = 'SENIOR LEVERAGE' WHERE FieldId = 33
ELSE
	PRINT 'SeniorLeverage exists!'
GO
IF NOT EXISTS(SELECT 1 FROM CLO.Field WHERE FieldGroupId = 3 AND FieldName = 'EnterpriseValue')
	UPDATE CLO.Field SET FieldName = 'EnterpriseValue', JsonPropertyName = 'enterpriseValue', FieldTitle = 'ENTERPRISE VALUE' WHERE FieldId = 35
ELSE
	PRINT 'EnterpriseValue exists!'
GO
IF NOT EXISTS(SELECT 1 FROM CLO.Field WHERE FieldGroupId = 3 AND FieldName = 'LTMFCF')
	UPDATE CLO.Field SET FieldName = 'LTMFCF', JsonPropertyName = 'ltmfcf', FieldTitle = 'LTM FCF' WHERE FieldId = 38
ELSE
	PRINT 'LTMFCF exists!'
GO
UPDATE CLO.Field
SET DisplayWidth = 160, CellTemplate = '<div class="ui-grid-cell-contents"  uib-tooltip="{{row.entity.comments}}" tooltip-append-to-body="true" style="cursor:pointer;"> <span>{{row.entity.comments}} </span></div>'
WHERE FieldId = 39
GO
UPDATE CLO.Field
SET DisplayWidth = 100
WHERE FieldId IN (33, 34, 35)
GO
UPDATE CLO.Field
SET FieldTitle = 'BUSINESS DESCRIPTION'
WHERE FieldId = 26
GO
UPDATE CLO.Field
SET DisplayWidth = 160, CellTemplate = '<div class="ui-grid-cell-contents"  uib-tooltip="{{row.entity.comments}}" tooltip-append-to-body="true" style="cursor:pointer;"> <span>{{row.entity.comments}} </span></div>'
WHERE FieldId = 39
GO
UPDATE CLO.Field
SET DisplayWidth = 100
WHERE FieldId IN (33, 34, 35)
GO
UPDATE CLO.Field
SET FieldTitle = 'BUSINESS DESCRIPTION'
WHERE FieldId = 26
GO
UPDATE CLO.Field
SET Hidden = 1
WHERE FieldId IN (27, 30, 116)
GO
