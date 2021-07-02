CREATE TABLE CLO.AnalystResearchDetail
(
	  AnalystResearchDetailId INT NOT NULL PRIMARY KEY IDENTITY(1, 1)
	, AnalystResearchHeaderId INT NOT NULL REFERENCES CLO.AnalystResearchHeader(AnalystResearchHeaderId)
	, AsOfDate Date NOT NULL
	, Revenues numeric(38, 4) NULL
	, YoYGrowth numeric(10, 4) NULL
	, OrganicGrowth numeric(10, 4) NULL
	, CashEBITDA numeric(38, 4) NULL
	, Margin numeric(10, 4) NULL
	, TransactionExpenses numeric(10, 4) NULL
	, RestructuringAndIntegration numeric(10, 4) NULL
	, Other1 numeric(10, 4) NULL
	, Other2 numeric(10, 4) NULL
	, PFEBITDA numeric(38, 4) NULL
	, LTMPFEBITDA numeric(38, 4) NULL
	, PFCostSaves numeric(10, 4) NULL
	, PFAcquisitionAdjustment numeric(10, 4) NULL
	, CovenantEBITDA numeric(38, 4) NULL
	, Interest numeric(10, 4) NULL
	, CashTaxes numeric(10, 4) NULL
	, WorkingCapital numeric(38, 4) NULL
	, RestructuringOneTime numeric(10, 4) NULL
	, OCF numeric(38, 4) NULL
	, CapitalExpenditures numeric(38, 4) NULL
	, FCF numeric(38, 4) NULL
	, ABLRCF numeric(38, 4) NULL
	, FirstLienDebt numeric(38, 4) NULL
	, TotalDebt numeric(38, 4) NULL
	, EquityMarketCap numeric(38, 4) NULL
	, Cash numeric(38, 4) NULL
	, LTMRevenues numeric(38, 4) NULL
	, LTMEBITDA numeric(38, 4) NULL
	, LTMFCF numeric(38, 4) NULL
	, SeniorLeverage numeric(10, 4) NULL
	, TotalLeverage numeric(10, 4) NULL
	, NetTotalLeverage numeric(10, 4) NULL
	, FCFDebt numeric(10, 4) NULL
	, EstimatedEnterpriseValue numeric(10, 4) NULL
	, EnterpriseValue numeric(10, 4) NULL
	, Comments varchar(max) NULL
	, CreatedOn datetime  default (getdate())
	, CreatedBy varchar(100) NULL
	, LastUpdatedOn datetime  default (getdate())
	, LastUpdatedBy varchar(100) null
    CONSTRAINT UQ_AnalystResearchDetail_AnalystResearchHeaderId_AsOfDate UNIQUE(AnalystResearchHeaderId,AsOfDate)
)
