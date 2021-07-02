CREATE PROCEDURE [CLO].[spGetSecurityWatch]
	@paramSecurityIds [IntegerArray] READONLY
AS

SELECT 
	s.*
    FROM  [CLO].vw_Security_Watch s  with(nolock) 
	WHERE 
	s.SecurityId IN (SELECT [value] FROM @paramSecurityIds)

RETURN 0
