CREATE TABLE CLO.AnalystResearchImportLog
(
	  AnalystResearchImportLogId INT NOT NULL PRIMARY KEY IDENTITY(1, 1)
	, AnalystResearchFileName VARCHAR(400) NOT NULL
	, SheetName VARCHAR(255) NOT NULL
	, IssuerName VARCHAR(255) NULL
	, Processed BIT NOT NULL DEFAULT(0)
	, ImportError VARCHAR(255) NULL
	, CreatedOn DATETIME NOT NULL DEFAULT(GETDATE())
	, CreatedBy VARCHAR(100) NULL
)
GO
CREATE NONCLUSTERED INDEX IDX_AnalystResearchImportLog_ProcessedAnalystResearchFileName ON CLO.AnalystResearchImportLog
(
	Processed ASC, AnalystResearchFileName ASC
)
GO