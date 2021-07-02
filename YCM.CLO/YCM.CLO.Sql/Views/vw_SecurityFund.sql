CREATE VIEW CLO.vw_SecurityFund
AS 
SELECT
	s.SecurityId
	, s.SecurityCode
	, s.SecurityDesc
	, s.BBGId
	, s.Issuer
	, s.IssuerDesc
	, s.Facility
	, s.CallDate
	, s.MaturityDate
	, s.SnpIndustry
	, s.MoodyIndustry
	, s.IsFloating
	, s.LienType
	, s.IssuerId
	, s.MoodyIndustryId

	, s.IsOnWatch
	, s.WatchId
	, s.WatchObjectTypeId
	, s.WatchObjectId
	, s.WatchComments
	, s.WatchLastUpdatedOn
	, s.WatchUser

	, s.SellCandidateId
	, s.SellCandidateObjectTypeId
	, s.SellCandidateObjectId
	, s.SellCandidateComments
	, s.SellCandidateLastUpdatedOn
	, s.SellCandidateUser

	, s.OrigSecurityCode
	, s.OrigSecurityDesc
	, s.OrigBBGId
	, s.OrigIssuer
	, s.GICSIndustry
	, s.OrigFacility
	, s.OrigCallDate
	, s.OrigMaturityDate
	, s.OrigSnpIndustry
	, s.OrigMoodyIndustry
	, s.OrigIsFloating
	, s.OrigLienType
	, s.OrigMoodyFacilityRatingAdjusted
	, s.OrigMoodyCashFlowRatingAdjusted
	, s.SecurityLastUpdatedOn
	, s.SecurityLastUpdatedBy
	, s.SecurityCreatedOn
	, s.SecurityCreatedBy
	, s.SourceId
	, s.MoodyFacilityRatingAdjusted
	, s.MoodyCashFlowRatingAdjusted
	, s.HasPositions
	, f.FundId
	, f.FundCode
	, f.FundDesc
	, f.PrincipalCash
	, f.EquityPar
	, f.LiabilityPar 
	, f.TargetPar
	, f.WALifeAdjustment
	, ISNULL(NULLIF(f.IsStale,CAST(0 AS BIT)),f.IsPrincipalCashStale) IsStale
	, ta.Allocation Allocation
	, ta.HasBuy HasBuy
	, ta.TotalAllocation  TotalAllocation
	, ta.TradePrice
	, s.LienTypeId
	, s.IsInDefault
	, s.DefaultDate
FROM CLO.vw_security s WITH(NOLOCK)
	CROSS JOIN [clo].[vw_Fund] f WITH(NOLOCK)
	LEFT JOIN  CLO.GetActiveTrades() ta ON s.SecurityId = ta.SecurityId AND f.FundId = ta.FundId
WHERE f.IsActive = 1 OR f.IsWarehouse = 1
GO

