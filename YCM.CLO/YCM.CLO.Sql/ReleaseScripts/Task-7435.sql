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
--IF NOT EXISTS(SELECT * FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Security') AND name = 'YorkBbgId')
--	ALTER TABLE CLO.Security ADD YorkBbgId VARCHAR(1000) NULL
--ELSE
--	PRINT 'YorkBbgId EXISTS!'
--GO
--UPDATE CLO.Security
--SET YorkBbgId = BBGId
--GO
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
--ALTER VIEW [CLO].[vw_PivotedPositionExposure]
--as

--SELECT [SecurityId]
--      ,[CLO-1]
--      ,[CLO-2]
--      ,[CLO-3]
--      ,[CLO-4]
--      ,[CLO-5]
--	  ,[WH]
--FROM (
--    SELECT 
--        Exposure,FundCode,SecurityId
--    FROM [CLO].[vw_PositionCountryFund] with(nolock)
--) as s
--PIVOT
--(
--    max(Exposure)
--    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],[WH])
--)AS pvt
--GO
ALTER VIEW [CLO].[vw_PivotedPositionIsCovLite]
as

SELECT [SecurityId]
      ,[CLO-1]
      ,[CLO-2]
      ,[CLO-3]
      ,[CLO-4]
      ,[CLO-5]
	  ,WH
FROM (
    SELECT 
      (case when IsCovLite = 1 then 1 else 0 end) IsCovLite,FundCode,SecurityId
    FROM [CLO].[vw_PositionCountryFund] with(nolock)
) as s
PIVOT
(
    max(IsCovLite)
    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],[WH])
)AS pvt
GO
--ALTER VIEW [CLO].[vw_PivotedTradeAllocation]
--	AS 

--SELECT [SecurityId]
--      ,[CLO-1]
--      ,[CLO-2]
--      ,[CLO-3]
--      ,[CLO-4]
--      ,[CLO-5]
--	  ,[WH]
--FROM (
--    SELECT 
--        Allocation,FundCode,SecurityId
--    FROM [CLO].[GetActiveTrades]()
--) as s
--PIVOT
--(
--    max(Allocation)
--    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],[WH])
--)AS pvt
--GO
--ALTER VIEW [CLO].[vw_PivotedTradeTotalAllocation]
--	AS 

--SELECT [SecurityId]
--      ,[CLO-1]
--      ,[CLO-2]
--      ,[CLO-3]
--      ,[CLO-4]
--      ,[CLO-5]
--	  ,WH
--FROM (
--    SELECT 
--        TotalAllocation,FundCode,SecurityId
--    FROM [CLO].[GetActiveTrades]()
--) as s
--PIVOT
--(
--    max(TotalAllocation)
--    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],[WH])
--)AS pvt
--GO
--ALTER VIEW [CLO].[vw_PivotedTradeAllocation]
--	AS 

--SELECT [SecurityId]
--      ,[CLO-1]
--      ,[CLO-2]
--      ,[CLO-3]
--      ,[CLO-4]
--      ,[CLO-5]
--	  ,WH
--FROM (
--    SELECT 
--        Allocation,FundCode,SecurityId
--    FROM [CLO].[GetActiveTrades]()
--) as s
--PIVOT
--(
--    max(Allocation)
--    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],WH)
--)AS pvt
--GO
--ALTER VIEW [CLO].[vw_PivotedFundTargetPar]
--	AS 

--SELECT [CLO-1]
--      ,[CLO-2]
--      ,[CLO-3]
--      ,[CLO-4]
--      ,[CLO-5]
--	  ,WH
--FROM (
--    SELECT 
--        TargetPar,FundCode
--    FROM [CLO].[Fund] with(nolock)
--) as s
--PIVOT
--(
--    max(TargetPar)
--    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],WH)
--)AS pvt
--GO

--ALTER VIEW [CLO].[vw_PivotedPositionExposure]
--as

--SELECT [SecurityId]
--      ,[CLO-1]
--      ,[CLO-2]
--      ,[CLO-3]
--      ,[CLO-4]
--      ,[CLO-5]
--	  ,[WH]
--FROM (
--    SELECT 
--        Exposure,FundCode,SecurityId
--    FROM [CLO].[vw_PositionCountryFund] with(nolock)
--) as s
--PIVOT
--(
--    max(Exposure)
--    FOR [FundCode] IN ([CLO-1],[CLO-2],[CLO-3],[CLO-4],[CLO-5],[WH])
--)AS pvt
--GO
--ALTER VIEW [CLO].[vw_AggregatePosition]
--AS
--SELECT 
--		  [CLO].[GetPrevDayDateId]() PositionDateId
--		  ,NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0)	[CLO1NumExposure]
--		  ,NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0)	[CLO2NumExposure]
--		  ,NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0)	[CLO3NumExposure]
--		  ,NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0)	[CLO4NumExposure]
--		  ,NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0)	[CLO5NumExposure]
--		  ,NULLIF((ISNULL(MAX(pos.[WH]),0) + ISNULL(MAX(allocation.WH),0)),0)		[TRSNumExposure]

