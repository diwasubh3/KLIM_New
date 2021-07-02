CREATE TABLE [CLO].[Facility]
(
	[FacilityId] SMALLINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FacilityDesc] varchar(100) not null,

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,

)
