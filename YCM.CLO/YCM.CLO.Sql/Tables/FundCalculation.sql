﻿CREATE TABLE [CLO].[FundCalculation]
(
	[FundCalculationId]		INT NOT NULL identity(1,1),
	[DateId] int not null,
	[FundId]		int not null references		[CLO].Fund(FundId),

	[Par] numeric (32,4),
	[BODPar] numeric (32,4),
	[Spread]  numeric (32,4),
	[BODSpread]  numeric (32,4),
	[TotalCoupon]  numeric (32,4),
	[BODTotalCoupon]  numeric (32,4),
	[WARF]  numeric (32,4),
	[BODWARF]  numeric (32,4),
	[MoodyRecovery]  numeric (32,4),
	[BODMoodyRecovery]  numeric (32,4),
	[Bid]  numeric (32,4),
	[BODBid]  numeric (32,4),
	[PrincipalCash]  numeric (32,4),
	[BODPrincipalCash]  numeric (32,4),
	[Diversity]  numeric (32,4),
	[BODDiversity]  numeric (32,4),
	[CleanNav]   numeric (32,4),
	[BODCleanNav]  numeric (32,4),
	[WAMaturityDays]  numeric (32,4),
	[BODWAMaturityDays]  numeric (32,4),
	[AssetPar]  numeric (32,4),
	[PriorDayExposure]  numeric (32,4),
	[PriorDayPrincipalCash]  numeric (32,4),
	[MatrixImpliedSpread]		numeric (32,8),
	[CreatedOn]					datetime null default (getdate()),
	[CreatedBy]					varchar(100) null,
	[LastUpdatedOn]				datetime null default (getdate()),
	[LastUpdatedBy]				varchar(100) null,
	PRIMARY KEY ([DateId],[FundId]),
	[SnpRecovery]					NUMERIC(32,8),
	[BBMVOC] numeric (32,4),
	[WALTrigger] numeric (32,4),
	[WALCushion] numeric (32,4),
	[TimeToReinvest] numeric (32,4),


	[TimeToNonCallEnd] numeric (32,4),
	[WACostOfDebt] numeric (32,4),
	[TotalManagementFees] numeric (32,4),
	[Net] numeric (32,4),
	
	[TotalDebt] numeric (32,4),
	[EquityLeverage] numeric (32,4),
	[AnnualExcessSpreadToEquity] numeric (32,4),
	[ClassDMVOC] numeric (32,4),
	[ClassEMVOC] numeric (32,4),
	[ClassFMVOC] numeric (32,4),
	[EquityNav] numeric (32,4),

	[B3ToAssetParPct] NUMERIC (32,4),
	[BMinusToAssetParPct] NUMERIC (32,4),
)