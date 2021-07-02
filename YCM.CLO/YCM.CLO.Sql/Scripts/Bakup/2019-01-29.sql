USE DataMarts
GO

ALTER TABLE dbo.WsoExtractAsset
	ADD SnPAssetRecoveryRating VARCHAR(255) NULL
GO

UPDATE [dbo].[WsoDatasetQuery]
SET sql = 'SELECT  tblAssets.AssetId ,
        tblAssets.Asset ,
        tblAssets.ABSAverageLifeCollateralDescriptor ,
        tblAssets.ABSCategory ,
        tblAssets.ABSCurrentFactor ,
        tblAssets.ABSSpecificCode ,
        tblAssets.ABSType ,
        tblAssets.AccretedValue ,
        tblAssets.AccretionFactor ,
        tblAssets.AccruedFees ,
        tblAssets.AccruedInterest ,
        tblAssets.AggregateAmortCost ,
        tblAssets.AssetCategoryMoody ,
        tblAssets.AssetCategorySP ,
        tblAssets.AssetType ,
        tblAssets.AttachedEquity ,
        tblAssets.AvgLife ,
        tblAssets.CalculationAmount ,
        tblAssets.CalculationAmount2 ,
        tblAssets.CalculationAmount3 ,
        tblAssets.CalendarID ,
        tblAssets.CallDate ,
        tblAssets.CapitalizedInterest ,
        tblAssets.ConvertibleType ,
        tblAssets.Convexity ,
        tblAssets.CostBasis ,
        tblAssets.CostPrice ,
        tblAssets.CountryOfOperation ,
        tblAssets.CouponType ,
        tblAssets.CreditLinkedAmount ,
        tblAssets.CreditLinkedCounterInstitution ,
        tblAssets.CurrencyOther ,
        tblAssets.CurrencyTypeID ,
        tblAssets.CurrentYield ,
        tblAssets.CurrentYTM ,
        tblAssets.CurrentYTW ,
        tblAssets.CUSIP ,
        tblAssets.DateOffset ,
        tblAssets.DayCount ,
        tblAssets.DefaultDate ,
        tblAssets.DefaultProbabilityMoody ,
        tblAssets.DefaultType ,
        tblAssets.Description ,
        tblAssets.EarnedInterest ,
        tblAssets.EmergingMarketRegion ,
        tblAssets.ExclusionAmountMoody ,
        tblAssets.ExclusionAmountSP ,
        tblAssets.ExpectedLossMoody ,
        tblAssets.FacilityType ,
        tblAssets.FirstPaymentDate ,
        tblAssets.FirstSettleDate ,
        tblAssets.FixedRate ,
        tblAssets.Frequency ,
        tblAssets.Guarantor ,
        tblAssets.Insurer ,
        tblAssets.InterestAccrualDate ,
        tblAssets.InterestAndFeesProjected ,
        tblAssets.InterestAndFeesProjectedForNextPeriod ,
        tblAssets.InterestAndFeesToDate ,
        tblAssets.InterestAndFeesToDateForNextPeriod ,
        tblAssets.IsABS ,
        tblAssets.IsBankruptcyRelated ,
        tblAssets.IsBridgeSecurity ,
        tblAssets.IsCallable ,
        tblAssets.IsConflictOfInterest ,
        tblAssets.IsConvertible ,
        tblAssets.IsCreditAvailable ,
        tblAssets.IsCreditDefaultSwap ,
        tblAssets.IsCurrentPay ,
        tblAssets.IsDeepDiscount ,
        tblAssets.IsDefeased ,
        tblAssets.IsDeferredInterestOutstanding ,
        tblAssets.IsDeferredInterestAllowed ,
        tblAssets.IsDerivative ,
        tblAssets.IsDIP ,
        tblAssets.IsEligibleInvestment ,
        tblAssets.IsEmergingMarket ,
        tblAssets.IsEnhancedBond ,
        tblAssets.IsEstimatedRatingFitch ,
        tblAssets.IsEstimatedRatingMoody ,
        tblAssets.IsEstimatedRatingSP ,
        tblAssets.IsException ,
        tblAssets.IsFXRisk ,
        tblAssets.IsGovtSponsored ,
        tblAssets.IsGuaranteed ,
        tblAssets.IsHedged ,
        tblAssets.ISIN ,
        tblAssets.IsInDefault ,
        tblAssets.IsIO ,
        tblAssets.IsLBOFund ,
        tblAssets.IsLBR6MOAgent ,
        tblAssets.IsLBR6MOMajor ,
        tblAssets.IsLBR6MOSuper ,
        tblAssets.IsLeaseFinancingTransaction ,
        tblAssets.IsMezzanine ,
        tblAssets.IsMostSenior ,
        tblAssets.IsOffer ,
        tblAssets.IsOriginal ,
        tblAssets.IsPIKSecurity ,
        tblAssets.IsPO ,
        tblAssets.IsPreferredSecurity ,
        tblAssets.IsRatingFitchDerived ,
        tblAssets.IsRatingMoodyDerived ,
        tblAssets.IsRatingSPDerived ,
        tblAssets.IsResecuritization ,
        tblAssets.IsRestructured ,
        tblAssets.IsSFO ,
        tblAssets.IsSovereign ,
        tblAssets.IsSpecialSituation ,
        tblAssets.IsSPVJurisdiction ,
        tblAssets.Issuer ,
        tblAssets.IssueDate ,
        tblAssets.IssueSize ,
        tblAssets.IssuingTransaction ,
        tblAssets.IsSynthetic ,
        tblAssets.IsSyntheticCounterInstitutionInDefault ,
        tblAssets.IsTaxJurisdiction ,
        tblAssets.LCAmount ,
        tblAssets.LeadUnderwriter ,
        tblAssets.LiborOptionExists ,
        tblAssets.LienType ,
        tblAssets.Liquidity ,
        tblAssets.LongDatedAmount ,
        tblAssets.LossRateMoody ,
        tblAssets.LossSeverityMoody ,
        tblAssets.MacaulayDuration ,
        tblAssets.MarkDate ,
        tblAssets.MarketValue ,
        tblAssets.MarketValueAdjustedMoody ,
        tblAssets.MarketValueAdjustedSP ,
        tblAssets.MarketValueOCMoody ,
        tblAssets.MarketValueOCSP ,
        tblAssets.MarketValueSettleTrue ,
        tblAssets.MarkFactor ,
        tblAssets.MarkPrice ,
        tblAssets.MaturityDate ,
        tblAssets.MaturityExceptions ,
        tblAssets.ModDuration ,
        tblAssets.NextPaymentDate ,
        tblAssets.OriginalCapPercent ,
        tblAssets.OriginalPAPI ,
        tblAssets.Outstanding ,
        tblAssets.OutstandingAdjusted ,
        tblAssets.PAI ,
        tblAssets.ParAmount ,
        tblAssets.ParAmountTraded ,
        tblAssets.PaymentNonBusinessDirection ,
        tblAssets.PaymentOffsetDays ,
        tblAssets.PaymentOffsetDirection ,
        tblAssets.PaymentOffsetType ,
        tblAssets.PIKBeginDate ,
        tblAssets.PIKCapInterest ,
        tblAssets.PIKMargin ,
        tblAssets.PIKPercentage ,
        tblAssets.PrimeOptionExists ,
        tblAssets.PrincipalBalance ,
        tblAssets.PrincipalBalanceEI ,
        tblAssets.PriorityCategoryFitch ,
        tblAssets.PriorityCategoryMoody ,
        tblAssets.PriorityCategorySP ,
        tblAssets.PutDate ,
        tblAssets.Quantity ,
        tblAssets.RateAdjustFreq ,
        tblAssets.RateOption ,
        tblAssets.RatingMoody ,
        tblAssets.RatingMoodyAtIssuance ,
        tblAssets.RatingMoodyDerived ,
        tblAssets.RatingMoodyIssuance ,
        tblAssets.RatingMoodySecurity ,
        tblAssets.RatingMoodyShort ,
        tblAssets.RatingFitch ,
        tblAssets.RatingFitchAtIssuance ,
        tblAssets.RatingFitchDerived ,
        tblAssets.RatingFitchSecurity ,
        tblAssets.RatingSP ,
        tblAssets.RatingSPAtIssuance ,
        tblAssets.RatingSPDerived ,
        tblAssets.RatingSPSecurity ,
        tblAssets.RatingSPShort ,
        tblAssets.RatingDCR ,
        tblAssets.RatingDCRDerived ,
        tblAssets.Region ,
        tblAssets.RecoveryRateFitch ,
        tblAssets.RecoveryRateMoody ,
        tblAssets.RecoveryRateSP ,
        tblAssets.ReferenceAssetID ,
        tblAssets.RegistrationType ,
        tblAssets.SecurityID ,
        tblAssets.SecurityLevel ,
        tblAssets.SecurityLevelFitch ,
        tblAssets.SecurityLevelMoody ,
        tblAssets.SecurityLevelSP ,
        tblAssets.SelfPurchasedAmount ,
        tblAssets.Servicer ,
        tblAssets.SFOType ,
        tblAssets.SpreadLibor ,
        tblAssets.SpreadPrime ,
        tblAssets.StdDevMoody ,
        tblAssets.SyntheticCounterInstitution ,
        tblAssets.SyntheticType ,
        tblAssets.TradesWithAccrued ,
        tblAssets.TransferAgent ,
        tblAssets.UpgradedDowngraded ,
        tblAssets.WeightedAvgAllInRate ,
        tblAssets.WeightedAvgSpread ,
        tblAssets.WeightedAvgDaysToReset ,
        tblAssets.WeightedAvgDaysToResetAmount ,
        tblAssets.YieldToWorst ,
        tblAssets.YTM ,
        tblAssets.YTW ,
        tblAssets.UserAmount1 ,
        tblAssets.UserAmount2 ,
        tblAssets.UserAmount3 ,
        tblAssets.UserAmount4 ,
        tblAssets.UserAmount5 ,
        tblAssets.UserAmount6 ,
        tblAssets.UserAmount7 ,
        tblAssets.UserAmount8 ,
        tblAssets.UserAmount9 ,
        tblAssets.UserAmount10 ,
        tblAssets.UserAmount11 ,
        tblAssets.UserAmount12 ,
        tblAssets.UserAmount13 ,
        tblAssets.UserAmount14 ,
        tblAssets.UserAmount15 ,
        tblAssets.UserAmount16 ,
        tblAssets.UserAmount17 ,
        tblAssets.UserAmount18 ,
        tblAssets.UserAmount19 ,
        tblAssets.UserAmount20 ,
        tblAssets.UserFlag1 ,
        tblAssets.UserFlag2 ,
        tblAssets.UserFlag3 ,
        tblAssets.UserFlag4 ,
        tblAssets.UserFlag5 ,
        tblAssets.UserFlag6 ,
        tblAssets.UserFlag7 ,
        tblAssets.UserFlag8 ,
        tblAssets.UserFlag9 ,
        tblAssets.UserFlag10 ,
        tblAssets.UserFlag11 ,
        tblAssets.UserFlag12 ,
        tblAssets.UserFlag13 ,
        tblAssets.UserFlag14 ,
        tblAssets.UserFlag15 ,
        tblAssets.UserFlag16 ,
        tblAssets.UserFlag17 ,
        tblAssets.UserFlag18 ,
        tblAssets.UserFlag19 ,
        tblAssets.UserFlag20 ,
        tblAssets.UserString1 ,
        tblAssets.UserString2 ,
        tblAssets.UserString3 ,
        tblAssets.UserString4 ,
        tblAssets.UserString5 ,
        tblAssets.UserString6 ,
        tblAssets.UserString7 ,
        tblAssets.UserString8 ,
        tblAssets.UserString9 ,
        tblAssets.UserString10 ,
        tblAssets.UserPercentage1 ,
        tblAssets.UserPercentage2 ,
        tblAssets.UserPercentage3 ,
        tblAssets.UserPercentage4 ,
        tblAssets.UserPercentage5 ,
        tblAssets.UserDecimal1 ,
        tblAssets.UserDecimal2 ,
        tblAssets.UserDecimal3 ,
        tblAssets.UserDecimal4 ,
        tblAssets.UserDecimal5 ,
        tblAssets.UserNumber1 ,
        tblAssets.UserNumber2 ,
        tblAssets.UserNumber3 ,
        tblAssets.UserNumber4 ,
        tblAssets.UserNumber5 ,
        tblAssets.UserDate1 ,
        tblAssets.UserDate2 ,
        tblAssets.UserDate3 ,
        tblAssets.UserDate4 ,
        tblAssets.UserDate5 ,
        tblAssets.FacilityIncrease ,
        tblAssets.ParAmountNativeHypo ,
        tblAssets.OutstandingNativeHypo ,
        tblAssetsUDFs.BankDeal_GlobalAmount ,
        tblAssetsUDFs.ExcessCurrentPay ,
        tblAssetsUDFs.ExcessMoodyCaa ,
        tblAssetsUDFs.ExcessSPCCC ,
        tblAssetsUDFs.FeeSpread ,
        tblAssetsUDFs.InterestFromReinvestment ,
        tblAssetsUDFs.IsDelayedDraw ,
        tblAssetsUDFs.IsRatingMoodyPending ,
        tblAssetsUDFs.LiborBaseRateFloor ,
        tblAssetsUDFs.MarkPrice_BidPrice ,
        tblAssetsUDFs.PaymentDate ,
        tblAssetsUDFs.PrimarySpread ,
        tblAssetsUDFs.PrimarySpreadType ,
        tblAssetsUDFs.PrimeBaseRateFloor ,
        tblAssetsUDFs.PriorityCategorySPI ,
        tblAssetsUDFs.PriorityCategorySPII ,
        tblAssetsUDFs.PriorityCategorySPIII ,
        tblAssetsUDFs.PriorityCategorySPIV ,
        tblAssetsUDFs.PriorityCategorySPV ,
        tblAssetsUDFs.PriorityCategorySPVI ,
        tblAssetsUDFs.RatingMoodyIsShadow ,
        tblAssetsUDFs.RatingMoodyIssue ,
        tblAssetsUDFs.RatingSPIssue ,
        tblAssetsUDFs.RatingSPIssueCreditWatch ,
        tblAssetsUDFs.RecoveryAmountCurrentPay ,
        tblAssetsUDFs.RecoveryAmountMoodyCaa ,
        tblAssetsUDFs.RecoveryAmountSPCCC ,
        tblAssetsUDFs.RecoveryRateMoodyFeed ,
        tblAssetsUDFs.RecoveryRateSPI ,
        tblAssetsUDFs.RecoveryRateSPII ,
        tblAssetsUDFs.RecoveryRateSPIII ,
        tblAssetsUDFs.RecoveryRateSPIV ,
        tblAssetsUDFs.RecoveryRateSPV ,
        tblAssetsUDFs.RecoveryRateSPVI ,
        tblAssetsUDFs.UserString11 ,
        tblAssetsUDFs.UserString12 ,
        tblAssetsUDFs.UserString13 ,
        tblAssetsUDFs.UserString14 ,
        tblAssetsUDFs.UserString15 ,
        tblAssetsUDFs.UserString16 ,
        tblAssets.CoreSystem ,

        tblAssets.RecordModified ,
        tblAssets.Scenario ,
        tblAssetsUDFs.MarkPrice_AskPrice ,
        tblAssetsUDFs.RatingMoodyIssueCreditWatch ,
        tblAssetsUDFs.UserPercentage6,
		tblAssetsUDFs.CapitalizedInterestOrig,
		tblAssetsUDFs.SnPAssetRecoveryRating
