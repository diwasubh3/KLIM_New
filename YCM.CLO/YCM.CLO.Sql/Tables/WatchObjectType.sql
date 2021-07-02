CREATE TABLE CLO.WatchObjectType
(
    WatchObjectTypeId INT NOT NULL IDENTITY(1, 1)
	, WatchObjectTypeDescription VARCHAR(100) NOT NULL
	, CONSTRAINT PK_WatchObjectType PRIMARY KEY(WatchObjectTypeId)
	, CONSTRAINT UX_WatchObjectType_WatchObjectTypeDescription UNIQUE(WatchObjectTypeDescription)
)