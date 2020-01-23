

CREATE FUNCTION [Common].[udf_GetLastDayOfWeek]
(
	@Date DATETIME
)
RETURNS DATE
AS
BEGIN

	DECLARE @WeekNum INT = DATEPART(WEEK, @Date);
	DECLARE @YearNum CHAR(4) = DATEPART(YEAR, @Date);

	DECLARE @Result DATE =
		DATEADD(second, -1,
			DATEADD(day,
				DATEDIFF(day, 0,
					DATEADD(wk,
						DATEDIFF(wk, 5,
							CAST(RTRIM(@YearNum * 10000
								+ 1 * 100 + 1) AS DATETIME))
							+ ( @WeekNum + -1 ), 5)) + 1, 0));

	RETURN @Result

END