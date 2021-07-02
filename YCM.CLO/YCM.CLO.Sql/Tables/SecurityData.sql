CREATE TABLE [CLO].[SecurityData]
(
	[SecurityDataId] BIGINT NOT NULL IDENTITY(1,1),
	[DateId] INT NOT NULL,
	[SecurityId]	int NOT NULL references		[CLO].[Security]([SecurityId]),
	
	
	[IssuerId] int not null references [CLO].Issuer(IssuerId),
	[FacilityId]  SMALLINT not null references [CLO].[Facility]([FacilityId]),
	[CallDate] datetime null,
	
	[MaturityDate] datetime,
	[GICSIndustry]	VARCHAR(500),
	[SnPIndustryId]		smallint not null references [CLO].[Industry]([IndustryId]),
	[MoodyIndustryId]	smallint not null references [CLO].[Industry]([IndustryId]),
	
	[IsFloating] bit NOT NULL default(0),
	[LienTypeId] SMALLINT NOT NULL references [CLO].[LienType]([LienTypeId]),
	[CreditScore] smallint null default(0), 
	
	[ISIN] varchar(20) NULL,
	
	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null, 
    
	[IsInDefault] BIT NULL,
	[DefaultDate] DATETIME,

	CONSTRAINT [PK_SecurityData] PRIMARY KEY ([DateId], [SecurityId]),
)