FROM    tblAssets
INNER JOIN (SELECT  DatasetID ,
                    Key1Value AS AssetId ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''BankDeal_GlobalAmount''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS BankDeal_GlobalAmount ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''ExcessCurrentPay''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS ExcessCurrentPay ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''ExcessMoodyCaa''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS ExcessMoodyCaa ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''ExcessSPCCC''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS ExcessSPCCC ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''FeeSpread''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS FeeSpread ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''InterestFromReinvestment''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS InterestFromReinvestment ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''IsDelayedDraw''
                                         THEN BooleanValue
                                         ELSE 0
                                    END), 0) AS BIT) AS IsDelayedDraw ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''IsRatingMoodyPending''
                                         THEN BooleanValue
                                         ELSE 0
                                    END), 0) AS BIT) AS IsRatingMoodyPending ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''LiborBaseRateFloor''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS LiborBaseRateFloor ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''MarkPrice_AskPrice''
                                         THEN SingleValue
                                         ELSE NULL
                                    END), 0) AS REAL) AS MarkPrice_AskPrice ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''MarkPrice_BidPrice''
                                         THEN SingleValue
                                         ELSE NULL
                                    END), 0) AS REAL) AS MarkPrice_BidPrice ,
                    MAX(CASE WHEN FieldName = ''PaymentDate'' THEN DateValue
                             ELSE NULL
                        END) AS PaymentDate ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PrimarySpread''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS PrimarySpread ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PrimarySpreadType''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS PrimarySpreadType ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PrimeBaseRateFloor''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS PrimeBaseRateFloor ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPI''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPII''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPIII''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPIII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPIV''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPIV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPV''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPVI''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPVI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingMoodyIsShadow''
                                         THEN BooleanValue
                                         ELSE 0
                                    END), 0) AS BIT) AS RatingMoodyIsShadow ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingMoodyIssue''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingMoodyIssue ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingMoodyIssueCreditWatch''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingMoodyIssueCreditWatch ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingSPIssue''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingSPIssue ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingSPIssueCreditWatch''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingSPIssueCreditWatch ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryAmountCurrentPay''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS RecoveryAmountCurrentPay ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryAmountMoodyCaa''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS RecoveryAmountMoodyCaa ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryAmountSPCCC''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS RecoveryAmountSPCCC ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateMoodyFeed''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateMoodyFeed ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPI''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPII''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPIII''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPIII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPIV''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPIV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPV''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPVI''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPVI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserPercentage6''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS UserPercentage6 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString11''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString11 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString12''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString12 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString13''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString13 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString14''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString14 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString15''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString15 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString16''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString16,
					CAST( ISNULL(MAX(CASE WHEN FieldName = ''CapitalizedInterestOrig'' 
										THEN CurrencyValue 
										ELSE NULL  
										END), 0) AS money) AS ''CapitalizedInterestOrig'',
					CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString18''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS SnPAssetRecoveryRating					
            FROM    tblUDFData
            WHERE   DatasetID = {0}
                    AND TableName = ''tblAssets''
            GROUP BY DatasetID ,
                    Key1Value
           ) tblAssetsUDFs ON tblAssets.DatasetID = tblAssetsUDFs.DatasetID
                              AND tblAssets.AssetId = tblAssetsUDFs.AssetId
                              AND tblAssetsUDFs.DatasetID = {0}
