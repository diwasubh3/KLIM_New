CREATE TABLE [CLO].[StageTradeBlotter]
(
   TradeBlotterJobId INT NOT NULL REFERENCES [CLO].[TradeBlotterJob](Id)
  ,AssetPrimaryId				VARCHAR(100) NOT NULL 
  ,Portfolio_Name               VARCHAR(100) NOT NULL
  ,Issuer_Name                  VARCHAR(200) NOT NULL
  ,Asset_Name                   VARCHAR(100) NOT NULL
  ,Asset_SecurityID             VARCHAR(100) NOT NULL
  ,Asset_MaturityDate           DATETIME null
  ,Asset_Rate1                  NUMERIC(28,5) NOT NULL
  ,Asset_AssetDetail_Type       INTEGER  NOT NULL
  ,Portfolio_EntityId           VARCHAR(100) NOT NULL
  ,Issuer_EntityId              VARCHAR(100) null
  ,CurrencyType_Identifier      VARCHAR(10) NOT NULL
  ,TradeTypeDescription         VARCHAR(10) NOT NULL
  ,Trade_SettleDate             VARCHAR(30) NOT NULL
  ,Trade_TradeDate              VARCHAR(30) NOT NULL
  ,Trade_AccruedInterest        NUMERIC(28,10)  NOT NULL
  ,CounterBank_Name             VARCHAR(100) NOT NULL
  ,BrokerBank_Name              VARCHAR(100) NULL
  ,TradeDescription             VARCHAR(1000) NULL
  ,Trade_Commissions            NUMERIC(28,5) NULL
  ,Trade_Fees                   NUMERIC(28,4) NULL
  ,Trade_Amount                 NUMERIC(28,5) NULL
  ,Trade_ID                     INTEGER  NOT NULL
  ,Trade_AccruedPIK             NUMERIC(28,5)  NULL
  ,Trade_AccruedFee             NUMERIC(28,5)  NULL
  ,Trade_Accruals               NUMERIC(9,4) NOT NULL
  ,CounterBank_EntityId         VARCHAR(50) null
  ,BrokerBank_EntityId          VARCHAR(50) NULL 
  ,Trade_TradeGroup_ID          INT  NULL
  ,CounterCompanyFundName       VARCHAR(30) null
  ,Position_ID                  INTEGER  NOT NULL
  ,Trade_TradeGroup_DESC        VARCHAR(100) null
  ,TotalFees                    NUMERIC(28,5) NOT NULL
  ,AccruedInterest              NUMERIC(28,5) NOT NULL
  ,TradeAmount                  NUMERIC(28,5) NOT NULL
  ,TotalAmount                  NUMERIC(28,5) NOT NULL
  ,CounterParty                 VARCHAR(100) NOT NULL
  ,CounterPartyEntity           VARCHAR(100) null
  ,CounterParty_Display         VARCHAR(100) NOT NULL
  ,Issuer_DisplayName           VARCHAR(200) NOT NULL, 
  	[Trade_OriginalCommitment] [NUMERIC](28, 5) NULL,
	[Trade_OriginalParAmount] [NUMERIC](28, 5) NULL,
    PRIMARY KEY ([TradeBlotterJobId], [Trade_ID])
)
