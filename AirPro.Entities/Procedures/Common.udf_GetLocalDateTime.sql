

CREATE FUNCTION Common.udf_GetLocalDateTime
(
	@DateTime DATETIMEOFFSET
	,@Offset CHAR(10)
)
RETURNS DATETIME
AS
BEGIN

	DECLARE @Result DATETIME

	IF (LEFT(@Offset, 1) != '+' AND LEFT(@Offset, 1) != '-')
		SET @Offset = '+' + @Offset

	SELECT @Result = CAST(SWITCHOFFSET(NULLIF(@DateTime, '0001-01-01'), LEFT(@Offset, 6)) AS DATETIME)

	RETURN @Result

END
GO