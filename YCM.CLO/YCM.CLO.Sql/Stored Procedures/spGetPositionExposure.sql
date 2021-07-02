CREATE PROCEDURE [CLO].[spGetPositionExposure]
	@paramSecurityId int = null
AS
	SELECT *
    FROM  [CLO].vw_Position_Exposure p  with(nolock) 
	WHERE 
	   p.SecurityId = @paramSecurityId
	order by p.FundCode

RETURN 0
