CREATE PROCEDURE [CLO].[spGetAllPositionForSecurities]
	@paramSecurityIds [IntegerArray] READONLY
AS

	SELECT * FROM [CLO].[vw_AggregatePosition] with(nolock) WHERE 
	(SecurityId IN (SELECT [value] FROM @paramSecurityIds))

RETURN 0
