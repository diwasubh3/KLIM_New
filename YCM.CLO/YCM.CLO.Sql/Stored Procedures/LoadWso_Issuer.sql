CREATE PROCEDURE [CLO].[LoadWso_Issuer]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
BEGIN

    IF NOT EXISTS (SELECT * FROM CLO.Issuer WHERE IssuerId = -1)
    BEGIN
        SET IDENTITY_INSERT CLO.Issuer ON;
        INSERT INTO CLO.Issuer
        (
            IssuerId,
            IssuerDesc,
			IssuerCode,
            CreatedOn,
            CreatedBy,
            LastUpdatedOn,
            LastUpdatedBy
        )
        VALUES
        (   -1,
            '***MISSING***', -- IssuerDesc - varchar(100)
			'***MISSING***', -- IssuerCode - varchar(100)
            GETDATE(),       -- CreatedOn - datetime
            'WSO',           -- CreatedBy - varchar(100)
            GETDATE(),       -- LastUpdatedOn - datetime
            'WSO'            -- LastUpdatedBy - varchar(100)
        )
        SET IDENTITY_INSERT CLO.Issuer OFF;
    END

    BEGIN TRANSACTION

    INSERT INTO CLO.Issuer
    (
        IssuerDesc,
		IssuerCode,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy
    )
    SELECT DISTINCT
        RTRIM(LTRIM(EA.Issuer)),
		RTRIM(LTRIM(EA.[Issuer Name Alternative])),
        GETDATE(), -- CreatedOn - datetime
        'WSO',     -- CreatedBy - varchar(100)
        GETDATE(), -- LastUpdatedOn - datetime
        'WSO'      -- LastUpdatedBy - varchar(100)
    FROM [CLO].[WsoExtractAssets] EA
        JOIN @DatasetKeys D
            ON D.DatasetKey = EA.DatasetKey
		LEFT JOIN CLO.Issuer i WITH(NOLOCK) ON i.IssuerCode = RTRIM(LTRIM(EA.[Issuer Name Alternative]))
    WHERE EA.SecurityID LIKE 'LX%'
          AND i.IssuerId IS NULL
    
	UPDATE CLO.Issuer
	SET IssuerDesc = RTRIM(LTRIM(EA.Issuer))
	FROM [CLO].[WsoExtractAssets] EA
        JOIN @DatasetKeys D
            ON D.DatasetKey = EA.DatasetKey
		LEFT JOIN CLO.Issuer i WITH(NOLOCK) ON i.IssuerCode = RTRIM(LTRIM(EA.[Issuer Name Alternative])) AND IssuerDesc <> RTRIM(LTRIM(EA.Issuer))
    WHERE EA.SecurityID LIKE 'LX%'      
	
    COMMIT TRANSACTION
END
