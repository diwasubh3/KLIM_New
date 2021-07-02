CREATE PROCEDURE [CLO].[LoadWso_ByDatasetKeys]
(
    @datasetKeys CLO.DatasetKeys READONLY,
	@asOfDateId INT
)
AS
BEGIN
	BEGIN TRANSACTION
	EXEC CLO.LoadWso_Country @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_Industry @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_Rating @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_Fund @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_Facility @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_Issuer @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_Security @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_MarketData @asOfDateId, @datasetKeys
	EXEC CLO.LoadWso_Position @asOfDateId, @datasetKeys
	COMMIT TRANSACTION
END