WHERE   tblAssets.DatasetID = {0};'
WHERE Watcher = 'DatasetArchiveWatcher'  AND DestinationTable = 'dbo.WsoExtractAsset'
GO

UPDATE dbo.WsoDatasetQuery
SET sql = 'SELECT  tblAssets.AssetId ,
        tblAssets.Asset ,
        tblAssets.ABSAverageLifeCollateralDescriptor ,
        tblAssets.ABSCategory ,
        tblAssets.ABSCurrentFactor ,
        tblAssets.ABSSpecificCode ,
        tblAssets.ABSType ,
        tblAssets.AccretedValue ,
        tblAssets.AccretionFactor ,
        tblAssets.AccruedFees ,
        tblAssets.AccruedInterest ,
        tblAssets.AggregateAmortCost ,
        tblAssets.AssetCategoryMoody ,
        tblAssets.AssetCategorySP ,
        tblAssets.AssetType ,
        tblAssets.AttachedEquity ,
        tblAssets.AvgLife ,
        tblAssets.CalculationAmount ,
        tblAssets.CalculationAmount2 ,
        tblAssets.CalculationAmount3 ,
        tblAssets.CalendarID ,
        tblAssets.CallDate ,
        tblAssets.CapitalizedInterest ,
        tblAssets.ConvertibleType ,
        tblAssets.Convexity ,
        tblAssets.CostBasis ,
        tblAssets.CostPrice ,
        tblAssets.CountryOfOperation ,
        tblAssets.CouponType ,
        tblAssets.CreditLinkedAmount ,
        tblAssets.CreditLinkedCounterInstitution ,
        tblAssets.CurrencyOther ,
        tblAssets.CurrencyTypeID ,
        tblAssets.CurrentYield ,
        tblAssets.CurrentYTM ,
        tblAssets.CurrentYTW ,
        tblAssets.CUSIP ,
        tblAssets.DateOffset ,
        tblAssets.DayCount ,
        tblAssets.DefaultDate ,
        tblAssets.DefaultProbabilityMoody ,
        tblAssets.DefaultType ,
        tblAssets.Description ,
        tblAssets.EarnedInterest ,
        tblAssets.EmergingMarketRegion ,
        tblAssets.ExclusionAmountMoody ,
        tblAssets.ExclusionAmountSP ,
        tblAssets.ExpectedLossMoody ,
        tblAssets.FacilityType ,
        tblAssets.FirstPaymentDate ,
        tblAssets.FirstSettleDate ,
        tblAssets.FixedRate ,
        tblAssets.Frequency ,
        tblAssets.Guarantor ,
        tblAssets.Insurer ,
        tblAssets.InterestAccrualDate ,
        tblAssets.InterestAndFeesProjected ,
        tblAssets.InterestAndFeesProjectedForNextPeriod ,
        tblAssets.InterestAndFeesToDate ,
        tblAssets.InterestAndFeesToDateForNextPeriod ,
        tblAssets.IsABS ,
        tblAssets.IsBankruptcyRelated ,
        tblAssets.IsBridgeSecurity ,
        tblAssets.IsCallable ,
        tblAssets.IsConflictOfInterest ,
        tblAssets.IsConvertible ,
        tblAssets.IsCreditAvailable ,
        tblAssets.IsCreditDefaultSwap ,
        tblAssets.IsCurrentPay ,
        tblAssets.IsDeepDiscount ,
        tblAssets.IsDefeased ,
        tblAssets.IsDeferredInterestOutstanding ,
        tblAssets.IsDeferredInterestAllowed ,
        tblAssets.IsDerivative ,
        tblAssets.IsDIP ,
        tblAssets.IsEligibleInvestment ,
        tblAssets.IsEmergingMarket ,
        tblAssets.IsEnhancedBond ,
        tblAssets.IsEstimatedRatingFitch ,
        tblAssets.IsEstimatedRatingMoody ,
        tblAssets.IsEstimatedRatingSP ,
        tblAssets.IsException ,
        tblAssets.IsFXRisk ,
        tblAssets.IsGovtSponsored ,
        tblAssets.IsGuaranteed ,
        tblAssets.IsHedged ,
        tblAssets.ISIN ,
        tblAssets.IsInDefault ,
        tblAssets.IsIO ,
        tblAssets.IsLBOFund ,
        tblAssets.IsLBR6MOAgent ,
        tblAssets.IsLBR6MOMajor ,
        tblAssets.IsLBR6MOSuper ,
        tblAssets.IsLeaseFinancingTransaction ,
        tblAssets.IsMezzanine ,
        tblAssets.IsMostSenior ,
        tblAssets.IsOffer ,
        tblAssets.IsOriginal ,
        tblAssets.IsPIKSecurity ,
        tblAssets.IsPO ,
        tblAssets.IsPreferredSecurity ,
        tblAssets.IsRatingFitchDerived ,
        tblAssets.IsRatingMoodyDerived ,
        tblAssets.IsRatingSPDerived ,
        tblAssets.IsResecuritization ,
        tblAssets.IsRestructured ,
        tblAssets.IsSFO ,
        tblAssets.IsSovereign ,
        tblAssets.IsSpecialSituation ,
        tblAssets.IsSPVJurisdiction ,
        tblAssets.Issuer ,
        tblAssets.IssueDate ,
        tblAssets.IssueSize ,
        tblAssets.IssuingTransaction ,
        tblAssets.IsSynthetic ,
        tblAssets.IsSyntheticCounterInstitutionInDefault ,
        tblAssets.IsTaxJurisdiction ,
        tblAssets.LCAmount ,
        tblAssets.LeadUnderwriter ,
        tblAssets.LiborOptionExists ,
        tblAssets.LienType ,
        tblAssets.Liquidity ,
        tblAssets.LongDatedAmount ,
        tblAssets.LossRateMoody ,
        tblAssets.LossSeverityMoody ,
        tblAssets.MacaulayDuration ,
        tblAssets.MarkDate ,
        tblAssets.MarketValue ,
        tblAssets.MarketValueAdjustedMoody ,
        tblAssets.MarketValueAdjustedSP ,
        tblAssets.MarketValueOCMoody ,
        tblAssets.MarketValueOCSP ,
        tblAssets.MarketValueSettleTrue ,
        tblAssets.MarkFactor ,
        tblAssets.MarkPrice ,
        tblAssets.MaturityDate ,
        tblAssets.MaturityExceptions ,
        tblAssets.ModDuration ,
        tblAssets.NextPaymentDate ,
        tblAssets.OriginalCapPercent ,
        tblAssets.OriginalPAPI ,
        tblAssets.Outstanding ,
        tblAssets.OutstandingAdjusted ,
        tblAssets.PAI ,
        tblAssets.ParAmount ,
        tblAssets.ParAmountTraded ,
        tblAssets.PaymentNonBusinessDirection ,
        tblAssets.PaymentOffsetDays ,
        tblAssets.PaymentOffsetDirection ,
        tblAssets.PaymentOffsetType ,
        tblAssets.PIKBeginDate ,
        tblAssets.PIKCapInterest ,
        tblAssets.PIKMargin ,
        tblAssets.PIKPercentage ,
        tblAssets.PrimeOptionExists ,
        tblAssets.PrincipalBalance ,
        tblAssets.PrincipalBalanceEI ,
        tblAssets.PriorityCategoryFitch ,
        tblAssets.PriorityCategoryMoody ,
        tblAssets.PriorityCategorySP ,
        tblAssets.PutDate ,
        tblAssets.Quantity ,
        tblAssets.RateAdjustFreq ,
        tblAssets.RateOption ,
        tblAssets.RatingMoody ,
        tblAssets.RatingMoodyAtIssuance ,
        tblAssets.RatingMoodyDerived ,
        tblAssets.RatingMoodyIssuance ,
        tblAssets.RatingMoodySecurity ,
        tblAssets.RatingMoodyShort ,
        tblAssets.RatingFitch ,
        tblAssets.RatingFitchAtIssuance ,
        tblAssets.RatingFitchDerived ,
        tblAssets.RatingFitchSecurity ,
        tblAssets.RatingSP ,
        tblAssets.RatingSPAtIssuance ,
        tblAssets.RatingSPDerived ,
        tblAssets.RatingSPSecurity ,
        tblAssets.RatingSPShort ,
        tblAssets.RatingDCR ,
        tblAssets.RatingDCRDerived ,
        tblAssets.Region ,
        tblAssets.RecoveryRateFitch ,
        tblAssets.RecoveryRateMoody ,
        tblAssets.RecoveryRateSP ,
        tblAssets.ReferenceAssetID ,
        tblAssets.RegistrationType ,
        tblAssets.SecurityID ,
        tblAssets.SecurityLevel ,
        tblAssets.SecurityLevelFitch ,
        tblAssets.SecurityLevelMoody ,
        tblAssets.SecurityLevelSP ,
        tblAssets.SelfPurchasedAmount ,
        tblAssets.Servicer ,
        tblAssets.SFOType ,
        tblAssets.SpreadLibor ,
        tblAssets.SpreadPrime ,
        tblAssets.StdDevMoody ,
        tblAssets.SyntheticCounterInstitution ,
        tblAssets.SyntheticType ,
        tblAssets.TradesWithAccrued ,
        tblAssets.TransferAgent ,
        tblAssets.UpgradedDowngraded ,
        tblAssets.WeightedAvgAllInRate ,
        tblAssets.WeightedAvgSpread ,
        tblAssets.WeightedAvgDaysToReset ,
        tblAssets.WeightedAvgDaysToResetAmount ,
        tblAssets.YieldToWorst ,
        tblAssets.YTM ,
        tblAssets.YTW ,
        tblAssets.UserAmount1 ,
        tblAssets.UserAmount2 ,
        tblAssets.UserAmount3 ,
        tblAssets.UserAmount4 ,
        tblAssets.UserAmount5 ,
        tblAssets.UserAmount6 ,
        tblAssets.UserAmount7 ,
        tblAssets.UserAmount8 ,
        tblAssets.UserAmount9 ,
        tblAssets.UserAmount10 ,
        tblAssets.UserAmount11 ,
        tblAssets.UserAmount12 ,
        tblAssets.UserAmount13 ,
        tblAssets.UserAmount14 ,
        tblAssets.UserAmount15 ,
        tblAssets.UserAmount16 ,
        tblAssets.UserAmount17 ,
        tblAssets.UserAmount18 ,
        tblAssets.UserAmount19 ,
        tblAssets.UserAmount20 ,
        tblAssets.UserFlag1 ,
        tblAssets.UserFlag2 ,
        tblAssets.UserFlag3 ,
        tblAssets.UserFlag4 ,
        tblAssets.UserFlag5 ,
        tblAssets.UserFlag6 ,
        tblAssets.UserFlag7 ,
        tblAssets.UserFlag8 ,
        tblAssets.UserFlag9 ,
        tblAssets.UserFlag10 ,
        tblAssets.UserFlag11 ,
        tblAssets.UserFlag12 ,
        tblAssets.UserFlag13 ,
        tblAssets.UserFlag14 ,
        tblAssets.UserFlag15 ,
        tblAssets.UserFlag16 ,
        tblAssets.UserFlag17 ,
        tblAssets.UserFlag18 ,
        tblAssets.UserFlag19 ,
        tblAssets.UserFlag20 ,
        tblAssets.UserString1 ,
        tblAssets.UserString2 ,
        tblAssets.UserString3 ,
        tblAssets.UserString4 ,
        tblAssets.UserString5 ,
        tblAssets.UserString6 ,
        tblAssets.UserString7 ,
        tblAssets.UserString8 ,
        tblAssets.UserString9 ,
        tblAssets.UserString10 ,
        tblAssets.UserPercentage1 ,
        tblAssets.UserPercentage2 ,
        tblAssets.UserPercentage3 ,
        tblAssets.UserPercentage4 ,
        tblAssets.UserPercentage5 ,
        tblAssets.UserDecimal1 ,
        tblAssets.UserDecimal2 ,
        tblAssets.UserDecimal3 ,
        tblAssets.UserDecimal4 ,
        tblAssets.UserDecimal5 ,
        tblAssets.UserNumber1 ,
        tblAssets.UserNumber2 ,
        tblAssets.UserNumber3 ,
        tblAssets.UserNumber4 ,
        tblAssets.UserNumber5 ,
        tblAssets.UserDate1 ,
        tblAssets.UserDate2 ,
        tblAssets.UserDate3 ,
        tblAssets.UserDate4 ,
        tblAssets.UserDate5 ,
        tblAssets.FacilityIncrease ,
        tblAssets.ParAmountNativeHypo ,
        tblAssets.OutstandingNativeHypo ,
        tblAssetsUDFs.BankDeal_GlobalAmount ,
        tblAssetsUDFs.ExcessCurrentPay ,
        tblAssetsUDFs.ExcessMoodyCaa ,
        tblAssetsUDFs.ExcessSPCCC ,
        tblAssetsUDFs.FeeSpread ,
        tblAssetsUDFs.InterestFromReinvestment ,
        tblAssetsUDFs.IsDelayedDraw ,
        tblAssetsUDFs.IsRatingMoodyPending ,
        tblAssetsUDFs.LiborBaseRateFloor ,
        tblAssetsUDFs.MarkPrice_BidPrice ,
        tblAssetsUDFs.PaymentDate ,
        tblAssetsUDFs.PrimarySpread ,
        tblAssetsUDFs.PrimarySpreadType ,
        tblAssetsUDFs.PrimeBaseRateFloor ,
        tblAssetsUDFs.PriorityCategorySPI ,
        tblAssetsUDFs.PriorityCategorySPII ,
        tblAssetsUDFs.PriorityCategorySPIII ,
        tblAssetsUDFs.PriorityCategorySPIV ,
        tblAssetsUDFs.PriorityCategorySPV ,
        tblAssetsUDFs.PriorityCategorySPVI ,
        tblAssetsUDFs.RatingMoodyIsShadow ,
        tblAssetsUDFs.RatingMoodyIssue ,
        tblAssetsUDFs.RatingSPIssue ,
        tblAssetsUDFs.RatingSPIssueCreditWatch ,
        tblAssetsUDFs.RecoveryAmountCurrentPay ,
        tblAssetsUDFs.RecoveryAmountMoodyCaa ,
        tblAssetsUDFs.RecoveryAmountSPCCC ,
        tblAssetsUDFs.RecoveryRateMoodyFeed ,
        tblAssetsUDFs.RecoveryRateSPI ,
        tblAssetsUDFs.RecoveryRateSPII ,
        tblAssetsUDFs.RecoveryRateSPIII ,
        tblAssetsUDFs.RecoveryRateSPIV ,
        tblAssetsUDFs.RecoveryRateSPV ,
        tblAssetsUDFs.RecoveryRateSPVI ,
        tblAssetsUDFs.UserString11 ,
        tblAssetsUDFs.UserString12 ,
        tblAssetsUDFs.UserString13 ,
        tblAssetsUDFs.UserString14 ,
        tblAssetsUDFs.UserString15 ,
        tblAssetsUDFs.UserString16 ,
        tblAssets.CoreSystem ,

        tblAssets.RecordModified ,
        tblAssets.Scenario ,
        tblAssetsUDFs.MarkPrice_AskPrice ,
        tblAssetsUDFs.RatingMoodyIssueCreditWatch ,
        tblAssetsUDFs.UserPercentage6 ,
		tblAssetsUDFs.CapitalizedInterestOrig,
		tblAssetsUDFs.SnPAssetRecoveryRating
