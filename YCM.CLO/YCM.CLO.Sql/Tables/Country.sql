CREATE TABLE [CLO].[Country]
(
	[CountryId] SMALLINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[CountryDesc] varchar(100) NOT NULL ,

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,


)
