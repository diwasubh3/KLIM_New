CREATE PROCEDURE [CLO].[spGetFundRestrictionTypes]
	
AS
	SELECT * FROM CLO.FundRestrictionType  with(nolock) 
		
RETURN 0
