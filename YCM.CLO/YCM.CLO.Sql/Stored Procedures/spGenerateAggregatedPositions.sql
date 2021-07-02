CREATE PROCEDURE [CLO].[spGenerateAggregatedPositions]

AS

delete from CLO.AggregatedPosition where PositionDateId = CLO.GetPrevDayDateId()

SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9]
into #PivotedMatrixImpliedSpread
FROM 
	(SELECT [MatrixImpliedSpread] = MAX(c.[MatrixImpliedSpread]), FundCode = f.ParentFundCode, c.SecurityId
			FROM CLO.vw_Calculation c WITH (NOLOCK)
			JOIN CLO.Fund f WITH(NOLOCK) ON f.FundId = c.FundId
			WHERE c.DateId = CLO.GetPrevDayDateId()
			GROUP BY f.ParentFundCode, c.SecurityId
	) AS s PIVOT
(MAX([MatrixImpliedSpread]) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9]) )AS pvt


SELECT SecurityId, [CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9]
into #PivotedMatrixWarfRecovery
FROM 
	(SELECT MatrixWARFRecovery = MAX(c.MatrixWARFRecovery), FundCode = f.ParentFundCode, c.SecurityId
			FROM CLO.vw_Calculation c WITH (NOLOCK)
			JOIN CLO.Fund f WITH(NOLOCK) ON f.FundId = c.FundId
			WHERE c.DateId = CLO.GetPrevDayDateId()
			GROUP BY f.ParentFundCode, c.SecurityId
	) AS s PIVOT
(MAX(MatrixWARFRecovery) FOR FundCode IN ([CLO-1], [CLO-2], [CLO-3], [CLO-4], [CLO-5], [CLO-6], [CLO-7], [CLO-8], [CLO-9]) )AS pvt

		;WITH wsodata AS
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
			SELECT AssetLoanXIDAssetIDName AS SecurityCode, F.FundId, SUM(C.ContractGlobalAmount) AS GlobalAmount FROM CLO.LoanContract C 
			INNER JOIN Yoda.CLO.Fund F
			ON F.PortfolioName = C.PortfolioName
			WHERE C.AsOfDate = DataMarts.dbo.GetDateFromDateId(Yoda.CLO.GetPrevDayDateId())
			and (f.IsActive = 1 OR f.IsWarehouse = 1)
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

