CREATE PROCEDURE [Technician].[usp_SaveProfileSchedules]
	@UserGuid NVARCHAR(50)
	,@UpdatedBy NVARCHAR(50)
	,@Schedules [Technician].[udt_Schedules] readonly

	/*
		Author: Manuel Sauceda
		Date: 2018-04-01
		Description: Will commit a Merge operation based on the parameters
		Usage:
			DECLARE 
				@UserGuid NVARCHAR(50) = 'DB97F422-2814-E811-9E59-000D3A75F92D',
				@UpdatedBy NVARCHAR(50) = 'DB97F422-2814-E811-9E59-000D3A75F92D',
				@Schedules [Technician].[udt_Schedules]

			INSERT INTO @Schedules VALUES (1, '09:00:00', '17:00:00', '13:00:00', '14:00:00')
				,(2, '09:00:00', '17:00:00', '13:00:00', '14:00:00')
				,(3, '09:00:00', '17:00:00', '13:00:00', '14:00:00')
				,(4, '09:00:00', '17:00:00', '13:00:00', '14:00:00')
				,(5, '09:00:00', '15:00:00', '13:00:00', '14:00:00')
				,(6, '10:00:00', '14:00:00', '13:00:00', '14:00:00')

			EXEC [Technician].[usp_SaveProfileSchedules] @UserGUid, @UpdatedBy, @Schedules
	*/
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN cTran;
		
	DECLARE @UserTimeZone VARCHAR(100)
	SELECT @UserTimeZone = u.TimeZoneInfoId
	FROM Access.Users u
	WHERE u.UserGuid = @UpdatedBy
	DECLARE @CurrentDt DATE = GETUTCDATE() AT TIME ZONE @UserTimeZone

	BEGIN TRY
		--Estimate Plans
		MERGE [Technician].[ProfileSchedules] AS T
		USING (SELECT @UserGuid AS [UserGuid]
					,[DayOfWeek]
					,((CAST(@CurrentDt AS DATETIME) + CAST([StartTime] AS DATETIME)) AT TIME ZONE @UserTimeZone) AT TIME ZONE u.TimeZoneInfoId [StartTime]
					,((CAST(@CurrentDt AS DATETIME) + CAST([EndTime] AS DATETIME)) AT TIME ZONE @UserTimeZone) AT TIME ZONE u.TimeZoneInfoId [EndTime]
					,((CAST(@CurrentDt AS DATETIME) + CAST([BreakStart] AS DATETIME)) AT TIME ZONE @UserTimeZone) AT TIME ZONE u.TimeZoneInfoId [BreakStart]
					,((CAST(@CurrentDt AS DATETIME) + CAST([BreakEnd] AS DATETIME)) AT TIME ZONE @UserTimeZone) AT TIME ZONE u.TimeZoneInfoId [BreakEnd]
				FROM @Schedules
				INNER JOIN Access.Users u ON u.UserGuid = @UserGuid) AS S
			ON (T.[UserGuid] = S.[UserGuid] AND T.[DayOfWeek] = S.[DayOfWeek])
		WHEN MATCHED THEN
			UPDATE SET	T.[StartTime] = S.[StartTime], T.[EndTime] = S.[EndTime],
						T.[BreakStart] = S.[BreakStart], T.[BreakEnd] = S.[BreakEnd]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([UserGuid], [DayOfWeek], [StartTime], [EndTime], [BreakStart], [BreakEnd])
			VALUES (S.[UserGuid], S.[DayOfWeek], S.[StartTime], S.[EndTime], S.[BreakStart], S.[BreakEnd])
		OUTPUT $action;

		COMMIT TRAN cTran;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN cTran;
		THROW
	END CATCH
END