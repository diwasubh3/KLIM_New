CREATE PROCEDURE [CLO].[spCleanUpTradeSwaps]
	@tradeSwapId int = 0
AS
	
	DELETE FROM CLO.TradeSwapSnapShot WHERE TradeSwapId = @tradeSwapId

RETURN 0
