CREATE PROC CLO.spGetRatingsChange @startDateId INT, @endDateId INT

AS

SELECT
td.SecurityCode,
td.Issuer,
td.TotalPar,
td.MoodyCashFlowRatingAdjusted AS 'MoodyCFJAdjNew',
yd.MoodyCashFlowRatingAdjusted AS 'MoodyCFRAdjOld',
td.MoodyFacilityRating AS 'MoodyFacitityRatingNew',
yd.MoodyFacilityRating AS 'MoodyFacitityRatingOld',
td.SnPIssuerRating AS 'SnPIssuerRatingNew',
yd.SnPIssuerRating AS 'SnPIssuerRatingOld'
FROM CLO.AggregatedPosition td
INNER JOIN CLO.AggregatedPosition yd ON td.SecurityId = yd.SecurityId AND yd.PositionDateId = @startDateId
WHERE td.PositionDateId = @endDateId
AND 
(
	   td.MoodyCashFlowRatingAdjusted <> yd.MoodyCashFlowRatingAdjusted
	OR td.MoodyFacilityRating <> yd.MoodyFacilityRating
	OR td.SnPIssuerRating <> yd.SnPIssuerRating
)

GO