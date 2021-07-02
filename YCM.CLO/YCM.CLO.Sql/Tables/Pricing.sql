CREATE TABLE [CLO].[Pricing]
(
	[PricingId] BIGINT NOT NULL identity(1,1),
	[DateId] int not null ,
	[SecurityId]	int NOT NULL references		[CLO].[Security]([SecurityId]),
	[Bid] numeric(38,10) not null,
	[Offer] numeric(38,10) not null,
	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,
	PRIMARY KEY ([DateId] ,[SecurityId])
)
