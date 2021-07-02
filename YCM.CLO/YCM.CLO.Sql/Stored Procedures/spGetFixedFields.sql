CREATE PROCEDURE [CLO].[spGetFields]
	@paramFieldGroupName VARCHAR(100)
AS
	if(@paramFieldGroupName = 'Matrix Point Based Fund Restrictions Fields')
	begin
		SELECT * FROM [CLO].[Field] f WITH(NOLOCK) 
		JOIN [CLO].[FieldGroup] fg ON f.FieldGroupId = fg.FieldGroupId 
		WHERE 
		f.FieldTitle in ('WARF', 'SPREAD', 'DIVERSITY')
		AND fg.FieldGroupName = 'Fund Restrictions'
		AND f.FieldName LIKE ('WSO%')
		ORDER BY f.SortOrder
	end
	else 
	begin
		SELECT * FROM [CLO].[Field] f WITH(NOLOCK) 
		JOIN [CLO].[FieldGroup] fg ON f.FieldGroupId = fg.FieldGroupId 
		WHERE 
	
		fg.FieldGroupName = @paramFieldGroupName
		ORDER BY f.SortOrder
	end
RETURN 0
