CREATE TABLE [CLO].[User]
(
	[UserId] INT NOT NULL PRIMARY KEY identity(1,1),
	[FullName] varchar(100) not null,
	
	IsCLOAnalyst bit default(0),
	IsHFAnalyst bit default(0),

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,


)
