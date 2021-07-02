CREATE PROCEDURE [CLO].[LoadWso]
(
    @datasetKey INT,
	@asOfDateId INT
)
AS
BEGIN
	DECLARE @datasetKeys AS CLO.DatasetKeys
	INSERT INTO @datasetKeys VALUES (@datasetKey)
	EXEC CLO.LoadWso_ByDatasetKeys @datasetKeys, @asOfDateId
END