--		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0), '#,###')	[CLO1Exposure]
--		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0), '#,###')	[CLO2Exposure]
--		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0), '#,###')	[CLO3Exposure]
--		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0), '#,###')	[CLO4Exposure]
--		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0), '#,###')	[CLO5Exposure]
--		  ,FORMAT(NULLIF((ISNULL(MAX(pos.[WH]),0) + ISNULL(MAX(allocation.WH),0)),0), '#,###')		[TRSExposure]

--		  ,FORMAT(((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0))/ISNULL(MAX(targetPar.[CLO-1]),1)) , 'p')  [CLO1PctExposure]   
--		  ,FORMAT(((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0))/ISNULL(MAX(targetPar.[CLO-2]),1)) , 'p')  [CLO2PctExposure] 
--		  ,FORMAT(((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0))/ISNULL(MAX(targetPar.[CLO-3]),1)) , 'p')  [CLO3PctExposure] 
--		  ,FORMAT(((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0))/ISNULL(MAX(targetPar.[CLO-4]),1)) , 'p')  [CLO4PctExposure] 
--		  ,FORMAT(((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0))/ISNULL(MAX(targetPar.[CLO-5]),1)) , 'p')  [CLO5PctExposure] 
--		  ,FORMAT(((ISNULL(MAX(pos.[WH]),0) + ISNULL(MAX(allocation.WH),0))/ISNULL(MAX(targetPar.WH),1)) , 'p')		  [TRSPctExposure] 

