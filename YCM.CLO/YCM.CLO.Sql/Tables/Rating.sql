CREATE TABLE [CLO].[Rating]
(
	[RatingId] SMALLINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[RatingDesc] varchar(100) not null,
	[Rank] SMALLINT NULL,
	[IsMoody] BIT NULL,
	[IsSnP] BIT NULL,
	[IsFitch] BIT NULL,
	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,

)
