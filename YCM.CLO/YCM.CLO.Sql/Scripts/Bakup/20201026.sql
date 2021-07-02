UPDATE CLO.Fund
SET IsActive = 1,
DisplayText = 8,
SortOrder = 800,
PrincipalCash = NULL,
CLOFileName = NULL,
IsWarehouse = 1
WHERE FundId IN (5)

GO
UPDATE CLO.Field
SET CellTemplate = '<div class="ui-grid-cell-contents pull-left" style="cursor:pointer" context-menu="grid.appScope.showMenu(row.entity, row.entity.isOnWatch, row.entity.isSellCandidate)" uib-tooltip="Security : {{row.entity.securityDesc}}" tooltip-append-to-body="true"><span  style= "width: 15px; display: inline-block" ><a style="color: black" ng-show="row.entity.isOnWatch" ><i class="fa fa-binoculars" ></i></a></span> <span  style= "width: 15px; display: inline-block" ><a ng-show="row.entity.isSellCandidate" style="font-weight: 1000 !important;color: red"><span class="isp">S</span></i></a></span> <span style= "width: 15px; display:inline-block"><a ng-show="row.entity.isOnAlert" style= "color: red" ><i class="fa fa-exclamation-triangle" ></i></a></span><span style= "width: 15px; display:inline-block"><span ng-if="row.entity.hasBuyTrade"  class="buy isp">B</span><span ng-if="row.entity.hasSellTrade" class="isp sell">S</span></span><span>{{row.entity.securityCode}}</span></div><div class="pull-right "  uib-tooltip="{{''Original Value : '' + (row.entity[''orig''+col.colDef.field.capitalizeFirstLetter()]?row.entity[''orig''+col.colDef.field.capitalizeFirstLetter()]:'''')}}" ng-if="row.entity[''orig''+col.colDef.field.capitalizeFirstLetter()] != row.entity[col.colDef.field]" tooltip-append-to-body="true"><img class="i-c" src="CLO/Content/Images/Comment.png"></div>'
WHERE FieldId = 52
GO

UPDATE [Yoda].[CLO].[Fund]
Set
ParentFundCode = FundCode,
ParentFundId = FundId
GO

UPDATE CLO.Fund
SET LiabilityPar = 0,
EquityPar = 0,
TargetPar = 0
WHERE FundId = 6

GO
UPDATE CLO.Fund
SET CanFilter = 1
GO

UPDATE CLO.Fund
SET CanFilter = 0
WHERE fundid = 6
GO

UPDATE [Yoda].[CLO].[Fund]
SET 
FundCode = 'CLO-8', FundDesc = 'CLO-8', 
ParentFundCode = 'CLO-8', ParentFundId = 5, 
LastUpdatedOn = GETDATE(), LastUpdatedBy = 'osantiago', PortfolioName = 'York Warehouse (CLO-8)'
WHERE FundId = 5

--UPDATE [Yoda].[CLO].[Fund]
--SET 
--FundCode = 'CLO-8b', FundDesc = 'CLO-8 B', 
--ParentFundCode = 'CLO-8', ParentFundId = 5,
--LastUpdatedOn = GETDATE(), LastUpdatedBy = 'osantiago', PortfolioName = 'York Warehouse (CLO-8 Sleeve B)'
--WHERE FundId = 6

--GO

INSERT INTO [CLO].[Field]
           ([FieldGroupId]
           ,[FieldName]
           ,[JsonPropertyName]
           ,[FieldTitle]
           ,[JsonFormatString]
           ,[DisplayWidth]
           ,[IsPercentage]
           ,[SortOrder]
           ,[FieldType]
           ,[HeaderCellClass]
           ,[CellClass]
           ,[CellTemplate]
           ,[Hidden]
           ,[PinnedLeft]
           ,[IsSecurityOverride]
           ,[ShowInFilter]
           ,[FilterOrder])
SELECT [FieldGroupId]
      ,'CLO8Exposure'
      ,'clO8Exposure'
      ,'CLO-8'
      ,[JsonFormatString]
      ,[DisplayWidth]
      ,[IsPercentage]
      ,[SortOrder]
      ,[FieldType]
      ,[HeaderCellClass]
      ,[CellClass]
      ,'<div class="ui-grid-cell-contents"  uib-tooltip="{{row.entity.clO8NumExposure| currency : '''' : 2}}" style="cursor:pointer;" tooltip-append-to-body="true"><span>{{row.entity.clO8Exposure}} </span></div>'
      ,[Hidden]
      ,[PinnedLeft]
      ,[IsSecurityOverride]
      ,[ShowInFilter]
      ,[FilterOrder]
  FROM [Yoda].[CLO].[Field]
  WHERE FieldId = 138

INSERT INTO [CLO].[Field]
           ([FieldGroupId]
           ,[FieldName]
           ,[JsonPropertyName]
           ,[FieldTitle]
           ,[JsonFormatString]
           ,[DisplayWidth]
           ,[IsPercentage]
           ,[SortOrder]
           ,[FieldType]
           ,[HeaderCellClass]
           ,[CellClass]
           ,[CellTemplate]
           ,[Hidden]
           ,[PinnedLeft]
           ,[IsSecurityOverride]
           ,[ShowInFilter]
           ,[FilterOrder])
