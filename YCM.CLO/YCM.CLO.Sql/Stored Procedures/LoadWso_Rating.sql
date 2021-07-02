CREATE PROCEDURE [CLO].[LoadWso_Rating]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS

BEGIN
BEGIN TRANSACTION
    IF NOT EXISTS (SELECT * FROM CLO.Rating WHERE RatingId = -1)
    BEGIN
        SET IDENTITY_INSERT CLO.Rating ON;
        INSERT INTO CLO.Rating
        (
            RatingId,
            RatingDesc,
            CreatedOn,
            CreatedBy,
            LastUpdatedOn,
            LastUpdatedBy
        )
        VALUES
        (   -1,
            '***MISSING***', -- RatingDesc - varchar(100)
            GETDATE(),       -- CreatedOn - datetime
            'WSO',           -- CreatedBy - varchar(100)
            GETDATE(),       -- LastUpdatedOn - datetime
            'WSO'            -- LastUpdatedBy - varchar(100)
        )
        SET IDENTITY_INSERT CLO.Rating OFF;
    END

    INSERT INTO CLO.Rating
    (
        RatingDesc,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy
    )
    SELECT DISTINCT
        RTRIM(LTRIM(RatingValue)) AS RatingDesc,
        GETDATE(), -- CreatedOn - datetime
        'WSO',     -- CreatedBy - varchar(100)
        GETDATE(), -- LastUpdatedOn - datetime
        'WSO'      -- LastUpdatedBy - varchar(100)
    FROM
    (
        SELECT DISTINCT
            CAST(EA.RatingSP AS VARCHAR(10)) AS RatingSP,
            CAST(EA.[Moody's CF Rating] AS VARCHAR(10)) AS MoodyCFRating,
            CAST(EA.[Moody's DP Rating - WARF (ADJ)] AS VARCHAR(10)) AS MoodyDPRating,
            CAST(EA.[S&P Issuer Rating] AS VARCHAR(10)) AS SPIssuerRating,
            CAST(EA.RatingSPIssue AS VARCHAR(10)) AS SPFacilityRating
        FROM CLO.WsoExtractAssets EA
            JOIN @DatasetKeys DK
                ON DK.DatasetKey = EA.DatasetKey
    ) RatingColumns UNPIVOT(RatingValue FOR RatingColumns IN(RatingSP, MoodyCFRating, MoodyDPRating, SPIssuerRating, SPFacilityRating))RatingValues
    WHERE LEN(RTRIM(LTRIM(RatingValue))) > 0
          AND RTRIM(LTRIM(RatingValue)) NOT IN
              (
                  SELECT R.RatingDesc FROM CLO.Rating R
              )
COMMIT TRANSACTION
END


