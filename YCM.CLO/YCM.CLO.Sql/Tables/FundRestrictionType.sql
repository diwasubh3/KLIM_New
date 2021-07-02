CREATE TABLE [CLO].[FundRestrictionType]
(
	[FundRestrictionTypeId] SMALLINT NOT NULL PRIMARY KEY identity(1,1),
	[FundRestrictionTypeName] varchar(100),
	[DisplayColor] varchar(100),
	[SortOrder] smallint
)
