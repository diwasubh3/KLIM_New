CREATE PROCEDURE [CLO].[spGetPositionForSecurities]
	@paramFundCode VARCHAR(100) = NULL,
	@paramSecurityIds [IntegerArray] READONLY
AS

SELECT 
		   p.*
    FROM  [CLO].vw_position p  with(nolock) 

	WHERE 
	(@paramFundCode IS NULL OR p.FundCode = @paramFundCode)
	AND (p.SecurityId IN (SELECT [value] FROM @paramSecurityIds))
	

RETURN 0
