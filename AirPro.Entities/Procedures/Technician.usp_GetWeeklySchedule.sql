
CREATE PROCEDURE Technician.usp_GetWeeklySchedule
	@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	IF (OBJECT_ID('tempdb..#Schedule') IS NOT NULL) DROP TABLE #Schedule;

	-- Load User Local Time.
	DECLARE @UserLocal DATETIME;
	SELECT
		@UserLocal = CAST(GETUTCDATE() AS DATETIMEOFFSET) AT TIME ZONE u.TimeZoneInfoId
	FROM Access.Users u
	WHERE u.UserGuid = @UserGuid

	-- Load Schedule Start Date.
	DECLARE @StartDate DATETIME = DATEADD(DAY, -6, Common.udf_GetLastDayOfWeek(@UserLocal));

	SELECT
		s.UserGuid
		,s.UserTimeZoneInfoId
		,b.StartTime
		,b.EndTime
	INTO #Schedule
	FROM 
	(
		SELECT
			u.UserGuid
			,u.TimeZoneInfoId [UserTimeZoneInfoId]
			,CAST(CONVERT(VARCHAR(10), @StartDate + ps.DayOfWeek, 101) AS DATETIME) + CAST(ps.StartTime AS DATETIME) AT TIME ZONE u.TimeZoneInfoId [StartTime]
			,CAST(CONVERT(VARCHAR(10), @StartDate + ps.DayOfWeek, 101) AS DATETIME) + CAST(ps.EndTime AS DATETIME) AT TIME ZONE u.TimeZoneInfoId [EndTime]
			,CAST(CONVERT(VARCHAR(10), @StartDate + ps.DayOfWeek, 101) AS DATETIME) + CAST(ps.BreakStart AS DATETIME) AT TIME ZONE u.TimeZoneInfoId [BreakStartTime]
			,CAST(CONVERT(VARCHAR(10), @StartDate + ps.DayOfWeek, 101) AS DATETIME) + CAST(ps.BreakEnd AS DATETIME) AT TIME ZONE u.TimeZoneInfoId [BreakEndTime]
		FROM Technician.ProfileSchedules ps
		INNER JOIN Access.Users u
			ON ps.UserGuid = u.UserGuid
				AND u.LockoutEnabled = 1
				AND ISNULL(u.LockoutEndDateUtc, GETUTCDATE() - 1) < GETUTCDATE()
		INNER JOIN Technician.Profiles p
			ON ps.UserGuid = p.UserGuid
				AND p.ActiveInd = 1
		WHERE ps.StartTime IS NOT NULL
			AND ps.EndTime IS NOT NULL
	) s
	OUTER APPLY
	(
		SELECT StartTime, EndTime
		FROM
		(
			SELECT s.StartTime [StartTime], CASE WHEN s.BreakStartTime < s.EndTime THEN s.BreakStartTime ELSE s.EndTime END [EndTime]
			UNION
			SELECT CASE WHEN s.BreakEndTime < s.EndTime THEN s.BreakEndTime ELSE s.StartTime END [StartTime], s.EndTime
		) d
		WHERE d.EndTime > d.StartTime
			AND (s.StartTime <> ISNULL(s.BreakStartTime, GETUTCDATE())
				OR s.EndTime <> ISNULL(s.BreakEndtime, GETUTCDATE()))
	) b
	WHERE b.StartTime IS NOT NULL
		AND b.EndTime IS NOT NULL

	DECLARE @PtoUserGuid UNIQUEIDENTIFIER
	DECLARE @PtoStart DATETIMEOFFSET
	DECLARE @PtoEnd DATETIMEOFFSET
	DECLARE timeoff_cursor CURSOR STATIC FOR
	SELECT
		s.UserGuid
		,CAST(pto.StartDate AS DATETIME) AT TIME ZONE s.UserTimeZoneInfoId [PtoStartDate]
		,CAST(pto.EndDate AS DATETIME) AT TIME ZONE s.UserTimeZoneInfoId [PtoEndDate]
	FROM #Schedule s
	LEFT JOIN Technician.ProfileTimeOffEntries pto
		ON pto.UserGuid = s.UserGuid
			AND CAST(pto.StartDate AS DATETIME) AT TIME ZONE s.UserTimeZoneInfoId <= s.EndTime
			AND CAST(pto.EndDate AS DATETIME) AT TIME ZONE s.UserTimeZoneInfoId >= s.StartTime
	WHERE pto.TimeOffEntryId IS NOT NULL
	OPEN timeoff_cursor

	FETCH NEXT FROM timeoff_cursor
	INTO @PtoUserGuid, @PtoStart, @PtoEnd

	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE @ScheduleStart DATETIMEOFFSET
		DECLARE @ScheduleEnd DATETIMEOFFSET
		DECLARE schedule_cursor CURSOR DYNAMIC FOR
		SELECT s.StartTime, s.EndTime
		FROM #Schedule s
		WHERE s.UserGuid = @PtoUserGuid
			AND @PtoStart < s.EndTime
			AND @PtoEnd > s.StartTime

		OPEN schedule_cursor

		FETCH NEXT FROM schedule_cursor
		INTO @ScheduleStart, @ScheduleEnd

		WHILE @@FETCH_STATUS = 0
		BEGIN

			-- Delete Current Schedule.
			DELETE s
			FROM #Schedule s
			WHERE s.UserGuid = @PtoUserGuid
				AND s.StartTime = @ScheduleStart
				AND s.EndTime = @ScheduleEnd

			-- Insert New Schedule(s).
			INSERT INTO #Schedule (UserGuid, StartTime, EndTime)
			SELECT
				@PtoUserGuid
				,s.StartTime
				,s.EndTime
			FROM
			(
				SELECT
					CASE WHEN @PtoStart < @ScheduleStart THEN @PtoEnd ELSE @ScheduleStart END [StartTime]
					,CASE WHEN @PtoStart > @ScheduleStart THEN @PtoStart ELSE @ScheduleEnd END [EndTime]

				UNION

				SELECT
					@PtoEnd [StartTime]
					,@ScheduleEnd
				WHERE @PtoStart IS NOT NULL
					AND @PtoStart > @ScheduleStart
					AND @PtoEnd < @ScheduleEnd
			) s

			FETCH NEXT FROM schedule_cursor
			INTO @ScheduleStart, @ScheduleEnd

		END

		CLOSE schedule_cursor
		DEALLOCATE schedule_cursor

		FETCH NEXT FROM timeoff_cursor
		INTO @PtoUserGuid, @PtoStart, @PtoEnd
	
	END

	CLOSE timeoff_cursor
	DEALLOCATE timeoff_cursor

	SELECT
		s.UserGuid
		,u.Email [UserEmail]
		,u.LastName + ', ' + u.FirstName [UserFullName]
		,p.DisplayName [ProfileDisplayName]
		,s.StartTime
		,s.EndTime
	FROM #Schedule s
	INNER JOIN Access.Users u
		ON s.UserGuid = u.UserGuid
	INNER JOIN Technician.Profiles p
		ON s.UserGuid = p.UserGuid
			AND p.ActiveInd = 1
	ORDER BY UserGuid, StartTime, EndTime

END
GO