SELECT [FieldGroupId]
      ,'CLO8PctExposure'
      ,'clO8PctExposure'
      ,'CLO-8%'
      ,[JsonFormatString]
      ,[DisplayWidth]
      ,[IsPercentage]
      ,[SortOrder]
      ,[FieldType]
      ,[HeaderCellClass]
      ,[CellClass]
      ,NULL
      ,[Hidden]
      ,[PinnedLeft]
      ,[IsSecurityOverride]
      ,[ShowInFilter]
      ,[FilterOrder]
  FROM [Yoda].[CLO].[Field]
  WHERE FieldId = 139

INSERT INTO [CLO].[Field]
           ([FieldGroupId]
           ,[FieldName]
           ,[JsonPropertyName]
           ,[FieldTitle]
           ,[JsonFormatString]
           ,[DisplayWidth]
           ,[IsPercentage]
           ,[SortOrder]
           ,[FieldType]
           ,[HeaderCellClass]
           ,[CellClass]
           ,[CellTemplate]
           ,[Hidden]
           ,[PinnedLeft]
           ,[IsSecurityOverride]
           ,[ShowInFilter]
           ,[FilterOrder])
SELECT [FieldGroupId]
      ,'CLO8MatrixWarfRecovery'
      ,'clO8MatrixWarfRecovery'
      ,'CLO-8 MATRIX WARF RECOVERY'
      ,[JsonFormatString]
      ,[DisplayWidth]
      ,[IsPercentage]
      ,[SortOrder]
      ,[FieldType]
      ,[HeaderCellClass]
      ,[CellClass]
      ,NULL
      ,[Hidden]
      ,[PinnedLeft]
      ,[IsSecurityOverride]
      ,[ShowInFilter]
      ,[FilterOrder]
  FROM [Yoda].[CLO].[Field]
  WHERE FieldId = 159

INSERT INTO [CLO].[Field]
           ([FieldGroupId]
           ,[FieldName]
           ,[JsonPropertyName]
           ,[FieldTitle]
           ,[JsonFormatString]
           ,[DisplayWidth]
           ,[IsPercentage]
           ,[SortOrder]
           ,[FieldType]
           ,[HeaderCellClass]
           ,[CellClass]
           ,[CellTemplate]
           ,[Hidden]
           ,[PinnedLeft]
           ,[IsSecurityOverride]
           ,[ShowInFilter]
           ,[FilterOrder])
SELECT [FieldGroupId]
      ,'CLO8MatrixImpliedSpread'
      ,'clO8MatrixImpliedSpread'
      ,'CLO-8 MATRIX SPREAD'
      ,[JsonFormatString]
      ,[DisplayWidth]
      ,[IsPercentage]
      ,[SortOrder]
      ,[FieldType]
      ,[HeaderCellClass]
      ,[CellClass]
      ,NULL
      ,[Hidden]
      ,[PinnedLeft]
      ,[IsSecurityOverride]
      ,[ShowInFilter]
      ,[FilterOrder]
  FROM [Yoda].[CLO].[Field]
  WHERE FieldId = 160
 
INSERT INTO [CLO].[Field]
           ([FieldGroupId]
           ,[FieldName]
           ,[JsonPropertyName]
           ,[FieldTitle]
           ,[JsonFormatString]
           ,[DisplayWidth]
           ,[IsPercentage]
           ,[SortOrder]
           ,[FieldType]
           ,[HeaderCellClass]
           ,[CellClass]
           ,[CellTemplate]
           ,[Hidden]
           ,[PinnedLeft]
           ,[IsSecurityOverride]
           ,[ShowInFilter]
           ,[FilterOrder])
SELECT [FieldGroupId]
      ,'CLO8DifferentialImpliedSpread'
      ,'clO8DifferentialImpliedSpread'
      ,'CLO-8 DIFF SPREAD'
      ,[JsonFormatString]
      ,[DisplayWidth]
      ,[IsPercentage]
      ,[SortOrder]
      ,[FieldType]
      ,[HeaderCellClass]
      ,[CellClass]
      ,NULL
      ,[Hidden]
      ,[PinnedLeft]
      ,[IsSecurityOverride]
      ,[ShowInFilter]
      ,[FilterOrder]
  FROM [Yoda].[CLO].[Field]
  WHERE FieldId = 161
  
INSERT INTO [CLO].[Field]
           ([FieldGroupId]
           ,[FieldName]
           ,[JsonPropertyName]
           ,[FieldTitle]
           ,[JsonFormatString]
           ,[DisplayWidth]
           ,[IsPercentage]
           ,[SortOrder]
           ,[FieldType]
           ,[HeaderCellClass]
           ,[CellClass]
           ,[CellTemplate]
           ,[Hidden]
           ,[PinnedLeft]
           ,[IsSecurityOverride]
           ,[ShowInFilter]
           ,[FilterOrder])
SELECT [FieldGroupId]
      ,'CLO8NumExposure'
      ,'clO8NumExposure'
      ,'CLO8NumExposure'
      ,[JsonFormatString]
      ,[DisplayWidth]
      ,[IsPercentage]
      ,[SortOrder]
      ,[FieldType]
      ,[HeaderCellClass]
      ,[CellClass]
      ,NULL
      ,[Hidden]
      ,[PinnedLeft]
      ,[IsSecurityOverride]
      ,[ShowInFilter]
      ,[FilterOrder]
  FROM [Yoda].[CLO].[Field]
  WHERE FieldId = 168


