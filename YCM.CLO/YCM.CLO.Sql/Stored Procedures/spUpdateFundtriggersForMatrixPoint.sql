CREATE PROCEDURE [clo].[spUpdateFundtriggersForMatrixPoint]
	@FundId INT = 1
AS
	
DECLARE 
@MatrixPoint INT,
@WALWarfAdj DECIMAL(28,10),
@WarfRecovery DECIMAL(28,10) = 43, @Spread  DECIMAL(28,10) = .0005, @Warf DECIMAL(28,10) = 40, @Diversity DECIMAL(28,10) = 5


DECLARE @MaxWarfTrigger [DECIMAL](38, 6)

select @WALWarfAdj = isnull(WALWARFAdj,0), 
@MaxWarfTrigger = ISNULL([MaxWarfTrigger],9999999) 
FROM CLO.Fund with(nolock) where FundId = @FundId

DECLARE @matrixWarfRecovery DECIMAL(28,10), @matrixSpread  DECIMAL(28,10), 
@matrixWarf   DECIMAL(28,10), @matrixWarfModifier DECIMAL(28,10), @matrixDiversity DECIMAL(28,10)

DECLARE @fundMoodysRecovery DECIMAL(28,10)

SELECT TOP 1 @fundMoodysRecovery = MoodyRecovery FROM CLO.FundCalculation WHERE FundId = @FundId ORDER BY DateId DESC

SELECT *,
ROW_NUMBER() OVER(PARTITION BY FundId ORDER BY CreatedOn DESC) AS Rownum
INTO #matrixdatas
FROM clo.MatrixPoint WHERE FundId  = @FundId

DELETE FROM #matrixdatas WHERE Rownum > 1

 if exists(select * from #matrixdatas)
 begin

	 SELECT TOP 1 
	 @matrixSpread = Spread,
	 @matrixWarfModifier = WARFModifier,
	 @matrixWarf = WARF,
	 @matrixDiversity = Diversity
	 FROM #matrixdatas

	 SELECT * FROM #matrixdatas
 
	 SELECT @matrixWarf '@matrixWarf',@fundMoodysRecovery AS '@fundMoodysRecovery',@WarfRecovery  '@WarfRecovery', @matrixWarfModifier '@matrixWarfModifier'

	 SELECT @matrixWarfRecovery = @WALWarfAdj + @matrixWarf + ((@fundMoodysRecovery - @WarfRecovery) * @matrixWarfModifier)

	  SELECT @matrixWarfRecovery

	 UPDATE fr
	 SET fr.RestrictionValue = (@matrixSpread + @Spread)*100
	 FROM clo.FundRestriction fr
	 JOIN CLO.Field fi ON fi.FieldId = fr.FieldId
	 WHERE  FundRestrictionTypeId = 1 AND FundId = @FundId AND fi.FieldTitle = 'SPREAD'

	 UPDATE fr
	 SET fr.RestrictionValue = @matrixSpread * 100 
	 FROM clo.FundRestriction fr
	 JOIN CLO.Field fi ON fi.FieldId = fr.FieldId
	 WHERE  FundRestrictionTypeId = 2 AND FundId = @FundId AND fi.FieldTitle = 'SPREAD'

	 SELECT @matrixWarfRecovery = ISNULL(@MaxWarfTrigger,@matrixWarfRecovery) WHERE @matrixWarfRecovery > @MaxWarfTrigger

	 UPDATE fr
	 SET fr.RestrictionValue = round(@matrixWarfRecovery - @Warf,0)
	 FROM clo.FundRestriction fr
	 JOIN CLO.Field fi ON fi.FieldId = fr.FieldId
	 WHERE  FundRestrictionTypeId = 1 AND FundId = @FundId AND fi.FieldTitle = 'WARF'

	 UPDATE fr
	 SET fr.RestrictionValue = round(@matrixWarfRecovery ,0)
	 FROM clo.FundRestriction fr
	 JOIN CLO.Field fi ON fi.FieldId = fr.FieldId
	 WHERE  FundRestrictionTypeId = 2 AND FundId = @FundId AND fi.FieldTitle = 'WARF'


	 UPDATE fr
	 SET fr.RestrictionValue = @matrixDiversity + @Diversity
	 FROM clo.FundRestriction fr
	 JOIN CLO.Field fi ON fi.FieldId = fr.FieldId
	 WHERE  FundRestrictionTypeId = 1 AND FundId = @FundId AND fi.FieldTitle = 'DIVERSITY'

	 UPDATE fr
	 SET fr.RestrictionValue = @matrixDiversity 
	 FROM clo.FundRestriction fr
	 JOIN CLO.Field fi ON fi.FieldId = fr.FieldId
	 WHERE  FundRestrictionTypeId = 2 AND FundId = @FundId AND fi.FieldTitle = 'DIVERSITY'

 end

 DROP TABLE #matrixdatas


 
RETURN 0
