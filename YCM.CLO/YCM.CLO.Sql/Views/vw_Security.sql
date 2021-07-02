CREATE VIEW CLO.vw_Security
AS
	WITH PivotedSecurityOverrides_cfe
		AS (SELECT	SecurityId, SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate, MaturityDate, SnpIndustry
				, MoodyIndustry, IsCovLite, IsFloating, LienType, MoodyFacilityRatingAdjusted, MoodyCashFlowRatingAdjusted
			FROM CLO.vw_CurrentActiveSecurityOverrides WITH (NOLOCK)
			PIVOT (MAX(OverrideValue) FOR FieldName IN (SecurityCode, SecurityDesc, BBGId, Issuer, Facility, CallDate
				, MaturityDate, SnpIndustry, MoodyIndustry, IsCovLite, IsFloating, LienType, MoodyFacilityRatingAdjusted
				, MoodyCashFlowRatingAdjusted) ) AS AvgIncomePerDay)

	SELECT s.SecurityId, ISNULL(os.SecurityCode, s.SecurityCode) SecurityCode
		, ISNULL(os.SecurityDesc, s.SecurityDesc) SecurityDesc, ISNULL(os.BBGId, s.BBGId) BBGId
		, COALESCE(os.Issuer, i.IssuerCode, i.IssuerDesc) Issuer, i.IssuerDesc IssuerDesc
		, ISNULL(os.Facility, f.FacilityDesc) Facility, ISNULL(CAST(os.CallDate AS DATE), s.CallDate) CallDate
		, ISNULL(CAST(os.MaturityDate AS DATE), s.MaturityDate) MaturityDate
		, ISNULL(os.SnpIndustry, snpindustry.IndustryDesc) SnpIndustry
		, ISNULL(os.MoodyIndustry, moodyindustry.IndustryDesc) MoodyIndustry
		, ISNULL(CAST(CASE	WHEN os.IsFloating = 'Fixed' THEN 0 WHEN os.IsFloating = 'Floating' THEN 1 END AS BIT), s.IsFloating) IsFloating
		, ISNULL(os.LienType, lientype.LienTypeDesc) LienType
		, s.IssuerId, ISNULL(wi.WatchId, ws.WatchId) WatchId
		, CAST (CASE	WHEN ISNULL(wi.WatchId, ws.WatchId) IS NULL THEN 0 ELSE 1 END AS BIT) IsOnWatch
		, ISNULL(wi.WatchObjectTypeId, ws.WatchObjectTypeId) WatchObjectTypeId
		, ISNULL(wi.WatchObjectId, ws.WatchObjectId) WatchObjectId, ISNULL(wi.WatchComments, ws.WatchComments) WatchComments
		, FORMAT(ISNULL(wi.WatchLastUpdatedOn, ws.WatchLastUpdatedOn), 'MM/dd/yyyy hh:mm tt') WatchLastUpdatedOn
		, ISNULL(wi.WatchUser, ws.WatchUser) WatchUser
		, s.MoodyIndustryId, s.SecurityCode OrigSecurityCode, s.SecurityDesc OrigSecurityDesc
		, s.BBGId OrigBBGId, ISNULL(i.IssuerCode, i.IssuerDesc) OrigIssuer, s.GICSIndustry, LTRIM(f.FacilityDesc) OrigFacility
		, CASE WHEN s.CallDate <> '1900-01-01' THEN CONVERT(VARCHAR(10), s.CallDate, 101) ELSE NULL END OrigCallDate
		, CONVERT(VARCHAR(10), s.MaturityDate, 101) OrigMaturityDate, snpindustry.IndustryDesc OrigSnpIndustry
		, moodyindustry.IndustryDesc OrigMoodyIndustry, CASE	WHEN (s.IsFloating = 1) THEN 'Floating' ELSE 'Fixed' END OrigIsFloating
		, lientype.LienTypeDesc OrigLienType, NULL OrigMoodyFacilityRatingAdjusted, NULL OrigMoodyCashFlowRatingAdjusted
		, s.LastUpdatedOn SecurityLastUpdatedOn, s.LastUpdatedBy SecurityLastUpdatedBy, s.CreatedOn SecurityCreatedOn
		, s.CreatedBy SecurityCreatedBy, s.SourceId, os.MoodyFacilityRatingAdjusted, os.MoodyCashFlowRatingAdjusted
		, CAST(NULL AS BIT) HasPositions
		, lientype.LienTypeId
		, ISNULL(sci.WatchId, scs.WatchId) SellCandidateId
		, ISNULL(sci.WatchObjectTypeId, scs.WatchObjectTypeId) SellCandidateObjectTypeId
		, ISNULL(sci.WatchObjectId, scs.WatchObjectId) SellCandidateObjectId, ISNULL(sci.WatchComments, scs.WatchComments) SellCandidateComments
		, FORMAT(ISNULL(sci.WatchLastUpdatedOn, scs.WatchLastUpdatedOn), 'MM/dd/yyyy hh:mm tt') SellCandidateLastUpdatedOn
		, ISNULL(sci.WatchUser, scs.WatchUser) SellCandidateUser
		, s.IsInDefault
		, s.DefaultDate
	FROM CLO.Security s WITH (NOLOCK)
	LEFT JOIN CLO.Issuer i WITH (NOLOCK) ON i.IssuerId = s.IssuerId
	LEFT JOIN CLO.Facility f WITH (NOLOCK) ON f.FacilityId = s.FacilityId
	LEFT JOIN CLO.Industry snpindustry WITH (NOLOCK) ON snpindustry.IndustryId = s.SnPIndustryId
														AND snpindustry.IsSnP = 1
	LEFT JOIN CLO.Industry moodyindustry WITH (NOLOCK) ON moodyindustry.IndustryId = s.MoodyIndustryId
														AND moodyindustry.IsMoody = 1
	LEFT JOIN CLO.LienType lientype WITH (NOLOCK) ON lientype.LienTypeId = s.LienTypeId
	LEFT JOIN CLO.Watch ws WITH (NOLOCK) ON (ws.WatchObjectTypeId = 1 AND ws.WatchTypeId = 1
														AND ws.WatchObjectId = s.SecurityId)
	LEFT JOIN CLO.Watch wi WITH (NOLOCK) ON (wi.WatchObjectId = s.IssuerId AND wi.WatchTypeId = 1
														AND wi.WatchObjectTypeId = 2)
	LEFT JOIN CLO.Watch scs WITH (NOLOCK) ON (scs.WatchObjectTypeId = 1 AND scs.WatchTypeId = 2
														AND scs.WatchObjectId = s.SecurityId)
	LEFT JOIN CLO.Watch sci WITH (NOLOCK) ON (sci.WatchObjectId = s.IssuerId AND sci.WatchTypeId = 2
														AND sci.WatchObjectTypeId = 2)
	LEFT JOIN PivotedSecurityOverrides_cfe os ON os.SecurityId = s.SecurityId
	WHERE ISNULL(IsDeleted, 0) = 0
GO
