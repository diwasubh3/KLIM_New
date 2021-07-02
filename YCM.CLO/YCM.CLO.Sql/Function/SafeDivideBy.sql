CREATE FUNCTION [CLO].[SafeDivideBy](@Numerator decimal(38,12), @Denominator decimal(38,12))
	RETURNS decimal(38,12)
AS
BEGIN
	
	IF @Denominator IS NULL RETURN NULL
	
	IF @Denominator = 0 RETURN 0

	RETURN @Numerator/@Denominator
		
END