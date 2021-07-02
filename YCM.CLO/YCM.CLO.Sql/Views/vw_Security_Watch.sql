CREATE VIEW CLO.vw_Security_Watch
AS
	SELECT s.SecurityId, s.SecurityCode, s.SecurityDesc, s.Issuer, s.IssuerId, s.IsOnWatch
		, s.WatchObjectTypeId, s.WatchObjectId, s.WatchId, s.WatchComments, s.WatchLastUpdatedOn, s.WatchUser
		, s.SellCandidateObjectTypeId, s.SellCandidateObjectId, s.SellCandidateId, s.SellCandidateComments, s.SellCandidateLastUpdatedOn, s.SellCandidateUser
	FROM CLO.vw_Security s WITH (NOLOCK)
GO
