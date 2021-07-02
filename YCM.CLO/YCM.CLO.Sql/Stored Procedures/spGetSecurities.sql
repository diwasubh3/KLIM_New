CREATE PROCEDURE [CLO].[spGetSecurities]

AS

SELECT 
MAX([SecurityId]					  )	[SecurityId]								
,MAX([SecurityCode]					  )	[SecurityCode]							
,MAX([SecurityDesc]					  )	[SecurityDesc]							
,MAX([BBGId]						  )	[BBGId]									
,MAX([Issuer]						  )	[Issuer]									
,MAX([Facility]						  )	[Facility]								
,MAX([CallDate]						  )	[CallDate]								
,MAX([MaturityDate]					  )	[MaturityDate]							
,MAX([SnpIndustry]					  )	[SnpIndustry]							
,MAX([MoodyIndustry]				  )	[MoodyIndustry]							
,[IsFloating]							[IsFloating]								
,MAX([LienType]						  )	[LienType]								
,MAX([IssuerId]						  )	[IssuerId]								
,MAX([WatchId]						  )	[WatchId]								
,MAX([MoodyIndustryId]				  )	[MoodyIndustryId]						
,[IsOnWatch]							[IsOnWatch]								
,MAX([WatchObjectTypeId]			  )	[WatchObjectTypeId]						
,MAX([WatchObjectId]				  )	[WatchObjectId]							
,MAX([WatchComments]				  )	[WatchComments]							
,MAX([WatchLastUpdatedOn]			  )	[WatchLastUpdatedOn]						
,MAX([WatchUser]					  )	[WatchUser]								
,MAX([OrigSecurityCode]				  )	[OrigSecurityCode]						
,MAX([OrigSecurityDesc]				  )	[OrigSecurityDesc]						
,MAX([OrigBBGId]					  )	[OrigBBGId]								
,MAX([OrigIssuer]					  )	[OrigIssuer]								
,MAX([GICSIndustry]					  )	[GICSIndustry]							
,MAX([OrigFacility]					  )	[OrigFacility]							
,MAX([OrigCallDate]					  )	[OrigCallDate]							
,MAX([OrigMaturityDate]				  )	[OrigMaturityDate]						
,MAX([OrigSnpIndustry]				  )	[OrigSnpIndustry]						
,MAX([OrigMoodyIndustry]			  )	[OrigMoodyIndustry]						
,MAX([OrigIsFloating]				  )	[OrigIsFloating]							
,MAX([OrigLienType]					  )	[OrigLienType]							
,MAX([OrigMoodyFacilityRatingAdjusted])	[OrigMoodyFacilityRatingAdjusted]		
,MAX([OrigMoodyCashFlowRatingAdjusted])	[OrigMoodyCashFlowRatingAdjusted]		
,MAX([SecurityLastUpdatedOn]		  )	[SecurityLastUpdatedOn]					
,MAX([SecurityLastUpdatedBy]		  )	[SecurityLastUpdatedBy]					
,MAX([SecurityCreatedOn]			  )	[SecurityCreatedOn]						
,MAX([SecurityCreatedBy]			  )	[SecurityCreatedBy]						
,MAX([SourceId]						  )	[SourceId]								
,MAX([MoodyFacilityRatingAdjusted]	  )	[MoodyFacilityRatingAdjusted]			
,MAX([MoodyCashFlowRatingAdjusted]	  )	[MoodyCashFlowRatingAdjusted]			
,ISNULL([HasPositions],0)					[HasPositions]
INTO #securities
FROM [CLO].vw_SecurityMarketCalculation
WHERE ISNULL(SourceId,0) = 0
GROUP BY [SecurityId],[HasPositions],[IsFloating],[IsOnWatch]

UPDATE #securities
SET HasPositions = 1
WHERE securityid IN 
(SELECT SecurityId FROM [CLO].Position WHERE DateId = [CLO].[GetPrevDayDateId]() 
AND ISNULL([Exposure],0) <> 0 
GROUP BY SecurityId)

SELECT * FROM #securities  
ORDER BY HasPositions DESC,SecurityCode ASC	

DROP TABLE #securities

RETURN 0
