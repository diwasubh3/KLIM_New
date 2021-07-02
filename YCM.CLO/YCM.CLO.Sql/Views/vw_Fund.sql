CREATE VIEW [clo].[vw_Fund]
AS 
SELECT 
	   [FundId] = [ParentFundId]
      ,[FundCode] = [ParentFundCode]
      ,[FundDesc] = MAX([FundDesc])
      ,[PrincipalCash] = SUM([PrincipalCash])
      ,[WSOLastUpdatedOn] = MAX([WSOLastUpdatedOn])
      ,[LiabilityPar] = SUM([LiabilityPar])
      ,[EquityPar] = SUM([EquityPar])
      ,[TargetPar] = SUM([TargetPar])
      ,[WALifeAdjustment] = MAX([WALifeAdjustment])
      ,[RecoveryMultiplier] = MAX([RecoveryMultiplier])
      ,[AssetParPercentageThreshold] = MAX([AssetParPercentageThreshold])
      ,[CreatedOn] = MAX([CreatedOn])
      ,[CreatedBy] = MAX([CreatedBy])
      ,[LastUpdatedOn] = MAX([LastUpdatedOn])
      ,[LastUpdatedBy] = MAX([LastUpdatedBy])
      ,[CLOFileName] = MAX([CLOFileName])
      ,[IsStale] = ISNULL([IsStale],0)
      ,[IsPrincipalCashStale] = ISNULL([IsPrincipalCashStale],0)
      ,[DisplayText] = MAX([DisplayText])
      ,[IsActive] = ISNULL([IsActive],0)
      ,[WSOSpread] = MAX([WSOSpread])
      ,[WSOWARF] = MAX([WSOWARF])
      ,[WSOMoodyRecovery] = MAX([WSOMoodyRecovery])
      ,[WSOWALife] = MAX([WSOWALife])
      ,[WSODiversity] = MAX([WSODiversity])
      ,[PortfolioName] = MAX([PortfolioName])
      ,[SortOrder] = MAX([SortOrder])
      ,[WALWARFAdj] = MAX([WALWARFAdj])
      ,[MaxWarfTrigger] = MAX([MaxWarfTrigger])
      ,[ClassEPar] = MAX([ClassEPar])
      ,[WALDate] = MAX([WALDate])
      ,[ReInvestEndDate] = MAX([ReInvestEndDate])
      ,[WalDateAdj] = MAX([WalDateAdj])
	  ,IsWareHouse = ISNULL(IsWareHouse,0)

	  ,[BloombergCode] = MAX([BloombergCode]),
	   [PricingDate]= MAX([PricingDate]),
	   [MgmtFees] = MAX([MgmtFees]),
	   [OperatingExpenses] = MAX([OperatingExpenses]),
	   [ClosingDate] = MAX([ClosingDate]),
	   [NonCallEndsDate] = MAX([NonCallEndsDate]),
	   [FinalMaturity] = MAX([FinalMaturity]),
	   [ProjectedEquityDistribtion]  = MAX([ProjectedEquityDistribtion])

FROM [clo].Fund WITH(NOLOCK)
WHERE IsActive = 1 or IsWareHouse = 1
GROUP BY [ParentFundId],[ParentFundCode],ISNULL([IsActive],0),ISNULL([IsStale],0),ISNULL([IsPrincipalCashStale],0),ISNULL(IsWareHouse,0)