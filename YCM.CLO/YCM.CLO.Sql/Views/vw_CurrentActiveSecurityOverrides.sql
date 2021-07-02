CREATE VIEW [CLO].[vw_CurrentActiveSecurityOverrides]  WITH SCHEMABINDING 
	AS 		
	SELECT SecurityId,OverrideValue,FieldName
		from [CLO].SecurityOverride so  with (nolock)
		join [CLO].Field f with(nolock) on so.FieldId = f.FieldId
		where	(so.IsDeleted IS NULL OR so.IsDeleted =0)
		AND	(so.EffectiveFrom IS NULL OR so.EffectiveFrom <= CONVERT(date, getdate()))
		AND (so.EffectiveTo IS NULL OR so.EffectiveTo > CONVERT(date, getdate()))
		