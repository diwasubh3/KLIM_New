CREATE TABLE [CLO].[Trade]
(
	[TradeId] BIGINT NOT NULL PRIMARY KEY identity(1,1),
	[SecurityId] INT NOT NULL references [CLO].[Security]([SecurityId]),
	[DateId] int not null,
	
	[IsBuy] BIT DEFAULT(0),
	[TradeAmount]  numeric(38,4),
	[TradePrice]  numeric(38,4),
	[SellAll] bit default(0),
	[KeepOnBlotter] bit default(0),
	[BidOfferPrice] numeric(38,4),
	[Comments] varchar(1000),
	[IsCancelled] BIT DEFAULT(0),
	[FinalAllocation] numeric(38,4),

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,

)
