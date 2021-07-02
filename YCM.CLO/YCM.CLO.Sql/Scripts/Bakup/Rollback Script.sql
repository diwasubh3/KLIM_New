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
		tblAssetsUDFs.CapitalizedInterestOrig
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
										END), 0) AS money) AS ''CapitalizedInterestOrig''	
            FROM    tblUDFData
            WHERE   DatasetID = {0}
                    AND TableName = ''tblAssets''
            GROUP BY DatasetID ,
                    Key1Value
           ) tblAssetsUDFs ON tblAssets.DatasetID = tblAssetsUDFs.DatasetID
                              AND tblAssets.AssetId = tblAssetsUDFs.AssetId
                              AND tblAssetsUDFs.DatasetID = {0}
WHERE   tblAssets.DatasetID = {0};'
WHERE Watcher = 'DatasetArchiveWatcher'

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
		tblAssetsUDFs.CapitalizedInterestOrig
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
										END), 0) AS money) AS ''CapitalizedInterestOrig''
            FROM    tblUDFData
            WHERE   DatasetID = {0}
                    AND TableName = ''tblAssets''
            GROUP BY DatasetID ,
                    Key1Value
           ) tblAssetsUDFs ON tblAssets.DatasetID = tblAssetsUDFs.DatasetID
                              AND tblAssets.AssetId = tblAssetsUDFs.AssetId
                              AND tblAssetsUDFs.DatasetID = {0}
WHERE   tblAssets.DatasetID = {0};'
WHERE Watcher = 'DatasetBatchJobWatcher'