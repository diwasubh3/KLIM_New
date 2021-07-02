CREATE PROCEDURE [CLO].[spCleanPositionsBasedOnPrincipalCash]
	@fundId int = 0,
	@dateId INT
AS
	declare @positionsCount int = 0

	select @positionsCount = count(*) 
	from  [CLO].Position p with(nolock)
	where p.DateId = @dateId and p.FundId = @fundId 


	if (@positionsCount <> 0)
	BEGIN
		
		DELETE FROM [CLO].[WSOPosition]   WHERE DateId = @dateId and FundId = @fundId
		DELETE FROM [CLO].[WSOMarketData] WHERE DateId = @dateId and FundId = @fundId

		INSERT INTO [CLO].[WSOPosition]  ([FundId]
           ,[SecurityId]
           ,[DateId]
           ,[MarketValue]
           ,[Exposure]
           ,[PctExposure]
           ,[PxPrice]
           ,[IsCovLite]
           ,[CountryId]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[LastUpdatedOn]
           ,[LastUpdatedBy]
           ,[IsStale]
		   ,[CapitalizedInterestOrig]
		   ,[SnPAssetRecoveryRating]
		   )
		   SELECT 
		   [FundId]
           ,[SecurityId]
           ,[DateId]
           ,[MarketValue]
           ,[Exposure]
           ,[PctExposure]
           ,[PxPrice]
           ,[IsCovLite]
           ,[CountryId]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[LastUpdatedOn]
           ,[LastUpdatedBy]
           ,[IsStale]
		   ,[CapitalizedInterestOrig]
		   ,[SnPAssetRecoveryRating]
		   FROM [CLO].Position WHERE DateID = @dateId AND ISNULL(IsStale,0) = 0  and FundId = @fundId
		
		INSERT INTO [CLO].[WSOMarketData]
           ([DateId]
           ,[SecurityId]
           ,[FundId]
           ,[Bid]
           ,[Offer]
           ,[CostPrice]
           ,[Spread]
           ,[LiborFloor]
           ,[MoodyCashFlowRatingId]
           ,[MoodyCashFlowRatingAdjustedId]
           ,[MoodyFacilityRatingId]
           ,[MoodyRecovery]
           ,[SnPIssuerRatingId]
           ,[SnPIssuerRatingAdjustedId]
           ,[SnPFacilityRatingId]
           ,[SnPfacilityRatingAdjustedId]
           ,[SnPRecoveryRatingId]
           ,[MoodyOutlook]
           ,[MoodyWatch]
           ,[SnPWatch]
           ,[NextReportingDate]
           ,[FiscalYearEndDate]
           ,[AgentBank]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[LastUpdatedOn]
           ,[LastUpdatedBy]
		   ,[IsStale]
		   )
		   SELECT 
		   [DateId]
           ,[SecurityId]
           ,[FundId]
           ,[Bid]
           ,[Offer]
           ,[CostPrice]
           ,[Spread]
           ,[LiborFloor]
           ,[MoodyCashFlowRatingId]
           ,[MoodyCashFlowRatingAdjustedId]
           ,[MoodyFacilityRatingId]
           ,[MoodyRecovery]
           ,[SnPIssuerRatingId]
           ,[SnPIssuerRatingAdjustedId]
           ,[SnPFacilityRatingId]
           ,[SnPfacilityRatingAdjustedId]
           ,[SnPRecoveryRatingId]
           ,[MoodyOutlook]
           ,[MoodyWatch]
           ,[SnPWatch]
           ,[NextReportingDate]
           ,[FiscalYearEndDate]
           ,[AgentBank]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[LastUpdatedOn]
           ,[LastUpdatedBy]
		   ,[IsStale]
		FROM [CLO].MarketData WHERE DateID = @dateId AND ISNULL(IsStale,0) = 0  and FundId = @fundId

		DELETE FROM CLO.Position	WHERE DateId = @dateId  and FundId = @fundId
		DELETE FROM CLO.MarketData	WHERE DateId = @dateId  and FundId = @fundId
		
	END


RETURN 0
