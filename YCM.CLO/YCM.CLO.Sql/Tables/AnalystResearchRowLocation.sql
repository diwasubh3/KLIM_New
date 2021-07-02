CREATE TABLE CLO.AnalystResearchRowLocation
(
	  AnalystResearchRowLocationId INT NOT NULL PRIMARY KEY IDENTITY(1, 1)
	, RowIndex INT NOT NULL
	, ClassName VARCHAR(255) NOT NULL
	, PropertyName VARCHAR(255) NOT NULL
	, CreatedOn DATETIME NOT NULL DEFAULT(GETDATE())
	, CreatedBy VARCHAR(100) NULL
	, LastUpdatedOn DATETIME NOT NULL DEFAULT(GETDATE())
	, LastUpdatedBy VARCHAR(100) NULL
    CONSTRAINT UQ_AnalystResearchRowLocation_ClassPropertyName UNIQUE(ClassName, PropertyName)
)