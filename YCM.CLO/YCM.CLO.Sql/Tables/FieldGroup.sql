CREATE TABLE [CLO].[FieldGroup]
(
	[FieldGroupId] SMALLINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FieldGroupName] VARCHAR(100),
	[SortOrder] smallint,
	[DisplayIcon] varchar(100),
	[ShowOnPositions] bit default(0)
)
