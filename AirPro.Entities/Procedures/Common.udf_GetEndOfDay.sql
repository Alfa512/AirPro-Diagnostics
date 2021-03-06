
CREATE FUNCTION Common.udf_GetEndOfDay 
(
	@Date DATETIME
)
RETURNS DATETIME
AS
BEGIN

	DECLARE @Result DATETIME

	SELECT @Result = DATEADD(MS, -2, DATEADD(DAY, 1, @Date))

	RETURN @Result

END
GO