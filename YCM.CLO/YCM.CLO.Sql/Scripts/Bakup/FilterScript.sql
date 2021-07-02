
UPDATE clo.Field
SET ShowInFilter = 1
WHERE FieldId IN (9,10,11,12,3,65,66,120)

GO

UPDATE clo.Field
SET ShowInFilter = 1
WHERE FieldId IN (71,72,17)

GO

INSERT INTO CLO.Field(FieldGroupId, FieldName, JsonPropertyName, FieldTitle, JsonFormatString, Displaywidth, IsPercentage, SortOrder, FieldType, HeaderCellClass, CellClass, CellTemplate, Hidden, PinnedLeft, IsSecurityOverride)
VALUES (5,'IsFilterSuccess' , 'isFilterSuccess'  , 'Filtered'  , null,0, 0, 350, 1, NULL,NULL,NULL,1,NULL,NULL)
GO

UPDATE CLO.Field
SET FieldTitle = 'OFFER YIELD B/W',
DisplayWidth=110
WHERE FieldId = 120

GO