INSERT INTO [CLO].[AggregatedPosition]
           ([PositionDateId]
           ,[CLO1NumExposure]
           ,[CLO2NumExposure]
           ,[CLO3NumExposure]
           ,[CLO4NumExposure]
           ,[CLO5NumExposure]
           ,[CLO6NumExposure]
           ,[CLO7NumExposure]
		   ,[CLO8NumExposure]
		   ,[CLO9NumExposure]

           ,[CLO1Exposure]
           ,[CLO2Exposure]
           ,[CLO3Exposure]
           ,[CLO4Exposure]
           ,[CLO5Exposure]
           ,[CLO6Exposure]
           ,[CLO7Exposure]
		   ,[CLO8Exposure]
		   ,[CLO9Exposure]

           ,[CLO1PctExposure]
           ,[CLO2PctExposure]
           ,[CLO3PctExposure]
           ,[CLO4PctExposure]
           ,[CLO5PctExposure]
           ,[CLO6PctExposure]
           ,[CLO7PctExposure]
		   ,[CLO8PctExposure]
		   ,[CLO9PctExposure]

           ,[CLO1TargetPar]
           ,[CLO2TargetPar]
           ,[CLO3TargetPar]
           ,[CLO4TargetPar]
           ,[CLO5TargetPar]
           ,[CLO6TargetPar]
           ,[CLO7TargetPar]
		   ,[CLO8TargetPar]
		   ,[CLO9TargetPar]


           ,[SecurityId]
           ,[SecurityCode]
           ,[SecurityDesc]
           ,[BBGId]
           ,[Issuer]
           ,[IssuerId]
           ,[IssuerDesc]
           ,[Facility]
           ,[CallDate]
           ,[CountryDesc]
           ,[MaturityDate]
           ,[SnpIndustry]
           ,[MoodyIndustry]

           ,[IsCovLite]
           ,[IsFloating]
           ,[LienType]
           ,[IsOnWatch]
           ,[WatchObjectTypeId]
           ,[WatchObjectId]
           ,[WatchId]
           ,[WatchComments]
           ,[WatchLastUpdatedOn]
           ,[WatchUser]

           ,[SellCandidateObjectTypeId]
           ,[SellCandidateObjectId]
           ,[SellCandidateId]
           ,[SellCandidateComments]
           ,[SellCandidateLastUpdatedOn]
           ,[SellCandidateUser]
           ,[OrigSecurityCode]
           ,[OrigSecurityDesc]
           ,[OrigBBGId]
           ,[OrigIssuer]

           ,[OrigFacility]
           ,[OrigCallDate]
           ,[OrigMaturityDate]
           ,[OrigSnpIndustry]
           ,[OrigMoodyIndustry]
           ,[OrigIsFloating]
           ,[OrigLienType]
           ,[OrigMoodyFacilityRatingAdjusted]
           ,[OrigMoodyCashFlowRatingAdjusted]
           ,[Bid]

           ,[Offer]
           ,[BidNum]
           ,[OfferNum]
           ,[Spread]
           ,[LiborFloor]
           ,[MoodyCashFlowRating]
           ,[MoodyCashFlowRatingAdjusted]
           ,[MoodyFacilityRating]
           ,[MoodyRecovery]
           ,[SnPIssuerRating]
           ,[SnPIssuerRatingAdjusted]
           ,[SnPFacilityRating]
           ,[SnPfacilityRatingAdjusted]
           ,[SnPRecoveryRating]
           ,[MoodyOutlook]

           ,[MoodyWatch]
           ,[SnPWatch]
           ,[NextReportingDate]
           ,[FiscalYearEndDate]
           ,[YieldBid]
           ,[YieldOffer]
           ,[YieldMid]
           ,[CappedYieldBid]
           ,[CappedYieldOffer]
           ,[CappedYieldMid]
           ,[TargetYieldBid]
           ,[TargetYieldOffer]
           ,[TargetYieldMid]
           ,[BetterWorseBid]
           ,[BetterWorseOffer]

           ,[BetterWorseMid]
           ,[TotalCoupon]
           ,[WARF]
           ,[WARFRecovery]
           ,[Life]
           ,[TotalPar]
           ,[TotalParNum]
           ,[BODTotalPar]
           ,[MoodyFacilityRatingAdjusted]
           ,[CLOAnalyst]
           ,[HFAnalyst]
           ,[AsOfDate]
           ,[CreditScore]
           ,[LiquidityScore]
           ,[OneLLeverage]

           ,[TotalLeverage]
           ,[EVMultiple]
           ,[LTMRevenues]
           ,[LTMEBITDA]
           ,[FCF]
           ,[Comments]
           ,[BusinessDescription]
           ,[AgentBank]
           ,[SecurityMaturityDate]
           ,[IsOnAlert]
           ,[LTMFCF]
           ,[EnterpriseValue]
           ,[SeniorLeverage]
           ,[CostPrice]
           ,[CostPriceNum]

           ,[PrevDayBidNum]
           ,[PrevDayOfferNum]
           ,[PrevDayBid]
           ,[PrevDayOffer]
           ,[PriceMove]
           ,[SearchText]
           ,[LienTypeId]
           ,[ScoreDescription]
           ,[GlobalAmount]
		   ,[IsPrivate]
		   ,[Sponsor]

           ,[CLO1MatrixImpliedSpread]
           ,[CLO2MatrixImpliedSpread]
           ,[CLO3MatrixImpliedSpread]
           ,[CLO4MatrixImpliedSpread]
           ,[CLO5MatrixImpliedSpread]
           ,[CLO6MatrixImpliedSpread]
		   ,[CLO7MatrixImpliedSpread]
           ,[CLO8MatrixImpliedSpread]
           ,[CLO9MatrixImpliedSpread]

           ,[CLO1DifferentialImpliedSpread]
           ,[CLO2DifferentialImpliedSpread]
           ,[CLO3DifferentialImpliedSpread]
           ,[CLO4DifferentialImpliedSpread]
           ,[CLO5DifferentialImpliedSpread]
           ,[CLO6DifferentialImpliedSpread]
           ,[CLO7DifferentialImpliedSpread]
		   ,[CLO8DifferentialImpliedSpread]
		   ,[CLO9DifferentialImpliedSpread]


		   ,[CLO1MatrixWarfRecovery]
		   ,[CLO2MatrixWarfRecovery]
		   ,[CLO3MatrixWarfRecovery]
		   ,[CLO4MatrixWarfRecovery]
		   ,[CLO5MatrixWarfRecovery]
		   ,[CLO6MatrixWarfRecovery]
		   ,[CLO7MatrixWarfRecovery]
		   ,[CLO8MatrixWarfRecovery]
		   ,[CLO9MatrixWarfRecovery]

		   
		   ,[zSnPAssetRecoveryRating]
		   ,[SnpWarf]
		   ,[SnpLgd]
		   ,[MoodysLgd]
		   ,[YieldAvgLgd]
		   ,[SnpAAARecovery]
		   ,[SnpCreditWatch]
		   ,[LiborBaseRate]

		   ,[LiborCategory]
		   ,[LiborTransitionNote]

		   ,[IsInDefault]
		   ,[DefaultDate]


		   ,[Fund-4 MoodyRecovery]
		   ,[Fund-10 MoodyRecovery]
		   ,[Fund-2 MoodyRecovery]
		   ,[Fund-1 MoodyRecovery]
		   ,[Fund-3 MoodyRecovery]
		   ,[Fund-7 MoodyRecovery]
		   ,[Fund-8 MoodyRecovery]
		   ,[Fund-9 MoodyRecovery]

		   )

