CREATE PROCEDURE [clo].[spUpsertAnalystResearch]
	@issuerId int = 0,
	@asOfDate DATETIME = NULL,
	@agentBank VARCHAR(1000) = null
	
AS

IF (EXISTS(SELECT TOP 1 issuerid FROM clo.analystresearch WHERE issuerid = @issuerId))
BEGIN
	UPDATE clo.AnalystResearch 	SET agentBank = @agentBank	WHERE issuerid=@issuerId
END
ELSE
BEGIN
	INSERT INTO clo.AnalystResearch (IssuerId,AsOfDate,AgentBank) VALUES (@issuerId, @asOfDate, @agentBank)
END

	
RETURN 0
