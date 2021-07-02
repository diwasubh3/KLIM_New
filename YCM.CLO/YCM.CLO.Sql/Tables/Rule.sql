CREATE TABLE [CLO].[Rule]
(
	[RuleId] SMALLINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[RuleName] VARCHAR(100),
	[ExecutionStoredProcedure]  VARCHAR(100),
	[SortOrder] smallint
)