SELECT 
		  CLO.GetPrevDayDateId() PositionDateId
		  , NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0)	CLO1NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0)	CLO2NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0)	CLO3NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0)	CLO4NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0)	CLO5NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-6]),0) + ISNULL(MAX(allocation.[CLO-6]),0)),0)	CLO6NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-7]),0) + ISNULL(MAX(allocation.[CLO-7]),0)),0)	CLO7NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-8]),0) + ISNULL(MAX(allocation.[CLO-8]),0)),0)	CLO8NumExposure
		  , NULLIF((ISNULL(MAX(pos.[CLO-9]),0) + ISNULL(MAX(allocation.[CLO-9]),0)),0)	CLO9NumExposure

		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0)),0), '#,###')	CLO1Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0)),0), '#,###')	CLO2Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0)),0), '#,###')	CLO3Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0)),0), '#,###')	CLO4Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0)),0), '#,###')	CLO5Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-6]),0) + ISNULL(MAX(allocation.[CLO-6]),0)),0), '#,###')	CLO6Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-7]),0) + ISNULL(MAX(allocation.[CLO-7]),0)),0), '#,###')	CLO7Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-8]),0) + ISNULL(MAX(allocation.[CLO-8]),0)),0), '#,###')	CLO8Exposure
		  , FORMAT(NULLIF((ISNULL(MAX(pos.[CLO-9]),0) + ISNULL(MAX(allocation.[CLO-9]),0)),0), '#,###')	CLO9Exposure

		  , FORMAT(((ISNULL(MAX(pos.[CLO-1]),0) + ISNULL(MAX(allocation.[CLO-1]),0))/ISNULL(MAX(targetPar.[CLO-1]),1)) , 'p')  CLO1PctExposure   
		  , FORMAT(((ISNULL(MAX(pos.[CLO-2]),0) + ISNULL(MAX(allocation.[CLO-2]),0))/ISNULL(MAX(targetPar.[CLO-2]),1)) , 'p')  CLO2PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-3]),0) + ISNULL(MAX(allocation.[CLO-3]),0))/ISNULL(MAX(targetPar.[CLO-3]),1)) , 'p')  CLO3PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-4]),0) + ISNULL(MAX(allocation.[CLO-4]),0))/ISNULL(MAX(targetPar.[CLO-4]),1)) , 'p')  CLO4PctExposure 
		  , FORMAT(((ISNULL(MAX(pos.[CLO-5]),0) + ISNULL(MAX(allocation.[CLO-5]),0))/ISNULL(MAX(targetPar.[CLO-5]),1)) , 'p')  CLO5PctExposure
		  , FORMAT(((ISNULL(MAX(pos.[CLO-6]),0) + ISNULL(MAX(allocation.[CLO-6]),0))/ISNULL(MAX(targetPar.[CLO-6]),1)) , 'p')  CLO6PctExposure
		  , FORMAT(((ISNULL(MAX(pos.[CLO-7]),0) + ISNULL(MAX(allocation.[CLO-7]),0))/ISNULL(MAX(targetPar.[CLO-7]),1)) , 'p')  CLO7PctExposure
		  , FORMAT(((ISNULL(MAX(pos.[CLO-8]),0) + ISNULL(MAX(allocation.[CLO-8]),0))/ISNULL(MAX(targetPar.[CLO-8]),1)) , 'p')  CLO8PctExposure
		  , FORMAT(((ISNULL(MAX(pos.[CLO-9]),0) + ISNULL(MAX(allocation.[CLO-9]),0))/ISNULL(MAX(targetPar.[CLO-9]),1)) , 'p')  CLO9PctExposure

		  , MAX(targetPar.[CLO-1]) CLO1TargetPar
		  , MAX(targetPar.[CLO-2]) CLO2TargetPar
		  , MAX(targetPar.[CLO-3]) CLO3TargetPar
		  , MAX(targetPar.[CLO-4]) CLO4TargetPar
		  , MAX(targetPar.[CLO-5]) CLO5TargetPar
		  , MAX(targetPar.[CLO-6]) CLO6TargetPar
		  , MAX(targetPar.[CLO-7]) CLO7TargetPar
		  , MAX(targetPar.[CLO-8]) CLO8TargetPar
		  , MAX(targetPar.[CLO-9]) CLO9TargetPar

		  , s.SecurityId
		  , MAX(s.SecurityCode		)		SecurityCode
		  , MAX(s.SecurityDesc		)		SecurityDesc
		  , MAX(s.BBGId			)		BBGId
		  , MAX(s.Issuer			)		Issuer
		  , MAX(s.IssuerId			)		IssuerId
		  , MAX(s.IssuerDesc			)	IssuerDesc
		  , MAX(LTRIM(s.Facility)) Facility
		  , MAX(CASE WHEN s.CallDate <> '1900-01-01' THEN  CONVERT(VARCHAR(10), s.CallDate, 101) ELSE NULL END) CallDate
		  , COALESCE(MAX(country.[CLO-1]), MAX(country.[CLO-2]), MAX(country.[CLO-3]), MAX(country.[CLO-4]), MAX(country.[CLO-5]), MAX(country.[CLO-6]), MAX(country.[CLO-7]), MAX(country.[CLO-8]), MAX(country.[CLO-9]))  CountryDesc
		  , MAX(CONVERT(VARCHAR(10), s.MaturityDate, 101)) MaturityDate
		  , MAX(s.SnpIndustry)				SnpIndustry
		  , MAX(s.MoodyIndustry)			MoodyIndustry

		  , COALESCE(MAX(covlite.[CLO-1]), MAX(covlite.[CLO-2]), MAX(covlite.[CLO-3]), MAX(covlite.[CLO-4]), MAX(covlite.[CLO-5]), MAX(covlite.[CLO-6]), MAX(covlite.[CLO-7]), MAX(covlite.[CLO-8]), MAX(covlite.[CLO-9]))  IsCovLite
		  , MAX(CASE WHEN (s.IsFloating = 1) THEN 'Floating' ELSE 'Fixed' END) IsFloating
		  , MAX(s.LienType						)		LienType
		  , s.IsOnWatch					
		  , MAX(s.WatchObjectTypeId				)		WatchObjectTypeId
		  , MAX(s.WatchObjectId					)		WatchObjectId					
		  , MAX(s.WatchId						)		WatchId						
		  , MAX(s.WatchComments				)		WatchComments				
		  , MAX(s.WatchLastUpdatedOn				)		WatchLastUpdatedOn				
		  , MAX(s.WatchUser						)		WatchUser						

		  , MAX(s.SellCandidateObjectTypeId) SellCandidateObjectTypeId
		  , MAX(s.SellCandidateObjectId) SellCandidateObjectId					
		  , MAX(s.SellCandidateId) SellCandidateId						
		  , MAX(s.SellCandidateComments) SellCandidateComments				
		  , MAX(s.SellCandidateLastUpdatedOn) SellCandidateLastUpdatedOn				
		  , MAX(s.SellCandidateUser) SellCandidateUser						
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
		  , MAX(ISNULL(c.MoodyCashFlowRatingAdjusted, m.MoodyCashFlowRatingAdjusted))	AS OrigMoodyCashFlowRatingAdjusted
		  , CONVERT(VARCHAR, CAST(AVG(m.Bid) AS MONEY),1)  Bid

		  , CONVERT(VARCHAR, CAST(AVG(m.Offer)AS MONEY),1) Offer
		  , MAX(m.Bid) BidNum
		  , MAX(m.Offer) OfferNum
		  , CAST(MAX(m.Spread) AS NUMERIC(20,2)) Spread	
		  , CAST(MAX(m.LiborFloor) AS NUMERIC(20,2))	LiborFloor
		  , MAX(m.MoodyCashFlowRating) MoodyCashFlowRating
		  , COALESCE(MAX(c.MoodyCashFlowRatingAdjusted),MAX(s.MoodyCashFlowRatingAdjusted), MAX(m.MoodyCashFlowRatingAdjusted)) MoodyCashFlowRatingAdjusted
		  , MAX(m.MoodyFacilityRating)		MoodyFacilityRating
		  , MAX(m.[Portal MoodyRecovery])	MoodyRecovery
		  , MAX(m.SnPIssuerRating)  		SnPIssuerRating				
		  , MAX(c.SnPIssuerRatingAdjusted)	SnPIssuerRatingAdjusted		
		  , MAX(m.SnPFacilityRating			)		SnPFacilityRating			
		  , MAX(m.SnPfacilityRatingAdjusted	)		SnPfacilityRatingAdjusted	
		  , MAX(m.SnPRecoveryRating			)		SnPRecoveryRating			
		  , MAX(m.MoodyOutlook					)		MoodyOutlook					

		  , MAX(m.MoodyWatch					)		MoodyWatch					
		  , MAX(m.SnPWatch						)		SnPWatch						
		  , CONVERT(VARCHAR(10), MAX(m.NextReportingDate), 101) NextReportingDate
		  , CONVERT(VARCHAR(10), MAX(m.FiscalYearEndDate), 101) FiscalYearEndDate
		  , CAST(AVG(c.YieldBid			)AS NUMERIC(20,2))			YieldBid				
		  , CAST(AVG(c.YieldOffer			)AS NUMERIC(20,2))			YieldOffer			
		  , CAST(AVG(c.YieldMid			)AS NUMERIC(20,2))			YieldMid				
		  , CAST(AVG(c.CappedYieldBid		)AS NUMERIC(20,2))			CappedYieldBid		
		  , CAST(AVG(c.CappedYieldOffer	)AS NUMERIC(20,2))			CappedYieldOffer		
		  , CAST(AVG(c.CappedYieldMid		)AS NUMERIC(20,2))			CappedYieldMid		
		  , CAST(AVG(c.TargetYieldBid		)AS NUMERIC(20,2))			TargetYieldBid		
		  , CAST(AVG(c.TargetYieldOffer	)AS NUMERIC(20,2))			TargetYieldOffer		
		  , CAST(AVG(c.TargetYieldMid		)AS NUMERIC(20,2))			TargetYieldMid		
		  , CAST(AVG(c.BetterWorseBid		)AS NUMERIC(20,2))			BetterWorseBid		
		  , CAST(AVG(c.BetterWorseOffer	)AS NUMERIC(20,2))			BetterWorseOffer		

		  , CAST(AVG(c.BetterWorseMid		)AS NUMERIC(20,2))			BetterWorseMid		
		  , CAST(AVG(c.TotalCoupon			)AS NUMERIC(20,2))			TotalCoupon			
		  , CAST(AVG(c.WARF)AS NUMERIC(20,2))			WARF					
		  , CAST(AVG(c.WARFRecovery		)AS NUMERIC(20,2))			WARFRecovery			
		  , CAST(AVG(c.Life				)AS NUMERIC(20,2))			Life
		  , FORMAT(NULLIF(ISNULL(MAX(TP.TotalExposure), 0),0), '#,###') TotalPar
		  , ISNULL(MAX(TP.TotalExposure), 0) TotalParNum
		  , (ISNULL(MAX(c.TotalPar),0)) BODTotalPar
		  , ISNULL(MAX(s.MoodyFacilityRatingAdjusted),  MAX(c.MoodyFacilityRatingAdjusted)) MoodyFacilityRatingAdjusted
		  , MAX(a.CLOAnalyst)			CLOAnalyst
		  , MAX(a.HFAnalyst)				HFAnalyst
		  , CONVERT(VARCHAR(10), MAX(a.AsOfDate), 101) AsOfDate
		  , MAX(a.CreditScore)	CreditScore	
		  , MAX(a.LiquidityScore)	LiquidityScore		
		  , MAX(a.OneLLeverage)	OneLLeverage		

		  , CONVERT(VARCHAR, CAST(MAX(a.TotalLeverage) AS MONEY), 1) TotalLeverage
		  , CONVERT(VARCHAR, CAST(MAX(a.EVMultiple) AS MONEY), 1) EVMultiple
		  , CONVERT(VARCHAR, CAST(MAX(a.LTMRevenues) AS MONEY), 1) LTMRevenues
		  , CONVERT(VARCHAR, CAST(MAX(a.LTMEBITDA )AS MONEY), 1) LTMEBITDA
		  , CONVERT(VARCHAR, CAST(MAX(a.FCF) AS MONEY), 1) FCF
		  , MAX(a.Comments)						Comments	
		  , MAX(a.BusinessDescription)			BusinessDescription	
		  , MAX(a.AgentBank			)			AgentBank			
		  , MAX(s.MaturityDate) SecurityMaturityDate
		  , CAST(0 AS BIT) AS IsOnAlert
		  , CAST(MAX(a.LTMFCF) AS NUMERIC(38,2)) LTMFCF
		  , CAST(MAX(a.EnterpriseValue) AS NUMERIC(10,2)) EnterpriseValue
		  , CAST(MAX(a.SeniorLeverage) AS NUMERIC(10,2)) SeniorLeverage
		  , CONVERT(VARCHAR, CAST(MAX(m.CostPrice) AS MONEY), 1) CostPrice
		  , MAX(m.CostPrice) CostPriceNum

		  , MAX(pm.Bid)	PrevDayBidNum
		  , MAX(pm.Offer)	PrevDayOfferNum
		  , CONVERT(VARCHAR, CAST(MAX(pm.Bid) AS MONEY), 1) PrevDayBid
		  , CONVERT(VARCHAR, CAST(MAX(pm.Offer) AS MONEY), 1) PrevDayOffer
		  , CASE WHEN ISNULL(MAX(m.Bid),0) <> 0 AND ISNULL(MAX(pm.Bid),0) <> 0 THEN (MAX(m.Bid) - MAX(pm.Bid)) ELSE NULL END PriceMove
		  , NULL AS SearchText
		  , MAX(s.LienTypeId) LienTypeId
		  , MAX(cs.ScoreDescription) ScoreDescription
		  , ISNULL(MAX(lc.MaxAmt), MAX(wso.MaxIssueSize)) GlobalAmount
		  , a.IsPrivate
		  , a.Sponsor
		  , CAST(ISNULL(MAX(matrix.[CLO-1]),0) as decimal(28,2)) CLO1MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-2]),0) as decimal(28,2)) CLO2MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-3]),0) as decimal(28,2)) CLO3MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-4]),0) as decimal(28,2)) CLO4MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-5]),0) as decimal(28,2)) CLO5MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-6]),0) as decimal(28,2)) CLO6MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-7]),0) as decimal(28,2)) CLO7MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-8]),0) as decimal(28,2)) CLO8MatrixImpliedSpread
		  , CAST(ISNULL(MAX(matrix.[CLO-9]),0) as decimal(28,2)) CLO9MatrixImpliedSpread

		  , CLO1DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-1]),0)   as decimal(28,2))
		  , CLO2DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-2]),0)   as decimal(28,2))
		  , CLO3DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-3]),0)   as decimal(28,2))
		  , CLO4DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-4]),0)   as decimal(28,2))
		  , CLO5DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-5]),0)   as decimal(28,2))
		  , CLO6DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-6]),0)   as decimal(28,2))
		  , CLO7DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-7]),0)   as decimal(28,2))
		  , CLO8DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-8]),0)   as decimal(28,2))
		  , CLO9DifferentialImpliedSpread = CAST( ISNULL(MAX(m.Spread),0) - ISNULL(MAX(matrix.[CLO-9]),0)   as decimal(28,2))

		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-1]),0) as decimal(28,2)) CLO1MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-2]),0) as decimal(28,2)) CLO2MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-3]),0) as decimal(28,2)) CLO3MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-4]),0) as decimal(28,2)) CLO4MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-5]),0) as decimal(28,2)) CLO5MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-6]),0) as decimal(28,2)) CLO6MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-7]),0) as decimal(28,2)) CLO7MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-8]),0) as decimal(28,2)) CLO8MatrixWarfRecovery
		  , CAST(ISNULL(MAX(matrixwarfrecovery.[CLO-9]),0) as decimal(28,2)) CLO9MatrixWarfRecovery


		  , MAX(c.zSnPAssetRecoveryRating)
		  , AVG(c.SnpWarf)
		  ,	AVG(C.SnpLgd)
		  , AVG(c.MoodysLgd)
		  , AVG(c.YieldAvgLgd)
		  , MAX(c.SnpAAARecovery)
		  , MAX(m.SnpCreditWatch)
		  , FORMAT(AVG(NULLIF(ISNULL(m.LiborBaseRate,0),0)) , 'p')  LiborBaseRate   

		  ,MAX(a.[LiborCategory])
		  ,MAX(a.[LiborTransitionNote])
		  
		  , CAST(MAX(1 * s.IsInDefault) AS BIT) IsInDefault
		  , MAX(s.DefaultDate) DefaultDate



		  , MAX(m.[Fund 4 MoodyRecovery]) [Fund-4 MoodyRecovery]
		  , MAX(m.[Fund 10 MoodyRecovery])															[Fund-10 MoodyRecovery]
		  , MAX(m.[Fund 2 MoodyRecovery])															[Fund-2 MoodyRecovery]
		  , MAX(m.[Fund 1 MoodyRecovery])															[Fund-1 MoodyRecovery]
		  , MAX(m.[Fund 3 MoodyRecovery])															[Fund-3 MoodyRecovery]
		  , MAX(m.[Fund 7 MoodyRecovery])															[Fund-7 MoodyRecovery]
		  , MAX(m.[Fund 8 MoodyRecovery])															[Fund-8 MoodyRecovery]
		  , MAX(m.[Fund 9 MoodyRecovery])															[Fund-9 MoodyRecovery]


    FROM  CLO.vw_Security s  WITH (NOLOCK)
	      LEFT  JOIN CLO.vw_SecurityMarket m WITH (NOLOCK) ON s.SecurityId			= m.SecurityId 
		  LEFT  JOIN CLO.vw_PrevDaySecurityMarket pm WITH (NOLOCK) ON s.SecurityId	= pm.SecurityId 
		  LEFT  JOIN CLO.vw_CurrentAnalystResearch a WITH (NOLOCK) ON a.IssuerId	= s.IssuerId
		  LEFT  JOIN CLO.vw_Calculation c WITH (NOLOCK) ON s.SecurityId			= c.SecurityId 
		  LEFT  JOIN CLO.vw_PivotedPositionExposure		pos  WITH (NOLOCK) ON pos.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.vw_PivotedPositionCountry		country  WITH (NOLOCK) ON country.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.vw_PivotedPositionIsCovLite	covlite  WITH (NOLOCK) ON covlite.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.vw_PivotedTradeAllocation	allocation  WITH (NOLOCK) ON allocation.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.#PivotedMatrixImpliedSpread	matrix  WITH (NOLOCK) ON matrix.SecurityId = s.SecurityId
		  LEFT  JOIN CLO.#PivotedMatrixWarfRecovery		matrixwarfrecovery  WITH (NOLOCK) ON matrixwarfrecovery.SecurityId = s.SecurityId

		  LEFT OUTER JOIN CLO.CreditScore cs ON CAST(a.CreditScore AS INT) = cs.Id
		  LEFT OUTER JOIN wsodata wso ON wso.SecurityCode = s.SecurityCode
		  LEFT OUTER JOIN loancontract lc ON lc.SecurityCode = s.SecurityCode
		  LEFT OUTER JOIN totalPar TP ON TP.SecurityId = s.SecurityId
		  CROSS JOIN CLO.vw_PivotedFundTargetPar	targetpar
	GROUP BY S.SecurityId,s.IsOnWatch, a.IsPrivate, a.Sponsor
	

	UPDATE [CLO].[AggregatedPosition]
	SET [MoodyRecovery] = COALESCE([Fund-4 MoodyRecovery],[Fund-10 MoodyRecovery],[Fund-2 MoodyRecovery],[Fund-1 MoodyRecovery],[Fund-3 MoodyRecovery],[Fund-7 MoodyRecovery],[Fund-8 MoodyRecovery],[Fund-9 MoodyRecovery])
	WHERE [PositionDateId] = CLO.GetPrevDayDateId() AND [MoodyRecovery] IS NULL AND COALESCE([Fund-4 MoodyRecovery],[Fund-10 MoodyRecovery],[Fund-2 MoodyRecovery],[Fund-1 MoodyRecovery],[Fund-3 MoodyRecovery],[Fund-7 MoodyRecovery],[Fund-8 MoodyRecovery],[Fund-9 MoodyRecovery]) IS NOT NULL


	drop table #PivotedMatrixImpliedSpread
	drop table #PivotedMatrixWarfRecovery

RETURN 0
