CREATE VIEW [CLO].[vw_CLOSummary]
	AS 

	SELECT 
	f.FundCode,
	ISNULL(NULLIF(f.IsStale,CAST(0 AS BIT)),f.[IsPrincipalCashStale]) IsStale,

	p.DateId DateId,
	p.Par,
	p.BODPar,
	p.Spread,
	p.BODSpread,
	p.TotalCoupon,
	p.BODTotalCoupon,
	p.WARF,
	p.BODWARF,
	p.MoodyRecovery,
	p.BODMoodyRecovery,
	p.Bid,
	p.BODBid,
	p.PrincipalCash,
	p.BODPrincipalCash,
	f.FundId,
	p.Diversity,
	p.BODDiversity,
	p.CleanNav ,
	p.BODCleanNav,
	p.WAMaturityDays,
	p.BODWAMaturityDays,
	p.AssetPar,
	p.PriorDayExposure,
	p.PriorDayPrincipalCash,
	WSOSpread = ISNULL(f.WSOSpread,p.Spread),
	WSOWARF = ISNULL(f.WSOWARF,p.WARF) ,
	WSOMoodyRecovery = ISNULL(f.WSOMoodyRecovery,p.MoodyRecovery) ,
	WSOWALife = ISNULL(f.WSOWALife,WAMaturityDays) ,
	WSODiversity = ISNULL(f.WSODiversity,p.Diversity) ,
	f.IsWarehouse ,
	f.SortOrder ,
	p.MatrixImpliedSpread,
	p.SnpRecovery,
	p.BBMVOC,
	p.WALCushion,
	p.TimeToReinvest,
	p.[B3ToAssetParPct],
	p.[BMinusToAssetParPct]
	FROM CLO.FundCalculation p with(nolock)
	RIGHT JOIN [clo].Fund f WITH(NOLOCK) ON f.FundId = p.FundId
	where f.IsActive = 1 and p.DateId = CLO.GetPrevDayDateId()

GO
