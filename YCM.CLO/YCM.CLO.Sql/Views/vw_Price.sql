CREATE VIEW [CLO].[vw_Price]  WITH SCHEMABINDING 
	AS 
	
	with pricing_cfe 
	as (
	SELECT 
	
	   [PricingId]
      ,[DateId]
      ,[SecurityId]
      ,[Bid]
      ,[Offer]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[LastUpdatedOn]
      ,[LastUpdatedBy] 
	  ,ROW_NUMBER() OVER ( PARTITION BY SecurityId
                                                ORDER BY DateId DESC,createdon desc ) AS RowNum	
	FROM CLO.Pricing with(nolock))

	select DateId,SecurityId,Bid,Offer from pricing_cfe where RowNum = 1