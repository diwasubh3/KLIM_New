CREATE PROCEDURE [CLO].[LoadWso_Country]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
BEGIN
    /*
DECLARE @DatasetKeys TABLE (DatasetKey INT)
INSERT INTO @DatasetKeys
(
    DatasetKey
)
SELECT (1997)
UNION
SELECT (1998)
UNION
SELECT (1999)
UNION
SELECT (2000)
*/
    IF NOT EXISTS (SELECT * FROM CLO.Country WHERE CountryId = -1)
    BEGIN
        SET IDENTITY_INSERT CLO.Country ON;
        INSERT INTO CLO.Country
        (
            CountryId,
            CountryDesc,
            CreatedOn,
            CreatedBy,
            LastUpdatedOn,
            LastUpdatedBy
        )
        VALUES
        (   -1,
            '***MISSING***', -- IssuerDesc - varchar(100)
            GETDATE(),       -- CreatedOn - datetime
            'WSO',           -- CreatedBy - varchar(100)
            GETDATE(),       -- LastUpdatedOn - datetime
            'WSO'            -- LastUpdatedBy - varchar(100)
        )
        SET IDENTITY_INSERT CLO.Country OFF;
    END

    BEGIN TRANSACTION

    INSERT INTO CLO.Country
    (
        CountryDesc,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy
    )
    SELECT DISTINCT
        EA.CountryOfOperation,
        GETDATE(), -- CreatedOn - datetime
        'WSO',     -- CreatedBy - varchar(100)
        GETDATE(), -- LastUpdatedOn - datetime
        'WSO'      -- LastUpdatedBy - varchar(100)
    FROM CLO.WSOExtractAssets EA
        JOIN @DatasetKeys D
            ON D.DatasetKey = EA.DatasetKey
    WHERE EA.SecurityID LIKE 'LX%'
          AND EA.CountryOfOperation NOT IN
              (
                  SELECT C.CountryDesc FROM CLO.Country C
              )

    --UPDATE S
    --SET S.CountryId = C.CountryId
    ----SELECT S.SecurityCode, SPI.IndustryCodeId, MDI.IndustryCodeId
    --FROM CLO.Security S
    --    JOIN
    --     (
    --         SELECT DISTINCT
    --             EA.SecurityID,
    --             EA.CountryOfOperation
    --         FROM DataMarts.dbo.WsoExtractAsset EA
    --             JOIN @DatasetKeys D
    --                 ON D.DatasetKey = EA.DatasetKey
    --         WHERE EA.SecurityID LIKE 'LX%'
    --     ) E
    --        ON E.SecurityID = S.SecurityCode
    --    JOIN CLO.Country C
    --        ON C.CountryDesc = E.CountryOfOperation
    --WHERE S.CountryId = -1

    COMMIT TRANSACTION
END
--SELECT S.SecurityCode, C.CountryDesc
--FROM CLO.CLO.Security S
--JOIN CLO.CLO.Country C ON C.CountryId = S.CountryId
--WHERE S.CountryId = -1



/*
SELECT DISTINCT
    EA.SecurityID,
EA.CountryOfOperation,EA.*
FROM DataMarts.dbo.WsoExtractAsset EA
WHERE EA.DatasetKey IN ( 1997, 1998, 1999, 2000 )
      AND EA.SecurityID LIKE 'LX%'
      --AND EA.SecurityID IN ( 'LX120047' )
--	AND EA.CallDate IS NOT NULL
--AND EA.UserFlag4 = 1
*/

--ROLLBACK TRANSACTION