CREATE PROCEDURE [CLO].[LoadWso_Position]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
BEGIN
    BEGIN TRANSACTION

    DELETE P
    FROM CLO.Position P
        JOIN DataMarts.dbo.WsoDatasets D
            ON D.DealID = P.FundId
        JOIN @datasetKeys K
            ON K.DatasetKey = D.DatasetKey
    WHERE P.DateId = @asOfDateId


	DELETE P
    FROM CLO.WSOPosition P
        JOIN DataMarts.dbo.WsoDatasets D
            ON D.DealID = P.FundId
        JOIN @datasetKeys K
            ON K.DatasetKey = D.DatasetKey
    WHERE P.DateId = @asOfDateId


    INSERT INTO CLO.Position
    (
        FundId,
        SecurityId,
        DateId,
		MarketValue,
        Exposure,
        PctExposure,
        PxPrice,
        IsCovLite,
        CountryId,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy,
		CapitalizedInterestOrig,
		SnPAssetRecoveryRating
    )
    SELECT AggPos.DealID,
        S.SecurityId,
        @asOfDateId,
		EA.MarketValue,
        EA.Quantity AS Exposure,
        EA.Quantity / AggPos.TotalExposure AS PctExposure,
        EA.MarkPrice AS PxPrice,
        EA.[Is Cov-Lite],
        ISNULL(
        (
            SELECT TOP 1
                C.CountryId
            FROM CLO.Country C
            WHERE EA.CountryOfOperation = C.CountryDesc
        ),
                  -1
              ),   -- CountryId - smallint
        GETDATE(), -- CreatedOn - datetime
        'WSO',     -- CreatedBy - varchar(100)
        GETDATE(), -- LastUpdatedOn - datetime
        'WSO',      -- LastUpdatedBy - varchar(100)
		EA.CapitalizedInterestOrig,
		EA.[SnPAssetRecoveryRating]
    FROM CLO.WsoExtractAssets EA
        JOIN @datasetKeys K
            ON EA.DatasetKey = K.DatasetKey
        JOIN DataMarts.dbo.WsoDatasets D
            ON K.DatasetKey = D.DatasetKey
        LEFT JOIN CLO.Security S
            ON EA.SecurityID = S.SecurityCode --WANT IT TO BOMB IF SECURITY IS NOT SETUP
        JOIN
         (
             SELECT D1.DealID,
                 SUM(EA1.Quantity) AS TotalExposure
             FROM CLO.WsoExtractAssets EA1
                 JOIN @datasetKeys K1
                     ON K1.DatasetKey = EA1.DatasetKey
                 JOIN DataMarts.dbo.WsoDatasets D1
                     ON K1.DatasetKey = D1.DatasetKey
             GROUP BY D1.DealID
         ) AggPos
            ON D.DealID = AggPos.DealID
    WHERE ABS(EA.Quantity) > 0

    COMMIT TRANSACTION
END
