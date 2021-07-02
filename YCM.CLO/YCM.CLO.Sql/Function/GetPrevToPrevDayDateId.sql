CREATE FUNCTION [CLO].[GetPrevToPrevDayDateId]()
RETURNS INT  WITH SCHEMABINDING 
BEGIN
	DECLARE @server VARCHAR(100) 
	SELECT @server = @@SERVERNAME
	DECLARE @daycount INT = 2,	@todaysDay INT
	SELECT @todaysDay = DATEPART (WEEKDAY , GETDATE() )  

	--IF(@todaysDay = 2)
	--	SELECT @daycount = @daycount + 2

	--IF((@server = 'DC-U-DWH-SQL01' OR @server = 'DC-D-DWH-SQL01') AND (@todaysDay = 2))
	--BEGIN
	--	SELECT @daycount = @daycount + 1
	--END
		
	RETURN dbo.[GetDateIdFromDate](DATEADD(DAY,DATEDIFF(DAY,@daycount,GETDATE()),0))
	
END
GO


