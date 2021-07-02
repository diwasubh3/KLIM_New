CREATE TABLE CLO.LoanContract
(
	  AssetLoanXIDAssetIDName VARCHAR(50)
	, ContractDisplayName VARCHAR(50)
	, PortfolioDisplayName VARCHAR(255)
	, IssuerDisplayName VARCHAR(1000)
	, PositionDesc VARCHAR(1000)
	, DayCount VARCHAR(50)
	, ResetDays INT
	, PortfolioName VARCHAR(255)
	, IssuerName VARCHAR(1000)
	, AssetName VARCHAR(255)
	, PositionName VARCHAR(255)
	, PortfolioEntityId VARCHAR(50)
	, IssuerEntityId VARCHAR(50)
	, PositionID INT
	, ContractContract VARCHAR(50)
	, ContractAmount DECIMAL(30, 4)
	, ContractGlobalAmount DECIMAL(30, 4)
	, ContractAllInRate DECIMAL(30, 18)
	, ContractBaseRate DECIMAL(30, 18)
	, ContractSpread DECIMAL(30, 18)
	, ContractStartDate DATE
	, ContractMaturityDate DATE
	, ContractNextPaymentDate DATE
	, ContractInterestReceived DECIMAL(30, 4)
	, ContractInterestDue DECIMAL(30, 4)
	, MonthCountName VARCHAR(25)
	, YearCountName VARCHAR(25)
	, ContractFacilityOptionName VARCHAR(50)
	, ContractTypeDescription VARCHAR(50)
	, ContractContractType INT
	, AsOfDate DATETIME
	, ContractCurrencyTypeIdentifier VARCHAR(25)
	, ContractEntityId VARCHAR(255)
	, ContractFrequency INT
	, ContractAmountString VARCHAR(255)
	, CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO
CREATE INDEX IX_LoanContract_AsOfDate ON CLO.LoanContract(AsOfDate)
GO
CREATE NONCLUSTERED INDEX IX_LoanContract_PortfolioName_AsOfDate_AssetLoanXIDAssetIDName
ON CLO.LoanContract(PortfolioName, AsOfDate) INCLUDE (AssetLoanXIDAssetIDName, ContractGlobalAmount)
GO
