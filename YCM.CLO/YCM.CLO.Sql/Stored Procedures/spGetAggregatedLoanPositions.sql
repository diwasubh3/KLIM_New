CREATE PROCEDURE [CLO].[spGetAggregatedLoanPositions]
	@dateId INT, @priorDateId INT, @fundId INT = NULL
/*
sample call
	EXEC CLO.spGetAggregatedLoanPositions 20180222, 20180221, 3
*/
AS
	DECLARE @fundPosition TABLE(FundId INT, FundCode VARCHAR(100), IssuerId INT, SecurityId INT, SecurityCode VARCHAR(100), Issuer VARCHAR(100), Facility VARCHAR(100), Exposure NUMERIC(38, 4), PriorExposure NUMERIC(38, 4))
	DECLARE @prevFundPosition TABLE(FundId INT, FundCode VARCHAR(100), IssuerId INT, SecurityId INT, SecurityCode VARCHAR(100), Issuer VARCHAR(100), Facility VARCHAR(100), Exposure NUMERIC(38, 4))

	INSERT INTO @fundPosition (FundId, FundCode, SecurityId, SecurityCode, IssuerId, Issuer, Facility, Exposure, PriorExposure)
	SELECT F.FundId, F.FundCode, F.SecurityId, F.SecurityCode, F.IssuerId, F.Issuer, F.Facility, SUM(P.Exposure) Exposure, 0
	FROM CLO.vw_SecurityFund F INNER JOIN CLO.Position P ON P.FundId = F.FundId
	AND P.SecurityId = F.SecurityId
	WHERE P.DateId = @dateId
	AND (@fundId IS NULL OR P.FundId = @fundId)
	GROUP BY F.FundId, F.FundCode, F.SecurityId, F.SecurityCode, F.IssuerId, F.Issuer, F.Facility

	INSERT INTO @prevFundPosition (FundId, FundCode, SecurityId, SecurityCode, IssuerId, Issuer, Facility, Exposure)
	SELECT F.FundId, F.FundCode, F.SecurityId, F.SecurityCode, F.IssuerId, F.Issuer, F.Facility, SUM(P.Exposure) Exposure
	FROM CLO.vw_SecurityFund F INNER JOIN CLO.Position P ON P.FundId = F.FundId
	AND P.SecurityId = F.SecurityId
	WHERE P.DateId = @priorDateId
	AND (@fundId IS NULL OR P.FundId = @fundId)
	GROUP BY F.FundId, F.FundCode, F.SecurityId, F.SecurityCode, F.IssuerId, F.Issuer, F.Facility

	SELECT ISNULL(C.FundId, P.FundId) AS FundId, ISNULL(C.FundCode, P.FundCode) AS FundCode, ISNULL(C.SecurityId, P.SecurityId) AS SecurityId
	, ISNULL(C.SecurityCode, P.SecurityCode) AS SecurityCode, ISNULL(C.IssuerId, P.IssuerId) AS IssuerId
	, ISNULL(C.Issuer, P.Issuer) AS Issuer, ISNULL(C.Facility, P.Facility) AS Facility
	, ISNULL(C.Exposure, 0) AS Exposure, ISNULL(P.Exposure, 0) AS PriorExposure
	FROM @fundPosition C FULL OUTER JOIN @prevFundPosition P
	ON P.FundId = C.FundId
	AND P.IssuerId = C.IssuerId
	AND P.SecurityId = C.SecurityId
	AND P.Facility = C.Facility
/*
	UPDATE C
	SET C.PriorExposure = P.Exposure
	FROM @fundPosition C INNER JOIN @prevFundPosition P
	ON P.FundId = C.FundId
	AND P.Facility = C.Facility
	AND P.IssuerId = C.IssuerId
	AND P.SecurityId = C.SecurityId

	SELECT FundId, FundCode, SecurityId, SecurityCode, IssuerId, Issuer, Facility, Exposure, PriorExposure
	FROM @fundPosition
	WHERE @fundId IS NULL OR FundId = @fundId
*/
