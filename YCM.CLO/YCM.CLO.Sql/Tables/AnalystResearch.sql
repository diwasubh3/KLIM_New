CREATE TABLE [CLO].[AnalystResearch]
(
	[AnalystResearchId] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[IssuerId]	int NOT NULL references		[CLO].[Issuer]([IssuerId]),

	[CLOAnalystUserId] int null references [CLO].[User]([UserId]),
	[HFAnalystUserId] int null references [CLO].[User]([UserId]),

	[AsOfDate] datetime null,
	[CreditScore] numeric(10,4) null,
	[LiquidityScore] numeric(10,4) null,
	[OneLLeverage] numeric(10,4) null,
	[TotalLeverage] numeric(10,4) null,
	[EVMultiple] numeric(10,4) null,
	[LTMRevenues] numeric(38,4) null,
	[LTMEBITDA] numeric(38,4) null,
	[FCF] numeric(38,4) null,

	[Comments] varchar(max) null,
	[BusinessDescription] VARCHAR(max) NULL,

	[CreatedOn] datetime  default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime  default (getdate()),
	[LastUpdatedBy] varchar(100) null,
	[AgentBank] varchar(100) null, 
    CONSTRAINT UQ_AnalystResearch_IssuerId_AsOfDate UNIQUE(IssuerId,AsOfDate)
)
