CREATE TABLE [CLO].[LienType]
(
	[LienTypeId] SMALLINT NOT NULL PRIMARY KEY identity(1,1),
	[LienTypeDesc] varchar(100) not null ,

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,


)
