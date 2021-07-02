CREATE PROCEDURE [CLO].[spLoadStalePositions]
	@fundId int = 0,
	@currentDateId INT,
	@prevDateId int
AS
	declare @positionsCount int = 0

	select @positionsCount = count(*) 
	from [CLO].Position p with(nolock)
	where p.DateId = @currentDateId and p.FundId = @fundId 

	if (@positionsCount = 0)
	BEGIN

		DELETE FROM CLO.Position	WHERE DateId = @currentDateId and FundId = @fundId AND ISNULL(IsStale,0) = 1
		DELETE FROM CLO.MarketData	WHERE DateId = @currentDateId and FundId = @fundId AND ISNULL(IsStale,0) = 1

		Insert into CLO.Position
		(   [FundId]
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
		   ,[IsStale])
		SELECT 
		   p.[FundId]
		  ,p.[SecurityId]
		  ,@currentDateId
		  ,p.[MarketValue]
		  ,p.[Exposure]
		  ,p.[PctExposure]
		  ,p.[PxPrice]
		  ,p.[IsCovLite]
		  ,p.[CountryId]
		  ,GETDATE()
		  ,'System'
		  ,GETDATE()
		  ,'System'
		  ,cast(1 as bit)
		from CLO.Position p WITH(nolock)
		LEFT JOIN CLO.Position existing with(nolock) ON existing.SecurityId = p.SecurityId AND existing.DateId = @currentDateId AND existing.FundId = p.FundId
		where p.[DateId] = @prevDateId and p.FundId = @fundId AND existing.SecurityId IS NULL

		Insert into CLO.MarketData
		(   [DateId]
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
		   ,[IsStale])
		SELECT 
		   @currentDateId
		   ,m.[SecurityId]
           ,m.[FundId]
           ,m.[Bid]
           ,m.[Offer]
           ,m.[CostPrice]
           ,m.[Spread]
           ,m.[LiborFloor]
           ,m.[MoodyCashFlowRatingId]
           ,m.[MoodyCashFlowRatingAdjustedId]
           ,m.[MoodyFacilityRatingId]
           ,m.[MoodyRecovery]
           ,m.[SnPIssuerRatingId]
           ,m.[SnPIssuerRatingAdjustedId]
           ,m.[SnPFacilityRatingId]
           ,m.[SnPfacilityRatingAdjustedId]
           ,m.[SnPRecoveryRatingId]
           ,m.[MoodyOutlook]
           ,m.[MoodyWatch]
           ,m.[SnPWatch]
           ,m.[NextReportingDate]
           ,m.[FiscalYearEndDate]
           ,m.[AgentBank]
		  ,GETDATE()
		  ,'System'
		  ,GETDATE()
		  ,'System'
		  ,cast(1 as bit)
		from CLO.MarketData m WITH(nolock)
		LEFT JOIN CLO.MarketData existing with(nolock) ON existing.SecurityId = m.SecurityId AND existing.DateId = @currentDateId AND existing.FundId = m.FundId
		where m.[DateId] = @prevDateId and m.FundId = @fundId AND existing.SecurityId IS NULL
		
		Update CLO.Fund
		set IsStale = cast(1 as bit)
		where FundId = @fundId
	END
	else 
	BEGIN
		
		DECLARE @stalePositionsCount int
		SELECT @stalePositionsCount = COUNT(*) FROM CLO.Position WITH(NOLOCK) WHERE DateId = @currentDateId AND FundId = @fundId AND ISNULL(IsStale,0) = 1

		IF(@stalePositionsCount > 0)
			Update CLO.Fund
			set IsStale = cast(1 as bit)
			where FundId = @fundId
		ELSE	
			Update CLO.Fund
			set IsStale = cast(0 as bit)
			where FundId = @fundId
	END

RETURN 0
