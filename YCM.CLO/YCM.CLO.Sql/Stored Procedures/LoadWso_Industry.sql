CREATE PROCEDURE [CLO].[LoadWso_Industry]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
BEGIN

    IF NOT EXISTS (SELECT * FROM CLO.Industry WHERE IndustryId = -1)
    BEGIN
        SET IDENTITY_INSERT CLO.Industry ON;
        INSERT INTO CLO.Industry
        (
            IndustryId,
            IndustryDesc,
            IsSnP,
            IsMoody,
            CreatedOn,
            CreatedBy,
            LastUpdatedOn,
            LastUpdatedBy
        )
        VALUES
        (   -1,
            '***MISSING***', -- IndustryCodeDesc - varchar(100)
            1,               -- IsSnP - bit
            1,               -- IsMoody - bit
            GETDATE(),       -- CreatedOn - datetime
            'WSO',           -- CreatedBy - varchar(100)
            GETDATE(),       -- LastUpdatedOn - datetime
            'WSO'            -- LastUpdatedBy - varchar(100)
        )
        SET IDENTITY_INSERT CLO.Industry OFF;
    END

    BEGIN TRANSACTION

    INSERT INTO CLO.Industry
    (
        IndustryDesc,
        IsSnP,
        IsMoody,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy
    )
    SELECT DISTINCT
        COALESCE(I.SPIndustry, I.MDIndustry), -- IndustryCodeDesc - varchar(100)
        CASE
            WHEN I.SPIndustry IS NOT NULL THEN
                1
            ELSE
                0
        END,                                  -- IsSnP - bit
        CASE
            WHEN I.MDIndustry IS NOT NULL THEN
                1
            ELSE
                0
        END,                                  -- IsMoody - bit
        GETDATE(),                            -- CreatedOn - datetime
        'WSO',                                -- CreatedBy - varchar(100)
        GETDATE(),                            -- LastUpdatedOn - datetime
        'WSO'                                 -- LastUpdatedBy - varchar(100)
    FROM
    (
        --SELECT REPLACE(SP.Industry, ' and ', ' & ') AS SPIndustry,
		SELECT SP.Industry AS SPIndustry,
            MD.Industry AS MDIndustry
        FROM
            (
                SELECT DISTINCT
                    EA.[SIC S&P] AS Industry
                FROM CLO.WSOExtractAssets EA
                    JOIN @DatasetKeys D
                        ON D.DatasetKey = EA.DatasetKey
                
            ) SP
            FULL OUTER JOIN
            (
                SELECT DISTINCT
                    EA.[SIC Moody] AS Industry
                FROM CLO.WSOExtractAssets EA
                    JOIN @DatasetKeys D
                        ON D.DatasetKey = EA.DatasetKey
       
            ) MD
                --ON MD.Industry = REPLACE(SP.Industry, ' and ', ' & ')
				ON MD.Industry = SP.Industry
    ) I
    WHERE COALESCE(I.SPIndustry, I.MDIndustry)NOT IN
          (
              SELECT I.IndustryDesc FROM CLO.Industry I
          )
    COMMIT TRANSACTION
END
