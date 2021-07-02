CREATE TABLE [CLO].[MatrixPoint]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[FundId] int references CLO.Fund(FundId),
	[Spread] numeric(28,10),
	[Diversity] numeric(28,10),
	[WARF] numeric(28,10),
	[WARFModifier] numeric(28,10),
	[DataPointType] smallint,
	[TopMajorSpread] numeric(28,10),
	[BottomMajorSpread] numeric(28,10),
	[LeftMajorDiversity] numeric(28,10),
	[RightMajorDiversity] numeric(28,10),

	[TopSpread] numeric(28,10),
	[BottomSpread] numeric(28,10),
	[LeftDiversity] numeric(28,10),
	[RightDiversity] numeric(28,10),

	[CreatedBy] varchar(100),
	[CreatedOn] datetime
)
