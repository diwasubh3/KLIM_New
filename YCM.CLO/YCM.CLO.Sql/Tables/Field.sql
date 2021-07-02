CREATE TABLE [CLO].[Field]
(
	[FieldId]	SMALLINT NOT NULL  IDENTITY(1,1),
	[FieldGroupId] smallint not null references CLO.FieldGroup(FieldGroupId),
	[FieldName] VARCHAR(100), 
	[JsonPropertyName] VARCHAR(100), 
	[FieldTitle] varchar(100),
	[JsonFormatString] varchar(200),
	[DisplayWidth] int default(140),
	[IsPercentage] bit default(0),
	[SortOrder] DECIMAL(18, 4) ,
	[FieldType] smallint ,
	[HeaderCellClass] varchar(100),
	[CellClass] varchar(100),
	[CellTemplate] varchar(max),
	[Hidden] bit,
	[PinnedLeft] bit,
	[IsSecurityOverride] bit,
	[ShowInFilter] BIT,
	[FilterOrder] NUMERIC(8,2),
    CONSTRAINT [PK_Field] PRIMARY KEY ([FieldId])
)
