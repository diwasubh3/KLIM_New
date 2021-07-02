CREATE TABLE [CLO].[TradeBlotter]
(
   TradeId                      INTEGER  NOT NULL
  ,AssetPrimaryId				VARCHAR(100) NOT NULL 
  ,Portfolio_Name               VARCHAR(100) NOT NULL
  ,Issuer_Name                  VARCHAR(200) NOT NULL
  ,Asset_Name                   VARCHAR(100) NOT NULL
  ,Currency						VARCHAR(10) NOT NULL
  ,TradeType					VARCHAR(50) NOT NULL
  ,TradeSettleDate              VARCHAR(30) NOT NULL
  ,TradeDate					VARCHAR(30) NOT NULL
  ,CounterBankName              VARCHAR(100) NOT NULL
  ,BrokerBankName               VARCHAR(100) NULL
  ,TradeDescription             VARCHAR(1000) NULL
  ,TradeAmount                  NUMERIC(30,5) NULL
  ,AccruedInterest              NUMERIC(30,5) NOT NULL
  ,CounterParty                 VARCHAR(100) NOT NULL
  ,CounterPartyEntity           VARCHAR(100) null
  ,CounterPartyDisplay          VARCHAR(100) NOT NULL
  ,IssuerDisplayName            VARCHAR(200) NOT NULL
  ,TradeCommissions             NUMERIC(28,5) NULL
  ,[TradeOriginalCommitment] [NUMERIC](28, 5) NULL
,[TradeOriginalParAmount] [NUMERIC](28, 5) NULL
  ,CONSTRAINT [PK_TradeBlotter] PRIMARY KEY ([TradeId]), 
)

GO

CREATE NONCLUSTERED INDEX IX_CLO_TradeBlotter_DATE ON CLO.TradeBlotter(TradeDate,TradeSettleDate)