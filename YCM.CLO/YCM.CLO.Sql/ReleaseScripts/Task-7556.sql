USE Yoda
GO
IF OBJECT_ID(N'CLO.LoanContract') IS NOT NULL
	DROP TABLE CLO.LoanContract
GO
IF OBJECT_ID(N'CLO.FileImportColumnMap') IS NOT NULL
	DROP TABLE CLO.FileImportColumnMap
GO
IF OBJECT_ID(N'CLO.FileImportColumnType') IS NOT NULL
	DROP TABLE CLO.FileImportColumnType
GO
IF OBJECT_ID(N'CLO.FileImport') IS NOT NULL
	DROP TABLE CLO.FileImport
GO
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
	, CreatedOn DATETIME NOT NULL DEFAULT(GETDATE())
)
GO
CREATE INDEX IX_LoanContract_AsOfDate ON CLO.LoanContract(AsOfDate)
GO
--CREATE INDEX IX_LoanContract_AsOfDate_PortfolioName_AssetLoanXIDAssetIDName ON CLO.LoanContract(AsOfDate, PortfolioName, AssetLoanXIDAssetIDName)
--GO
CREATE NONCLUSTERED INDEX IX_LoanContract_PortfolioName_AsOfDate_AssetLoanXIDAssetIDName
ON CLO.LoanContract(PortfolioName, AsOfDate) INCLUDE (AssetLoanXIDAssetIDName, ContractGlobalAmount)
GO
CREATE TABLE CLO.FileImportColumnType
(
      Id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY
	, TypeDescription VARCHAR(50) NOT NULL UNIQUE
)
GO
INSERT INTO CLO.FileImportColumnType (TypeDescription)
VALUES('string')
GO
INSERT INTO CLO.FileImportColumnType (TypeDescription)
VALUES('numeric')
GO
INSERT INTO CLO.FileImportColumnType (TypeDescription)
VALUES('date')
GO
CREATE TABLE CLO.FileImport
(
	  Id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY
	, FileNameMask VARCHAR(1000) NOT NULL UNIQUE
	, TableName VARCHAR(128) NOT NULL
	, DeleteWhereClause VARCHAR(1000)
	, FileLocation VARCHAR(1000) NOT NULL
	, HasHeader BIT NOT NULL
	, UseDateIdOnFileMask BIT NOT NULL
	, UseDateIdOnDeleteClauseMask BIT NOT NULL
)
GO
CREATE INDEX IX_FileImport_FileNameMask ON CLO.FileImport(FileNameMask)
GO
INSERT INTO CLO.FileImport (FileNameMask, TableName, DeleteWhereClause, FileLocation, HasHeader, UseDateIdOnFileMask, UseDateIdOnDeleteClauseMask)
VALUES('LoanContracts{0}.CSV', 'CLO.LoanContract', ' WHERE AsOfDate = {0} ', '\\ycmdata\shared\TPI\incoming\CLOLoanContract', 1, 1, 0)
GO
CREATE TABLE CLO.FileImportColumnMap
(
	  Id INT NOT NULL IDENTITY(1, 1)
	, FileImportId INT NOT NULL REFERENCES CLO.FileImport(Id)
	, TableColumnName VARCHAR(128) NOT NULL
	, FileColumnName VARCHAR(255)
	, FileColumnIndex INT
	, FileImportColumnTypeId INT NOT NULL REFERENCES CLO.FileImportColumnType(Id)
	, CONSTRAINT UX_FileImportColumnMap_FileImportId_TableColumnName UNIQUE NONCLUSTERED
	(
		FileImportId, TableColumnName
	)
)
GO
CREATE INDEX IX_FileImportColumnMap_FileImportId ON CLO.FileImportColumnMap(FileImportId)
GO
IF NOT EXISTS(SELECT * FROM syscolumns WHERE id = OBJECT_ID(N'CLO.Fund') AND name = 'PortfolioName')
	ALTER TABLE CLO.Fund ADD PortfolioName VARCHAR(255)
ELSE
	PRINT 'PortfolioName EXISTS!'
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-1 Ltd.'
WHERE FundId = 1
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-2 Ltd.'
WHERE FundId = 2
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-3 Ltd.'
WHERE FundId = 3
GO
UPDATE CLO.Fund
SET PortfolioName = 'York CLO-4 Ltd.'
WHERE FundId = 4
GO
USE [DataMarts]
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_WsoExtractAsset_DateId_SecurityId' AND object_id = OBJECT_ID(N'dbo.WsoExtractAsset'))
	CREATE NONCLUSTERED INDEX IX_WsoExtractAsset_DateId_SecurityId
	ON dbo.WsoExtractAsset (DateId,SecurityID)
	INCLUDE (IssueSize)
ELSE
	PRINT 'IX_WsoExtractAsset_DateId_SecurityId exists!'
GO
