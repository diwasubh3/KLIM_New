CREATE PROCEDURE [CLO].[spAddPositionsForTempSecurities]
	
AS

INSERT INTO [CLO].[Position]
           ([FundId]
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
           ,[LastUpdatedBy])
SELECT f.FundId,s.SecurityId,CLO.GetPrevDayDateId(),0,0,0,0,0,NULL,GETDATE(),'System',NULL,null FROM CLO.Security s WITH(NOLOCK)
LEFT JOIN CLO.Position p ON p.SecurityId = s.SecurityId AND p.DateId = CLO.GetPrevDayDateId()
CROSS JOIN CLO.Fund f WITH(NOLOCK)
WHERE SourceId = 1 AND (IsDeleted IS NULL OR IsDeleted = 0) AND p.PositionId IS NULL

RETURN 0
