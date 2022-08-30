﻿CREATE TABLE [CLO].[Calculation]
(
	[CalculationId]		INT NOT NULL identity(1,1),
	[DateId] int not null,
	[SecurityId]	int NOT NULL references		[CLO].[Security]([SecurityId]),
	[FundId]		int not null references		[CLO].Fund(FundId),
	[YieldBid]						numeric (32,4),
	[YieldOffer]					numeric (32,4),
	[YieldMid]						numeric (32,4),
	[CappedYieldBid]				numeric (32,4),
	[CappedYieldOffer]				numeric (32,4),
	[CappedYieldMid]				numeric (32,4),
	[TargetYieldBid]				numeric (32,4),
	[TargetYieldOffer]				numeric (32,4),
	[TargetYieldMid]				numeric (32,4),
	[BetterWorseBid]				numeric (32,4),
	[BetterWorseOffer]				numeric (32,4),
	[BetterWorseMid]				numeric (32,4),
	[TotalCoupon]					numeric (32,4),
	[WARF]							numeric (32,4),
	[WARFRecovery]					numeric (32,4),
	[Life]							numeric (32,4),
	[TotalPar]						NUMERIC (32,4),
	[MoodyFacilityRatingAdjustedId] smallint references [CLO].[Rating]([RatingId]),
	[CreatedOn]						DATETIME null default (getdate()),
	[CreatedBy]						VARCHAR(100) null,
	[LastUpdatedOn]					DATETIME null default (getdate()),
	[LastUpdatedBy]					VARCHAR(100) null,
	[MatrixImpliedSpread]			DECIMAL (32,8),
	[MatrixWARFRecovery]			NUMERIC (32,4),
	[zSnPAssetRecoveryRating]		VARCHAR(2),
	[SnpWarf]						NUMERIC (32,8),
	[SnpLgd]						NUMERIC (32,8),
	[MoodysLgd]						NUMERIC (32,8),
	[YieldAvgLgd]					NUMERIC (32,8),
	[SnpAAARecovery]				NUMERIC (32,8),
	[SnPIssuerRatingAdjusted]		VARCHAR(5),
	[MoodyCashFlowRatingAdjustedId] SMALLINT NULL references [CLO].[Rating]([RatingId]),
	PRIMARY KEY ([DateId],[FundId],[SecurityId])
)