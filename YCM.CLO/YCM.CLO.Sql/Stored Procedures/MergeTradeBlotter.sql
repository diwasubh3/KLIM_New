CREATE PROCEDURE [CLO].[MergeTradeBlotter]
	@JobId INT
AS
	
	DELETE FROM CLO.TradeBlotter 
	WHERE TradeId in (SELECT Trade_ID FROM [CLO].[StageTradeBlotter] WHERE TradeBlotterJobId = @JobId)

	INSERT INTO [CLO].[TradeBlotter]
			   ([TradeId]
			   ,[AssetPrimaryId]
			   ,[Portfolio_Name]
			   ,[Issuer_Name]
			   ,[Asset_Name]
			   ,[Currency]
			   ,[TradeType]
			   ,[TradeSettleDate]
			   ,[TradeDate]
			   ,[CounterBankName]
			   ,[BrokerBankName]
			   ,[TradeDescription]
			   ,[TradeAmount]
			   ,[AccruedInterest]
			   ,[CounterParty]
			   ,[CounterPartyEntity]
			   ,[CounterPartyDisplay]
			   ,[IssuerDisplayName]
			   ,[TradeCommissions]
			   ,TradeOriginalCommitment
			   ,TradeOriginalParAmount
			   )
     
	 SELECT 
				[Trade_ID]
			   ,[AssetPrimaryId]
			   ,[Portfolio_Name]
			   ,[Issuer_Name]
			   ,[Asset_Name]
			   ,[CurrencyType_Identifier]
			   ,[TradeTypeDescription]
			   ,[Trade_SettleDate]
			   ,[Trade_TradeDate]
			   ,[CounterBank_Name]
			   ,[BrokerBank_Name]
			   ,[TradeDescription]
			   ,[TradeAmount]
			   ,[AccruedInterest]
			   ,[CounterParty]
			   ,[CounterPartyEntity]
			   ,[CounterParty_Display]
			   ,[Issuer_DisplayName]
			   ,NULLIF(ABS(ISNULL([TradeAmount],0)*0.0025),0) [TradeCommissions],
			   Trade_OriginalCommitment,
			   Trade_OriginalParAmount
	 FROM 
			CLO.StageTradeBlotter WHERE  
			TradeBlotterJobId = @JobId
			AND RTRIM(LTRIM(ISNULL([CounterBank_Name],''))) <> 'INTERNAL'

RETURN 0
