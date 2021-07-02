CREATE TABLE [CLO].[FundAssetClass]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FundId] INT REFERENCES [CLO].Fund(FundId) NOT NULL,
	[AssetClassId] INT references [CLO].[AssetClass]([AssetClassId]) NOT NULL,
	[Notional] NUMERIC(38,10) NULL,
	[Spread] NUMERIC(38,10) NULL,
	[Libor] NUMERIC(38,10) NULL,
	[StartDate] DATETIME NULL,
	[EndDate] DATETIME NULL,
	[MoodyRatingId] SMALLINT REFERENCES CLO.Rating([RatingId]) NULL,

	[CreatedBy] VARCHAR(100)  NULL,
	[CreatedOn] DATETIME NULL DEFAULT(GETDATE()),
	[LastModifiedBy] VARCHAR(100) NULL,
	[LastModifiedOn] DATETIME NULL,
	[OverrideCalcSpread] NUMERIC(38,10) NULL,
)
