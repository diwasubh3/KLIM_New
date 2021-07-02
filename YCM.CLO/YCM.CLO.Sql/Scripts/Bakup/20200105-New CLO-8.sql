UPDATE CLO.Fund 
SET IsActive = 0,
IsWarehouse = 0,
FundCode='CLO-8 Old'
WHERE FundId = 5
GO

INSERT INTO CLO.Fund
(
    FundCode,
    FundDesc,
    PrincipalCash,
    WSOLastUpdatedOn,
    LiabilityPar,
    EquityPar,
    TargetPar,
    WALifeAdjustment,
    RecoveryMultiplier,
    AssetParPercentageThreshold,
    CreatedOn,
    CreatedBy,
    LastUpdatedOn,
    LastUpdatedBy,
    CLOFileName,
    IsStale,
    IsPrincipalCashStale,
    DisplayText,
    IsActive,
    WSOSpread,
    WSOWARF,
    WSOMoodyRecovery,
    WSOWALife,
    WSODiversity,
    IsWarehouse,
    PortfolioName,
    SortOrder,
    WALWARFAdj,
    MaxWarfTrigger,
    ClassEPar,
    WALDate,
    ReInvestEndDate,
    WalDateAdj,
    ParentFundId,
    ParentFundCode,
    CanFilter
)

values ( 'CLO-8', 'CLO-8', 1954245.8300000000, N'2021-01-05T11:43:52.72', 409250000.0000000000, 36500000.0000000000, 450000000.0000000000, NULL, 1.0000000000, 0.2500000000, N'2021-01-05T11:43:52.72', 'WSO', N'2021-01-05T11:43:52.72', 'WSO', 'TradeDateCashCLO8{dateid}.csv', 0, 0, '8', 1, NULL, NULL, NULL, NULL, NULL, 0, 'York CLO-8 Ltd.', 800, NULL, 3300.000000, 407250000.0000000000, N'2020-11-25T00:00:00', N'2023-10-20T00:00:00', 8.000000, 10, 'CLO-8', 1 )
GO

UPDATE CLO.FundRestriction
SET FundId = 10
WHERE FundId = 5
GO

UPDATE CLO.MatrixData
SET FundId = 10
WHERE FundId = 5
GO

UPDATE CLO.MatrixPoint
SET FundId = 10
WHERE FundId = 5
GO

SELECT * FROM CLO.Fund