--		  ,s.SecurityId
--		  ,MAX(s.[SecurityCode]		)		[SecurityCode]
--		  ,MAX(s.[SecurityDesc]		)		[SecurityDesc]
--		  ,MAX(s.[BBGId]			)		[BBGId]
--		  ,MAX(s.[Issuer]			)		[Issuer]
--		  ,MAX(s.[IssuerId]			)		[IssuerId]
--		  ,MAX(s.[IssuerDesc]			)	[IssuerDesc]
--		  ,MAX(LTRIM(s.[Facility])) [Facility]
--		  ,MAX(case when s.CallDate <> '1900-01-01' then  CONVERT(VARCHAR(10), s.[CallDate], 101) else null END) [CallDate]
--		  ,COALESCE(MAX(country.[CLO-1]),MAX(country.[CLO-2]),MAX(country.[CLO-3]),MAX(country.[CLO-4]))  [CountryDesc]
--		  ,MAX(CONVERT(VARCHAR(10), s.[MaturityDate], 101)) [MaturityDate]
--		  ,MAX(s.[SnpIndustry])				[SnpIndustry]
--		  ,MAX(s.[MoodyIndustry])			[MoodyIndustry]
--		  ,COALESCE(MAX(covlite.[CLO-1]),MAX(covlite.[CLO-2]),MAX(covlite.[CLO-3]),MAX(covlite.[CLO-4]))  [IsCovLite]
--		  ,MAX(case when (s.[IsFloating] = 1) then 'Floating' else 'Fixed' END) [IsFloating]
--		  ,MAX(s.[LienType]						)		[LienType]
--		  ,s.[IsOnWatch]					
--		  ,MAX(s.WatchObjectTypeId				)		WatchObjectTypeId
--		  ,MAX(s.WatchObjectId					)		WatchObjectId					
--		  ,MAX(s.[WatchId]						)		[WatchId]						
--		  ,MAX(s.[WatchComments]				)		[WatchComments]				
--		  ,MAX(s.WatchLastUpdatedOn				)		WatchLastUpdatedOn				
--		  ,MAX(s.WatchUser						)		WatchUser						
--		  ,MAX(s.[OrigSecurityCode]				)		[OrigSecurityCode]				
--		  ,MAX(s.[OrigSecurityDesc]				)		[OrigSecurityDesc]				
--		  ,MAX(s.[OrigBBGId]					)		[OrigBBGId]					
--		  ,MAX(s.[OrigIssuer]					)		[OrigIssuer]					
--		  ,MAX(s.[OrigFacility]					)		[OrigFacility]					
--		  ,MAX(s.[OrigCallDate] 				)		[OrigCallDate] 				
--		  ,MAX(s.[OrigMaturityDate]				)		[OrigMaturityDate]				
--		  ,MAX(s.[OrigSnpIndustry]				)		[OrigSnpIndustry]				
--		  ,MAX(s.[OrigMoodyIndustry]			)		[OrigMoodyIndustry]			
--		  ,MAX(s.[OrigIsFloating] 				)		[OrigIsFloating] 				
--		  ,MAX(s.[OrigLienType]					)		[OrigLienType]					
--		  ,MAX(c.[MoodyFacilityRatingAdjusted]	)	AS OrigMoodyFacilityRatingAdjusted
--		  ,MAX(m.[MoodyCashFlowRatingAdjusted]	)	AS OrigMoodyCashFlowRatingAdjusted
--		  ,CONVERT(varchar, CAST(AVG(m.[Bid]) AS MONEY),1)  [Bid]
--		  ,CONVERT(varchar, CAST(AVG(m.[Offer])AS MONEY),1) [Offer]
--		  ,MAX(m.[Bid]) BidNum
--		  ,MAX(m.[Offer]) OfferNum
--		  ,cast(MAX(m.[Spread]) as numeric(20,2)) [Spread]	
--		  ,cast(MAX(m.[LiborFloor]) as numeric(20,2))	[LiborFloor]
--		  ,MAX(m.[MoodyCashFlowRating]) [MoodyCashFlowRating]
--		  ,ISNULL(MAX(s.[MoodyCashFlowRatingAdjusted]), MAX(m.[MoodyCashFlowRatingAdjusted])) [MoodyCashFlowRatingAdjusted]
--		  ,MAX(m.[MoodyFacilityRating])		[MoodyFacilityRating]
--		  ,MAX(m.[MoodyRecovery]				)		[MoodyRecovery]
--		  ,MAX(m.[SnPIssuerRating]				)		[SnPIssuerRating]				
--		  ,MAX(m.[SnPIssuerRatingAdjusted]		)		[SnPIssuerRatingAdjusted]		
--		  ,MAX(m.[SnPFacilityRating]			)		[SnPFacilityRating]			
--		  ,MAX(m.[SnPfacilityRatingAdjusted]	)		[SnPfacilityRatingAdjusted]	
--		  ,MAX(m.[SnPRecoveryRating]			)		[SnPRecoveryRating]			
--		  ,MAX(m.[MoodyOutlook]					)		[MoodyOutlook]					
--		  ,MAX(m.[MoodyWatch]					)		[MoodyWatch]					
--		  ,MAX(m.[SnPWatch]						)		[SnPWatch]						
--		  ,CONVERT(VARCHAR(10), MAX(m.[NextReportingDate]), 101) [NextReportingDate]
--		  ,CONVERT(VARCHAR(10), MAX(m.[FiscalYearEndDate]), 101) [FiscalYearEndDate]
		  
		  
--		  ,CAST(AVG(c.[YieldBid]			)as numeric(20,2))			[YieldBid]				
--		  ,CAST(AVG(c.[YieldOffer]			)as numeric(20,2))			[YieldOffer]			
--		  ,CAST(AVG(c.[YieldMid]			)as numeric(20,2))			[YieldMid]				
--		  ,CAST(AVG(c.[CappedYieldBid]		)as numeric(20,2))			[CappedYieldBid]		
--		  ,CAST(AVG(c.[CappedYieldOffer]	)as numeric(20,2))			[CappedYieldOffer]		
--		  ,CAST(AVG(c.[CappedYieldMid]		)as numeric(20,2))			[CappedYieldMid]		
--		  ,CAST(AVG(c.[TargetYieldBid]		)as numeric(20,2))			[TargetYieldBid]		
--		  ,CAST(AVG(c.[TargetYieldOffer]	)as numeric(20,2))			[TargetYieldOffer]		
--		  ,CAST(AVG(c.[TargetYieldMid]		)as numeric(20,2))			[TargetYieldMid]		
--		  ,CAST(AVG(c.[BetterWorseBid]		)as numeric(20,2))			[BetterWorseBid]		
--		  ,CAST(AVG(c.[BetterWorseOffer]	)as numeric(20,2))			[BetterWorseOffer]		
--		  ,CAST(AVG(c.[BetterWorseMid]		)as numeric(20,2))			[BetterWorseMid]		
--		  ,CAST(AVG(c.[TotalCoupon]			)as numeric(20,2))			[TotalCoupon]			
--		  ,CAST(AVG(c.[WARF]				)as numeric(20,2))			[WARF]					
--		  ,CAST(AVG(c.[WARFRecovery]		)as numeric(20,2))			[WARFRecovery]			
--		  ,CAST(AVG(c.[Life]				)as numeric(20,2))			[Life]				
--		  ,FORMAT(NULLIF((ISNULL(MAX(c.[TotalPar]),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0), '#,###')	[TotalPar]
--		  ,NULLIF((ISNULL(MAX(c.[TotalPar]),0) + ISNULL(MAX(totalallocation.[CLO-1]),0)),0) [TotalParNum]
--		  ,(ISNULL(MAX(c.[TotalPar]),0)) [BODTotalPar]
--		  ,ISNULL(MAX(s.[MoodyFacilityRatingAdjusted]),  MAX(c.[MoodyFacilityRatingAdjusted])) [MoodyFacilityRatingAdjusted]
--		  ,MAX([a].[CLOAnalyst])			[CLOAnalyst]
--		  ,MAX([a].[HFAnalyst])				[HFAnalyst]
--		  ,CONVERT(VARCHAR(10), MAX([a].[AsOfDate]), 101) [AsOfDate]
--		  ,MAX([a].[CreditScore]		)	[CreditScore]	
--		  ,MAX([a].[LiquidityScore]		)	[LiquidityScore]		
--		  ,MAX([a].[OneLLeverage]		)	[OneLLeverage]		
--		  ,CONVERT(varchar, CAST(MAX([a].[TotalLeverage]) AS money), 1) [TotalLeverage]
--		  ,CONVERT(varchar, CAST(MAX([a].[EVMultiple]) AS money), 1) [EVMultiple]
--		  ,CONVERT(varchar, CAST(MAX([a].[LTMRevenues]) AS money), 1) [LTMRevenues]
--		  ,CONVERT(varchar, CAST(MAX([a].[LTMEBITDA] )AS money), 1) [LTMEBITDA]
--		  ,CONVERT(varchar, CAST(MAX([a].[FCF]) AS money), 1) [FCF]
--		  ,MAX([a].[Comments])						[Comments]	
--		  ,MAX([a].[BusinessDescription])			[BusinessDescription]	
--		  ,MAX(a.[AgentBank]			)			[AgentBank]			
--		  ,MAX(s.[MaturityDate]) SecurityMaturityDate
--		  ,Cast(0 as bit) as IsOnAlert
--		  ,CAST(MAX(a.LTMFCF) AS NUMERIC(38,2)) LTMFCF
--		  ,CAST(MAX(a.EnterpriseValue) AS NUMERIC(10,2)) EnterpriseValue
--		  ,CAST(MAX(a.SeniorLeverage) AS NUMERIC(10,2)) SeniorLeverage
		  

--		  ,CONVERT(varchar, CAST(MAX(m.[CostPrice]) AS money), 1) [CostPrice]
--		  ,MAX(m.[CostPrice]) [CostPriceNum]
--		  ,MAX(pm.[Bid])	PrevDayBidNum
--		  ,MAX(pm.[Offer])	PrevDayOfferNum
--		  ,CONVERT(varchar, CAST(MAX(pm.[Bid]) AS money), 1) [PrevDayBid]
--		  ,CONVERT(varchar, CAST(MAX(pm.[Offer]) AS money), 1) [PrevDayOffer]
--		  ,CASE WHEN ISNULL(MAX(m.[Bid]),0) <> 0 AND ISNULL(MAX(pm.[Bid]),0) <> 0 THEN (MAX(m.Bid) - MAX(pm.Bid)) ELSE NULL END PriceMove

--		  ,NULL As SearchText

--    FROM  [CLO].[vw_Security] s  WITH (NOLOCK)
--	      LEFT  JOIN [CLO].vw_SecurityMarket m WITH (NOLOCK) ON s.SecurityId			= m.SecurityId 
--		  LEFT  JOIN [CLO].vw_PrevDaySecurityMarket pm WITH (NOLOCK) ON s.SecurityId	= pm.SecurityId 
--		  LEFT  JOIN [CLO].vw_CurrentAnalystResearch a WITH (NOLOCK) ON a.IssuerId	= s.IssuerId
--		  LEFT  JOIN [CLO].vw_Calculation c WITH (NOLOCK) ON s.SecurityId			= c.SecurityId 
--		  LEFT	JOIN [CLO].Rating cfra WITH(NOLOCK) ON cfra.[RatingDesc]			= ISNULL(s.[MoodyCashFlowRatingAdjusted], m.[MoodyCashFlowRatingAdjusted])
--		  LEFT	JOIN [CLO].Rating fra WITH(NOLOCK) ON fra.[RatingDesc]				= ISNULL(s.[MoodyFacilityRatingAdjusted],  c.[MoodyFacilityRatingAdjusted])
--		  LEFT  JOIN [CLO].[vw_PivotedPositionExposure]		pos  WITH (NOLOCK) on pos.SecurityId = s.SecurityId
--		  LEFT  JOIN [CLO].[vw_PivotedPositionCountry]		country  WITH (NOLOCK) on country.SecurityId = s.SecurityId
--		  LEFT  JOIN [CLO].[vw_PivotedPositionIsCovLite]	covlite  WITH (NOLOCK) on covlite.SecurityId = s.SecurityId
--		  LEFT  JOIN [CLO].[vw_PivotedTradeTotalAllocation]	totalallocation  WITH (NOLOCK) on totalallocation.SecurityId = s.SecurityId
--		  LEFT  JOIN [CLO].[vw_PivotedTradeAllocation]	allocation  WITH (NOLOCK) on allocation.SecurityId = s.SecurityId
--		  CROSS JOIN [CLO].[vw_PivotedFundTargetPar]	targetpar
--	group by S.SecurityId,s.[IsOnWatch]
--GO
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
AND summary.IsWarehouse = 0

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
--ALTER VIEW [CLO].[vw_SecurityFund]
--AS 
--SELECT
--	   s.[SecurityId]
--      ,s.[SecurityCode]
--      ,s.[SecurityDesc]
--      ,s.[BBGId]
--      ,s.[Issuer]
--	  ,s.[IssuerDesc]
--      ,s.[Facility]
--      ,s.[CallDate]
--      ,s.[MaturityDate]
--      ,s.[SnpIndustry]
--      ,s.[MoodyIndustry]
--      ,s.[IsFloating]
--      ,s.[LienType]
--      ,s.[IssuerId]
--      ,s.[WatchId]
--      ,s.[MoodyIndustryId]
--      ,s.[IsOnWatch]
--      ,s.[WatchObjectTypeId]
--      ,s.[WatchObjectId]
--      ,s.[WatchComments]
--      ,s.[WatchLastUpdatedOn]
--      ,s.[WatchUser]
--      ,s.[OrigSecurityCode]
--      ,s.[OrigSecurityDesc]
--      ,s.[OrigBBGId]
--      ,s.[OrigIssuer]
--      ,s.[GICSIndustry]
--      ,s.[OrigFacility]
--      ,s.[OrigCallDate]
--      ,s.[OrigMaturityDate]
--      ,s.[OrigSnpIndustry]
--      ,s.[OrigMoodyIndustry]
--      ,s.[OrigIsFloating]
--      ,s.[OrigLienType]
--      ,s.[OrigMoodyFacilityRatingAdjusted]
--      ,s.[OrigMoodyCashFlowRatingAdjusted]
--      ,s.[SecurityLastUpdatedOn]
--      ,s.[SecurityLastUpdatedBy]
--      ,s.[SecurityCreatedOn]
--      ,s.[SecurityCreatedBy]
--      ,s.[SourceId]
--      ,s.[MoodyFacilityRatingAdjusted]
--      ,s.[MoodyCashFlowRatingAdjusted]
--      ,s.[HasPositions]
--	  ,f.FundId,
--f.FundCode,
--f.FundDesc,
--f.PrincipalCash,
--f.EquityPar,
--f.LiabilityPar ,
--f.TargetPar,
--f.WALifeAdjustment,
--ISNULL(NULLIF(f.IsStale,CAST(0 AS BIT)),f.[IsPrincipalCashStale]) IsStale,
--ta.Allocation Allocation,
--ta.HasBuy HasBuy,
--ta.TotalAllocation  TotalAllocation,
--ta.TradePrice
--FROM 
--[CLO].[vw_security] s WITH(NOLOCK)
--CROSS JOIN [CLO].Fund f WITH(NOLOCK)
--LEFT JOIN  [CLO].[GetActiveTrades]() ta ON s.SecurityId = ta.SecurityId AND f.FundId = ta.FundId
--where f.IsActive = 1 OR f.FundId = 5
--GO
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
