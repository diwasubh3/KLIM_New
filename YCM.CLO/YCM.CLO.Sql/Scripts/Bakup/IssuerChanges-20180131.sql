USE YODA
GO

ALTER PROCEDURE [CLO].[LoadWso_Issuer]
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

GO


DECLARE @Security TABLE (SecurityId INT, IssuerCode VARCHAR(100), IssuerId INT)
DECLARE @IssuerCode TABLE(IssuerId INT, IssuerCode VARCHAR(100))
DECLARE @UnusedIssuer TABLE(IssuerId INT, IssuerCode VARCHAR(100))

PRINT 'get the max issuer ids that are currently in use'
INSERT INTO @Security(SecurityId, IssuerCode, IssuerId)
SELECT S.SecurityId, I.IssuerCode, MAX(M.IssuerId) MaxIssuerId
FROM CLO.Issuer I INNER JOIN CLO.Security S
ON S.IssuerId = I.IssuerId
INNER JOIN CLO.Issuer M ON I.IssuerCode = M.IssuerCode
GROUP BY S.SecurityId, I.IssuerCode

PRINT 'get the distinct list of issuers in use'
INSERT INTO @IssuerCode(IssuerId, IssuerCode)
SELECT MAX(IssuerId), IssuerCode FROM @Security GROUP BY IssuerCode

PRINT 'find unused issuers'
INSERT INTO @UnusedIssuer(IssuerId, IssuerCode)
SELECT MAX(IssuerId), IssuerCode
FROM CLO.Issuer I
WHERE NOT EXISTS (SELECT * FROM @IssuerCode WHERE IssuerCode = I.IssuerCode)
GROUP BY IssuerCode
HAVING COUNT(1) > 1

BEGIN TRY
BEGIN TRANSACTION
	PRINT 'update the security table with the max IssuerId'
	UPDATE S
	SET S.IssuerId = U.IssuerId
	--SELECT *
	FROM @Security U INNER JOIN CLO.Security S
	ON S.SecurityId = U.SecurityId
	--ORDER BY IssuerCode, U.IssuerId

	PRINT 'update the unused dupe issuers'
	UPDATE I
	SET IssuerCode = '*' + I.IssuerCode
	--SELECT U.IssuerId, U.IssuerCode
	--, I.IssuerId, I.IssuerCode
	--, M.IssuerId, M.IssuerCode
	--, '*' + I.IssuerCode AS UpdatedCode
	FROM CLO.Issuer I INNER JOIN @UnusedIssuer U
	ON I.IssuerCode = U.IssuerCode
	LEFT OUTER JOIN @UnusedIssuer M
	ON M.IssuerId = I.IssuerId
	WHERE M.IssuerId IS NULL
	--ORDER BY 2, 3

	PRINT 'update the dupe issuers in use'
	UPDATE I
	SET IssuerCode = '*' + I.IssuerCode
	--SELECT I.IssuerId, I.IssuerDesc, I.IssuerCode
	--, S.IssuerId, S.IssuerCode
	--, '*' + I.IssuerCode AS UpdatedCode
	FROM CLO.Issuer I INNER JOIN @IssuerCode C
	ON C.IssuerCode = I.IssuerCode
	LEFT OUTER JOIN @IssuerCode S
	ON I.IssuerId = S.IssuerId
	WHERE S.IssuerId IS NULL
	--ORDER BY 3, 1 DESC
COMMIT
END TRY
BEGIN CATCH
	ROLLBACK
END CATCH
