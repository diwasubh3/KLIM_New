CREATE VIEW [CLO].[vw_AnalystResearch]  WITH SCHEMABINDING 
	AS 
	
	WITH    analystrefresh_cfe
              AS ( SELECT   [AnalystResearchId]
      ,[IssuerId]
      ,[CLOAnalystUserId]
      ,[HFAnalystUserId]
      ,[AsOfDate]
      ,[CreditScore]
      ,[LiquidityScore]
      ,[OneLLeverage]
      ,[TotalLeverage]
      ,[EVMultiple]
      ,[LTMRevenues]
      ,[LTMEBITDA]
      ,[FCF]
      ,[Comments]
      ,[BusinessDescription]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[LastUpdatedOn]
      ,[LastUpdatedBy] 
	  ,[AgentBank]
      ,                      ROW_NUMBER() OVER ( PARTITION BY IssuerId
							ORDER BY AsOfDate DESC ) AS ROWNUM
                   FROM     CLO.AnalystResearch WITH ( NOLOCK )
                 )
	
	SELECT 
	
	[AnalystResearchId], 
	a.[IssuerId],
	a.CLOAnalystUserId,
	a.HFAnalystUserId,
	[cloanalyst].[FullName] CLOAnalyst,
	[hfanalyst].[FullName] HFAnalyst,
	[AsOfDate],
	[CreditScore],
	[LiquidityScore],
	[OneLLeverage],
	[TotalLeverage],
	[EVMultiple],
	[LTMRevenues],
	[LTMEBITDA],
	[FCF],
	[Comments],
	[BusinessDescription],
	[AgentBank],
	ISNULL(issuer.IssuerCode,issuer.IssuerDesc) IssuerCode,
	issuer.IssuerDesc

	FROM analystrefresh_cfe a  with(nolock)

	left join CLO.[User] cloanalyst with(nolock)   on cloanalyst.UserId = a.CLOAnalystUserId
	left join CLO.[User] hfanalyst with(nolock)   on hfanalyst.UserId = a.HFAnalystUserId
	join CLO.Issuer issuer with(nolock) on a.IssuerId = issuer.IssuerId

	where ROWNUM = 1 or AsOfDate >= CONVERT(date, getdate())

