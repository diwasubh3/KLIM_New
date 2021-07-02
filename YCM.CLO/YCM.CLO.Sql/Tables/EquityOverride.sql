CREATE TABLE [clo].[EquityOverride]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FundId] INT NOT NULL,
	[SecurityCode] VARCHAR(100) NOT NULL,
	[Notional] NUMERIC(38,10) NULL,
	[Bid] NUMERIC(28,10) NULL,
	[IsDeleted] bit	NOT NULL DEFAULT(0),

	[CreatedBy] VARCHAR(100) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastModifiedBy] VARCHAR(100) NULL,
	[LastModifiedOn] DATETIME NULL,


)
