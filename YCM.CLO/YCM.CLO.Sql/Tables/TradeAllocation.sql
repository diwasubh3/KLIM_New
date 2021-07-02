CREATE TABLE [CLO].[TradeAllocation]
(
	[TradeAllocationId] BIGINT NOT NULL  identity(1,1),
	[TradeId] BIGINT NOT NULL references [CLO].[Trade](TradeId),
	[CurrentAllocation] numeric(38,4),
	[NewAllocation] numeric(38,4),
	[FundId] INT NOT NULL references [CLO].[Fund](FundId),
	[FinalAllocation] numeric(38,4),

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null, 
    CONSTRAINT [PK_TradeAllocation] PRIMARY KEY ([TradeId], [FundId]),

)
