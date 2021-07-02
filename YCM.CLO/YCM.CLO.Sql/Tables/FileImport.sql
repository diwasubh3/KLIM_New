CREATE TABLE CLO.FileImport
(
	  Id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY
	, FileNameMask VARCHAR(1000) NOT NULL UNIQUE
	, TableName VARCHAR(128) NOT NULL
	, DeleteWhereClause VARCHAR(1000)
	, FileLocation VARCHAR(1000) NOT NULL
	, HasHeader BIT NOT NULL
	, UseDateIdOnFileMask BIT NOT NULL
	, UseDateIdOnDeleteClauseMask BIT NOT NULL
)
GO
CREATE INDEX IX_FileImport_FileNameMask ON CLO.FileImport(FileNameMask)
GO
