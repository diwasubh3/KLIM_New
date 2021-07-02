CREATE TABLE [CLO].[SecurityOverride]
(
	[SecurityOverrideId] INT NOT NULL PRIMARY KEY identity(1,1),
	[SecurityId] INT NOT NULL references [CLO].[Security]([SecurityId]),
	[FieldId]	SMALLINT NOT NULL  references [CLO].[Field](FieldId),
	[ExistingValue] varchar(100)  null,
	[OverrideValue] varchar(100) not null,
	[EffectiveFrom] Datetime null,
	[EffectiveTo] datetime null,
	[Comments] varchar(1000) null,
	[IsDeleted] bit default(0),
	[IsConflict] bit default(0),
	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,
	
)
