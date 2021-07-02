CREATE TABLE CLO.FileImportColumnMap
(
	  Id INT NOT NULL IDENTITY(1, 1)
	, FileImportId INT NOT NULL REFERENCES CLO.FileImport(Id)
	, TableColumnName VARCHAR(128) NOT NULL
	, FileColumnName VARCHAR(255)
	, FileColumnIndex INT
	, FileImportColumnTypeId INT NOT NULL REFERENCES CLO.FileImportColumnType(Id)
	, CONSTRAINT UX_FileImportColumnMap_FileImportId_TableColumnName UNIQUE NONCLUSTERED
	(
		FileImportId, TableColumnName
	)
)
GO
CREATE INDEX IX_FileImportColumnMap_FileImportId ON CLO.FileImportColumnMap(FileImportId)
GO
