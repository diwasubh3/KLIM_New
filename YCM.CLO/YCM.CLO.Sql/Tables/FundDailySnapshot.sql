﻿CREATE TABLE [CLO].[FundDailySnapshot]
(
	[DateId] int not null,
	[FundId] INT NOT NULL,
	[FundCode] varchar(100) not null,
	[FundDesc] varchar(500) null,
	[PrincipalCash] NUMERIC(38,10),
	[WSOLastUpdatedOn] datetime ,
	[LiabilityPar] NUMERIC(38,10),
	[EquityPar] NUMERIC(38,10),
	[TargetPar] NUMERIC(38,10),
	[RecoveryMultiplier] NUMERIC(38,10),
	[WALifeAdjustment] NUMERIC(38,10),
	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,
	[CLOFileName] varchar(1000),
	[IsStale] bit,

	[IsPrincipalCashStale] [BIT] NULL,
	[DisplayText] [VARCHAR](100) NULL,
	[IsActive] [BIT]  NULL,
	[WSOSpread] [DECIMAL](38, 6) NULL,
	[WSOWARF] [DECIMAL](38, 6) NULL,
	[WSOMoodyRecovery] [DECIMAL](38, 6) NULL,
	[WSOWALife] [DECIMAL](38, 6) NULL,
	[WSODiversity] [DECIMAL](38, 6) NULL,
	[IsWarehouse] [BIT] NULL,
	[PortfolioName] [VARCHAR](255) NULL,
	[SortOrder] [INT] NULL,
	[WALWARFAdj] [DECIMAL](38, 6) NULL,
	[MaxWarfTrigger] [DECIMAL](38, 6) NULL,
	[ClassEPar] [NUMERIC](38, 10) NULL,
	[WALDate] [DATETIME] NULL,
	[ReInvestEndDate] [DATETIME] NULL,
	[WalDateAdj] [DECIMAL](38, 6) NULL,
	[ParentFundId] INT NULL,
	[ParentFundCode] [VARCHAR](100) NULL,
	[CanFilter] BIT DEFAULT(1) NULL,

	[BloombergCode] VARCHAR(100) NULL,
	[PricingDate] DATETIME NULL,
	[MgmtFees] DECIMAL(33,6) NULL,
	[OperatingExpenses] DECIMAL(33,6) NULL,
	[ClosingDate] DATETIME NULL,
	[NonCallEndsDate] DATETIME NULL,
	[FinalMaturity] DATETIME NULL,
	[ProjectedEquityDistribtion] DECIMAL(33,6) NULL,
	CONSTRAINT [PK_FundDailySnapshot] PRIMARY KEY ([DateId], [FundId])
)