﻿CREATE VIEW CLO.vw_AggregatePosition
AS
	SELECT [PositionDateId]
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
	  , zSnPAssetRecoveryRating
	  , SnpWarf
	  , SnpLgd
	  , MoodysLgd
	  , YieldAvgLgd
	  , SnpAAARecovery
	  , SnpCreditWatch
	  , CASE 
	      WHEN LiborBaseRate IS NULL THEN '0.00%'
	      ELSE LiborBaseRate
	    END LiborBaseRate
	  , [Fund-4 WARF]
	,[LiborCategory]
	,[LiborTransitionNote]
	  FROM CLO.AggregatedPosition 

	  WHERE 

	  PositionDateId = CLO.GetPrevDayDateId()
GO