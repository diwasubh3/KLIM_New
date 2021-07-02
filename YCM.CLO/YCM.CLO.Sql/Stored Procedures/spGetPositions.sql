CREATE PROCEDURE [CLO].[spGetPositions]
	@paramSecurityId int = null,
	@paramSecurityCode VARCHAR(100) = NULL,
	@paramFundCode VARCHAR(100) = NULL,
	@paramOnlyWithExposures BIT = null
AS
	SELECT *
    FROM  [CLO].vw_position p  with(nolock) 
	WHERE 
	   (@paramSecurityId IS NULL OR p.SecurityId = @paramSecurityId)
	AND (@paramSecurityCode IS NULL OR p.SecurityCode = @paramSecurityCode)
	AND (@paramFundCode IS NULL OR p.FundCode = @paramFundCode)
	AND (ISNULL(@paramOnlyWithExposures,0) = 0 OR ISNULL(p.NumExposure,0) <> 0)
	
RETURN 0
