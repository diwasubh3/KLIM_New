CREATE FUNCTION [CLO].[GetPrevDayDateId]()  
returns INT  WITH SCHEMABINDING 
BEGIN
	DECLARE @server VARCHAR(100) 
	SELECT @server = @@SERVERNAME
	DECLARE @daycount INT = 1,	@todaysDay INT
	SELECT @todaysDay = DATEPART (weekday , GETDATE() )  

	--IF((@server = 'DC-U-DWH-SQL01' OR @server = 'DC-D-DWH-SQL01'))
	--begin
	--	SELECT @daycount = @daycount + 1
	--	--IF(@todaysDay = 2)
	--	--	SELECT @daycount = @daycount + 2
	--END
    
	RETURN dbo.[GetDateIdFromDate](DATEADD(day,datediff(day,@daycount,GETDATE()),0))
	
end