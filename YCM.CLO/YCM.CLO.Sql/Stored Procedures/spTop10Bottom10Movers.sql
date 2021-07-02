CREATE PROCEDURE [CLO].[spTop10Bottom10Movers]
	@ruleSectionName varchar(100),
	@fundCode varchar(100),
	@dateId int
AS
	
select top 10
p.SecurityId,PriceMove,
ROW_NUMBER () Over (order by 
case when @ruleSectionName = 'Top' then p.PriceMove end desc,
case when @ruleSectionName = 'Bottom' then p.PriceMove end asc) RowNum
into #securities
from clo.vw_Position p with(nolock)
WHERE ISNULL(BidNum,0) <> 0 AND ISNULL(PrevDayBidNum,0) <> 0 AND  p.TotalParNum IS NOT NULL  AND  p.TotalParNum <> 0 
AND p.Bid <> p.PrevDayBid
AND p.PositionDateId is not null
GROUP BY Securityid,PriceMove
order by RowNum

select a.*
FROM 
clo.vw_AggregatePosition a
JOIN #securities s ON s.SecurityId = a.SecurityId
ORDER BY s.RowNum

DROP TABLE #securities

RETURN 0
