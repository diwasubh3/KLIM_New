CREATE PROCEDURE [CLO].[spGetAllPositions]
	@paramSecurityId int = null,
	@paramSecurityCode VARCHAR(100) = NULL,
	@paramOnlyWithExposures BIT = null
AS
	
	If (@paramSecurityId is NULL AND @paramSecurityCode IS not NULL)
		SELECT @paramSecurityId = SecurityId from clo.Security where SecurityCode = @paramSecurityCode

	SELECT * FROM [CLO].[vw_AggregatePosition]  with(nolock)  WHERE 
	   (@paramSecurityId IS NULL OR SecurityId = @paramSecurityId)
	AND (@paramSecurityCode IS NULL OR SecurityCode = @paramSecurityCode)
	
	AND (ISNULL(@paramOnlyWithExposures,0) = 0 OR ISNULL(BODTotalPar,0) <> 0)

RETURN 0
