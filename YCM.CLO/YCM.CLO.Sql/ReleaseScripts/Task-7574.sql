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
	, 'Global Amount' FieldTitle, JsonFormatString, 100 AS DisplayWidth, IsPercentage, SortOrder + 10 AS SortOrder, FieldType, HeaderCellClass
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
			SELECT SecurityID AS SecurityCode, MIN(IssueSize) AS MinIssueSize, MAX(IssueSize) AS MaxIssueSize
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
		)

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

		  , MAX(targetPar.[CLO-1]) CLO1TargetPar
		  , MAX(targetPar.[CLO-2]) CLO2TargetPar
		  , MAX(targetPar.[CLO-3]) CLO3TargetPar
		  , MAX(targetPar.[CLO-4]) CLO4TargetPar
		  , MAX(targetPar.[CLO-5]) CLO5TargetPar
		  , MAX(targetPar.[WH]) TRSTargetPar

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
