CREATE TABLE CLO.AnalystResearchFile
(
	  AnalystResearchFileId INT NOT NULL PRIMARY KEY IDENTITY(1, 1)
	, AnalystResearchFileName VARCHAR(400) NOT NULL
	, LastFileUpdate DATETIME NOT NULL
	, CreatedOn DATETIME NOT NULL DEFAULT(GETDATE())
	, CreatedBy VARCHAR(100) NULL
	, LastUpdatedOn DATETIME NOT NULL DEFAULT(GETDATE())
	, LastUpdatedBy VARCHAR(100) NULL
    CONSTRAINT UQ_AnalystResearchFile_AnalystResearchFileName UNIQUE(AnalystResearchFileName)
)