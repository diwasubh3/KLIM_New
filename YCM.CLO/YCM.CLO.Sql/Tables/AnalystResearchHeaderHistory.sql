CREATE TABLE CLO.AnalystResearchHeaderHistory
(
	  AnalystResearchHeaderHistoryId INT NOT NULL PRIMARY KEY IDENTITY(1, 1)
	, AnalystResearchHeaderId INT NOT NULL
	, IssuerId INT NOT NULL
	, BusinessDescription varchar(max) NULL
	, CLOAnalystId INT NULL
	, HFAnalystId INT NULL
	, CreditScore numeric(10, 4) NULL
	, AgentBank varchar(100) NULL
	, CreatedOn datetime
	, CreatedBy varchar(100) NULL
	, LastUpdatedOn datetime
	, LastUpdatedBy varchar(100) NULL
    , Operation VARCHAR(10) NOT NULL
	, [LiborCategory] [VARCHAR](50) NULL
	, [LiborTransitionNote] [VARCHAR](50) NULL
)
