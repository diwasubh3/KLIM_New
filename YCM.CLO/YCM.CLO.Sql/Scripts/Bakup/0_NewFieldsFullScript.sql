USE [YODA]
GO

--SnpWarf

INSERT INTO CLO.ParameterType (ParameterTypeName) VALUES ('SnpIssuerAdjusted')

DECLARE @parameterTypeId INT = (SELECT TOP 1 ParameterTypeId  FROM CLO.ParameterType WHERE ParameterTypeName = 'SnpIssuerAdjusted')

INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES
(@parameterTypeId,'AAA',0.00135130371260269),
(@parameterTypeId,'AA+',0.0026750229855169),
(@parameterTypeId,'AA',0.00463628510383944),
(@parameterTypeId,'AA-',0.00639013428938094),
(@parameterTypeId,'A+',0.00994951998911727),
(@parameterTypeId,'A',0.0146349036033426),
(@parameterTypeId,'A-',0.019983426721514),
(@parameterTypeId,'BBB+',0.0271006220726603),
(@parameterTypeId,'BBB',0.0361167685136378),
(@parameterTypeId,'BBB-',0.0540421901624465),
(@parameterTypeId,'BB+',0.078491608420934),
(@parameterTypeId,'BB',0.123362740219725),
(@parameterTypeId,'BB-',0.156544159156972),
(@parameterTypeId,'B+',0.198199902715255),
(@parameterTypeId,'B',0.285950238945874),
(@parameterTypeId,'B-',0.361010926277928),
(@parameterTypeId,'CCC+',0.46413971356694),
(@parameterTypeId,'CCC',0.529299550096854),
(@parameterTypeId,'CCC-',0.575110154243008)
GO

ALTER TABLE CLO.Calculation 
ADD SnpWarf NUMERIC(32,8)
GO

ALTER TABLE CLO.AggregatedPosition
ADD SnpWarf NUMERIC(32,8)
GO

INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, DisplayWidth,IsPercentage, SortOrder, FieldType, CellClass)
VALUES (2, 'SnpWarfPct', 'snpWarfPct', 'S&P WARF', 80, 1,150, 4, 'text-right')
GO

DECLARE @fieldId INT = (SELECT TOP 1 FieldId FROM CLO.Field WHERE FieldName = 'SnpWarfPct')
INSERT INTO CLO.CustomViewField (FieldId, ViewId, SortOrder) VALUES (@fieldId, 1,770)

GO

--SnpLgd

ALTER TABLE CLO.Calculation 
ADD SnpLgd NUMERIC(32,8)
GO

ALTER TABLE CLO.AggregatedPosition
ADD SnpLgd NUMERIC(32,8)
GO

INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, DisplayWidth,IsPercentage, SortOrder, FieldType, CellClass)
VALUES (2, 'SnpLgdPct', 'snpLgdPct', 'S&P LGD', 80, 1,155, 4, 'text-right')
GO

DECLARE @fieldId INT = (SELECT TOP 1 FieldId FROM CLO.Field WHERE FieldName = 'SnpLgdPct')
INSERT INTO CLO.CustomViewField (FieldId, ViewId, SortOrder) VALUES (@fieldId, 1,770)
GO

--Moody's Lgd

ALTER TABLE CLO.Calculation 
ADD MoodysLgd NUMERIC(32,8)
GO

ALTER TABLE CLO.AggregatedPosition
ADD MoodysLgd NUMERIC(32,8)
GO

INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, DisplayWidth, IsPercentage, SortOrder, FieldType, CellClass)
VALUES (2, 'MoodysLgdPct', 'moodysLgdPct', 'MOODY''S LGD', 80, 1,155, 4, 'text-right')
GO

DECLARE @fieldId INT = (SELECT TOP 1 FieldId FROM CLO.Field WHERE FieldName = 'moodysLgdPct')
INSERT INTO CLO.CustomViewField (FieldId, ViewId, SortOrder) VALUES (@fieldId, 1,770)
GO

--YieldAvgLgd

ALTER TABLE CLO.Calculation 
ADD YieldAvgLgd NUMERIC(32,8)
GO

ALTER TABLE CLO.AggregatedPosition
ADD YieldAvgLgd NUMERIC(32,8)
GO

INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, DisplayWidth, IsPercentage, SortOrder, FieldType, CellClass)
VALUES (2, 'YieldAvgLgdPct', 'yieldAvgLgdPct', 'YIELD/AVG LGD', 80, 1,155, 4, 'text-right')
GO

DECLARE @fieldId INT = (SELECT TOP 1 FieldId FROM CLO.Field WHERE FieldName = 'YieldAvgLgdPct')
INSERT INTO CLO.CustomViewField (FieldId, ViewId, SortOrder) VALUES (@fieldId, 1,770)
GO

