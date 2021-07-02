SELECT * FROM syscolumns
WHERE name LIKE '%global%'
SELECT * FROM sysobjects
WHERE id = 1700253162

SELECT DealID, SecurityID, IssueSize, BankDeal_GlobalAmount, * FROM CLO.WsoExtractAssets
WHERE DateId = 20180506
AND AssetId = '4-499894'

SELECT * FROM DataMarts..WsoDatasets
WHERE AsOfDate = '20180506'

SELECT * FROM DataMarts..WsoDatasets
WHERE DatasetKey = 4187
SELECT * FROM DataMarts..WsoExtractTestResult
SELECT TOP 1 * FROM DataMarts..WsoDatasets
SELECT TOP 1 * FROM DataMarts..WsoDatasetQuery
SELECT ISIN, IssueSize, BankDeal_GlobalAmount, * FROM DataMarts..WsoExtractAsset
WHERE DateId = 20180506
AND AssetId = '4-499894'
SELECT * FROM CLO.Issuer
SELECT * FROM CLO.Security
WHERE SecurityCode = 'LX169662'

SELECT D.Title, A.Issuer, S.SecurityCode, A.ISIN, IssueSize, BankDeal_GlobalAmount, A.*
FROM DataMarts..WsoExtractAsset A INNER JOIN DataMarts..WsoDatasets D
ON D.DatasetKey = A.DatasetKey
INNER JOIN CLO.Security S
ON S.ISIN = A.ISIN
WHERE DateId = 20180506
AND D.Title <> 'York CLO-2 LTD. Reset'
AND AssetId = '4-499894'


SELECT * FROM CLO.Calculation
WHERE SecurityId = 2095

SELECT * FROM CLO.vw_AggregatePosition

SELECT DealID, SecurityID, AVG(IssueSize) AS IssueSize
FROM CLO.WsoExtractAssets
WHERE DateId = [CLO].[GetPrevDayDateId]()
AND SecurityID = 'LX169662'
GROUP BY DealID, SecurityID

SELECT * FROM CLO.vw_AggregatePosition
WHERE SecurityCode = 'LX169662'

SELECT * FROM CLO.Fund

SELECT * FROM sysobjects
WHERE name LIKE 'xl%'

EXEC dbo.xlsDataExplorerShortPositionsByFund @tradeDate = '2018-05-09 13:53:36', @fund = '', @longShort = '', @removeTRS = ''

TRUNCATE TABLE CLO.LoanContract

SELECT * FROM CLO.LoanContract
WHERE AssetLoanXIDAssetIDName = 'LX161296'

SELECT * FROM CLO.LoanContract
WHERE AsOfDate = '2018-05-09'

SELECT AsOfDate, CreatedOn, COUNT(1)
FROM CLO.LoanContract
GROUP BY AsOfDate, CreatedOn

SELECT * FROM CLO.LoanContract
WHERE ISNUMERIC(ContractAmountString) = 0

SELECT * FROM CLO.LoanContract
--WHERE AssetLoanXIDAssetIDName = 'LX155963'
WHERE ContractInterestDue IS NULL
SELECT CLO.GetPrevDayDateId()
SELECT * FROM CLO.Fund
CREATE TABLE CLO.FileImportColumnType
(
      Id INT NOT NULL IDENTITY(1, 1)
	, TypeDescription VARCHAR(50) NOT NULL
)

CREATE TABLE CLO.FileImportMap
(
	  Id INT NOT NULL IDENTITY(1, 1)
	, TableColumnName VARCHAR(128) NOT NULL
	, FileColumnName VARCHAR(255)
	, FileColumnIndex INT
	, FileImportColumnTypeId INT NOT NULL
)
SELECT Id, FileImportId, TableColumnName, FileColumnName, FileColumnIndex, FileImportColumnTypeId
FROM CLO.FileImportColumnMap
SELECT * FROM CLO.FileImportColumnType
SELECT * FROM CLO.FileImport
INSERT INTO CLO.FileImport (FileNameMask, TableName, DeleteWhereClause, FileLocation, HasHeader, UseDateIdOnFileMask, UseDateIdOnDeleteClauseMask)
VALUES('LoanContracts{0}.CSV', 'CLO.LoanContract', ' WHERE AsOfDate = {0} ', '\\ycmdata\shared\TPI\incoming\CLOLoanContract\', 1, 1, 0)

--INSERT INTO CLO.FileImportColumnMap (FileImportId, TableColumnName, FileColumnName, FileColumnIndex, FileImportColumnTypeId) VALUES(1, {0}, {1}, NULL, {3})

INSERT INTO CLO.FileImportColumnType (TypeDescription)
VALUES('string')
GO
INSERT INTO CLO.FileImportColumnType (TypeDescription)
VALUES('numeric')
GO
INSERT INTO CLO.FileImportColumnType (TypeDescription)
VALUES('date')
GO
