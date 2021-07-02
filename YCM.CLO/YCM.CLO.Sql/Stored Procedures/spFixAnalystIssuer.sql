CREATE PROCEDURE [CLO].[spFixAnalystIssuer]
	
AS

UPDATE  a
SET a.issuerid = s.IssuerId
FROM CLO.AnalystResearch a
JOIN CLO.Issuer i on a.IssuerId = i.IssuerId
JOIN CLO.vw_Security s ON s.Issuer = i.IssuerCode
JOIN CLO.POSITION p ON p.SecurityId = s.SecurityId AND p.DateId = CLO.GetPrevDayDateId()

RETURN 0
