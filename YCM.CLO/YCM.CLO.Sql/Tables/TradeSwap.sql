CREATE TABLE [CLO].[TradeSwap]
(
	[TradeSwapId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Parameters] VARCHAR(MAX) NOT NULL,
	[DateId] int not null,
	[Status] SMALLINT DEFAULT(0) NOT NULL,
	[CreatedBy] VARCHAR(100),
	[CreatedOn] DATETIME,
	[ProcessStartedOn] DATETIME,
	[ProcessCompletedOn] DATETIME,
	[Error] VARCHAR(max)
)
