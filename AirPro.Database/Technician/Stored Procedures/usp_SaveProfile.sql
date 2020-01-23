CREATE PROCEDURE [Technician].[usp_SaveProfile]
	@UserGuid NVARCHAR(50)
	,@DisplayName NVARCHAR(128)
	,@EmployeeID NVARCHAR(128) 
	,@ActiveInd BIT = 1
	,@OtherNotes NVARCHAR(MAX) 
	,@CurrentUser NVARCHAR(50) 
	,@VehicleMakeIds NVARCHAR(MAX)
	,@LocationId INT

	/*
		Author: Manuel Sauceda
		Date: 2018-03-04
		Description: Will commit a Merge operation based on the parameters
		Usage:
			DECLARE 
				@UserGuid NVARCHAR(50) = 'DB97F422-2814-E811-9E59-000D3A75F92D',
				@DisplayName NVARCHAR(128) = 'Manuel Sauc',
				@EmployeeID NVARCHAR(128) = 'some id',
				@ActiveInd BIT = 1,
				@OtherNotes NVARCHAR(MAX) = 'SOME NOTES HERE',
				@CurrentUser NVARCHAR(50) = 'DB97F422-2814-E811-9E59-000D3A75F92D',
				@VehicleMakeIds NVARCHAR(MAX) = '1,2,3'
				
			EXEC [Technician].[usp_SaveProfile] @UserGUid, @DisplayName, @EmployeeID, @ActiveInd, @OtherNotes, @CurrentUser, @VehicleMakeIds
	*/
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN cTran;

	BEGIN TRY
		DECLARE @CurrentDt DATETIMEOFFSET(7) = GETUTCDATE()
		DECLARE @output table([action] nvarchar(20))

		DECLARE @VehicleMakes TABLE (Id INT)
		INSERT INTO @VehicleMakes
		SELECT Id FROM Common.udf_IdListToTable(@VehicleMakeIds)

		--Merge Profiles
		MERGE [Technician].[Profiles] AS T
		USING (SELECT @UserGuid AS [UserGuid], @DisplayName AS [DisplayName]
					, @EmployeeID AS [EmployeeId], @ActiveInd AS [ActiveInd]
					, @OtherNotes AS [OtherNotes]
					, @LocationId AS [LocationId]) AS S
		ON (T.UserGuid = S.[UserGuid])
		WHEN MATCHED THEN
			UPDATE SET T.DisplayName = S.DisplayName, T.[EmployeeId] = S.[EmployeeId], 
					   T.OtherNotes = S.OtherNotes, T.ActiveInd = S.ActiveInd,
					   T.LocationId = S.LocationId,
					   T.UpdatedByUserGuid = @CurrentUser, T.UpdatedDt = GETUTCDATE()
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (UserGuid, DisplayName, EmployeeId, OtherNotes, ActiveInd, CreatedByUserGUid, CreatedDt, UpdatedByUserGuid, UpdatedDt, LocationId)
			VALUES (S.UserGuid, S.DisplayName, S.EmployeeID, S.OtherNotes, S.ActiveInd, @CurrentUser, @CurrentDt, @CurrentUser, @CurrentDt, @LocationId)
		OUTPUT $action INTO @output;

		--Merge Vehicle Makes
		MERGE [Technician].[ProfileVehicleMakes] AS T
		USING (SELECT @UserGuid AS [UserGuid], Id as [Val] FROM @VehicleMakes) AS S
		ON (T.UserGuid = S.[UserGuid] AND T.VehicleMakeId = S.Val)
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (UserGuid, VehicleMakeId)
			VALUES (S.UserGuid, S.Val)
		WHEN NOT MATCHED BY SOURCE AND T.UserGuid = @UserGuid THEN DELETE
		OUTPUT $action INTO @output;
	
		COMMIT TRAN cTran;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN cTran;
		THROW
	END CATCH
END