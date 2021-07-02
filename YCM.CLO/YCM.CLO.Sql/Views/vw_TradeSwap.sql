CREATE VIEW [CLO].[vw_TradeSwap]  WITH SCHEMABINDING 
	AS SELECT 
	 [TradeSwapSnapshotId]
      ,[TradeSwapId]
      ,[SellSecurityId]
      ,[SellFundId]
      ,CAST([SellExposure] AS DECIMAL(36,2))  [SellExposure]
      ,CAST([SellTotalExposure]		 AS DECIMAL(36,2))  [SellTotalExposure]
      ,CAST([SellSecurityBidPrice]	 AS DECIMAL(36,2))	[SellSecurityBidPrice]
      ,CAST([SellPctPosition]		 AS DECIMAL(36,2))	[SellPctPosition]
      ,CAST([SellSpread]			 AS DECIMAL(36,2))	[SellSpread]
      ,CAST([SellLiquidityScore]	 AS DECIMAL(36,2))	[SellLiquidityScore]
      ,[SellMaturityDate]
      ,[SellIssuer]
      ,[SellFacility]
	  ,[SellMoodyAdjCFR]
	  ,[SellMoodyAdjFacility]

      ,[BuySecurityId]
      ,[BuySecurityOfferPrice]
      ,[BuyFundId]
      ,CAST([BuyExposure]				 AS DECIMAL(36,2))	[BuyExposure]
      ,CAST([BuyTotalExposure]			 AS DECIMAL(36,2))	[BuyTotalExposure]
      ,CAST([BuyPctPosition]			 AS DECIMAL(36,2))	[BuyPctPosition]
      ,CAST([BuySpread]					 AS DECIMAL(36,2))	[BuySpread]
      ,CAST([BuyLiquidityScore]			 AS DECIMAL(36,2))	[BuyLiquidityScore]
      ,[BuyMaturityDate]
      ,[BuyIssuer]
      ,[BuyFacility]
	  ,[BuyMoodyAdjCFR]
	  ,[BuyMoodyAdjFacility]
	  ,buy.SecurityCode [BuySecurityCode]
	  ,sell.SecurityCode [SellSecurityCode]

      ,ts.[CreatedOn]   

	  ,CAST([SellRecovery]				 AS DECIMAL(36,2))		[SellRecovery]
	  ,CAST([BuyRecovery]				 AS DECIMAL(36,2))		[BuyRecovery]
	  ,CAST([SellYield]					 AS DECIMAL(36,2))		[SellYield]
	  ,CAST([BuyYield]					 AS DECIMAL(36,2))		[BuyYield]
	  ,CAST([BuySecurityBidPrice]		 AS DECIMAL(36,2))		[BuySecurityBidPrice]
	  ,CAST([SellSecurityOfferPrice]	 AS DECIMAL(36,2))		[SellSecurityOfferPrice]
	  ,CAST(BuySecurityCreditScore		 AS DECIMAL(36,2))		[BuySecurityCreditScore]
	  ,CAST([SellSecurityCreditScore]	 AS DECIMAL(36,2))		[SellSecurityCreditScore]


	FROM [clo].TradeSwapSnapshot ts WITH(NOLOCK) 
	JOIN [CLO].[Security] sell WITH(NOLOCK) ON ts.SellSecurityId = sell.SecurityId
	JOIN [CLO].[Security] buy  WITH(NOLOCK) ON ts.BuySecurityId = buy.SecurityId
	
	
