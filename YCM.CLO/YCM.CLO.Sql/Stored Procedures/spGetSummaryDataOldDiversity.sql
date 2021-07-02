CREATE PROCEDURE [CLO].[spGetSummaryDataOldDiversity]
	@dateId int = 0
AS

--declare @fundDiversities TABLE
--(
--	FundId int,
--	Diversity numeric(38,10),
--	BODDiversity numeric(38,10)
--)

--declare @ExposureBy_Issuer_MoodyIndustry_Portfolio table (
--       FundCode VARCHAR(100), 
--       FundId INT,
--       IssuerDesc VARCHAR(1000),
--       IssuerId INT, 
--       IndustryDesc VARCHAR(1000),
--       MoodyIndustryId INT, 
--       Exposure NUMERIC(28,2),
--	   BODExposure NUMERIC(28,2),
--       DiversityUnit NUMERIC(28,8), 
--       Diveristy NUMERIC(28,8),
--       BODDiversityUnit NUMERIC(28,8), 
--       BODDiveristy NUMERIC(28,8)
--)

--DECLARE @AVG_Fund_Exposure TABLE (FundId VARCHAR(100), AvgExposure NUMERIC(38,10), BODAvgExposure NUMERIC(38,10))

--INSERT INTO @ExposureBy_Issuer_MoodyIndustry_Portfolio
--SELECT p.FundCode , p.FundId, p.Issuer,p.IssuerId, p.MoodyIndustry,
--p.MoodyIndustryId, SUM(ISNULL(P.NumExposure,0)) Exposure  ,SUM(ISNULL(P.BODExposure,0)) BODExposure,  
--NULL AS DiversityUnit, NULL AS Diveristy,NULL AS BODDiversityUnit, NULL AS BODDiveristy
--FROM CLO.vw_Position p WITH(NOLOCK) 
--where  p.PositionDateId is not null
--GROUP BY p.IssuerId,p.Issuer, p.MoodyIndustryId,p.MoodyIndustry,p.FundCode,p.FundId
--ORDER by p.Issuer,p.MoodyIndustry,p.FundCode


--INSERT INTO @AVG_Fund_Exposure
--        ( FundId, AvgExposure,BODAvgExposure )
--SELECT FundId, AVG(Exposure) AvgExposure, AVG(BODExposure) BODAvgExposure
--FROM @ExposureBy_Issuer_MoodyIndustry_Portfolio
--WHERE Exposure <> 0
--GROUP BY FundId


--UPDATE @ExposureBy_Issuer_MoodyIndustry_Portfolio
--SET DiversityUnit = CASE WHEN AvgExposure = 0.0000 THEN 0.0000 ELSE (Exposure/AvgExposure) END ,
--BODDiversityUnit = CASE WHEN BODAvgExposure = 0.0000 THEN 0.0000 ELSE (BODExposure/BODAvgExposure) END 
--FROM @AVG_Fund_Exposure i WHERE i.FundId = [@ExposureBy_Issuer_MoodyIndustry_Portfolio].FundId


--UPDATE @ExposureBy_Issuer_MoodyIndustry_Portfolio
--SET DiversityUnit = 1.0000 
--WHERE DiversityUnit > 1.0

--UPDATE @ExposureBy_Issuer_MoodyIndustry_Portfolio
--SET BODDiversityUnit = 1.0000 
--WHERE BODDiversityUnit > 1.0


--declare @Diverisity TABLE (FundId int, MoodyIndustryId INT, IndustryDesc VARCHAR(1000),
--TotalDiversityUnit NUMERIC(28,2), DiversityValue NUMERIC(38,2),BODTotalDiversityUnit NUMERIC(28,2), BODDiversityValue NUMERIC(38,2) )

--INSERT INTO @Diverisity
--SELECT FundId, MoodyIndustryId, MAX(IndustryDesc) IndustryDesc,  
--SUM(DiversityUnit) TotalDiversityUnit, NULL AS DiversityValue ,
--SUM(BODDiversityUnit) BODTotalDiversityUnit, NULL AS BODDiversityValue 
--FROM @ExposureBy_Issuer_MoodyIndustry_Portfolio
--GROUP BY MoodyIndustryId, FundId 

--UPDATE @Diverisity
--SET DiversityValue = CASE 
--WHEN TotalDiversityUnit <=1 THEN ((((TotalDiversityUnit/0.1)+0.5)/10)+0)
--WHEN TotalDiversityUnit > 1 AND TotalDiversityUnit <= 3 THEN ((((TotalDiversityUnit/0.1)-9.5)/20) + 1)
--WHEN TotalDiversityUnit > 3 AND TotalDiversityUnit <= 6 THEN ((((TotalDiversityUnit/0.1)-29.5)/30) + 2)
--WHEN TotalDiversityUnit > 6 AND TotalDiversityUnit <= 10 THEN ((((TotalDiversityUnit/0.1)-59.5)/40) + 3)
--WHEN TotalDiversityUnit > 10 THEN ((((TotalDiversityUnit/0.1)-99.5)/100) + 4)
--end

--UPDATE @Diverisity
--SET BODDiversityValue = CASE 
--WHEN BODTotalDiversityUnit <=1 THEN ((((BODTotalDiversityUnit/0.1)+0.5)/10)+0)
--WHEN BODTotalDiversityUnit > 1 AND BODTotalDiversityUnit <= 3 THEN ((((BODTotalDiversityUnit/0.1)-9.5)/20) + 1)
--WHEN BODTotalDiversityUnit > 3 AND BODTotalDiversityUnit <= 6 THEN ((((BODTotalDiversityUnit/0.1)-29.5)/30) + 2)
--WHEN BODTotalDiversityUnit > 6 AND BODTotalDiversityUnit <= 10 THEN ((((BODTotalDiversityUnit/0.1)-59.5)/40) + 3)
--WHEN BODTotalDiversityUnit > 10 THEN ((((BODTotalDiversityUnit/0.1)-99.5)/100) + 4)
--end



--INSERT @fundDiversities
--SELECT FundId, SUM(DiversityValue) as Diversity,SUM(BODDiversityValue) as BODDiversity  FROM @Diverisity GROUP BY FundId
	
--select summary.[FundCode]
--	  ,summary.IsStale
--	  ,summary.DateId	
--      ,summary.[Par]
--	  ,summary.[BODPar]
--      ,summary.[Spread]
--	  ,summary.[BODSpread]
--      ,summary.[TotalCoupon]
--	  ,summary.[BODTotalCoupon]
--      ,summary.[WARF]
--	  ,summary.[BODWARF]
--      ,summary.[MoodyRecovery]
--	  ,summary.[BODMoodyRecovery]
--      ,summary.[Bid]
--	  ,summary.[BODBid]
--      ,summary.[PrincipalCash]
--	  ,summary.[BODPrincipalCash]
--      ,summary.[FundId]
--	  ,d.[Diversity]
--	  ,d.[BODDiversity]
--	  ,summary.[CleanNav]
--	  ,summary.[BODCleanNav]
--from
--CLO.vw_CLOSummary summary
--left join @fundDiversities d on d.FundId = summary.FundId 
--where summary.DateId = @dateId


