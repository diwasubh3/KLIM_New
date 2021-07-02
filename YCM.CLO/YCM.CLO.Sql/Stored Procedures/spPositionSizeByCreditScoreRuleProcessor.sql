create PROCEDURE [CLO].[spPositionSizeByCreditScoreByTotalCouponRuleProcessor]
	@ruleSectionName varchar(100),
	@fundCode varchar(100),
	@dateId int
AS


SELECT 
		p.SecurityId,IssuerId,creditscore,PctExposureNum,p.TotalCoupon
INTO #positions 
FROM  [CLO].[vw_Position] p
WHERE p.fundCode = @fundCode
 and p.PositionDateId is not null

select IssuerId,MAX(p.CreditScore) CreditScore,SUM(ISNULL(p.PctExposureNum,0)) PCTExposure 
into #CheckForCrediScoreAlerts
from #positions p with(nolock)
group by IssuerId


	
select top 10
p.SecurityId,
(case when @ruleSectionName = 'Top' then (pv.ParameterMinValueNumber - a.[PCTExposure]) 
     else (a.[PCTExposure] - pv.ParameterMaxValueNumber) end) 
Delta,
ROW_NUMBER () Over (order by 
case when @ruleSectionName = 'Top' then p.TotalCoupon end desc,
case when @ruleSectionName = 'Bottom' then p.TotalCoupon end asc) RowNum

INTO #result
from #positions p with(nolock)

join #CheckForCrediScoreAlerts a on a.IssuerId  = p.IssuerId
join clo.ParameterValue pv with (nolock) on ISNULL(p.CreditScore,0) = cast(pv.ParameterValueNumber as smallint)
join clo.ParameterType pt with(nolock) on pv.ParameterTypeId = pt.ParameterTypeId and pt.ParameterTypeName = 'Credit Score'
where p.CreditScore <> 0 

order by Delta desc, RowNum

SELECT pa.* FROM CLO.vw_AggregatePosition pa with(nolock) 
JOIN #result r ON pa.SecurityId = r.SecurityId
order by r.Delta desc, r.RowNum

drop table #CheckForCrediScoreAlerts
DROP TABLE #positions

RETURN 0
