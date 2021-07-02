CREATE TABLE CLO.WatchType
(
    WatchTypeId INT NOT NULL IDENTITY(1, 1)
	, WatchTypeDescription VARCHAR(100) NOT NULL
	, CONSTRAINT PK_WatchType PRIMARY KEY(WatchTypeId)
	, CONSTRAINT UX_WatchType_WatchTypeDescription UNIQUE(WatchTypeDescription)
)