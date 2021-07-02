CREATE PROCEDURE [CLO].[spCalculateSummaryData]
AS
	declare @dateId int = CLO.GetPrevDayDateId()
	

	delete from CLO.FundCalculation where DateId = @dateId
	
	;WITH traded_cfe AS 
	(
		SELECT FundId,SUM(TradedCash) TradedCash FROM [CLO].[GetActiveTrades]()
		GROUP BY FundID
	)


	SELECT 
	
	--ISNULL(NULLIF(f.IsStale,CAST(0 AS BIT)),f.[IsPrincipalCashStale]) IsStale,
	MAX(p.PositionDateId) DateId,
	(SUM(ISNULL(p.NumExposure,0)) + (ISNULL(MAX(p.PrincipalCash),0) + ISNULL(MAX(tc.TradedCash),0)) - SUM(p.CapitalizedInterestOrig)) Par,
	(SUM(ISNULL(p.BODExposure,0)) + (ISNULL(MAX(p.PrincipalCash),0))  - SUM(p.CapitalizedInterestOrig)) BODPar,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(agg.Spread,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END Spread,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(agg.Spread,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODSpread,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(agg.TotalCoupon,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END  TotalCoupon,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(agg.TotalCoupon,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODTotalCoupon,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL((agg.WARF ),0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END   WARF,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(agg.WARF,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END   BODWARF,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(agg.MoodyRecovery ,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END   MoodyRecovery,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(agg.MoodyRecovery ,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODMoodyRecovery,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 then SUM(ISNULL(agg.BidNum,0) * ISNULL(p.NumExposure,0))/SUM(ISNULL(p.NumExposure,0)) ELSE NULL END  Bid,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 then SUM(ISNULL(agg.BidNum,0) * ISNULL(p.BODExposure,0))/SUM(ISNULL(p.BODExposure,0)) ELSE NULL END  BODBid,
	ISNULL(MAX(p.PrincipalCash),0) + ISNULL(MAX(tc.TradedCash),0)  PrincipalCash,
	ISNULL(MAX(p.PrincipalCash),0)  BODPrincipalCash,
	f.FundId,

	CAST(NULL as numeric(38,10)) Diversity,
	CAST(NULL as numeric(38,10)) BODDiversity,
	
	CASE WHEN ISNULL(MAX(p.EquityPar),0) <> 0 then ((SUM((ISNULL(p.NumExposure,0) * coalesce(p.Bid,p.tradeprice,0))/(CASE WHEN p.Facility='Common' THEN 1 ELSE 100 END)) + (ISNULL(MAX(p.PrincipalCash),0) + ISNULL(MAX(tc.TradedCash),0)) - ISNULL(MAX(p.LiabilityPar),0))/ISNULL(MAX(p.EquityPar),0))*100 ELSE NULL END   CleanNav ,
	CASE WHEN ISNULL(MAX(p.EquityPar),0) <> 0 THEN ((SUM((ISNULL(p.BODExposure,0) * ISNULL(p.Bid,0))/(CASE WHEN p.Facility='Common' THEN 1 ELSE 100 END)) + ISNULL(MAX(p.PrincipalCash),0) - ISNULL(MAX(p.LiabilityPar),0))/ ISNULL(MAX(p.EquityPar),0))*100 ELSE NULL END  BODCleanNav,
	CASE WHEN SUM(ISNULL(p.NumExposure,0)) <> 0 THEN (((SUM(ISNULL(p.MaturityDays,0)*ISNULL(p.NumExposure,0))/SUM(CASE WHEN p.MaturityDays IS NOT NULL then ISNULL(p.NumExposure,0) ELSE 0 end))/365)+ISNULL(MAX(p.WALifeAdjustment),0)) ELSE NULL END WAMaturityDays,
	CASE WHEN SUM(ISNULL(p.BODExposure,0)) <> 0 THEN (((SUM(ISNULL(p.MaturityDays,0)*ISNULL(p.BODExposure,0))/SUM(CASE WHEN p.MaturityDays IS NOT NULL then ISNULL(p.BODExposure,0) ELSE 0 end))/365)+ISNULL(MAX(p.WALifeAdjustment),0)) ELSE NULL END BODWAMaturityDays,
	CAST(NULL as numeric(38,4)) AssetPar,
	CAST(NULL as numeric(38,4)) PriorDayExposure,
	CAST(NULL as numeric(38,10)) PriorDayPrincipalCash,
	AVG(p.NumExposure * (CASE WHEN ISNUMERIC(agg.zSnPAssetRecoveryRating) = 1 THEN CAST(agg.zSnPAssetRecoveryRating AS numeric(32,8))/100 ELSE 0 END))  AS SnpRecovery,
	[ClassEPar] = MAX(f.[ClassEPar]),
	[WALDate] = MAX(f.[WALDate]),
	[ReInvestEndDate] = MAX(f.[ReInvestEndDate]),
	[PricingDate] = MAX(f.[PricingDate]),
	[TotalMgmtFees] = ISNULL(MAX(f.[MgmtFees]),0) + ISNULL(MAX(f.[OperatingExpenses]),0),
	[OperatingExpenses] = MAX(f.[OperatingExpenses]),
	[ClosingDate] = MAX(f.[ClosingDate]),
	[NonCallEndsDate] = MAX(f.[NonCallEndsDate]),
	[FinalMaturity] = MAX(f.[FinalMaturity]),
	[ProjectedEquityDistribtion] = MAX(f.[ProjectedEquityDistribtion]),
	[EquityPar] = MAX(f.[EquityPar]),
	[TargetPar] = MAX(f.[TargetPar]),


	CAST(NULL as numeric(38,10)) BBMVOC,
	CAST(NULL as numeric(38,10)) WALTrigger,
	CAST(NULL as numeric(38,10)) WALCushion,
	CAST(NULL as numeric(38,10)) TimeToReinvest,
	[WalDateAdj] = MAX(f.[WalDateAdj]),
	FundWSOWALife = MAX(f.[WSOWALife]),

	CAST(NULL as numeric(32,4)) [TimeToNonCallEnd],
	CAST(NULL as numeric(32,4)) [WACostOfDebt],
	CAST(NULL as numeric(32,4)) [TotalManagementFees],
	CAST(NULL as numeric(32,4)) [Net],
	CAST(NULL as numeric(32,4)) [TotalDebt],
	CAST(NULL as numeric(32,4)) [EquityLeverage],
	CAST(NULL as numeric(32,4)) [AnnualExcessSpreadToEquity],
	CAST(NULL as numeric(32,4)) [ClassDMVOC],
	CAST(NULL as numeric(32,4)) [ClassEMVOC],
	CAST(NULL as numeric(32,4)) [ClassFMVOC],
	CAST(NULL as numeric(32,4)) [EquityNav],
	Facility = MAX(ISNULL(agg.Facility,'')),
	[B3ToAssetParPct] = CAST(NULL as numeric(32,4)),
	[BMinusToAssetParPct] = CAST(NULL as numeric(32,4))

	into #Summaries
	FROM CLO.vw_Position p with(nolock)
	LEFT JOIN traded_cfe tc ON tc.FundId = p.FundId
	left join clo.AggregatedPosition agg with(nolock) on p.SecurityId = agg.SecurityId and agg.PositionDateId = p.PositionDateId
	RIGHT JOIN [clo].vw_Fund f WITH(NOLOCK) ON f.FundId = p.FundId

	WHERE f.IsActive = 1 

	GROUP BY f.FundId


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
		   BODDiveristy NUMERIC(28,8),
		   Facility VARCHAR(1000),
		   Bid NUMERIC(28,8),
		   B3AssetPar  NUMERIC(28,8),
		   BMinusAssetPar NUMERIC(28,8)
	)

	CREATE TABLE #AVG_Fund_Exposure (FundId VARCHAR(100), AvgExposure NUMERIC(38,10), BODAvgExposure NUMERIC(38,10))

	INSERT INTO #ExposureBy_Issuer_MoodyIndustry_Portfolio
	SELECT p.FundCode , p.FundId, p.Issuer,p.IssuerId, p.MoodyIndustry,
	p.MoodyIndustryId, SUM(ISNULL(P.NumExposure,0)) Exposure  ,SUM(ISNULL(P.BODExposure,0)) BODExposure,  
	NULL AS DiversityUnit, NULL AS Diveristy,NULL AS BODDiversityUnit, NULL AS BODDiveristy,
	Facility = MAX(p.Facility),
	Bid = MAX(p.BidNum),
	B3AssetPar=SUM(CASE WHEN a.MoodyCashFlowRatingAdjusted='B3' THEN p.NumExposure ELSE 0 end),
	BMinusAssetPar=SUM(CASE WHEN a.SnPIssuerRatingAdjusted='B-' THEN p.NumExposure ELSE 0 end)
	FROM CLO.vw_Position p WITH(NOLOCK) 
	JOIN CLO.AggregatedPosition a WITH(NOLOCK) ON p.SecurityId=a.SecurityId AND a.PositionDateId=p.PositionDateId
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

	SELECT FundId, FundCode, SUM(Exposure) AS Exposure, Nav = SUM([CLO].[SafeDivideBy]((ISNULL(Exposure,0) * ISNULL(Bid,0)), CASE WHEN Facility='Common' THEN 1 ELSE 100 END)),
	B3AssetPar=SUM(B3AssetPar),
	BMinusAssetPar=SUM(BMinusAssetPar)
	INTO #assetPar
	FROM #ExposureBy_Issuer_MoodyIndustry_Portfolio
	GROUP BY FundId, FundCode


	SELECT FundId, WAAssetclassSpread = [CLO].[SafeDivideBy](SUM(ISNULL([Notional],0) * COALESCE([OverrideCalcSpread],ISNULL([Spread],0))),SUM(ISNULL([Notional],0))),
	TotalDebt = SUM(ISNULL([Notional],0)),
	ClassDTotalDebt = SUM(ISNULL(CASE WHEN fa.AssetClassId <= 12 THEN fa.Notional ELSE 0 END,0)),
	ClassETotalDebt = SUM(ISNULL(CASE WHEN fa.AssetClassId <= 13 THEN fa.Notional ELSE 0 END,0))
	INTO #WAAssetclassSpreads
	FROM  [CLO].[FundAssetClass] fa
	WHERE fa.AssetClassId <> -999
	GROUP BY [FundId]

	SELECT FundId 
	INTO #ClassFFunds
	FROM 
	[CLO].[FundAssetClass] fa
	WHERE fa.AssetClassId = 14 AND ISNULL(fa.Notional,0) <> 0



	UPDATE summ
	SET 
	summ.BBMVOC =  CASE WHEN summ.ClassEPar IS NOT NULL THEN 100*(((p.Exposure * summ.Bid/100)+summ.PrincipalCash)/(summ.ClassEPar*1.0)) ELSE	NULL END,
	summ.WALTrigger = CASE WHEN summ.WALDate IS NOT NULL THEN ISNULL(summ.[WalDateAdj],0) + (DATEDIFF(DAY,GETDATE(),summ.WALDate)/365.0) ELSE	NULL END,
	summ.WALCushion = CASE WHEN summ.WALDate IS NOT NULL THEN ISNULL(summ.[WalDateAdj],0) + (DATEDIFF(DAY,GETDATE(),summ.WALDate)/365.0) - COALESCE(summ.FundWSOWALife,summ.WAMaturityDays,0) ELSE NULL END,
	summ.TimeToReinvest = CASE WHEN summ.ReInvestEndDate IS NOT NULL THEN DATEDIFF(DAY,GETDATE(),summ.ReInvestEndDate)/365.0 ELSE NULL END,
	summ.TimeToNonCallEnd = CASE WHEN summ.[NonCallEndsDate] IS NOT NULL THEN DATEDIFF(DAY,GETDATE(),summ.[NonCallEndsDate])/365.0 ELSE NULL END,
	summ.WACostOfDebt = ac.WAAssetclassSpread,
	summ.Net = NULLIF((ISNULL(summ.Spread,0) - ISNULL(ac.WAAssetclassSpread,0) - ISNULL(summ.TotalMgmtFees,0)),0),
	summ.TotalDebt = ac.TotalDebt,
	summ.EquityLeverage = [CLO].[SafeDivideBy](summ.TargetPar,summ.EquityPar),
	summ.[AnnualExcessSpreadToEquity] = [CLO].[SafeDivideBy](((summ.TargetPar*summ.Spread*365/360)-(ac.TotalDebt*ac.WAAssetclassSpread*365/360) - (summ.TotalMgmtFees * summ.TargetPar * 365/360)),summ.EquityPar),
	summ.ClassDMVOC = [CLO].[SafeDivideBy](((p.Exposure * summ.Bid/100)+summ.PrincipalCash),ac.ClassDTotalDebt),
	summ.ClassEMVOC = [CLO].[SafeDivideBy](((p.Exposure * summ.Bid/100)+summ.PrincipalCash),ac.ClassETotalDebt),
	summ.ClassFMVOC = CASE WHEN ISNULL(classf.FundId,0) <> 0  THEN [CLO].[SafeDivideBy](((p.Exposure * summ.Bid/100)+summ.PrincipalCash),ac.TotalDebt) ELSE NULL END,
	summ.EquityNav = [CLO].[SafeDivideBy]((p.Nav + summ.PrincipalCash - ac.TotalDebt), summ.EquityPar),
	summ.[B3ToAssetParPct]=NULLIF([CLO].[SafeDivideBy](p.B3AssetPar,p.Exposure)*100,0),
	summ.[BMinusToAssetParPct] = NULLIF([CLO].[SafeDivideBy](p.BMinusAssetPar,p.Exposure)*100,0)
	FROM #Summaries summ
	LEFT OUTER JOIN #assetPar p ON p.FundId = summ.FundId
	LEFT JOIN #WAAssetclassSpreads ac ON ac.FundId = summ.FundId
	LEFT JOIN #ClassFFunds classf ON classf.FundId = summ.FundId


DELETE FROM CLO.FundCalculation WHERE DateId = @dateId

INSERT INTO CLO.FundCalculation
	([DateId],	[FundId], 	[Par] ,	[BODPar] ,	[Spread]  , 	[BODSpread]  ,
	[TotalCoupon]  ,	[BODTotalCoupon]  ,	[WARF]  ,
	[BODWARF]  ,	[MoodyRecovery]  ,	[BODMoodyRecovery]  ,
	[Bid]  ,	[BODBid]  ,	[PrincipalCash]  ,	[BODPrincipalCash]  ,
	[Diversity]  ,	[BODDiversity]  ,	[CleanNav]   ,
	[BODCleanNav]  ,	[WAMaturityDays]  ,
	[BODWAMaturityDays]  ,	[AssetPar]  ,
	[PriorDayExposure]  ,	[PriorDayPrincipalCash]  ,	[MatrixImpliedSpread] ,	SnpRecovery,
	BBMVOC,WALTrigger,WALCushion,TimeToReinvest,
	TimeToNonCallEnd, WACostOfDebt,Net,TotalDebt,EquityLeverage,[AnnualExcessSpreadToEquity],ClassDMVOC,ClassEMVOC,ClassFMVOC,EquityNav,[TotalManagementFees],
	[CreatedOn]	,	[CreatedBy], [B3ToAssetParPct], [BMinusToAssetParPct]	)

SELECT 
	  @dateId,summary.FundId,summary.[Par],summary.[BODPar],summary.[Spread],summary.[BODSpread]
      ,summary.[TotalCoupon]	  ,summary.[BODTotalCoupon]      ,summary.[WARF]
	  ,summary.[BODWARF]      , ROUND(CEILING(summary.[MoodyRecovery]*10)/10,1), ROUND(CEILING(summary.[BODMoodyRecovery]*10)/10,1) 
      ,summary.[Bid]	  ,summary.[BODBid]      ,summary.[PrincipalCash]	  ,summary.[BODPrincipalCash]
      ,d.[Diversity]	  ,d.[BODDiversity]	  ,summary.[CleanNav]
	  ,summary.[BODCleanNav]	  ,summary.[WAMaturityDays]
	  ,summary.[BODWAMaturityDays], p.Exposure AS AssetPar,
	  NULL [PriorDayExposure],NULL, NULL, summary.SnpRecovery,
	  summary.BBMVOC,summary.WALTrigger,summary.WALCushion,summary.TimeToReinvest,
	  TimeToNonCallEnd, WACostOfDebt,Net,TotalDebt,EquityLeverage,[AnnualExcessSpreadToEquity],ClassDMVOC,ClassEMVOC,ClassFMVOC,EquityNav,TotalMgmtFees,
	  GETDATE(),'System' AS CreatedBy, [B3ToAssetParPct], [BMinusToAssetParPct]
FROM
#Summaries summary WITH(NOLOCK)
LEFT JOIN #fundDiversities d WITH(NOLOCK) ON d.FundId = summary.FundId
LEFT OUTER JOIN @currentDay c ON summary.FundId = c.FundId
LEFT OUTER JOIN #assetPar p ON p.FundId = summary.FundId
WHERE summary.DateId = @dateId


DROP TABLE #ExposureBy_Issuer_MoodyIndustry_Portfolio
DROP TABLE #AVG_Fund_Exposure
DROP TABLE #fundDiversities
DROP TABLE #Diverisity
DROP TABLE #assetPar
DROP TABLE #Summaries
DROP TABLE #WAAssetclassSpreads
DROP TABLE #ClassFFunds