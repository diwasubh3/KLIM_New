CREATE TABLE CLO.Watch
(
	WatchId INT NOT NULL  identity(1,1),
	WatchObjectTypeId smallint not null,
	WatchObjectId int not null,
	WatchComments varchar(500), 
	WatchUser varchar(100),
	WatchLastUpdatedOn datetime default(getdate()),
	WatchTypeId INT NOT NULL DEFAULT(1),
    CONSTRAINT PK_Watch PRIMARY KEY (WatchId) ,
	CONSTRAINT uq_WatchObjectId_WatchObjectType_WatchTypeId UNIQUE NONCLUSTERED (WatchObjectId, WatchObjectTypeId, WatchTypeId)
)
