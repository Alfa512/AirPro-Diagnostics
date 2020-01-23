
CREATE PROCEDURE [Technician].[usp_SaveProfileTimeOff]
	@UserGuid NVARCHAR(50)
	,@UpdatedBy NVARCHAR(50)
	,@TimeoffEntries [Technician].[udt_TimeOff] readonly

	/*
		Author: Manuel Sauceda
		Date: 2018-04-01
		Description: Will commit a Merge operation based on the parameters
		Usage:
			DECLARE 
				@UserGuid NVARCHAR(50) = 'DB97F422-2814-E811-9E59-000D3A75F92D',
				@UpdatedBy NVARCHAR(50) = '00000000-0000-0000-0000-000000000000',
				@TimeoffEntries [Technician].[udt_TimeOff]

			INSERT INTO @TimeoffEntries VALUES (0, GETUTCDATE(), DATEADD(DAY, 1, GETUTCDATE()), 'Some Reason', 0)

			EXEC [Technician].[usp_SaveProfileTimeOff] @UserGUid, @UpdatedBy, @TimeoffEntries
	*/
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN cTran;

	DECLARE @CurrentDt DATETIME = GETUTCDATE()
	DECLARE @UserTimeZone VARCHAR(100)
	SELECT @UserTimeZone = u.TimeZoneInfoId
	FROM Access.Users u
	WHERE u.UserGuid = @UpdatedBy
		
	BEGIN TRY
		--Estimate Plans
		MERGE [Technician].[ProfileTimeOffEntries] AS T
		USING (SELECT [TimeOffEntryId]
					,@UserGuid AS [UserGuid]
					,SWITCHOFFSET([StartDate] AT TIME ZONE @UserTimeZone, '-00:00') [StartDate]
					,SWITCHOFFSET([EndDate] AT TIME ZONE @UserTimeZone, '-00:00') [EndDate]
					,[Reason]
					,[DeleteInd]
				FROM @TimeoffEntries) AS S
			ON (T.[UserGuid] = S.[UserGuid] AND T.[TimeOffEntryId] = S.[TimeOffEntryId])
		WHEN MATCHED THEN
			UPDATE SET T.[StartDate] = S.[StartDate], T.[EndDate] = S.[EndDate], T.Reason = S.Reason,
						T.DeleteInd = S.DeleteInd, T.UpdatedByUserGuid = @UpdatedBy, T.UpdatedDt = @CurrentDt
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([UserGuid], [StartDate], [EndDate], [Reason], [DeleteInd], [CreatedByUserGuid], [CreatedDt])
			VALUES (S.[UserGuid], S.[StartDate], S.[EndDate], S.[Reason], S.[DeleteInd], @UpdatedBy, @CurrentDt)
		OUTPUT $action;

		COMMIT TRAN cTran;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN cTran;
		THROW
	END CATCH
END
GO