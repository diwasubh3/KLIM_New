CREATE FUNCTION [CLO].[GetPrevDayDateId]()
RETURNS INT
BEGIN
	DECLARE @server VARCHAR(100)
	SELECT @server = @@SERVERNAME
	DECLARE @daycount INT = 1,	@todaysDay INT
	SELECT @todaysDay = DATEPART(WEEKDAY, GETDATE())

	IF((@server = 'DC-U-DWH-SQL01' OR @server = 'DC-D-DWH-SQL01'))
	BEGIN
		SELECT @daycount = @daycount + 1
		--IF(@todaysDay = 2)--Monday
		--	SELECT @daycount = @daycount + 2
	END

	RETURN dbo.GetDateIdFromDate(DATEADD(DAY, DATEDIFF(DAY, @daycount, GETDATE()), 0))
END
GO
