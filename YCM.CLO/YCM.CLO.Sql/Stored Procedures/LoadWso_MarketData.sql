CREATE PROCEDURE [CLO].[LoadWso_MarketData]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
    --DECLARE @DatasetKeys TABLE (DatasetKey INT)
    --INSERT INTO @DatasetKeys
    --(
    --    DatasetKey
    --)
    --SELECT (1997)
    --UNION
    --SELECT (1998)
    --UNION
    --SELECT (1999)
    --UNION
    --SELECT (2000)

    BEGIN TRANSACTION

    DELETE FROM CLO.MarketData
    WHERE DateId = @asOfDateId
	AND FundId IN (SELECT DISTINCT D.DealId
		FROM [CLO].[WsoExtractAssets] EA
        JOIN @DatasetKeys DK
            ON DK.DatasetKey = EA.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON DK.DatasetKey = D.DatasetKey)

    DELETE FROM CLO.WSOMarketData
    WHERE DateId = @asOfDateId
	AND FundId IN (SELECT DISTINCT D.DealId
		FROM [CLO].[WsoExtractAssets] EA
        JOIN @DatasetKeys DK
            ON DK.DatasetKey = EA.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON DK.DatasetKey = D.DatasetKey)

    INSERT INTO CLO.MarketData
    (
        FundId,
		DateId,
        SecurityId,
        Bid,
        Offer,
		CostPrice,
        Spread,
        LiborFloor,
		LiborBaseRate,
		[MoodyDPRatingId],
        MoodyCashFlowRatingId,
        MoodyCashFlowRatingAdjustedId,
        MoodyFacilityRatingId,
        MoodyRecovery,
        SnPIssuerRatingId,
        SnPIssuerRatingAdjustedId,
        SnPFacilityRatingId,
        SnPfacilityRatingAdjustedId,
        SnPRecoveryRatingId,
        MoodyOutlook,
        MoodyWatch,
        SnPWatch,
		SnpCreditWatch,
        NextReportingDate,
        FiscalYearEndDate,
        AgentBank,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy
    )
    SELECT 
		D.DealID AS FundId,
		@asOfDateId,
        (
            SELECT TOP 1
                SecurityId
            FROM CLO.Security
            WHERE SecurityCode = EA.SecurityId
        ) AS SecurityId,
        CAST(EA.MarkPrice_BidPrice AS NUMERIC(38, 10)) AS Bid, --NEED TO MAKE SURE ABOUT SCALE
        CAST(EA.MarkPrice_AskPrice AS NUMERIC(38, 10)) AS Offer,                                           --'***MISSING***' 
		CAST(EA.[CostPrice] AS NUMERIC(38, 10)) AS CostPrice,
        EA.WeightedAvgSpread * 100 AS Spread,
        ([Adjusted WAC] - [Adjusted WAS]) * 100 AS LiborFloor, --CALC PER WENDY
		--4)	Floor ?it?s working in such a way that it?s showing up as a negative number on our delayed draw loans, which isn?t right? using Spectrum Delayed Draw, as an example, it?s technically earning a ticking fee, not interest. So to translate that, we would probably expect to see 1% spread and 0% floor? you show 1% spread and -2.25% floor. In looking at the assets datasheet, you can fix this by calculating the all-in-rate minus the zAdjustedWAS, rather than subtracting the Spread Libor.
		EA.LiborBaseRateFloor,
		ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.[Moody's DP Rating - WARF] = R.RatingDesc
        ),
                  -1
              ) [MoodyDPRatingId],
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.[Moody's CF Rating] = R.RatingDesc
        ),
                  -1
              ) MoodyCashFlowRatingId,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.[Moody's DP Rating - WARF (ADJ)] = R.RatingDesc
        ),
                  -1
              ) AS MoodyCashFlowRatingAdjustedId,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.RatingMoodyIssuance = R.RatingDesc
        ),
                  -1
              ) AS MoodyFacilityRatingId,
        
        CAST(EA.RecoveryRateMoody AS NUMERIC(18, 4)) * 100 AS MoodyRecovery,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.[S&P Issuer Rating] = R.RatingDesc
        ),
                  -1
              ) AS SnPIssuerRatingId,
        ISNULL(
        (
            SELECT TOP 1 R.RatingId FROM CLO.Rating R WHERE EA.RatingSP = R.RatingDesc
        ),
                  -1
              ) AS SnPIssuerRatingAdjustedId,
        ISNULL(
        (
            SELECT TOP 1
                R.RatingId
            FROM CLO.Rating R
            WHERE EA.RatingSPIssue = R.RatingDesc
        ),
                  -1
              ) AS SnPFacilityRatingId,
        -1 AS SnPfacilityRatingAdjustedId,                     --SP NOT ADJUSTED PER WENDY
        -1 AS SnPRecoveryRatingId,
        CASE EA.[Moody's Outlook]
            WHEN 'NEG' THEN
                '-'
            WHEN 'POS' THEN
                '+'
            ELSE
                '0'
        END AS MoodyOutlook,
		CASE EA.[Moody's Credit Watch]
            WHEN 'On Watch for upgrade' THEN
                '+'
            WHEN 'On Watch for downgrade' THEN
                '-'
            ELSE
                'X'
        END AS MoodyWatch,
        CASE EA.RatingSPIssueCreditWatch
            WHEN 'NEG' THEN
                '-'
            WHEN 'POS' THEN
                '+'
            ELSE
                '0'
        END AS SnPWatch,
		CASE EA.[S&P Credit Watch]
            WHEN 'NEG' THEN
                '-'
            WHEN 'POS' THEN
                '+'
            ELSE
                '0'
        END AS SnpCreditWatch,
        '1/1/1900' AS NextReportingDate,                       --'***NOT NEEDED PER WILL***'
        '1/1/1900' AS FiscalYearEndDate,                       --'***NOT NEEDED PER WILL***'
        '***MISSING***' AS AgentBank,                          --'BLOOMBERG: ln_agent'
        GETDATE(),                                             -- CreatedOn - datetime
        'WSO',                                                 -- CreatedBy - varchar(100)
        GETDATE(),                                             -- LastUpdatedOn - datetime
        'WSO'                                                  -- LastUpdatedBy - varchar(100)
    FROM CLO.WsoExtractAssets EA
        JOIN @DatasetKeys DK
            ON DK.DatasetKey = EA.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON DK.DatasetKey = D.DatasetKey
    --WHERE EA.PrincipalBalance <> 0
    --GROUP BY
    --    EA.SecurityID,
    --    EA.RatingSP,
    --    EA.RatingSPIssue,
    --    EA.RatingSPIssueCreditWatch,
    --    EA.[S&P Issuer Rating],
    --    EA.MarkPrice_BidPrice,
    --    EA.SpreadLibor,
    --    EA.LiborBaseRateFloor,
    --    [Moody's CF Rating],
    --    [Moody's DP Rating - WARF (ADJ)],
    --    EA.RatingMoodyIssuance,
    --    EA.RecoveryRateMoody,
    --    EA.[Moody's Outlook]
    --ORDER BY EA.SecurityID
    --SELECT * FROM dbo.WSOExtractAssets WHERE SecurityID = 'LX133675' ORDER BY 1 desc
    COMMIT TRANSACTION