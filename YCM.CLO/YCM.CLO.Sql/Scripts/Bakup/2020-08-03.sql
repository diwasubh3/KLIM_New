USE YODA
GO

--UPDATE  [CLO].Field
--SET FieldTitle = 'WAS'
--where FieldTitle =  'spread' AND FieldGroupId = 4

--UPDATE [CLO].Field
--SET FieldGroupId = 7
--where FieldTitle in('TOTAL COUPON' , 'wa life')
--AND FieldGroupId = 4

--select * from [CLO].Field
--where FieldTitle in('TOTAL COUPON' , 'wa life')
--AND FieldGroupId = 4


--INSERT INTO [CLO].[Field]([FieldGroupId],[FieldName],[JsonPropertyName],[FieldTitle],[JsonFormatString],[DisplayWidth],[IsPercentage],[SortOrder],[FieldType],[HeaderCellClass],[CellClass],[CellTemplate],[Hidden],[PinnedLeft],[IsSecurityOverride],[ShowInFilter],[FilterOrder])     
--VALUES(4,'WALCushion','walCushion','WAL CUSHION','',120,0, 440,2,NULL,'text-right',NULL,1,NULL,NULL,0,NULL)

--INSERT INTO [CLO].[Field]([FieldGroupId],[FieldName],[JsonPropertyName],[FieldTitle],[JsonFormatString],[DisplayWidth],[IsPercentage],[SortOrder],[FieldType],[HeaderCellClass],[CellClass],[CellTemplate],[Hidden],[PinnedLeft],[IsSecurityOverride],[ShowInFilter],[FilterOrder])     
--VALUES(4, 'TimeToReinvest', 'timeToReinvest', 'TIME TO REINVEST',NULL,100,0, 470,2,'','text-right',NULL,1,NULL,NULL,0,NULL)

--INSERT INTO [CLO].[Field]([FieldGroupId],[FieldName],[JsonPropertyName],[FieldTitle],[JsonFormatString],[DisplayWidth],[IsPercentage],[SortOrder],[FieldType],[HeaderCellClass],[CellClass],[CellTemplate],[Hidden],[PinnedLeft],[IsSecurityOverride],[ShowInFilter],[FilterOrder])     
--VALUES(4, 'BB MOVC', 'bbmovc','BB MOVC','',100,0, 635,2,NULL,'text-right',NULL,1,NULL,NULL,0,NULL)

--DECLARE @Fields  TABLE(Fielid INT)
--SELECT @WalCushionID = FieldId FROM [CLO].[Field] WHERE FieldTitle in ('WAL CUSHION','TIME TO REINVEST','BB MOVC') AND FieldGroupId = 4

--INSERT INTO CLO.FundRestriction(FundId, FundRestrictionTypeId, FieldId, OperatorId, RestrictionValue)
--SELECT FundId, FundRestrictionTypeId,  Fielid, 4, CASE FundRestrictionTypeId WHEN 1 THEN .5 ELSE 0 END
--FROM CLO.Fund 
--CROSS JOIN CLO.FundRestrictionType
--CROSS JOIN @Fields
--WHERE IsActive = 1
