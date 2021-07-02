CREATE FUNCTION CLO.GetActiveTrades ()
RETURNS @returntable TABLE
	(
		 SecurityId INT
	   , FundId INT
	   , Allocation NUMERIC(38, 10)
	   , TradedCash NUMERIC(38, 10)
	   , TradePrice NUMERIC(38, 4)
	   , TotalAllocation NUMERIC(38, 10)
	   , HasBuy BIT
	   , HasSell BIT
	   , FundCode VARCHAR(100)
	)
AS
	BEGIN

		DECLARE @tradeallocations TABLE
		(
			 SecurityId INT
		   , FundId INT
		   , Allocation NUMERIC(38, 10)
		   , TradedCash NUMERIC(38, 10)
		   , TradePrice NUMERIC(38, 4)
		   , HasBuy BIT
		   , FundCode VARCHAR(100)
		)

		DECLARE @exposure TABLE
		(
			  FundId INT
			, SecurityId INT
			, Exposure DECIMAL(38, 4)
		)

		DECLARE @exclusions TABLE
		(
			  FundId INT
			, SecurityId INT
		)

		DECLARE @prevDayDateId INT 
		SELECT @prevDayDateId = CLO.GetPrevDayDateId()

		INSERT INTO @exposure (FundId, SecurityId, Exposure)
		SELECT f.ParentFundId, p.SecurityId, SUM(p.Exposure) AS Exposure
		FROM CLO.Position (NOLOCK) p
		JOIN clo.Fund f WITH(NOLOCK)  ON p.FundId = F.FundId	
		WHERE DateId = @prevDayDateId
		GROUP BY f.ParentFundId, p.SecurityId

		INSERT INTO @tradeallocations (SecurityId, FundId, Allocation, TradedCash, TradePrice, HasBuy, FundCode)
		SELECT t.SecurityId, f.ParentFundId
				, SUM(CASE WHEN ISNULL(t.IsBuy, 0) = 0 THEN -1 * ISNULL(ta.NewAllocation, 0) ELSE ISNULL(ta.NewAllocation, 0) END) Allocation
				, SUM(CASE WHEN ISNULL(t.IsBuy, 0) = 0 THEN (ISNULL(ta.NewAllocation, 0) * (ISNULL(t.TradePrice, 0) * 0.01))
						 ELSE (-1 * ISNULL(ta.NewAllocation, 0) * (ISNULL(t.TradePrice, 0) * 0.01)) END) TradedCash
				, MAX(t.TradePrice), CAST(MAX(CASE WHEN t.IsBuy = 1 THEN 1 ELSE 0 END) AS BIT)
				, f.ParentFundCode
		FROM CLO.Trade t WITH (NOLOCK)
		JOIN CLO.TradeAllocation ta WITH (NOLOCK) ON t.TradeId = ta.TradeId
		JOIN CLO.Fund f WITH (NOLOCK) ON ta.FundId = f.FundId
		WHERE (ISNULL(t.IsCancelled, 0) <> 1)
		AND (ISNULL(t.KeepOnBlotter, 0) = 1 OR t.DateId = @prevDayDateId)
		GROUP BY t.SecurityId, f.ParentFundId, f.ParentFundCode

		INSERT INTO @exposure (FundId, SecurityId, Exposure)
		SELECT FundId, SecurityId, SUM(Allocation)
		FROM @tradeallocations
		GROUP BY FundId, SecurityId

		INSERT INTO @exclusions (FundId, SecurityId)
		SELECT FundId, SecurityId
		FROM @exposure
		GROUP BY FundId, SecurityId
		HAVING SUM(Exposure) < 0

		INSERT INTO @returntable (SecurityId, FundId, Allocation, TradedCash, TotalAllocation, TradePrice, HasBuy, HasSell, FundCode)
		SELECT SecurityId, FundId, Allocation, TradedCash, SUM(Allocation) OVER (PARTITION BY SecurityId)
			, TradePrice, HasBuy, CASE WHEN HasBuy = 1 THEN 0 ELSE 1 END, FundCode
		FROM @tradeallocations A
		WHERE NOT EXISTS (SELECT * FROM @exclusions WHERE A.SecurityId = SecurityId AND A.FundId = FundId)

		RETURN
	END

