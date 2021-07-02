CREATE TABLE [CLO].[MatrixData]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[FundId] int references CLO.Fund(FundId),
	[Spread] numeric(28,10),
	[Diversity] numeric(28,10),
	[WARF] numeric(28,10),
	[WARFModifier] numeric(28,10),
	[DataPointType] smallint,
	[InterpolationType] smallint,
	[FromMajorMatrixDataId] int,
	[ToMajorMatrixDataId] int,
)
