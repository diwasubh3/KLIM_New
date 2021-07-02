CREATE PROCEDURE [CLO].[LoadWso_Security]
(
    @asOfDateId INT,
    @datasetKeys CLO.DatasetKeys READONLY
)
AS
BEGIN
	
    BEGIN TRANSACTION

	 SELECT 
            S.SecurityID,
            S.Description,
            LTRIM(RTRIM(S.Issuer)) Issuer,
			RTRIM(LTRIM(s.[Issuer Name Alternative])) IssuerCode,
            S.Asset,
			MAX(S.ISIN) AS ISIN,
            S.MaturityDate,
            S.[SIC S&P],
            S.[SIC Moody],
            S.CouponType,
			S.[Is First Lien],
			S.[IsInDefault],
			S.[DefaultDate]
		INTO #WSOSecurities
        FROM CLO.WsoExtractAssets S
            JOIN @datasetKeys DK
                ON DK.DatasetKey = S.DatasetKey
		GROUP BY
		    S.SecurityID,
            S.Description,
            S.Issuer,
			s.[Issuer Name Alternative],
            S.Asset,
            S.MaturityDate,
            S.[SIC S&P],
            S.[SIC Moody],
            S.CouponType,
			S.[Is First Lien],
			S.[IsInDefault],
			S.[DefaultDate]

	DELETE FROM [CLO].SecurityData 
	WHERE DateId = @asOfDateId AND 
	SecurityId IN (	SELECT SecurityId FROM [CLO].[Security] WHERE SecurityCode IN (SELECT SecurityId FROM #WSOSecurities))
	
	INSERT INTO [CLO].SecurityData 
	(DateId,SecurityId,	Isin,IssuerId,FacilityId,MaturityDate,SnPIndustryId	,MoodyIndustryId,IsFloating,LienTypeId,CreatedOn,CreatedBy,IsInDefault,DefaultDate)
	SELECT @asOfDateId,s.SecurityId,
	EA.ISIN,
	ISNULL((SELECT TOP 1 I.IssuerId FROM CLO.Issuer I WHERE EA.IssuerCode = I.IssuerCode),-1),
	ISNULL((SELECT TOP 1 F.FacilityId FROM CLO.Facility F WHERE EA.Asset = F.FacilityDesc),-1),
	EA.MaturityDate,  
	ISNULL((SELECT TOP 1 I.IndustryId FROM CLO.Industry I WHERE I.IndustryDesc = EA.[SIC S&P] AND I.IsSnP = 1), -1),
	ISNULL((SELECT TOP 1 I.IndustryId FROM CLO.Industry I WHERE I.IndustryDesc = EA.[SIC Moody] AND I.IsMoody = 1 ), -1 ),
	CASE WHEN EA.CouponType = 'Float' THEN  1  ELSE  0  END,
	CASE WHEN ISNULL(EA.[Is First Lien],0) = 0 THEN 2 ELSE 1 END, 
	GETDATE(),
	'WSO',
	EA.[IsInDefault],
	EA.[DefaultDate]
	FROM #WSOSecurities EA 
	JOIN [CLO].[Security] s WITH(NOLOCK)  ON s.SecurityCode = EA.SecurityId
		
	UPDATE CLO.Security
	SET 
		Isin				= EA.ISIN,
		SecurityDesc		= EA.Description,
        IssuerId			= ISNULL((SELECT TOP 1 I.IssuerId FROM CLO.Issuer I WHERE EA.IssuerCode = I.IssuerCode),-1),
		FacilityId			= ISNULL((SELECT TOP 1 F.FacilityId FROM CLO.Facility F WHERE EA.Asset = F.FacilityDesc),-1),
		MaturityDate		= EA.MaturityDate,  
        SnPIndustryId		= ISNULL((SELECT TOP 1 I.IndustryId FROM CLO.Industry I WHERE I.IndustryDesc = EA.[SIC S&P] AND I.IsSnP = 1), -1),
        MoodyIndustryId		= ISNULL((SELECT TOP 1 I.IndustryId FROM CLO.Industry I WHERE I.IndustryDesc = EA.[SIC Moody] AND I.IsMoody = 1 ), -1 ),
        IsFloating			= CASE WHEN EA.CouponType = 'Float' THEN  1  ELSE  0  END,
		LienTypeId			= CASE WHEN ISNULL(EA.[Is First Lien],0) = 0 THEN 2 ELSE 1 END, 
        LastUpdatedOn		= GETDATE(),
        LastUpdatedBy		= 'WSO',
		IsInDefault			= EA.IsInDefault,
		DefaultDate			= EA.DefaultDate
	FROM #WSOSecurities EA
	JOIN [CLO].[Security] s WITH(NOLOCK)  ON s.SecurityCode = EA.SecurityId

    INSERT INTO CLO.Security
    (
        SecurityCode,
        SecurityDesc,
        BBGId,
		Isin,
        IssuerId,
        FacilityId,
        CallDate,
        --CountryId,
        MaturityDate,
        SnPIndustryId,
        MoodyIndustryId,
        --IsCovLite,
        IsFloating,
        LienTypeId,
        CreditScore,
        CreatedOn,
        CreatedBy,
        LastUpdatedOn,
        LastUpdatedBy,
		IsInDefault,
		DefaultDate
    )
    SELECT EA.SecurityID,         -- SecurityCode - varchar(100)
        EA.Description,           -- SecurityDesc - varchar(500)
        '***BLOOMBERG: ID_BB***', -- BBGId - varchar(1000)
		EA.ISIN,
        ISNULL(
        (
            SELECT TOP 1 I.IssuerId FROM CLO.Issuer I WHERE EA.IssuerCode = I.IssuerCode
        ),
                  -1
              ),                  -- IssuerId - int
        ISNULL(
        (
            SELECT TOP 1
                F.FacilityId
            FROM CLO.Facility F
            WHERE EA.Asset = F.FacilityDesc
        ),
                  -1
              ),                  -- FacilityId - smallint
        '1/1/1900',               -- CallDate - datetime, ***BLOOMBERG: NXT_CALL_DT & OVERRIDES*** 
		---1,
        --ISNULL(
        --(
        --    SELECT TOP 1
        --        C.CountryId
        --    FROM CLO.Country C
        --    WHERE EA.CountryOfOperation = C.CountryDesc
        --),
        --          -1
        --      ),                  -- CountryId - smallint
        EA.MaturityDate,          -- MaturityDate - datetime
        ISNULL(
        (
            SELECT TOP 1
                I.IndustryId
            FROM CLO.Industry I
            WHERE I.IndustryDesc = EA.[SIC S&P]
                  AND I.IsSnP = 1
        ),
                  -1
              ),
        ISNULL(
        (
            SELECT TOP 1
                I.IndustryId
            FROM CLO.Industry I
            WHERE I.IndustryDesc = EA.[SIC Moody]
                  AND I.IsMoody = 1
        ),
                  -1
              ),                  -- MoodyIndustryCodeId - smallint
        --0,--EA.[Is Cov-Lite],         -- IsCovLite - bit
        CASE
            WHEN EA.CouponType = 'Float' THEN
                1
            ELSE
                0
        END,                      -- IsFloating - bit
        CASE 
			WHEN ISNULL(EA.[Is First Lien],0) = 0 
			THEN 2 
			ELSE 1 
		END,                      -- LienTypeId - smallint
        NULL,                     -- CreditScore - smallint ***MOVE TO TIMESERIES TABLE, USER ENTRY***
        GETDATE(),                -- CreatedOn - datetime
        'WSO',                    -- CreatedBy - varchar(100)
        GETDATE(),                -- LastUpdatedOn - datetime
        'WSO',                    -- LastUpdatedBy - varchar(100)
		EA.IsInDefault,
		EA.DefaultDate
    FROM #WSOSecurities EA
    WHERE EA.SecurityID NOT IN
          (
              SELECT DISTINCT S.SecurityCode FROM CLO.Security S
          ) 
    ORDER BY EA.SecurityID

	DROP TABLE #WSOSecurities

    COMMIT TRANSACTION
END

