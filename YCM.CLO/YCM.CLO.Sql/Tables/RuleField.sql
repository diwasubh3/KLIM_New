CREATE TABLE [CLO].[RuleField]
(
	[RuleFieldId] INT NOT NULL PRIMARY KEY identity(1,1),
	[RuleId]	SMALLINT	NOT	NULL REFERENCES [CLO].[Rule](RuleId),
	[RuleSectionTypeId] smallint null references [CLO].[RuleSectionType]([RuleSectionTypeId])  ,
	[FieldId]	SMALLINT	NOT	NULL REFERENCES [CLO].[Field](FieldId),
	[SortOrder] smallint null
)
