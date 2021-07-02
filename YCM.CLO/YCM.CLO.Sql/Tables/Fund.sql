CREATE TABLE [CLO].[Fund](
	[FundId] [INT] IDENTITY(1,1) NOT NULL,
	[FundCode] [VARCHAR](100) NOT NULL,
	[FundDesc] [VARCHAR](500) NULL,
	[PrincipalCash] [NUMERIC](38, 10) NULL,
	[WSOLastUpdatedOn] [DATETIME] NULL,
	[LiabilityPar] [NUMERIC](38, 10) NULL,
	[EquityPar] [NUMERIC](38, 10) NULL,
	[TargetPar] [NUMERIC](38, 10) NULL,
	[WALifeAdjustment] [NUMERIC](38, 10) NULL,
	[RecoveryMultiplier] [NUMERIC](38, 10) NULL,
	[AssetParPercentageThreshold] [NUMERIC](18, 10) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedBy] [VARCHAR](100) NULL,
	[LastUpdatedOn] [DATETIME] NULL,
	[LastUpdatedBy] [VARCHAR](100) NULL,
	[CLOFileName] [VARCHAR](1000) NULL,
	[IsStale] [BIT] NULL,
	[IsPrincipalCashStale] [BIT] NULL,
	[DisplayText] [VARCHAR](100) NULL,
	[IsActive] [BIT] NOT NULL,
	[WSOSpread] [DECIMAL](38, 6) NULL,
	[WSOWARF] [DECIMAL](38, 6) NULL,
	[WSOMoodyRecovery] [DECIMAL](38, 6) NULL,
	[WSOWALife] [DECIMAL](38, 6) NULL,
	[WSODiversity] [DECIMAL](38, 6) NULL,
	[IsWarehouse] [BIT] NOT NULL,
	[PortfolioName] [VARCHAR](255) NULL,
	[SortOrder] [INT] NOT NULL,
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





PRIMARY KEY CLUSTERED 
(
	[FundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_FundCode] UNIQUE NONCLUSTERED 
(
	[FundCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


GO

ALTER TABLE [CLO].[Fund] ADD  DEFAULT (GETDATE()) FOR [CreatedOn]
GO

ALTER TABLE [CLO].[Fund] ADD  DEFAULT (GETDATE()) FOR [LastUpdatedOn]
GO

ALTER TABLE [CLO].[Fund] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [CLO].[Fund] ADD  DEFAULT ((0)) FOR [IsWarehouse]
GO

ALTER TABLE [CLO].[Fund] ADD  DEFAULT ((10000)) FOR [SortOrder]
GO


