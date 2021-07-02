CREATE TABLE [CLO].[Industry]
(
	[IndustryId]	SMALLINT NOT NULL PRIMARY KEY identity(1,1),
	[IndustryDesc]	varchar(100) NOT NULL ,
	[IsSnP] bit not null default(0),
	[IsMoody] bit not null default(0),

	[MappedSnPIndustryId] SMALLINT,
	[MappedMoodyIndustryId] SMALLINT,

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,
)