--SnpAAARecovery

INSERT INTO CLO.ParameterType (ParameterTypeName) VALUES ('AAARecoveryMapping')

DECLARE @parameterTypeId INT = (SELECT ParameterTypeId FROM CLO.ParameterType WHERE ParameterTypeName = 'AAARecoveryMapping')

INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '100', 75.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '95', 70.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '90', 65.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '85', 62.50)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '80', 60.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '75', 55.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '70', 50.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '65', 45.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '60', 40.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '55', 35.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '50', 30.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '45', 28.50)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '40', 27.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '35', 23.50)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '30', 20.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '25', 17.50)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '20', 15.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '15', 10.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '10', 5.00)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '5', 3.50)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, '0', 2.00)
GO

INSERT INTO CLO.ParameterType (ParameterTypeName) VALUES ('AAARecoveryLienType')

DECLARE @parameterTypeId INT = (SELECT ParameterTypeId FROM CLO.ParameterType WHERE ParameterTypeName = 'AAARecoveryLienType')

INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, 'First Lien', 41)
INSERT INTO CLO.ParameterValue (ParameterTypeId, ParameterValueText, ParameterValueNumber) VALUES (@parameterTypeId, 'Second Lien', 18)
GO	

ALTER TABLE CLO.Calculation
ADD SnpAAARecovery NUMERIC(32,8)
GO

ALTER TABLE CLO.AggregatedPosition
ADD SnpAAARecovery NUMERIC(32,8)
GO

INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, DisplayWidth, IsPercentage, SortOrder, FieldType, CellClass)
VALUES (2, 'SnpAAARecovery', 'snpAAARecovery', 'S&P AAA RECOVERY', 80, 0,155, 2, 'text-right')
GO

DECLARE @fieldId INT = (SELECT TOP 1 FieldId FROM CLO.Field WHERE FieldName = 'SnpAAARecovery')
INSERT INTO CLO.CustomViewField (FieldId, ViewId, SortOrder) VALUES (@fieldId, 1,770)

GO

INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, DisplayWidth, IsPercentage, SortOrder, FieldType, CellClass)
VALUES (2, 'LiborBaseRate', 'liborBaseRate', 'Libor Base Rate', 80, 0,155, 2, 'text-right')
GO

DECLARE @fieldId INT = (SELECT TOP 1 FieldId FROM CLO.Field WHERE FieldName = 'LiborBaseRate')
INSERT INTO CLO.CustomViewField (FieldId, ViewId, SortOrder) VALUES (@fieldId, 1,790)
GO

--SnPIssuerRatingAdjusted

ALTER TABLE CLO.Calculation
ADD SnPIssuerRatingAdjusted VARCHAR(5)
GO

--Snp Credit Watch

ALTER TABLE CLO.Calculation 
ADD SnpCreditWatch VARCHAR(5)
GO

ALTER TABLE CLO.AggregatedPosition
ADD SnpCreditWatch VARCHAR(5)
GO

INSERT INTO CLO.Field (FieldGroupId, FieldName, JsonPropertyName, FieldTitle, DisplayWidth, IsPercentage, SortOrder, FieldType)
VALUES (2, 'SnpCreditWatch', 'snpCreditWatch', 'S&P Credit Watch', 80, 0,155, 4)
GO

DECLARE @fieldId INT = (SELECT TOP 1 FieldId FROM CLO.Field WHERE FieldName = 'SnpCreditWatch')
INSERT INTO CLO.CustomViewField (FieldId, ViewId, SortOrder) VALUES (@fieldId, 1,770)
GO

-- update S&P ASSET RECOVERY

UPDATE CLO.Field
SET DisplayWidth = 100
WHERE FieldTitle = 'S&P ASSET RECOVERY'
GO

UPDATE CLO.Field
SET CellClass = 'text-right'
WHERE FieldTitle = 'S&P ASSET RECOVERY'
GO

UPDATE CLO.Field
SET FieldTitle = 'MOODY''S WARF'
WHERE FieldName = 'WARF' AND FieldGroupId = 2
GO

UPDATE CLO.Field
SET DisplayWidth = 100
WHERE FieldName = 'WARF' AND FieldGroupId = 2
GO



--LiborBaseRate
ALTER TABLE [CLO].[AggregatedPosition]
ADD [LiborBaseRate]			VARCHAR(50)
GO


ALTER TABLE [CLO].[MarketData]
ADD [SnpCreditWatch] VARCHAR(5),
[LiborBaseRate] numeric(38,10) NULL
GO

ALTER TABLE [CLO].[OverrideMarketData]
ADD [SnpCreditWatch] VARCHAR(5),
	[LiborBaseRate] numeric(38,10) NULL
GO

