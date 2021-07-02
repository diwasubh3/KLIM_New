CREATE TABLE [CLO].[Security]
(
	[SecurityId] INT NOT NULL PRIMARY KEY identity(1,1),
	[SecurityCode] varchar(100) NOT NULL,
	[SecurityDesc] varchar(500) NULL,
	
	[BBGId] varchar(1000) NOT NULL,
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

	[SourceId] smallint null default(0),
	[IsDeleted] bit default(0),
	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,

	[IsInDefault] BIT NULL,
	[DefaultDate] DATETIME,
	
    CONSTRAINT UQ_SecurityCode UNIQUE(SecurityCode)   

)
