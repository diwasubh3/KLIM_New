CREATE VIEW CLO.vw_SecurityMarketCalculation
AS
	WITH	PivotedSecurityOverrides_cfe
				AS (
						SELECT SecurityId, SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate
							, MaturityDate, SnpIndustry, MoodyIndustry, IsCovLite, IsFloating, LienType
							, MoodyFacilityRatingAdjusted, MoodyCashFlowRatingAdjusted
						FROM CLO.vw_CurrentActiveSecurityOverrides
						PIVOT ( MAX(OverrideValue) FOR FieldName IN (SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate
								, MaturityDate, SnpIndustry, MoodyIndustry, IsCovLite, IsFloating, LienType
								, MoodyFacilityRatingAdjusted, MoodyCashFlowRatingAdjusted) ) AS AvgIncomePerDay
					)

	SELECT	s.SecurityId, ISNULL(os.SecurityCode, s.SecurityCode) SecurityCode
		, ISNULL(os.SecurityDesc, s.SecurityDesc) SecurityDesc, ISNULL(os.BBGId, s.BBGId) BBGId
		, ISNULL(os.Issuer, i.IssuerDesc) Issuer, ISNULL(os.Facility, f.FacilityDesc) Facility
		, ISNULL(CAST(os.CallDate AS DATE), s.CallDate) CallDate
		, ISNULL(CAST(os.MaturityDate AS DATE), s.MaturityDate) MaturityDate
		, ISNULL(os.SnpIndustry, snpindustry.IndustryDesc) SnpIndustry
		, ISNULL(os.MoodyIndustry, moodyindustry.IndustryDesc) MoodyIndustry
		, ISNULL(CAST(CASE	WHEN os.IsFloating = 'Fixed' THEN 0 WHEN os.IsFloating = 'Floating' THEN 1 END AS BIT), s.IsFloating) IsFloating
		, ISNULL(os.LienType, lientype.LienTypeDesc) LienType, s.IssuerId, w.WatchId, s.MoodyIndustryId
		, CAST(CASE	WHEN w.WatchId IS NULL THEN 0 ELSE 1 END AS BIT) IsOnWatch
		, w.WatchObjectTypeId, w.WatchObjectId, w.WatchComments
		, FORMAT(w.WatchLastUpdatedOn, 'MM/dd/yyyy hh:mm tt') WatchLastUpdatedOn
		, w.WatchUser, s.SecurityCode OrigSecurityCode, s.SecurityDesc OrigSecurityDesc
		, s.BBGId OrigBBGId, i.IssuerDesc OrigIssuer, s.GICSIndustry
		, LTRIM(f.FacilityDesc) OrigFacility
		, CASE WHEN s.CallDate <> '1900-01-01' THEN CONVERT(VARCHAR(10), s.CallDate, 101) ELSE NULL END OrigCallDate
		, CONVERT(VARCHAR(10), s.MaturityDate, 101) OrigMaturityDate, snpindustry.IndustryDesc OrigSnpIndustry
		, moodyindustry.IndustryDesc OrigMoodyIndustry
		, CASE WHEN (s.IsFloating = 1) THEN 'Floating' ELSE 'Fixed' END OrigIsFloating
		, lientype.LienTypeDesc OrigLienType, c.MoodyFacilityRatingAdjusted AS OrigMoodyFacilityRatingAdjusted
		, m.MoodyCashFlowRatingAdjusted AS OrigMoodyCashFlowRatingAdjusted, s.LastUpdatedOn SecurityLastUpdatedOn
		, s.LastUpdatedBy SecurityLastUpdatedBy, s.CreatedOn SecurityCreatedOn
		, s.CreatedBy SecurityCreatedBy, s.SourceId
		, ISNULL(os.MoodyFacilityRatingAdjusted, c.MoodyFacilityRatingAdjusted) MoodyFacilityRatingAdjusted
		, ISNULL(os.MoodyCashFlowRatingAdjusted, m.MoodyCashFlowRatingAdjusted) MoodyCashFlowRatingAdjusted
		, CAST(NULL AS BIT) HasPositions
	FROM	CLO.Security s WITH (NOLOCK)
	LEFT JOIN CLO.Issuer i WITH (NOLOCK) ON i.IssuerId = s.IssuerId
	LEFT JOIN CLO.Facility f WITH (NOLOCK) ON f.FacilityId = s.FacilityId
	LEFT JOIN CLO.Industry snpindustry WITH (NOLOCK) ON snpindustry.IndustryId = s.SnPIndustryId
														AND snpindustry.IsSnP = 1
	LEFT JOIN CLO.Industry moodyindustry WITH (NOLOCK) ON moodyindustry.IndustryId = s.MoodyIndustryId
														AND moodyindustry.IsMoody = 1
	LEFT JOIN CLO.LienType lientype WITH (NOLOCK) ON lientype.LienTypeId = s.LienTypeId
	LEFT JOIN CLO.Watch w ON (w.WatchObjectTypeId = 1
								AND w.WatchObjectId = s.SecurityId)
								OR (w.WatchObjectId = s.IssuerId
								AND w.WatchObjectTypeId = 2)
	LEFT JOIN PivotedSecurityOverrides_cfe os ON os.SecurityId = s.SecurityId
	LEFT JOIN CLO.vw_MarketData m ON s.SecurityId = m.SecurityId
	LEFT JOIN CLO.vw_CalculationSecurity c ON s.SecurityId = c.SecurityId
	WHERE	ISNULL(IsDeleted, 0) = 0
GO
