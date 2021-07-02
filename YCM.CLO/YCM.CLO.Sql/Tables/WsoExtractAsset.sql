--this table lives in DataMarts
CREATE TABLE dbo.WsoExtractAsset
(
	  ExtractAssetKey INT NOT NULL IDENTITY(1, 1) PRIMARY KEY
	, DatasetKey INT NOT NULL REFERENCES dbo.WsoDatasets (DatasetKey)
	, DateId INT NOT NULL
	, AssetId VARCHAR(50) NOT NULL
	, Asset VARCHAR(100) NULL
	, ABSAverageLifeCollateralDescriptor DECIMAL(28, 9) NULL
	, ABSCategory VARCHAR(50) NULL
	, ABSCurrentFactor DECIMAL(28, 9) NULL
	, ABSSpecificCode INT NULL
	, ABSType VARCHAR(50) NULL
	, AccretedValue DECIMAL(28, 9) NULL
	, AccretionFactor DECIMAL(28, 9) NULL
	, AccruedFees DECIMAL(28, 9) NULL
	, AccruedInterest DECIMAL(28, 9) NULL
	, AggregateAmortCost DECIMAL(28, 9) NULL
	, AssetCategoryMoody VARCHAR(100) NULL
	, AssetCategorySP VARCHAR(100) NULL
	, AssetType VARCHAR(50) NULL
	, AttachedEquity VARCHAR(100) NULL
	, AvgLife DECIMAL(28, 9) NULL
	, CalculationAmount DECIMAL(28, 9) NULL
	, CalculationAmount2 DECIMAL(28, 9) NULL
	, CalculationAmount3 DECIMAL(28, 9) NULL
	, CalendarID INT NULL
	, CallDate DATETIME NULL
	, CapitalizedInterest DECIMAL(28, 9) NULL
	, ConvertibleType VARCHAR(100) NULL
	, Convexity DECIMAL(28, 9) NULL
	, CostBasis VARCHAR(100) NULL
	, CostPrice DECIMAL(28, 9) NULL
	, CountryOfOperation VARCHAR(200) NULL
	, CouponType VARCHAR(100) NULL
	, CreditLinkedAmount DECIMAL(28, 9) NULL
	, CreditLinkedCounterInstitution VARCHAR(200) NULL
	, CurrencyOther VARCHAR(100) NULL
	, CurrencyTypeID INT NULL
	, CurrentYield DECIMAL(28, 9) NULL
	, CurrentYTM DECIMAL(28, 9) NULL
	, CurrentYTW DECIMAL(28, 9) NULL
	, CUSIP VARCHAR(50) NULL
	, DateOffset INT NULL
	, DayCount INT NULL
	, DefaultDate DATETIME NULL
	, DefaultProbabilityMoody DECIMAL(28, 9) NULL
	, DefaultType VARCHAR(100) NULL
	, Description VARCHAR(200) NULL
	, EarnedInterest DECIMAL(28, 9) NULL
	, EmergingMarketRegion VARCHAR(100) NULL
	, ExclusionAmountMoody DECIMAL(28, 9) NULL
	, ExclusionAmountSP DECIMAL(28, 9) NULL
	, ExpectedLossMoody DECIMAL(28, 9) NULL
	, FacilityType VARCHAR(100) NULL
	, FirstPaymentDate DATETIME NULL
	, FirstSettleDate DATETIME NULL
	, FixedRate DECIMAL(28, 9) NULL
	, Frequency INT NULL
	, Guarantor VARCHAR(200) NULL
	, Insurer VARCHAR(200) NULL
	, InterestAccrualDate DATETIME NULL
	, InterestAndFeesProjected DECIMAL(28, 9) NULL
	, InterestAndFeesProjectedForNextPeriod DECIMAL(28, 9) NULL
	, InterestAndFeesToDate DECIMAL(28, 9) NULL
	, InterestAndFeesToDateForNextPeriod DECIMAL(28, 9) NULL
	, IsABS BIT NULL
	, IsBankruptcyRelated BIT NOT NULL
	, IsBridgeSecurity BIT NOT NULL
	, IsCallable BIT NULL
	, IsConflictOfInterest BIT NOT NULL
	, IsConvertible BIT NOT NULL
	, IsCreditAvailable BIT NOT NULL
	, IsCreditDefaultSwap BIT NOT NULL
	, IsCurrentPay BIT NOT NULL
	, IsDeepDiscount BIT NOT NULL
	, IsDefeased BIT NOT NULL
	, IsDeferredInterestOutstanding BIT NOT NULL
	, IsDeferredInterestAllowed BIT NOT NULL
	, IsDerivative BIT NOT NULL
	, IsDIP BIT NOT NULL
	, IsEligibleInvestment BIT NOT NULL
	, IsEmergingMarket BIT NOT NULL
	, IsEnhancedBond BIT NULL
	, IsEstimatedRatingFitch BIT NOT NULL
	, IsEstimatedRatingMoody BIT NOT NULL
	, IsEstimatedRatingSP BIT NOT NULL
	, IsException BIT NOT NULL
	, IsFXRisk BIT NOT NULL
	, IsGovtSponsored BIT NOT NULL
	, IsGuaranteed BIT NULL
	, IsHedged BIT NOT NULL
	, ISIN VARCHAR(50) NULL
	, IsInDefault BIT NOT NULL
	, IsIO BIT NULL
	, IsLBOFund BIT NOT NULL
	, IsLBR6MOAgent BIT NOT NULL
	, IsLBR6MOMajor BIT NOT NULL
	, IsLBR6MOSuper BIT NOT NULL
	, IsLeaseFinancingTransaction BIT NOT NULL
	, IsMezzanine BIT NOT NULL
	, IsMostSenior BIT NULL
	, IsOffer BIT NOT NULL
	, IsOriginal BIT NOT NULL
	, IsPIKSecurity BIT NOT NULL
	, IsPO BIT NULL
	, IsPreferredSecurity BIT NOT NULL
	, IsRatingFitchDerived BIT NOT NULL
	, IsRatingMoodyDerived BIT NOT NULL
	, IsRatingSPDerived BIT NOT NULL
	, IsResecuritization BIT NOT NULL
	, IsRestructured BIT NULL
	, IsSFO BIT NOT NULL
	, IsSovereign BIT NOT NULL
	, IsSpecialSituation BIT NOT NULL
	, IsSPVJurisdiction BIT NOT NULL
	, Issuer VARCHAR(255) NULL
	, IssueDate DATETIME NULL
	, IssueSize DECIMAL(28, 9) NULL
	, IssuingTransaction VARCHAR(255) NULL
	, IsSynthetic BIT NOT NULL
	, IsSyntheticCounterInstitutionInDefault BIT NOT NULL
	, IsTaxJurisdiction BIT NOT NULL
	, LCAmount DECIMAL(28, 9) NULL
	, LeadUnderwriter VARCHAR(200) NULL
	, LiborOptionExists BIT NOT NULL
	, LienType VARCHAR(50) NULL
	, Liquidity VARCHAR(100) NULL
	, LongDatedAmount DECIMAL(28, 9) NULL
	, LossRateMoody DECIMAL(28, 9) NULL
	, LossSeverityMoody DECIMAL(28, 9) NULL
	, MacaulayDuration DECIMAL(28, 9) NULL
	, MarkDate DATETIME NULL
	, MarketValue DECIMAL(28, 9) NULL
	, MarketValueAdjustedMoody DECIMAL(28, 9) NULL
	, MarketValueAdjustedSP DECIMAL(28, 9) NULL
	, MarketValueOCMoody DECIMAL(28, 9) NULL
	, MarketValueOCSP DECIMAL(28, 9) NULL
	, MarketValueSettleTrue DECIMAL(28, 9) NULL
	, MarkFactor INT NULL
	, MarkPrice DECIMAL(28, 14) NULL
	, MaturityDate DATETIME NULL
	, MaturityExceptions VARCHAR(100) NULL
	, ModDuration DECIMAL(28, 9) NULL
	, NextPaymentDate DATETIME NULL
	, OriginalCapPercent DECIMAL(28, 9) NULL
	, OriginalPAPI DECIMAL(28, 9) NULL
	, Outstanding DECIMAL(28, 9) NULL
	, OutstandingAdjusted DECIMAL(28, 9) NULL
	, PAI DECIMAL(28, 9) NULL
	, ParAmount DECIMAL(28, 9) NULL
	, ParAmountTraded DECIMAL(28, 9) NULL
	, PaymentNonBusinessDirection INT NULL
	, PaymentOffsetDays INT NULL
	, PaymentOffsetDirection INT NULL
	, PaymentOffsetType INT NULL
	, PIKBeginDate DATETIME NULL
	, PIKCapInterest DECIMAL(28, 9) NULL
	, PIKMargin DECIMAL(28, 9) NULL
	, PIKPercentage DECIMAL(28, 9) NULL
	, PrimeOptionExists BIT NOT NULL
	, PrincipalBalance DECIMAL(28, 9) NULL
	, PrincipalBalanceEI DECIMAL(28, 9) NULL
	, PriorityCategoryFitch INT NULL
	, PriorityCategoryMoody INT NULL
	, PriorityCategorySP INT NULL
	, PutDate DATETIME NULL
	, Quantity DECIMAL(28, 9) NULL
	, RateAdjustFreq INT NULL
	, RateOption VARCHAR(100) NULL
	, RatingMoody VARCHAR(50) NULL
	, RatingMoodyAtIssuance VARCHAR(50) NULL
	, RatingMoodyDerived VARCHAR(50) NULL
	, RatingMoodyIssuance VARCHAR(50) NULL
	, RatingMoodySecurity VARCHAR(50) NULL
	, RatingMoodyShort VARCHAR(50) NULL
	, RatingFitch VARCHAR(50) NULL
	, RatingFitchAtIssuance VARCHAR(50) NULL
	, RatingFitchDerived VARCHAR(50) NULL
	, RatingFitchSecurity VARCHAR(50) NULL
	, RatingSP VARCHAR(50) NULL
	, RatingSPAtIssuance VARCHAR(50) NULL
	, RatingSPDerived VARCHAR(50) NULL
	, RatingSPSecurity VARCHAR(50) NULL
	, RatingSPShort VARCHAR(50) NULL
	, RatingDCR VARCHAR(50) NULL
	, RatingDCRDerived VARCHAR(50) NULL
	, Region VARCHAR(100) NULL
	, RecoveryRateFitch DECIMAL(28, 9) NULL
	, RecoveryRateMoody DECIMAL(28, 9) NULL
	, RecoveryRateSP DECIMAL(28, 9) NULL
	, ReferenceAssetID VARCHAR(50) NULL
	, RegistrationType VARCHAR(100) NULL
	, SecurityID VARCHAR(100) NULL
	, SecurityLevel VARCHAR(100) NULL
	, SecurityLevelFitch VARCHAR(100) NULL
	, SecurityLevelMoody VARCHAR(100) NULL
	, SecurityLevelSP VARCHAR(100) NULL
	, SelfPurchasedAmount DECIMAL(28, 9) NULL
	, Servicer VARCHAR(200) NULL
	, SFOType VARCHAR(100) NULL
	, SpreadLibor DECIMAL(28, 9) NULL
	, SpreadPrime DECIMAL(28, 9) NULL
	, StdDevMoody DECIMAL(28, 9) NULL
	, SyntheticCounterInstitution VARCHAR(200) NULL
	, SyntheticType VARCHAR(100) NULL
	, TradesWithAccrued BIT NULL
	, TransferAgent VARCHAR(200) NULL
	, UpgradedDowngraded VARCHAR(100) NULL
	, WeightedAvgAllInRate DECIMAL(28, 9) NULL
	, WeightedAvgSpread DECIMAL(28, 9) NULL
	, WeightedAvgDaysToReset DECIMAL(28, 9) NULL
	, WeightedAvgDaysToResetAmount DECIMAL(28, 9) NULL
	, YieldToWorst DECIMAL(28, 9) NULL
	, YTM DECIMAL(28, 9) NULL
	, YTW DECIMAL(28, 9) NULL
	, UserAmount1 DECIMAL(28, 9) NULL
	, UserAmount2 DECIMAL(28, 9) NULL
	, UserAmount3 DECIMAL(28, 9) NULL
	, UserAmount4 DECIMAL(28, 9) NULL
	, UserAmount5 DECIMAL(28, 9) NULL
	, UserAmount6 DECIMAL(28, 9) NULL
	, UserAmount7 DECIMAL(28, 9) NULL
	, UserAmount8 DECIMAL(28, 9) NULL
	, UserAmount9 DECIMAL(28, 9) NULL
	, UserAmount10 DECIMAL(28, 9) NULL
	, UserAmount11 DECIMAL(28, 9) NULL
	, UserAmount12 DECIMAL(28, 9) NULL
	, UserAmount13 DECIMAL(28, 9) NULL
	, UserAmount14 DECIMAL(28, 9) NULL
	, UserAmount15 DECIMAL(28, 9) NULL
	, UserAmount16 DECIMAL(28, 9) NULL
	, UserAmount17 DECIMAL(28, 9) NULL
	, UserAmount18 DECIMAL(28, 9) NULL
	, UserAmount19 DECIMAL(28, 9) NULL
	, UserAmount20 DECIMAL(28, 9) NULL
	, UserFlag1 BIT NULL
	, UserFlag2 BIT NULL
	, UserFlag3 BIT NULL
	, UserFlag4 BIT NULL
	, UserFlag5 BIT NULL
	, UserFlag6 BIT NULL
	, UserFlag7 BIT NULL
	, UserFlag8 BIT NULL
	, UserFlag9 BIT NULL
	, UserFlag10 BIT NULL
	, UserFlag11 BIT NULL
	, UserFlag12 BIT NULL
	, UserFlag13 BIT NULL
	, UserFlag14 BIT NULL
	, UserFlag15 BIT NULL
	, UserFlag16 BIT NULL
	, UserFlag17 BIT NULL
	, UserFlag18 BIT NULL
	, UserFlag19 BIT NULL
	, UserFlag20 BIT NULL
	, UserString1 VARCHAR(100) NULL
	, UserString2 VARCHAR(100) NULL
	, UserString3 VARCHAR(100) NULL
	, UserString4 VARCHAR(100) NULL
	, UserString5 VARCHAR(100) NULL
	, UserString6 VARCHAR(100) NULL
	, UserString7 VARCHAR(100) NULL
	, UserString8 VARCHAR(100) NULL
	, UserString9 VARCHAR(100) NULL
	, UserString10 VARCHAR(100) NULL
	, UserPercentage1 FLOAT NULL
	, UserPercentage2 FLOAT NULL
	, UserPercentage3 FLOAT NULL
	, UserPercentage4 FLOAT NULL
	, UserPercentage5 FLOAT NULL
	, UserDecimal1 DECIMAL(28, 9) NULL
	, UserDecimal2 DECIMAL(28, 9) NULL
	, UserDecimal3 DECIMAL(28, 9) NULL
	, UserDecimal4 DECIMAL(28, 9) NULL
	, UserDecimal5 DECIMAL(28, 9) NULL
	, UserNumber1 INT NULL
	, UserNumber2 INT NULL
	, UserNumber3 INT NULL
	, UserNumber4 INT NULL
	, UserNumber5 INT NULL
	, UserDate1 DATETIME NULL
	, UserDate2 DATETIME NULL
	, UserDate3 DATETIME NULL
	, UserDate4 DATETIME NULL
	, UserDate5 DATETIME NULL
	, FacilityIncrease DECIMAL(28, 9) NULL
	, ParAmountNativeHypo DECIMAL(28, 9) NULL
	, OutstandingNativeHypo DECIMAL(28, 9) NULL
	, BankDeal_GlobalAmount DECIMAL(28, 9) NULL
	, ExcessCurrentPay DECIMAL(28, 9) NULL
	, ExcessMoodyCaa DECIMAL(28, 9) NULL
	, ExcessSPCCC DECIMAL(28, 9) NULL
	, FeeSpread DECIMAL(28, 9) NULL
	, InterestFromReinvestment DECIMAL(28, 9) NULL
	, IsDelayedDraw BIT NULL
	, IsRatingMoodyPending BIT NULL
	, LiborBaseRateFloor DECIMAL(28, 9) NULL
	, MarkPrice_BidPrice DECIMAL(28, 14) NULL
	, PaymentDate DATETIME NULL
	, PrimarySpread DECIMAL(28, 9) NULL
	, PrimarySpreadType NVARCHAR(255) NULL
	, PrimeBaseRateFloor DECIMAL(28, 9) NULL
	, PriorityCategorySPI INT NULL
	, PriorityCategorySPII INT NULL
	, PriorityCategorySPIII INT NULL
	, PriorityCategorySPIV INT NULL
	, PriorityCategorySPV INT NULL
	, PriorityCategorySPVI INT NULL
	, RatingMoodyIsShadow BIT NULL
	, RatingMoodyIssue NVARCHAR(255) NULL
	, RatingSPIssue NVARCHAR(255) NULL
	, RatingSPIssueCreditWatch NVARCHAR(255) NULL
	, RecoveryAmountCurrentPay DECIMAL(28, 9) NULL
	, RecoveryAmountMoodyCaa DECIMAL(28, 9) NULL
	, RecoveryAmountSPCCC DECIMAL(28, 9) NULL
	, RecoveryRateMoodyFeed DECIMAL(28, 9) NULL
	, RecoveryRateSPI DECIMAL(28, 9) NULL
	, RecoveryRateSPII DECIMAL(28, 9) NULL
	, RecoveryRateSPIII DECIMAL(28, 9) NULL
	, RecoveryRateSPIV DECIMAL(28, 9) NULL
	, RecoveryRateSPV DECIMAL(28, 9) NULL
	, RecoveryRateSPVI DECIMAL(28, 9) NULL
	, UserString11 NVARCHAR(255) NULL
	, UserString12 NVARCHAR(255) NULL
	, UserString13 NVARCHAR(255) NULL
	, UserString14 NVARCHAR(255) NULL
	, UserString15 NVARCHAR(255) NULL
	, UserString16 NVARCHAR(255) NULL
	, CoreSystem VARCHAR(100) NULL
	, RecordModified BIT NULL
	, Scenario SMALLINT NULL
	, MarkPrice_AskPrice DECIMAL(28, 9) NULL
	, RatingMoodyIssueCreditWatch NVARCHAR(255) NULL
	, UserPercentage6 FLOAT NULL
	, CapitalizedInterestOrig DECIMAL(28, 9) NULL
)
GO
CREATE CLUSTERED INDEX CIX_WsoExtractAsset ON dbo.WsoExtractAsset (DatasetKey, DateId, AssetId)
GO
CREATE NONCLUSTERED INDEX IX_WsoExtractAsset_DateId_SecurityId ON dbo.WsoExtractAsset (DateId, SecurityID) INCLUDE (IssueSize)
GO
