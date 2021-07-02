using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class WSOExtractAssetMap : EntityTypeConfiguration<WSOExtractAsset>
    {
        public WSOExtractAssetMap()
        {

            // Primary Key
            this.HasKey(t => new { t.ExtractAssetKey, t.DatasetKey, t.DateId, t.AssetId, t.IsBankruptcyRelated, t.IsBridgeSecurity, t.IsConflictOfInterest, t.IsConvertible, t.IsCreditAvailable, t.IsCreditDefaultSwap, t.IsCurrentPay, t.IsDeepDiscount, t.IsDefeased, t.IsDeferredInterestOutstanding, t.IsDeferredInterestAllowed, t.IsDerivative, t.IsDIP, t.IsEligibleInvestment, t.IsEmergingMarket, t.IsEstimatedRatingFitch, t.IsEstimatedRatingMoody, t.IsEstimatedRatingSP, t.IsException, t.IsFXRisk, t.IsGovtSponsored, t.IsHedged, t.IsInDefault, t.IsLBOFund, t.IsLBR6MOAgent, t.IsLBR6MOMajor, t.IsLBR6MOSuper, t.IsLeaseFinancingTransaction, t.IsMezzanine, t.IsOffer, t.IsOriginal, t.IsPIKSecurity, t.IsPreferredSecurity, t.IsRatingFitchDerived, t.IsRatingMoodyDerived, t.IsRatingSPDerived, t.IsResecuritization, t.IsSFO, t.IsSovereign, t.IsSpecialSituation, t.IsSPVJurisdiction, t.IsSynthetic, t.IsSyntheticCounterInstitutionInDefault, t.IsTaxJurisdiction, t.LiborOptionExists, t.PrimeOptionExists });

            // Properties
            this.Property(t => t.ExtractAssetKey)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DatasetKey)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AssetId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Asset)
                .HasMaxLength(100);

            this.Property(t => t.ABSCategory)
                .HasMaxLength(50);

            this.Property(t => t.ABSType)
                .HasMaxLength(50);

            this.Property(t => t.AssetCategoryMoody)
                .HasMaxLength(100);

            this.Property(t => t.AssetCategorySP)
                .HasMaxLength(100);

            this.Property(t => t.AssetType)
                .HasMaxLength(50);

            this.Property(t => t.AttachedEquity)
                .HasMaxLength(100);

            this.Property(t => t.ConvertibleType)
                .HasMaxLength(100);

            this.Property(t => t.CostBasis)
                .HasMaxLength(100);

            this.Property(t => t.CountryOfOperation)
                .HasMaxLength(200);

            this.Property(t => t.CouponType)
                .HasMaxLength(100);

            this.Property(t => t.CreditLinkedCounterInstitution)
                .HasMaxLength(200);

            this.Property(t => t.CurrencyOther)
                .HasMaxLength(100);

            this.Property(t => t.CUSIP)
                .HasMaxLength(50);

            this.Property(t => t.DefaultType)
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(200);

            this.Property(t => t.EmergingMarketRegion)
                .HasMaxLength(100);

            this.Property(t => t.FacilityType)
                .HasMaxLength(100);

            this.Property(t => t.Guarantor)
                .HasMaxLength(200);

            this.Property(t => t.Insurer)
                .HasMaxLength(200);

            this.Property(t => t.ISIN)
                .HasMaxLength(50);

            this.Property(t => t.Issuer)
                .HasMaxLength(255);

            this.Property(t => t.IssuingTransaction)
                .HasMaxLength(255);

            this.Property(t => t.LeadUnderwriter)
                .HasMaxLength(200);

            this.Property(t => t.LienType)
                .HasMaxLength(50);

            this.Property(t => t.Liquidity)
                .HasMaxLength(100);

            this.Property(t => t.MaturityExceptions)
                .HasMaxLength(100);

            this.Property(t => t.RateOption)
                .HasMaxLength(100);

            this.Property(t => t.RatingMoody)
                .HasMaxLength(50);

            this.Property(t => t.RatingMoodyAtIssuance)
                .HasMaxLength(50);

            this.Property(t => t.RatingMoodyDerived)
                .HasMaxLength(50);

            this.Property(t => t.RatingMoodyIssuance)
                .HasMaxLength(50);

            this.Property(t => t.RatingMoodySecurity)
                .HasMaxLength(50);

            this.Property(t => t.RatingMoodyShort)
                .HasMaxLength(50);

            this.Property(t => t.RatingFitch)
                .HasMaxLength(50);

            this.Property(t => t.RatingFitchAtIssuance)
                .HasMaxLength(50);

            this.Property(t => t.RatingFitchDerived)
                .HasMaxLength(50);

            this.Property(t => t.RatingFitchSecurity)
                .HasMaxLength(50);

            this.Property(t => t.RatingSP)
                .HasMaxLength(50);

            this.Property(t => t.RatingSPAtIssuance)
                .HasMaxLength(50);

            this.Property(t => t.RatingSPDerived)
                .HasMaxLength(50);

            this.Property(t => t.RatingSPSecurity)
                .HasMaxLength(50);

            this.Property(t => t.RatingSPShort)
                .HasMaxLength(50);

            this.Property(t => t.RatingDCR)
                .HasMaxLength(50);

            this.Property(t => t.RatingDCRDerived)
                .HasMaxLength(50);

            this.Property(t => t.Region)
                .HasMaxLength(100);

            this.Property(t => t.ReferenceAssetID)
                .HasMaxLength(50);

            this.Property(t => t.RegistrationType)
                .HasMaxLength(100);

            this.Property(t => t.SecurityID)
                .HasMaxLength(100);

            this.Property(t => t.SecurityLevel)
                .HasMaxLength(100);

            this.Property(t => t.SecurityLevelFitch)
                .HasMaxLength(100);

            this.Property(t => t.SecurityLevelMoody)
                .HasMaxLength(100);

            this.Property(t => t.SecurityLevelSP)
                .HasMaxLength(100);

            this.Property(t => t.Servicer)
                .HasMaxLength(200);

            this.Property(t => t.SFOType)
                .HasMaxLength(100);

            this.Property(t => t.SyntheticCounterInstitution)
                .HasMaxLength(200);

            this.Property(t => t.SyntheticType)
                .HasMaxLength(100);

            this.Property(t => t.TransferAgent)
                .HasMaxLength(200);

            this.Property(t => t.UpgradedDowngraded)
                .HasMaxLength(100);

            this.Property(t => t.zIs_Floor_in_Effect)
                .HasMaxLength(100);

            this.Property(t => t.Counterparty_Type)
                .HasMaxLength(100);

            this.Property(t => t.OC_Recovery_Type)
                .HasMaxLength(100);

            this.Property(t => t.Moody_s_DP_Rating___WARF__ADJ_)
                .HasMaxLength(100);

            this.Property(t => t.Moody_s_CF_Rating)
                .HasMaxLength(100);

            this.Property(t => t.Moody_s_DP_Rating___WARF)
                .HasMaxLength(100);

            this.Property(t => t.S_P_Issuer_Rating)
                .HasMaxLength(100);

            this.Property(t => t.Moody_s_DP_Rating__Unnotched____WARF)
                .HasMaxLength(100);

            this.Property(t => t.Moody_s_Credit_Watch)
                .HasMaxLength(100);

            this.Property(t => t.Issuer_Name_Alternative)
                .HasMaxLength(100);

            this.Property(t => t.Issuer_Country)
                .HasMaxLength(255);

            this.Property(t => t.SIC_Moody)
                .HasMaxLength(255);

            this.Property(t => t.SIC_S_P)
                .HasMaxLength(255);

            this.Property(t => t.Moody_s_Outlook)
                .HasMaxLength(255);

            this.Property(t => t.S_P_Credit_Watch)
                .HasMaxLength(255);

            this.Property(t => t.WAL_Date_GV__Hidden_)
                .HasMaxLength(255);

            this.Property(t => t.PrimarySpreadType)
                .HasMaxLength(255);

            this.Property(t => t.RatingMoodyIssue)
                .HasMaxLength(255);

            this.Property(t => t.RatingSPIssue)
                .HasMaxLength(255);

            this.Property(t => t.RatingSPIssueCreditWatch)
                .HasMaxLength(255);

            this.Property(t => t.CoreSystem)
                .HasMaxLength(100);

            this.Property(t => t.RatingMoodyIssueCreditWatch)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("WSOExtractAssets", "CLO");
            this.Property(t => t.ExtractAssetKey).HasColumnName("ExtractAssetKey");
            this.Property(t => t.DatasetKey).HasColumnName("DatasetKey");
            this.Property(t => t.DateId).HasColumnName("DateId");
            this.Property(t => t.AssetId).HasColumnName("AssetId");
            this.Property(t => t.Asset).HasColumnName("Asset");
            this.Property(t => t.ABSAverageLifeCollateralDescriptor).HasColumnName("ABSAverageLifeCollateralDescriptor");
            this.Property(t => t.ABSCategory).HasColumnName("ABSCategory");
            this.Property(t => t.ABSCurrentFactor).HasColumnName("ABSCurrentFactor");
            this.Property(t => t.ABSSpecificCode).HasColumnName("ABSSpecificCode");
            this.Property(t => t.ABSType).HasColumnName("ABSType");
            this.Property(t => t.AccretedValue).HasColumnName("AccretedValue");
            this.Property(t => t.AccretionFactor).HasColumnName("AccretionFactor");
            this.Property(t => t.AccruedFees).HasColumnName("AccruedFees");
            this.Property(t => t.AccruedInterest).HasColumnName("AccruedInterest");
            this.Property(t => t.AggregateAmortCost).HasColumnName("AggregateAmortCost");
            this.Property(t => t.AssetCategoryMoody).HasColumnName("AssetCategoryMoody");
            this.Property(t => t.AssetCategorySP).HasColumnName("AssetCategorySP");
            this.Property(t => t.AssetType).HasColumnName("AssetType");
            this.Property(t => t.AttachedEquity).HasColumnName("AttachedEquity");
            this.Property(t => t.AvgLife).HasColumnName("AvgLife");
            this.Property(t => t.CalculationAmount).HasColumnName("CalculationAmount");
            this.Property(t => t.CalculationAmount2).HasColumnName("CalculationAmount2");
            this.Property(t => t.CalculationAmount3).HasColumnName("CalculationAmount3");
            this.Property(t => t.CalendarID).HasColumnName("CalendarID");
            this.Property(t => t.CallDate).HasColumnName("CallDate");
            this.Property(t => t.CapitalizedInterest).HasColumnName("CapitalizedInterest");
            this.Property(t => t.ConvertibleType).HasColumnName("ConvertibleType");
            this.Property(t => t.Convexity).HasColumnName("Convexity");
            this.Property(t => t.CostBasis).HasColumnName("CostBasis");
            this.Property(t => t.CostPrice).HasColumnName("CostPrice");
            this.Property(t => t.CountryOfOperation).HasColumnName("CountryOfOperation");
            this.Property(t => t.CouponType).HasColumnName("CouponType");
            this.Property(t => t.CreditLinkedAmount).HasColumnName("CreditLinkedAmount");
            this.Property(t => t.CreditLinkedCounterInstitution).HasColumnName("CreditLinkedCounterInstitution");
            this.Property(t => t.CurrencyOther).HasColumnName("CurrencyOther");
            this.Property(t => t.CurrencyTypeID).HasColumnName("CurrencyTypeID");
            this.Property(t => t.CurrentYield).HasColumnName("CurrentYield");
            this.Property(t => t.CurrentYTM).HasColumnName("CurrentYTM");
            this.Property(t => t.CurrentYTW).HasColumnName("CurrentYTW");
            this.Property(t => t.CUSIP).HasColumnName("CUSIP");
            this.Property(t => t.DateOffset).HasColumnName("DateOffset");
            this.Property(t => t.DayCount).HasColumnName("DayCount");
            this.Property(t => t.DefaultDate).HasColumnName("DefaultDate");
            this.Property(t => t.DefaultProbabilityMoody).HasColumnName("DefaultProbabilityMoody");
            this.Property(t => t.DefaultType).HasColumnName("DefaultType");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.EarnedInterest).HasColumnName("EarnedInterest");
            this.Property(t => t.EmergingMarketRegion).HasColumnName("EmergingMarketRegion");
            this.Property(t => t.ExclusionAmountMoody).HasColumnName("ExclusionAmountMoody");
            this.Property(t => t.ExclusionAmountSP).HasColumnName("ExclusionAmountSP");
            this.Property(t => t.ExpectedLossMoody).HasColumnName("ExpectedLossMoody");
            this.Property(t => t.FacilityType).HasColumnName("FacilityType");
            this.Property(t => t.FirstPaymentDate).HasColumnName("FirstPaymentDate");
            this.Property(t => t.FirstSettleDate).HasColumnName("FirstSettleDate");
            this.Property(t => t.FixedRate).HasColumnName("FixedRate");
            this.Property(t => t.Frequency).HasColumnName("Frequency");
            this.Property(t => t.Guarantor).HasColumnName("Guarantor");
            this.Property(t => t.Insurer).HasColumnName("Insurer");
            this.Property(t => t.InterestAccrualDate).HasColumnName("InterestAccrualDate");
            this.Property(t => t.InterestAndFeesProjected).HasColumnName("InterestAndFeesProjected");
            this.Property(t => t.InterestAndFeesProjectedForNextPeriod).HasColumnName("InterestAndFeesProjectedForNextPeriod");
            this.Property(t => t.InterestAndFeesToDate).HasColumnName("InterestAndFeesToDate");
            this.Property(t => t.InterestAndFeesToDateForNextPeriod).HasColumnName("InterestAndFeesToDateForNextPeriod");
            this.Property(t => t.IsABS).HasColumnName("IsABS");
            this.Property(t => t.IsBankruptcyRelated).HasColumnName("IsBankruptcyRelated");
            this.Property(t => t.IsBridgeSecurity).HasColumnName("IsBridgeSecurity");
            this.Property(t => t.IsCallable).HasColumnName("IsCallable");
            this.Property(t => t.IsConflictOfInterest).HasColumnName("IsConflictOfInterest");
            this.Property(t => t.IsConvertible).HasColumnName("IsConvertible");
            this.Property(t => t.IsCreditAvailable).HasColumnName("IsCreditAvailable");
            this.Property(t => t.IsCreditDefaultSwap).HasColumnName("IsCreditDefaultSwap");
            this.Property(t => t.IsCurrentPay).HasColumnName("IsCurrentPay");
            this.Property(t => t.IsDeepDiscount).HasColumnName("IsDeepDiscount");
            this.Property(t => t.IsDefeased).HasColumnName("IsDefeased");
            this.Property(t => t.IsDeferredInterestOutstanding).HasColumnName("IsDeferredInterestOutstanding");
            this.Property(t => t.IsDeferredInterestAllowed).HasColumnName("IsDeferredInterestAllowed");
            this.Property(t => t.IsDerivative).HasColumnName("IsDerivative");
            this.Property(t => t.IsDIP).HasColumnName("IsDIP");
            this.Property(t => t.IsEligibleInvestment).HasColumnName("IsEligibleInvestment");
            this.Property(t => t.IsEmergingMarket).HasColumnName("IsEmergingMarket");
            this.Property(t => t.IsEnhancedBond).HasColumnName("IsEnhancedBond");
            this.Property(t => t.IsEstimatedRatingFitch).HasColumnName("IsEstimatedRatingFitch");
            this.Property(t => t.IsEstimatedRatingMoody).HasColumnName("IsEstimatedRatingMoody");
            this.Property(t => t.IsEstimatedRatingSP).HasColumnName("IsEstimatedRatingSP");
            this.Property(t => t.IsException).HasColumnName("IsException");
            this.Property(t => t.IsFXRisk).HasColumnName("IsFXRisk");
            this.Property(t => t.IsGovtSponsored).HasColumnName("IsGovtSponsored");
            this.Property(t => t.IsGuaranteed).HasColumnName("IsGuaranteed");
            this.Property(t => t.IsHedged).HasColumnName("IsHedged");
            this.Property(t => t.ISIN).HasColumnName("ISIN");
            this.Property(t => t.IsInDefault).HasColumnName("IsInDefault");
            this.Property(t => t.IsIO).HasColumnName("IsIO");
            this.Property(t => t.IsLBOFund).HasColumnName("IsLBOFund");
            this.Property(t => t.IsLBR6MOAgent).HasColumnName("IsLBR6MOAgent");
            this.Property(t => t.IsLBR6MOMajor).HasColumnName("IsLBR6MOMajor");
            this.Property(t => t.IsLBR6MOSuper).HasColumnName("IsLBR6MOSuper");
            this.Property(t => t.IsLeaseFinancingTransaction).HasColumnName("IsLeaseFinancingTransaction");
            this.Property(t => t.IsMezzanine).HasColumnName("IsMezzanine");
            this.Property(t => t.IsMostSenior).HasColumnName("IsMostSenior");
            this.Property(t => t.IsOffer).HasColumnName("IsOffer");
            this.Property(t => t.IsOriginal).HasColumnName("IsOriginal");
            this.Property(t => t.IsPIKSecurity).HasColumnName("IsPIKSecurity");
            this.Property(t => t.IsPO).HasColumnName("IsPO");
            this.Property(t => t.IsPreferredSecurity).HasColumnName("IsPreferredSecurity");
            this.Property(t => t.IsRatingFitchDerived).HasColumnName("IsRatingFitchDerived");
            this.Property(t => t.IsRatingMoodyDerived).HasColumnName("IsRatingMoodyDerived");
            this.Property(t => t.IsRatingSPDerived).HasColumnName("IsRatingSPDerived");
            this.Property(t => t.IsResecuritization).HasColumnName("IsResecuritization");
            this.Property(t => t.IsRestructured).HasColumnName("IsRestructured");
            this.Property(t => t.IsSFO).HasColumnName("IsSFO");
            this.Property(t => t.IsSovereign).HasColumnName("IsSovereign");
            this.Property(t => t.IsSpecialSituation).HasColumnName("IsSpecialSituation");
            this.Property(t => t.IsSPVJurisdiction).HasColumnName("IsSPVJurisdiction");
            this.Property(t => t.Issuer).HasColumnName("Issuer");
            this.Property(t => t.IssueDate).HasColumnName("IssueDate");
            this.Property(t => t.IssueSize).HasColumnName("IssueSize");
            this.Property(t => t.IssuingTransaction).HasColumnName("IssuingTransaction");
            this.Property(t => t.IsSynthetic).HasColumnName("IsSynthetic");
            this.Property(t => t.IsSyntheticCounterInstitutionInDefault).HasColumnName("IsSyntheticCounterInstitutionInDefault");
            this.Property(t => t.IsTaxJurisdiction).HasColumnName("IsTaxJurisdiction");
            this.Property(t => t.LCAmount).HasColumnName("LCAmount");
            this.Property(t => t.LeadUnderwriter).HasColumnName("LeadUnderwriter");
            this.Property(t => t.LiborOptionExists).HasColumnName("LiborOptionExists");
            this.Property(t => t.LienType).HasColumnName("LienType");
            this.Property(t => t.Liquidity).HasColumnName("Liquidity");
            this.Property(t => t.LongDatedAmount).HasColumnName("LongDatedAmount");
            this.Property(t => t.LossRateMoody).HasColumnName("LossRateMoody");
            this.Property(t => t.LossSeverityMoody).HasColumnName("LossSeverityMoody");
            this.Property(t => t.MacaulayDuration).HasColumnName("MacaulayDuration");
            this.Property(t => t.MarkDate).HasColumnName("MarkDate");
            this.Property(t => t.MarketValue).HasColumnName("MarketValue");
            this.Property(t => t.MarketValueAdjustedMoody).HasColumnName("MarketValueAdjustedMoody");
            this.Property(t => t.MarketValueAdjustedSP).HasColumnName("MarketValueAdjustedSP");
            this.Property(t => t.MarketValueOCMoody).HasColumnName("MarketValueOCMoody");
            this.Property(t => t.MarketValueOCSP).HasColumnName("MarketValueOCSP");
            this.Property(t => t.MarketValueSettleTrue).HasColumnName("MarketValueSettleTrue");
            this.Property(t => t.MarkFactor).HasColumnName("MarkFactor");
            this.Property(t => t.MarkPrice).HasColumnName("MarkPrice");
            this.Property(t => t.MaturityDate).HasColumnName("MaturityDate");
            this.Property(t => t.MaturityExceptions).HasColumnName("MaturityExceptions");
            this.Property(t => t.ModDuration).HasColumnName("ModDuration");
            this.Property(t => t.NextPaymentDate).HasColumnName("NextPaymentDate");
            this.Property(t => t.OriginalCapPercent).HasColumnName("OriginalCapPercent");
            this.Property(t => t.OriginalPAPI).HasColumnName("OriginalPAPI");
            this.Property(t => t.Outstanding).HasColumnName("Outstanding");
            this.Property(t => t.OutstandingAdjusted).HasColumnName("OutstandingAdjusted");
            this.Property(t => t.PAI).HasColumnName("PAI");
            this.Property(t => t.ParAmount).HasColumnName("ParAmount");
            this.Property(t => t.ParAmountTraded).HasColumnName("ParAmountTraded");
            this.Property(t => t.PaymentNonBusinessDirection).HasColumnName("PaymentNonBusinessDirection");
            this.Property(t => t.PaymentOffsetDays).HasColumnName("PaymentOffsetDays");
            this.Property(t => t.PaymentOffsetDirection).HasColumnName("PaymentOffsetDirection");
            this.Property(t => t.PaymentOffsetType).HasColumnName("PaymentOffsetType");
            this.Property(t => t.PIKBeginDate).HasColumnName("PIKBeginDate");
            this.Property(t => t.PIKCapInterest).HasColumnName("PIKCapInterest");
            this.Property(t => t.PIKMargin).HasColumnName("PIKMargin");
            this.Property(t => t.PIKPercentage).HasColumnName("PIKPercentage");
            this.Property(t => t.PrimeOptionExists).HasColumnName("PrimeOptionExists");
            this.Property(t => t.PrincipalBalance).HasColumnName("PrincipalBalance");
            this.Property(t => t.PrincipalBalanceEI).HasColumnName("PrincipalBalanceEI");
            this.Property(t => t.PriorityCategoryFitch).HasColumnName("PriorityCategoryFitch");
            this.Property(t => t.PriorityCategoryMoody).HasColumnName("PriorityCategoryMoody");
            this.Property(t => t.PriorityCategorySP).HasColumnName("PriorityCategorySP");
            this.Property(t => t.PutDate).HasColumnName("PutDate");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.RateAdjustFreq).HasColumnName("RateAdjustFreq");
            this.Property(t => t.RateOption).HasColumnName("RateOption");
            this.Property(t => t.RatingMoody).HasColumnName("RatingMoody");
            this.Property(t => t.RatingMoodyAtIssuance).HasColumnName("RatingMoodyAtIssuance");
            this.Property(t => t.RatingMoodyDerived).HasColumnName("RatingMoodyDerived");
            this.Property(t => t.RatingMoodyIssuance).HasColumnName("RatingMoodyIssuance");
            this.Property(t => t.RatingMoodySecurity).HasColumnName("RatingMoodySecurity");
            this.Property(t => t.RatingMoodyShort).HasColumnName("RatingMoodyShort");
            this.Property(t => t.RatingFitch).HasColumnName("RatingFitch");
            this.Property(t => t.RatingFitchAtIssuance).HasColumnName("RatingFitchAtIssuance");
            this.Property(t => t.RatingFitchDerived).HasColumnName("RatingFitchDerived");
            this.Property(t => t.RatingFitchSecurity).HasColumnName("RatingFitchSecurity");
            this.Property(t => t.RatingSP).HasColumnName("RatingSP");
            this.Property(t => t.RatingSPAtIssuance).HasColumnName("RatingSPAtIssuance");
            this.Property(t => t.RatingSPDerived).HasColumnName("RatingSPDerived");
            this.Property(t => t.RatingSPSecurity).HasColumnName("RatingSPSecurity");
            this.Property(t => t.RatingSPShort).HasColumnName("RatingSPShort");
            this.Property(t => t.RatingDCR).HasColumnName("RatingDCR");
            this.Property(t => t.RatingDCRDerived).HasColumnName("RatingDCRDerived");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.RecoveryRateFitch).HasColumnName("RecoveryRateFitch");
            this.Property(t => t.RecoveryRateMoody).HasColumnName("RecoveryRateMoody");
            this.Property(t => t.RecoveryRateSP).HasColumnName("RecoveryRateSP");
            this.Property(t => t.ReferenceAssetID).HasColumnName("ReferenceAssetID");
            this.Property(t => t.RegistrationType).HasColumnName("RegistrationType");
            this.Property(t => t.SecurityID).HasColumnName("SecurityID");
            this.Property(t => t.SecurityLevel).HasColumnName("SecurityLevel");
            this.Property(t => t.SecurityLevelFitch).HasColumnName("SecurityLevelFitch");
            this.Property(t => t.SecurityLevelMoody).HasColumnName("SecurityLevelMoody");
            this.Property(t => t.SecurityLevelSP).HasColumnName("SecurityLevelSP");
            this.Property(t => t.SelfPurchasedAmount).HasColumnName("SelfPurchasedAmount");
            this.Property(t => t.Servicer).HasColumnName("Servicer");
            this.Property(t => t.SFOType).HasColumnName("SFOType");
            this.Property(t => t.SpreadLibor).HasColumnName("SpreadLibor");
            this.Property(t => t.SpreadPrime).HasColumnName("SpreadPrime");
            this.Property(t => t.StdDevMoody).HasColumnName("StdDevMoody");
            this.Property(t => t.SyntheticCounterInstitution).HasColumnName("SyntheticCounterInstitution");
            this.Property(t => t.SyntheticType).HasColumnName("SyntheticType");
            this.Property(t => t.TradesWithAccrued).HasColumnName("TradesWithAccrued");
            this.Property(t => t.TransferAgent).HasColumnName("TransferAgent");
            this.Property(t => t.UpgradedDowngraded).HasColumnName("UpgradedDowngraded");
            this.Property(t => t.WeightedAvgAllInRate).HasColumnName("WeightedAvgAllInRate");
            this.Property(t => t.WeightedAvgSpread).HasColumnName("WeightedAvgSpread");
            this.Property(t => t.WeightedAvgDaysToReset).HasColumnName("WeightedAvgDaysToReset");
            this.Property(t => t.WeightedAvgDaysToResetAmount).HasColumnName("WeightedAvgDaysToResetAmount");
            this.Property(t => t.YieldToWorst).HasColumnName("YieldToWorst");
            this.Property(t => t.YTM).HasColumnName("YTM");
            this.Property(t => t.YTW).HasColumnName("YTW");
            this.Property(t => t.UserAmount1).HasColumnName("UserAmount1");
            this.Property(t => t.UserAmount2).HasColumnName("UserAmount2");
            this.Property(t => t.UserAmount3).HasColumnName("UserAmount3");
            this.Property(t => t.UserAmount4).HasColumnName("UserAmount4");
            this.Property(t => t.UserAmount5).HasColumnName("UserAmount5");
            this.Property(t => t.UserAmount6).HasColumnName("UserAmount6");
            this.Property(t => t.UserAmount7).HasColumnName("UserAmount7");
            this.Property(t => t.UserAmount8).HasColumnName("UserAmount8");
            this.Property(t => t.UserAmount9).HasColumnName("UserAmount9");
            this.Property(t => t.UserAmount10).HasColumnName("UserAmount10");
            this.Property(t => t.UserAmount11).HasColumnName("UserAmount11");
            this.Property(t => t.UserAmount12).HasColumnName("UserAmount12");
            this.Property(t => t.UserAmount13).HasColumnName("UserAmount13");
            this.Property(t => t.UserAmount14).HasColumnName("UserAmount14");
            this.Property(t => t.UserAmount15).HasColumnName("UserAmount15");
            this.Property(t => t.UserAmount16).HasColumnName("UserAmount16");
            this.Property(t => t.UserAmount17).HasColumnName("UserAmount17");
            this.Property(t => t.UserAmount18).HasColumnName("UserAmount18");
            this.Property(t => t.UserAmount19).HasColumnName("UserAmount19");
            this.Property(t => t.UserAmount20).HasColumnName("UserAmount20");
            this.Property(t => t.Is_Second_Lien).HasColumnName("Is Second Lien");
            this.Property(t => t.UserFlag2).HasColumnName("UserFlag2");
            this.Property(t => t.Is_First_Lien).HasColumnName("Is First Lien");
            this.Property(t => t.UserFlag4).HasColumnName("UserFlag4");
            this.Property(t => t.Is_Current_Pay_as_Default).HasColumnName("Is Current Pay as Default");
            this.Property(t => t.UserFlag6).HasColumnName("UserFlag6");
            this.Property(t => t.UserFlag7).HasColumnName("UserFlag7");
            this.Property(t => t.UserFlag8).HasColumnName("UserFlag8");
            this.Property(t => t.Is_Covenant_Pari_Passu).HasColumnName("Is Covenant Pari Passu");
            this.Property(t => t.Is_Moody_Senior_Secured_Loan).HasColumnName("Is Moody Senior Secured Loan");
            this.Property(t => t.Is_Purchased_Discount).HasColumnName("Is Purchased Discount");
            this.Property(t => t.UserFlag12).HasColumnName("UserFlag12");
            this.Property(t => t.UserFlag13).HasColumnName("UserFlag13");
            this.Property(t => t.Is_Cov_Lite).HasColumnName("Is Cov-Lite");
            this.Property(t => t.Is_Moody_s_DP_Rating_Derived).HasColumnName("Is Moody's DP Rating Derived");
            this.Property(t => t.UserFlag16).HasColumnName("UserFlag16");
            this.Property(t => t.UserFlag17).HasColumnName("UserFlag17");
            this.Property(t => t.UserFlag18).HasColumnName("UserFlag18");
            this.Property(t => t.UserFlag19).HasColumnName("UserFlag19");
            this.Property(t => t.UserFlag20).HasColumnName("UserFlag20");
            this.Property(t => t.zIs_Floor_in_Effect).HasColumnName("zIs Floor in Effect");
            this.Property(t => t.Counterparty_Type).HasColumnName("Counterparty Type");
            this.Property(t => t.OC_Recovery_Type).HasColumnName("OC Recovery Type");
            this.Property(t => t.Moody_s_DP_Rating___WARF__ADJ_).HasColumnName("Moody's DP Rating - WARF (ADJ)");
            this.Property(t => t.Moody_s_CF_Rating).HasColumnName("Moody's CF Rating");
            this.Property(t => t.Moody_s_DP_Rating___WARF).HasColumnName("Moody's DP Rating - WARF");
            this.Property(t => t.S_P_Issuer_Rating).HasColumnName("S&P Issuer Rating");
            this.Property(t => t.Moody_s_DP_Rating__Unnotched____WARF).HasColumnName("Moody's DP Rating (Unnotched) - WARF");
            this.Property(t => t.Moody_s_Credit_Watch).HasColumnName("Moody's Credit Watch");
            this.Property(t => t.Issuer_Name_Alternative).HasColumnName("Issuer Name Alternative");
            this.Property(t => t.Issuer_Country).HasColumnName("Issuer Country");
            this.Property(t => t.SIC_Moody).HasColumnName("SIC Moody");
            this.Property(t => t.SIC_S_P).HasColumnName("SIC S&P");
            this.Property(t => t.Moody_s_Outlook).HasColumnName("Moody's Outlook");
            this.Property(t => t.S_P_Credit_Watch).HasColumnName("S&P Credit Watch");
            this.Property(t => t.WAL_Date_GV__Hidden_).HasColumnName("WAL Date GV (Hidden)");
            this.Property(t => t.Adjusted_WAS).HasColumnName("Adjusted WAS");
            this.Property(t => t.Adjusted_WAC).HasColumnName("Adjusted WAC");
            this.Property(t => t.All_In_Rate__w_Floor_).HasColumnName("All-In-Rate (w/Floor)");
            this.Property(t => t.LIBOR_Floor_for_accrual_WAS).HasColumnName("LIBOR Floor for accrual WAS");
            this.Property(t => t.LIBOR_Floor_Credit_Addback).HasColumnName("LIBOR Floor Credit Addback");
            this.Property(t => t.LIBOR_Base_Rate_on_Notes).HasColumnName("LIBOR Base Rate on Notes");
            this.Property(t => t.Cost_Price_Discounts).HasColumnName("Cost Price Discounts");
            this.Property(t => t.Cost_Price__Discounts_).HasColumnName("Cost Price (Discounts)");
            this.Property(t => t.UserDecimal3).HasColumnName("UserDecimal3");
            this.Property(t => t.UserDecimal4).HasColumnName("UserDecimal4");
            this.Property(t => t.UserDecimal5).HasColumnName("UserDecimal5");
            this.Property(t => t.WAL_Date_Case__Hidden_).HasColumnName("WAL Date Case (Hidden)");
            this.Property(t => t.UserNumber2).HasColumnName("UserNumber2");
            this.Property(t => t.UserNumber3).HasColumnName("UserNumber3");
            this.Property(t => t.UserNumber4).HasColumnName("UserNumber4");
            this.Property(t => t.UserNumber5).HasColumnName("UserNumber5");
            this.Property(t => t.UserDate1).HasColumnName("UserDate1");
            this.Property(t => t.UserDate2).HasColumnName("UserDate2");
            this.Property(t => t.UserDate3).HasColumnName("UserDate3");
            this.Property(t => t.UserDate4).HasColumnName("UserDate4");
            this.Property(t => t.UserDate5).HasColumnName("UserDate5");
            this.Property(t => t.FacilityIncrease).HasColumnName("FacilityIncrease");
            this.Property(t => t.ParAmountNativeHypo).HasColumnName("ParAmountNativeHypo");
            this.Property(t => t.OutstandingNativeHypo).HasColumnName("OutstandingNativeHypo");
            this.Property(t => t.BankDeal_GlobalAmount).HasColumnName("BankDeal_GlobalAmount");
            this.Property(t => t.ExcessCurrentPay).HasColumnName("ExcessCurrentPay");
            this.Property(t => t.ExcessMoodyCaa).HasColumnName("ExcessMoodyCaa");
            this.Property(t => t.ExcessSPCCC).HasColumnName("ExcessSPCCC");
            this.Property(t => t.FeeSpread).HasColumnName("FeeSpread");
            this.Property(t => t.InterestFromReinvestment).HasColumnName("InterestFromReinvestment");
            this.Property(t => t.IsDelayedDraw).HasColumnName("IsDelayedDraw");
            this.Property(t => t.IsRatingMoodyPending).HasColumnName("IsRatingMoodyPending");
            this.Property(t => t.LiborBaseRateFloor).HasColumnName("LiborBaseRateFloor");
            this.Property(t => t.MarkPrice_BidPrice).HasColumnName("MarkPrice_BidPrice");
            this.Property(t => t.PaymentDate).HasColumnName("PaymentDate");
            this.Property(t => t.PrimarySpread).HasColumnName("PrimarySpread");
            this.Property(t => t.PrimarySpreadType).HasColumnName("PrimarySpreadType");
            this.Property(t => t.PrimeBaseRateFloor).HasColumnName("PrimeBaseRateFloor");
            this.Property(t => t.PriorityCategorySPI).HasColumnName("PriorityCategorySPI");
            this.Property(t => t.PriorityCategorySPII).HasColumnName("PriorityCategorySPII");
            this.Property(t => t.PriorityCategorySPIII).HasColumnName("PriorityCategorySPIII");
            this.Property(t => t.PriorityCategorySPIV).HasColumnName("PriorityCategorySPIV");
            this.Property(t => t.PriorityCategorySPV).HasColumnName("PriorityCategorySPV");
            this.Property(t => t.PriorityCategorySPVI).HasColumnName("PriorityCategorySPVI");
            this.Property(t => t.RatingMoodyIsShadow).HasColumnName("RatingMoodyIsShadow");
            this.Property(t => t.RatingMoodyIssue).HasColumnName("RatingMoodyIssue");
            this.Property(t => t.RatingSPIssue).HasColumnName("RatingSPIssue");
            this.Property(t => t.RatingSPIssueCreditWatch).HasColumnName("RatingSPIssueCreditWatch");
            this.Property(t => t.RecoveryAmountCurrentPay).HasColumnName("RecoveryAmountCurrentPay");
            this.Property(t => t.RecoveryAmountMoodyCaa).HasColumnName("RecoveryAmountMoodyCaa");
            this.Property(t => t.RecoveryAmountSPCCC).HasColumnName("RecoveryAmountSPCCC");
            this.Property(t => t.RecoveryRateMoodyFeed).HasColumnName("RecoveryRateMoodyFeed");
            this.Property(t => t.RecoveryRateSPI).HasColumnName("RecoveryRateSPI");
            this.Property(t => t.RecoveryRateSPII).HasColumnName("RecoveryRateSPII");
            this.Property(t => t.RecoveryRateSPIII).HasColumnName("RecoveryRateSPIII");
            this.Property(t => t.RecoveryRateSPIV).HasColumnName("RecoveryRateSPIV");
            this.Property(t => t.RecoveryRateSPV).HasColumnName("RecoveryRateSPV");
            this.Property(t => t.RecoveryRateSPVI).HasColumnName("RecoveryRateSPVI");
            this.Property(t => t.CoreSystem).HasColumnName("CoreSystem");
            this.Property(t => t.RecordModified).HasColumnName("RecordModified");
            this.Property(t => t.Scenario).HasColumnName("Scenario");
            this.Property(t => t.MarkPrice_AskPrice).HasColumnName("MarkPrice_AskPrice");
            this.Property(t => t.RatingMoodyIssueCreditWatch).HasColumnName("RatingMoodyIssueCreditWatch");
        }
    }
}
