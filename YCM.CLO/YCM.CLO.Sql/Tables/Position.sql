CREATE TABLE [CLO].[Position]
(
	[PositionId] BIGINT NOT NULL   identity(1,1),
	[FundId] int not null references CLO.Fund(FundId),
	[SecurityId] int not null references CLO.Security(SecurityId),
	[DateId] int not null,
	[MarketValue]	numeric (38,4),
	[Exposure]		numeric (38,4),
	[PctExposure]	numeric (38,4),
	[PxPrice]		numeric (38,4),


	[IsCovLite] bit NOT NULL default(0),
	[CountryId] smallint references [CLO].[Country](CountryId), 

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null, 
	[IsStale] bit,
    CONSTRAINT [PK_Position] PRIMARY KEY ([DateId], [FundId], [SecurityId]),
	[CapitalizedInterestOrig]		numeric (38,4),
	[SnPAssetRecoveryRating] varchar(100)
)
