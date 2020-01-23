
CREATE PROCEDURE Reporting.usp_GetHourlyVolumeByWeekReport
	@UserGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @TimeZone NVARCHAR(100);
	SELECT TOP 1 @TimeZone = TimeZoneInfoId
	FROM Access.Users
	WHERE UserGuid = ISNULL(@UserGuid, Common.udf_GetEmptyGuid());

	DECLARE @Today DATETIME;
	SELECT @Today = CAST(GETUTCDATE() AT TIME ZONE @TimeZone AS DATE);

	WITH Base
	AS
	(
		SELECT
			RequestId
			,RequestTypeId
			,CreatedDt
			,[Hour]
			,[Weekday]
		FROM
		(
			SELECT
				rd.RequestId
				,rd.RequestTypeId
				,CAST(rd.ReportCreatedDt AT TIME ZONE @TimeZone AS DATE) [CreatedDt]
				,DATEPART(hour, CAST(rd.ReportCreatedDt AT TIME ZONE @TimeZone AS DATETIME)) [Hour]
				,DATENAME(w, DATEPART(weekday, CAST(rd.ReportCreatedDt AT TIME ZONE @TimeZone AS DATETIME))) [Weekday]
			FROM Reporting.ReportData rd
			WHERE rd.RequestId IS NOT NULL
				AND rd.ReportCreatedDt IS NOT NULL
				AND rd.RequestTypeId NOT IN (6, 8)
				AND rd.ReportCreatedDt >= DATEADD(Month, -1, getutcdate())
		) rd
		WHERE rd.Weekday != 'Sunday'
		GROUP BY
			RequestId
			,RequestTypeId
			,CreatedDt
			,[Hour]
			,[Weekday]
	),
	Daily
	AS
	(
		SELECT
			b.RequestTypeId
			,b.CreatedDt
			,[Hour]
			,[Weekday]
			,COUNT(DISTINCT b.RequestId) [RequestCount]
		FROM Base b
		GROUP BY
			b.RequestTypeId
			,b.CreatedDt
			,[Hour]
			,[Weekday]
	),
	LastWeek
	AS
	(
		SELECT
			d.RequestTypeId
			,[Hour]
			,[Weekday]
			,AVG(d.RequestCount) [LastWeekAvg]
		FROM Daily d
		WHERE d.CreatedDt >= DATEADD(wk, DATEDIFF(wk, 7, @Today), 0) AND d.CreatedDt < DATEADD(wk, DATEDIFF(wk, 7, @Today), 6)
		GROUP BY
			d.RequestTypeId
			,[Hour]
			,[Weekday]
	),
	WeekBefore
	AS
	(
		SELECT
			d.RequestTypeId			
			,[Hour]
			,[Weekday]
			,AVG(d.RequestCount) [WeekBeforeAvg]
		FROM Daily d
		WHERE d.CreatedDt >= DATEADD(wk, DATEDIFF(wk, 14, @Today), 0) AND d.CreatedDt < DATEADD(wk, DATEDIFF(wk, 14, @Today), 6)
		GROUP BY
			d.RequestTypeId			
			,[Hour]
			,[Weekday]
	)

	SELECT
		o.[Hour]
		,o.[Weekday]
		,rt.TypeName [RequestType]
		,o.LastWeekAvg [PreviousWeekAverage]
		,o.WeekBeforeAvg [PriorWeekAverage]
		,(o.LastWeekAvg - o.WeekBeforeAvg) [PreviousVsPriorAverage]
		,CASE WHEN o.LastWeekAvg +(o.LastWeekAvg - o.WeekBeforeAvg) < 0 THEN 0
			ELSE o.LastWeekAvg +(o.LastWeekAvg - o.WeekBeforeAvg)
		END [CurrentWeekEstimate] 
	FROM (SELECT ISNULL(l.RequestTypeId, w.RequestTypeId) RequestTypeId, ISNULL(l.Hour, w.Hour) Hour, ISNULL(l.Weekday, w.Weekday) Weekday, ISNULL(l.LastWeekAvg, 0) LastWeekAvg, ISNULL(w.WeekBeforeAvg, 0) WeekBeforeAvg FROM LastWeek l
	FULL OUTER JOIN WeekBefore w
		ON l.RequestTypeId = w.RequestTypeId AND l.Hour = w.Hour AND l.Weekday = w.Weekday) o	
	INNER JOIN Scan.RequestTypes rt
		ON o.RequestTypeId = rt.RequestTypeId
	ORDER BY
		o.Weekday, o.Hour

END