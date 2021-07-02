CREATE TABLE [CLO].[FundRestriction]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[FundId] INT NOT NULL references [CLO].[Fund](FundId),
	[FundRestrictionTypeId] SMALLINT NOT NULL references [CLO].[FundRestrictionType]([FundRestrictionTypeId]),
	[FieldId]	SMALLINT NOT NULL references [CLO].[Field]([FieldId]),
	[OperatorId] SMALLINT NOT NULL references [CLO].[Operator]([OperatorId]),
	[RestrictionValue] numeric(38,10)  NOT NULL
)