FROM    tblAssets
INNER JOIN (SELECT  DatasetID ,
                    Key1Value AS AssetId ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''BankDeal_GlobalAmount''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS BankDeal_GlobalAmount ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''ExcessCurrentPay''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS ExcessCurrentPay ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''ExcessMoodyCaa''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS ExcessMoodyCaa ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''ExcessSPCCC''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS ExcessSPCCC ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''FeeSpread''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS FeeSpread ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''InterestFromReinvestment''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS InterestFromReinvestment ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''IsDelayedDraw''
                                         THEN BooleanValue
                                         ELSE 0
                                    END), 0) AS BIT) AS IsDelayedDraw ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''IsRatingMoodyPending''
                                         THEN BooleanValue
                                         ELSE 0
                                    END), 0) AS BIT) AS IsRatingMoodyPending ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''LiborBaseRateFloor''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS LiborBaseRateFloor ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''MarkPrice_AskPrice''
                                         THEN SingleValue
                                         ELSE NULL
                                    END), 0) AS REAL) AS MarkPrice_AskPrice ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''MarkPrice_BidPrice''
                                         THEN SingleValue
                                         ELSE NULL
                                    END), 0) AS REAL) AS MarkPrice_BidPrice ,
                    MAX(CASE WHEN FieldName = ''PaymentDate'' THEN DateValue
                             ELSE NULL
                        END) AS PaymentDate ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PrimarySpread''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS PrimarySpread ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PrimarySpreadType''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS PrimarySpreadType ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PrimeBaseRateFloor''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS PrimeBaseRateFloor ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPI''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPII''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPIII''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPIII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPIV''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPIV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPV''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''PriorityCategorySPVI''
                                         THEN LongValue
                                         ELSE NULL
                                    END), 0) AS INT) AS PriorityCategorySPVI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingMoodyIsShadow''
                                         THEN BooleanValue
                                         ELSE 0
                                    END), 0) AS BIT) AS RatingMoodyIsShadow ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingMoodyIssue''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingMoodyIssue ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingMoodyIssueCreditWatch''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingMoodyIssueCreditWatch ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingSPIssue''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingSPIssue ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RatingSPIssueCreditWatch''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS RatingSPIssueCreditWatch ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryAmountCurrentPay''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS RecoveryAmountCurrentPay ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryAmountMoodyCaa''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS RecoveryAmountMoodyCaa ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryAmountSPCCC''
                                         THEN CurrencyValue
                                         ELSE NULL
                                    END), 0) AS MONEY) AS RecoveryAmountSPCCC ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateMoodyFeed''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateMoodyFeed ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPI''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPII''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPIII''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPIII ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPIV''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPIV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPV''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPV ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''RecoveryRateSPVI''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS RecoveryRateSPVI ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserPercentage6''
                                         THEN DoubleValue
                                         ELSE NULL
                                    END), 0) AS FLOAT) AS UserPercentage6 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString11''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString11 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString12''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString12 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString13''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString13 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString14''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString14 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString15''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString15 ,
                    CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString16''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS UserString16,
					CAST( ISNULL(MAX(CASE WHEN FieldName = ''CapitalizedInterestOrig'' 
										THEN CurrencyValue 
										ELSE NULL  
										END), 0) AS money) AS ''CapitalizedInterestOrig'',
					CAST(ISNULL(MAX(CASE WHEN FieldName = ''UserString18''
                                         THEN StringValue
                                         ELSE NULL
                                    END), '''') AS NVARCHAR(255)) AS SnPAssetRecoveryRating
            FROM    tblUDFData
            WHERE   DatasetID = {0}
                    AND TableName = ''tblAssets''
            GROUP BY DatasetID ,
                    Key1Value
           ) tblAssetsUDFs ON tblAssets.DatasetID = tblAssetsUDFs.DatasetID
                              AND tblAssets.AssetId = tblAssetsUDFs.AssetId
                              AND tblAssetsUDFs.DatasetID = {0}
WHERE   tblAssets.DatasetID = {0};'
WHERE Watcher = 'DatasetBatchJobWatcher' AND DestinationTable = 'dbo.WsoExtractAsset'
GO

USE YODA
GO

ALTER VIEW [CLO].[WsoExtractAssets] AS
SELECT
    EA.ExtractAssetKey,
    EA.DatasetKey,
    EA.DateId,
    EA.AssetId,
    EA.Asset,
    EA.ABSAverageLifeCollateralDescriptor,
    EA.ABSCategory,
    EA.ABSCurrentFactor,
    EA.ABSSpecificCode,
    EA.ABSType,
    EA.AccretedValue,
    EA.AccretionFactor,
    EA.AccruedFees,
    EA.AccruedInterest,
    EA.AggregateAmortCost,
    EA.AssetCategoryMoody,
    EA.AssetCategorySP,
    EA.AssetType,
    EA.AttachedEquity,
    EA.AvgLife,
    EA.CalculationAmount,
    EA.CalculationAmount2,
    EA.CalculationAmount3,
    EA.CalendarID,
    EA.CallDate,
    EA.CapitalizedInterest,
    EA.ConvertibleType,
    EA.Convexity,
    EA.CostBasis,
    EA.CostPrice,
    EA.CountryOfOperation,
    EA.CouponType,
    EA.CreditLinkedAmount,
    EA.CreditLinkedCounterInstitution,
    EA.CurrencyOther,
    EA.CurrencyTypeID,
    EA.CurrentYield,
    EA.CurrentYTM,
    EA.CurrentYTW,
    EA.CUSIP,
    EA.DateOffset,
    EA.DayCount,
    EA.DefaultDate,
    EA.DefaultProbabilityMoody,
    EA.DefaultType,
    EA.Description,
    EA.EarnedInterest,
    EA.EmergingMarketRegion,
    EA.ExclusionAmountMoody,
    EA.ExclusionAmountSP,
    EA.ExpectedLossMoody,
    EA.FacilityType,
    EA.FirstPaymentDate,
    EA.FirstSettleDate,
    EA.FixedRate,
    EA.Frequency,
    EA.Guarantor,
    EA.Insurer,
    EA.InterestAccrualDate,
    EA.InterestAndFeesProjected,
    EA.InterestAndFeesProjectedForNextPeriod,
    EA.InterestAndFeesToDate,
    EA.InterestAndFeesToDateForNextPeriod,
    EA.IsABS,
    EA.IsBankruptcyRelated,
    EA.IsBridgeSecurity,
    EA.IsCallable,
    EA.IsConflictOfInterest,
    EA.IsConvertible,
    EA.IsCreditAvailable,
    EA.IsCreditDefaultSwap,
    EA.IsCurrentPay,
    EA.IsDeepDiscount,
    EA.IsDefeased,
    EA.IsDeferredInterestOutstanding,
    EA.IsDeferredInterestAllowed,
    EA.IsDerivative,
    EA.IsDIP,
    EA.IsEligibleInvestment,
    EA.IsEmergingMarket,
    EA.IsEnhancedBond,
    EA.IsEstimatedRatingFitch,
    EA.IsEstimatedRatingMoody,
    EA.IsEstimatedRatingSP,
    EA.IsException,
    EA.IsFXRisk,
    EA.IsGovtSponsored,
    EA.IsGuaranteed,
    EA.IsHedged,
    EA.ISIN,
    EA.IsInDefault,
    EA.IsIO,
    EA.IsLBOFund,
    EA.IsLBR6MOAgent,
    EA.IsLBR6MOMajor,
    EA.IsLBR6MOSuper,
    EA.IsLeaseFinancingTransaction,
    EA.IsMezzanine,
    EA.IsMostSenior,
    EA.IsOffer,
    EA.IsOriginal,
    EA.IsPIKSecurity,
    EA.IsPO,
    EA.IsPreferredSecurity,
    EA.IsRatingFitchDerived,
    EA.IsRatingMoodyDerived,
    EA.IsRatingSPDerived,
    EA.IsResecuritization,
    EA.IsRestructured,
    EA.IsSFO,
    EA.IsSovereign,
    EA.IsSpecialSituation,
    EA.IsSPVJurisdiction,
    EA.Issuer,
    EA.IssueDate,
    EA.IssueSize,
    EA.IssuingTransaction,
    EA.IsSynthetic,
    EA.IsSyntheticCounterInstitutionInDefault,
    EA.IsTaxJurisdiction,
    EA.LCAmount,
    EA.LeadUnderwriter,
    EA.LiborOptionExists,
    EA.LienType,
    EA.Liquidity,
    EA.LongDatedAmount,
    EA.LossRateMoody,
    EA.LossSeverityMoody,
    EA.MacaulayDuration,
    EA.MarkDate,
    EA.MarketValue,
    EA.MarketValueAdjustedMoody,
    EA.MarketValueAdjustedSP,
    EA.MarketValueOCMoody,
    EA.MarketValueOCSP,
    EA.MarketValueSettleTrue,
    EA.MarkFactor,
    EA.MarkPrice,
    EA.MaturityDate,
    EA.MaturityExceptions,
    EA.ModDuration,
    EA.NextPaymentDate,
    EA.OriginalCapPercent,
    EA.OriginalPAPI,
    EA.Outstanding,
    EA.OutstandingAdjusted,
    EA.PAI,
    EA.ParAmount,
    EA.ParAmountTraded,
    EA.PaymentNonBusinessDirection,
    EA.PaymentOffsetDays,
    EA.PaymentOffsetDirection,
    EA.PaymentOffsetType,
    EA.PIKBeginDate,
    EA.PIKCapInterest,
    EA.PIKMargin,
    EA.PIKPercentage,
    EA.PrimeOptionExists,
    EA.PrincipalBalance,
    EA.PrincipalBalanceEI,
    EA.PriorityCategoryFitch,
    EA.PriorityCategoryMoody,
    EA.PriorityCategorySP,
    EA.PutDate,
    EA.Quantity,
    EA.RateAdjustFreq,
    EA.RateOption,
    EA.RatingMoody,
    EA.RatingMoodyAtIssuance,
    EA.RatingMoodyDerived,
    EA.RatingMoodyIssuance,
    EA.RatingMoodySecurity,
    EA.RatingMoodyShort,
    EA.RatingFitch,
    EA.RatingFitchAtIssuance,
    EA.RatingFitchDerived,
    EA.RatingFitchSecurity,
    EA.RatingSP,
    EA.RatingSPAtIssuance,
    EA.RatingSPDerived,
    EA.RatingSPSecurity,
    EA.RatingSPShort,
    EA.RatingDCR,
    EA.RatingDCRDerived,
    EA.Region,
    EA.RecoveryRateFitch,
    EA.RecoveryRateMoody,
    EA.RecoveryRateSP,
    EA.ReferenceAssetID,
    EA.RegistrationType,
    EA.SecurityID,
    EA.SecurityLevel,
    EA.SecurityLevelFitch,
    EA.SecurityLevelMoody,
    EA.SecurityLevelSP,
    EA.SelfPurchasedAmount,
    EA.Servicer,
    EA.SFOType,
    EA.SpreadLibor,
    EA.SpreadPrime,
    EA.StdDevMoody,
    EA.SyntheticCounterInstitution,
    EA.SyntheticType,
    EA.TradesWithAccrued,
    EA.TransferAgent,
    EA.UpgradedDowngraded,
    EA.WeightedAvgAllInRate,
    EA.WeightedAvgSpread,
    EA.WeightedAvgDaysToReset,
    EA.WeightedAvgDaysToResetAmount,
    EA.YieldToWorst,
    EA.YTM,
    EA.YTW,
    EA.UserAmount1,
    EA.UserAmount2,
    EA.UserAmount3,
    EA.UserAmount4,
    EA.UserAmount5,
    EA.UserAmount6,
    EA.UserAmount7,
    EA.UserAmount8,
    EA.UserAmount9,
    EA.UserAmount10,
    EA.UserAmount11,
    EA.UserAmount12,
    EA.UserAmount13,
    EA.UserAmount14,
    EA.UserAmount15,
    EA.UserAmount16,
    EA.UserAmount17,
    EA.UserAmount18,
    EA.UserAmount19,
    EA.UserAmount20,
    EA.UserFlag1 AS [Is Second Lien],
    EA.UserFlag2,
    EA.UserFlag3 AS [Is First Lien],
    EA.UserFlag4,
    EA.UserFlag5 AS [Is Current Pay as Default],
    EA.UserFlag6,
    EA.UserFlag7,
    EA.UserFlag8,
    EA.UserFlag9 AS [Is Covenant Pari Passu],
    EA.UserFlag10 AS [Is Moody Senior Secured Loan],
    EA.UserFlag11 AS [Is Purchased Discount],
    EA.UserFlag12,
    EA.UserFlag13,
    EA.UserFlag14 AS [Is Cov-Lite],
    EA.UserFlag15 AS [Is Moody's DP Rating Derived],
    EA.UserFlag16,
    EA.UserFlag17,
    EA.UserFlag18,
    EA.UserFlag19,
    EA.UserFlag20,
    EA.UserString1 AS [zIs Floor in Effect],
    EA.UserString2 AS [Counterparty Type],
    EA.UserString3 AS [OC Recovery Type],
    EA.UserString4 AS [Moody's DP Rating - WARF (ADJ)],
    EA.UserString5 AS [Moody's CF Rating],
    EA.UserString6 AS [Moody's DP Rating - WARF],
    EA.UserString7 AS [S&P Issuer Rating],
    EA.UserString8 AS [Moody's DP Rating (Unnotched) - WARF],
    EA.UserString9 AS [Moody's Credit Watch],
    EA.UserString10 AS [Issuer Name Alternative],
    EA.UserString11 AS [Issuer Country],
    EA.UserString12 AS [SIC Moody],
    EA.UserString13 AS [SIC S&P],
    EA.UserString14 AS [Moody's Outlook],
    EA.UserString15 AS [S&P Credit Watch],
    EA.UserString16 AS [WAL Date GV (Hidden)],
    EA.UserPercentage1 AS [Adjusted WAS],
    EA.UserPercentage2 AS [Adjusted WAC],
    EA.UserPercentage3 AS [All-In-Rate (w/Floor)],
    EA.UserPercentage4 AS [LIBOR Floor for accrual WAS],
    EA.UserPercentage5 AS [LIBOR Floor Credit Addback],
    EA.UserPercentage6 AS [LIBOR Base Rate on Notes],
    EA.UserDecimal1 AS [Cost Price Discounts],
    EA.UserDecimal2 AS [Cost Price (Discounts)],
    EA.UserDecimal3,
    EA.UserDecimal4,
    EA.UserDecimal5,
    EA.UserNumber1 AS [WAL Date Case (Hidden)],
    EA.UserNumber2,
    EA.UserNumber3,
    EA.UserNumber4,
    EA.UserNumber5,
    EA.UserDate1,
    EA.UserDate2,
    EA.UserDate3,
    EA.UserDate4,
    EA.UserDate5,
    EA.FacilityIncrease,
    EA.ParAmountNativeHypo,
    EA.OutstandingNativeHypo,
    EA.BankDeal_GlobalAmount,
    EA.ExcessCurrentPay,
    EA.ExcessMoodyCaa,
    EA.ExcessSPCCC,
    EA.FeeSpread,
    EA.InterestFromReinvestment,
    EA.IsDelayedDraw,
    EA.IsRatingMoodyPending,
    EA.LiborBaseRateFloor,
    EA.MarkPrice_BidPrice,
    EA.PaymentDate,
    EA.PrimarySpread,
    EA.PrimarySpreadType,
    EA.PrimeBaseRateFloor,
    EA.PriorityCategorySPI,
    EA.PriorityCategorySPII,
    EA.PriorityCategorySPIII,
    EA.PriorityCategorySPIV,
    EA.PriorityCategorySPV,
    EA.PriorityCategorySPVI,
    EA.RatingMoodyIsShadow,
    EA.RatingMoodyIssue,
    EA.RatingSPIssue,
    EA.RatingSPIssueCreditWatch,
    EA.RecoveryAmountCurrentPay,
    EA.RecoveryAmountMoodyCaa,
    EA.RecoveryAmountSPCCC,
    EA.RecoveryRateMoodyFeed,
    EA.RecoveryRateSPI,
    EA.RecoveryRateSPII,
    EA.RecoveryRateSPIII,
    EA.RecoveryRateSPIV,
    EA.RecoveryRateSPV,
    EA.RecoveryRateSPVI,
    EA.CoreSystem,
    RecordModified,
    Scenario,
    MarkPrice_AskPrice,
    RatingMoodyIssueCreditWatch,
	EA.CapitalizedInterestOrig,
	EA.[SnPAssetRecoveryRating]
FROM DataMarts.dbo.WsoExtractAsset EA
WHERE EA.SecurityID LIKE 'LX%'

GO

/*
Deployment script for Yoda

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar YorkCore_Geneva "YorkCore_Geneva"
:setvar DatabaseName "Yoda"
:setvar DefaultFilePrefix "Yoda"
:setvar DefaultDataPath "E:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "F:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\Data\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET RECOVERY FULL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = AUTO, OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30)) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Altering [CLO].[Position]...';


GO
ALTER TABLE [CLO].[Position]
    ADD [SnPAssetRecoveryRating] VARCHAR (100) NULL;


GO
PRINT N'Refreshing [CLO].[vw_PositionCountryFund]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[vw_PositionCountryFund]';


GO
PRINT N'Refreshing [CLO].[vw_PrevDaySecurityMarket]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[vw_PrevDaySecurityMarket]';


GO
PRINT N'Refreshing [CLO].[vw_SecurityMarket]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[vw_SecurityMarket]';


GO
PRINT N'Refreshing [CLO].[vw_PivotedPositionCountry]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[vw_PivotedPositionCountry]';


GO
PRINT N'Refreshing [CLO].[vw_PivotedPositionExposure]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[vw_PivotedPositionExposure]';


GO
PRINT N'Refreshing [CLO].[vw_PivotedPositionIsCovLite]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[vw_PivotedPositionIsCovLite]';


GO
PRINT N'Refreshing [CLO].[GetActiveTrades]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[GetActiveTrades]';


GO
PRINT N'Refreshing [CLO].[vw_PivotedTradeTotalAllocation]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[vw_PivotedTradeTotalAllocation]';


GO
PRINT N'Altering [CLO].[LoadWso_Position]...';


GO
ALTER PROCEDURE [CLO].[LoadWso_Position]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
BEGIN
    BEGIN TRANSACTION

    DELETE P
    FROM CLO.Position P
        JOIN DataMarts.dbo.WsoDatasets D
            ON D.DealID = P.FundId
        JOIN @datasetKeys K
            ON K.DatasetKey = D.DatasetKey
    WHERE P.DateId = @asOfDateId


	DELETE P
    FROM CLO.WSOPosition P
        JOIN DataMarts.dbo.WsoDatasets D
            ON D.DealID = P.FundId
        JOIN @datasetKeys K
            ON K.DatasetKey = D.DatasetKey
    WHERE P.DateId = @asOfDateId


    INSERT INTO CLO.Position
    (
        FundId,
        SecurityId,
        DateId,
		MarketValue,
        Exposure,
        PctExposure,
        PxPrice,
        IsCovLite,
        CountryId,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy,
		CapitalizedInterestOrig,
		SnPAssetRecoveryRating
    )
    SELECT AggPos.DealID,
        S.SecurityId,
        @asOfDateId,
		EA.MarketValue,
        EA.Quantity AS Exposure,
        EA.Quantity / AggPos.TotalExposure AS PctExposure,
        EA.MarkPrice AS PxPrice,
        EA.[Is Cov-Lite],
        ISNULL(
        (
            SELECT TOP 1
                C.CountryId
            FROM CLO.Country C
            WHERE EA.CountryOfOperation = C.CountryDesc
        ),
                  -1
              ),   -- CountryId - smallint
        GETDATE(), -- CreatedOn - datetime
        'WSO',     -- CreatedBy - varchar(100)
        GETDATE(), -- LastUpdatedOn - datetime
        'WSO',      -- LastUpdatedBy - varchar(100)
		EA.CapitalizedInterestOrig,
		EA.[SnPAssetRecoveryRating]
    FROM CLO.WsoExtractAssets EA
        JOIN @datasetKeys K
            ON EA.DatasetKey = K.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON K.DatasetKey = D.DatasetKey
        LEFT JOIN CLO.Security S
            ON EA.SecurityID = S.SecurityCode --WANT IT TO BOMB IF SECURITY IS NOT SETUP
        JOIN
         (
             SELECT D1.DealID,
                 SUM(EA1.Quantity) AS TotalExposure
             FROM CLO.WsoExtractAssets EA1
                 JOIN @datasetKeys K1
                     ON K1.DatasetKey = EA1.DatasetKey
                 JOIN DataMarts.dbo.WsoDatasets D1
                     ON K1.DatasetKey = D1.DatasetKey
             GROUP BY D1.DealID
         ) AggPos
            ON D.DealID = AggPos.DealID
    WHERE ABS(EA.Quantity) > 0

    COMMIT TRANSACTION
END
GO
PRINT N'Refreshing [CLO].[spGetPriceMove]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spGetPriceMove]';


GO
PRINT N'Refreshing [CLO].[spFixAnalystIssuer]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spFixAnalystIssuer]';


GO
PRINT N'Refreshing [CLO].[spAddPositionsForTempSecurities]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spAddPositionsForTempSecurities]';


GO
PRINT N'Refreshing [CLO].[spGetSecurities]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spGetSecurities]';


GO
PRINT N'Refreshing [CLO].[spCalculateSummaryData]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spCalculateSummaryData]';


GO
PRINT N'Refreshing [CLO].[spCleanPositionsBasedOnPrincipalCash]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spCleanPositionsBasedOnPrincipalCash]';


GO
PRINT N'Refreshing [CLO].[spGetAggregatedLoanPositions]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spGetAggregatedLoanPositions]';


GO
PRINT N'Refreshing [CLO].[spLoadStalePositions]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spLoadStalePositions]';


GO
PRINT N'Refreshing [CLO].[spRefillPositionsBasedOnPrincipalCash]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spRefillPositionsBasedOnPrincipalCash]';


GO
PRINT N'Refreshing [CLO].[spGenerateAggregatedPositions]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[spGenerateAggregatedPositions]';


GO
PRINT N'Refreshing [CLO].[LoadWso_ByDatasetKeys]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[LoadWso_ByDatasetKeys]';


GO
PRINT N'Refreshing [CLO].[LoadWso]...';


GO
EXECUTE sp_refreshsqlmodule N'[CLO].[LoadWso]';


GO
PRINT N'Update complete.';


GO

