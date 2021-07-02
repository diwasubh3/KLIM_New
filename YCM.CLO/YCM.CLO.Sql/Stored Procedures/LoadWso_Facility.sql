CREATE PROCEDURE [CLO].[LoadWso_Facility](@asOfDateId INT, @datasetKeys CLO.DatasetKeys READONLY)
AS
BEGIN
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

IF NOT EXISTS (SELECT * FROM CLO.Facility WHERE FacilityId = -1)
BEGIN
    SET IDENTITY_INSERT CLO.Facility ON;
    INSERT INTO CLO.Facility
    (
        FacilityId,
        FacilityDesc,
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
    SET IDENTITY_INSERT CLO.Facility OFF;
END

BEGIN TRANSACTION

INSERT INTO CLO.Facility
        (FacilityDesc ,
         CreatedOn ,
         CreatedBy ,
         LastUpdatedOn ,
         LastUpdatedBy)
SELECT DISTINCT
	EA.Asset,
    GETDATE(),       -- CreatedOn - datetime
    'WSO',           -- CreatedBy - varchar(100)
    GETDATE(),       -- LastUpdatedOn - datetime
    'WSO'            -- LastUpdatedBy - varchar(100)
FROM CLO.WSOExtractAssets EA
    JOIN @DatasetKeys D
        ON D.DatasetKey = EA.DatasetKey
WHERE EA.SecurityID LIKE 'LX%'
AND EA.Asset NOT IN (SELECT F.FacilityDesc FROM CLO.Facility F)

/*
UPDATE S
SET S.FacilityId = F.FacilityId
--SELECT S.SecurityCode, SPI.IndustryCodeId, MDI.IndustryCodeId
FROM CLO.Security S
    JOIN
     (
         SELECT DISTINCT
             EA.SecurityID,
             Asset
         FROM DataMarts.dbo.WsoExtractAsset EA
             JOIN @DatasetKeys D
                 ON D.DatasetKey = EA.DatasetKey
         WHERE EA.SecurityID LIKE 'LX%'
     ) E
        ON E.SecurityID = S.SecurityCode
    JOIN CLO.Facility F
        ON F.FacilityDesc = E.Asset
WHERE S.FacilityId = -1
*/

COMMIT TRANSACTION
END
/*
SELECT S.SecurityCode, F.FacilityDesc
FROM CLO.CLO.Security S
JOIN CLO.CLO.Facility F ON F.FacilityId= S.FacilityId
WHERE S.FacilityId = -1
*/


/*
SELECT DISTINCT
    EA.SecurityID,
EA.Issuer
FROM DataMarts.dbo.WsoExtractAsset EA
WHERE EA.DatasetKey IN ( 1997, 1998, 1999, 2000 )
      AND EA.SecurityID LIKE 'LX%'
      --AND EA.SecurityID IN ( 'LX120047' )
--	AND EA.CallDate IS NOT NULL
--AND EA.UserFlag4 = 1
*/

--ROLLBACK TRANSACTION