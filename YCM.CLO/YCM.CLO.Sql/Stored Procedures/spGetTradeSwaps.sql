CREATE PROCEDURE [CLO].[spGetTradeSwaps]
	@fundId int = 0,
	@tradeSwapId int
AS
	SELECT * FROM [CLO].[vw_TradeSwap]
	WHERE SellFundId = @fundId AND [TradeSwapId] = @tradeSwapId AND BuyFundId = @fundId
	ORDER BY [TradeSwapSnapshotId]
RETURN 0
