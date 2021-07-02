CREATE PROCEDURE [CLO].[LoadWso_Fund]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
BEGIN
    BEGIN TRANSACTION

    SET IDENTITY_INSERT CLO.Fund ON;

    INSERT INTO CLO.Fund
    (
        FundId,
        FundCode,
        FundDesc,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy,
		DisplayText
    )
    SELECT D.DealID,
        D.Title,
        D.Title,
        GETDATE(), -- CreatedOn - datetime
        'WSO',     -- CreatedBy - varchar(100)
        GETDATE(), -- LastUpdatedOn - datetime
        'WSO',      -- LastUpdatedBy - varchar(100)
		cast(D.DealID as varchar)
    FROM @datasetKeys K
        JOIN DataMarts.dbo.WsoDatasets D
            ON K.DatasetKey = D.DatasetKey
    WHERE D.DealID NOT IN
          (
              SELECT F.FundId FROM CLO.Fund F
          )
    SET IDENTITY_INSERT CLO.Fund OFF;

    UPDATE F
    SET F.WSOLastUpdatedOn = GETDATE(), F.IsStale = 0
    FROM CLO.Fund F
        JOIN DataMarts.dbo.WsoDatasets D
            ON F.FundId = D.DealID
        JOIN @datasetKeys K
            ON K.DatasetKey = D.DatasetKey

    COMMIT TRANSACTION
END

