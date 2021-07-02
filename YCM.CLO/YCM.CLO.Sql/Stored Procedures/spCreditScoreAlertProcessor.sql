CREATE PROCEDURE [CLO].[spGetCreditScoreAlertIssuers]
	@paramFundCode varchar(100),
	@dateId int
AS

select IssuerId,MAX(p.CreditScore) CreditScore,SUM(ISNULL(p.PctExposureNum,0)) PCTExposure 
into #CheckForCrediScoreAlerts
from CLO.vw_Position p with(nolock)
where p.FundCode = @paramFundCode and p.PositionDateId is not null
group by IssuerId

select 
IssuerId
from #CheckForCrediScoreAlerts a
join CLO.ParameterValue pv with(nolock) on pv.ParameterValueNumber = a.CreditScore
join CLO.ParameterType pt with(nolock) on pt.ParameterTypeId = pv.ParameterTypeId and pt.ParameterTypeName = 'Credit Score'
where (pv.ParameterMinValueNumber is not null and PCTExposure < pv.ParameterMinValueNumber) or 
(pv.ParameterMaxValueNumber is not null and PCTExposure > pv.ParameterMaxValueNumber)

drop table #CheckForCrediScoreAlerts

RETURN 0
