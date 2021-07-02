Use YODA
GO


UPDATE CLO.Field
SET ShowInFilter = 1
WHERE FieldName IN (
'CLO1Exposure'
,'CLO1PctExposure'
,'CLO2Exposure'
,'CLO2PctExposure'
,'CLO3Exposure'
,'CLO3PctExposure'
,'CLO4Exposure'
,'CLO4PctExposure'
,'CLO5Exposure'
,'CLO5PctExposure'
,'CLO6Exposure'
,'CLO6PctExposure'
,'CLO7Exposure'
,'CLO7PctExposure'
,'TotalPar'
)

         

DECLARE @FieldOrder TABLE (FieldName VARCHAR(100),SortOrder INT)
INSERT INTO @FieldOrder(FieldName, SortOrder)
VALUES
('Bid', 100   )
,('BidYield', 200   )
--,('CappedBidYield', 300   )
--,('CappedMidYield', 400   )
--,('CappedOfferYield', 500   )
,('LiborFloor', 600   )
,('GlobalAmount', 700   )
,('MaturityDate', 800   )
,('MidYield', 900   )
,('Offer', 1000   )
,('OfferYield', 1100   )
,('BetterWorseBid', 1200   )
,('BetterWorseOffer', 1300   )
,('Spread', 1400   )
,('TargetYieldBid', 1500   )
,('TargetYieldOffer', 1600   )

--Ratings

,('MoodyFacilityRating', 100   )
,('MoodyFacilityRatingAdjusted', 200)
,('MoodyCashFlowRating', 300 )
,('MoodyCashFlowRatingAdjusted', 400   )
,('MoodyRecovery', 500   )
,('SnPFacilityRating', 600   )
,('SnPIssuerRating', 700   )
,('SnPIssuerRatingAdjusted', 800   )
,('WARF', 900   )
,('WARF Recovery', 1000   )

-- Analyst
,('CreditScore', 100   )
,('EnterpriseValue', 200   )
,('LTMEBITDA', 300   )
,('LTMFCF', 400   )
,('LTMRevenues', 500   )
,('SeniorLeverage', 600   )
,('TotalLeverage', 700   )




UPDATE CLO.Field
SET FilterOrder  = fo.SortOrder,
ShowInFilter = 1
FROM @FieldOrder fo
WHERE fo.FieldName = CLO.Field.FieldName

