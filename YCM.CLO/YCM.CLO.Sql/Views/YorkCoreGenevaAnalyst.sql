CREATE VIEW [CLO].[vw_YorkCoreGenevaAnalyst]

AS

SELECT 
AnalystId, 
AnalystTypeId, 
AnalystCode, 
AnalystDesc, 
AppUserId
FROM YorkCore_Geneva..Analyst

GO


