CREATE PROCEDURE [CLO].[spGetFundRestrictions]
	@paramFundId INT = NULL
AS
	
	SELECT * FROM CLO.FundRestriction with(nolock) WHERE @paramFundId IS NULL OR FundId = @paramFundId
	
RETURN 0
