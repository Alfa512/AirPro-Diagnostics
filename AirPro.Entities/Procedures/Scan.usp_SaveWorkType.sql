
DROP PROCEDURE IF EXISTS Scan.usp_SaveWorkType;
GO

CREATE PROCEDURE Scan.usp_SaveWorkType
	@WorkTypeId INT
	,@WorkTypeName VARCHAR(MAX)
	,@WorkTypeSortOrder INT
	,@WorkTypeDescription VARCHAR(MAX)
	,@WorkTypeActiveInd BIT
	,@WorkTypeGroupId INT
	,@WorkTypeRequestTypeIds VARCHAR(MAX)
	,@WorkTypeVehicleMakeIds VARCHAR(MAX)
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	/****************************************
		Set 1: Lookup Work Type ID.
	****************************************/
	IF NOT EXISTS (SELECT 1 FROM Scan.WorkTypes WHERE WorkTypeId = @WorkTypeId) SET @WorkTypeId = NULL
	SELECT @WorkTypeId = ISNULL(@WorkTypeId, wt.WorkTypeId)
	FROM Scan.WorkTypes wt
	WHERE wt.WorkTypeName = @WorkTypeName
		AND wt.WorkTypeGroupId = @WorkTypeGroupId

	/****************************************
		Set 2: Set Default Sort Order.
	****************************************/
	SELECT @WorkTypeSortOrder = ISNULL(@WorkTypeSortOrder, MAX(WorkTypeSortOrder) + 1) FROM Scan.WorkTypes

	BEGIN TRAN

		BEGIN TRY

			/****************************************
				Set 3: Insert/Update Work Type.
			****************************************/
			DECLARE @InsertedWorkTypes TABLE (WorkTypeId INT);
			MERGE Scan.WorkTypes AS t
			USING
			(
				SELECT
					@WorkTypeId [WorkTypeId]
					,@WorkTypeName [WorkTypeName]
					,@WorkTypeSortOrder [WorkTypeSortOrder]
					,@WorkTypeDescription [WorkTypeDescription]
					,@WorkTypeActiveInd [WorkTypeActiveInd]
					,@WorkTypeGroupId [WorkTypeGroupId]
			) AS s
			ON (t.WorkTypeId = s.WorkTypeId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (WorkTypeName, WorkTypeSortOrder, WorkTypeDescription, WorkTypeActiveInd, WorkTypeGroupId, CreatedByUserGuid, CreatedDt)
				VALUES (WorkTypeName, WorkTypeSortOrder, WorkTypeDescription, WorkTypeActiveInd, WorkTypeGroupId, @UserGuid, GETUTCDATE())
			WHEN MATCHED THEN
				UPDATE
					SET WorkTypeName = s.WorkTypeName
						,WorkTypeSortOrder = s.WorkTypeSortOrder
						,WorkTypeDescription = s.WorkTypeDescription
						,WorkTypeActiveInd = s.WorkTypeActiveInd
						,WorkTypeGroupId = s.WorkTypeGroupId
						,UpdatedByUserGuid = @UserGuid
						,UpdatedDt = GETUTCDATE()
			OUTPUT INSERTED.WorkTypeId
			INTO @InsertedWorktypes;
			SELECT TOP 1 @WorkTypeId = ISNULL(@WorkTypeId, WorkTypeId) FROM @InsertedWorkTypes;

			/****************************************
				Set 4: Update Work Type Sort Order.
			****************************************/
			WITH SortOrder
			AS
			(
				SELECT
					wtg.WorkTypeId
					,ROW_NUMBER() OVER(ORDER BY wtg.WorkTypeSortOrder ASC, ISNULL(wtg.UpdatedDt, wtg.CreatedDt) ASC) [WorkTypeSortOrderAsc]
					,ROW_NUMBER() OVER(ORDER BY wtg.WorkTypeSortOrder ASC, ISNULL(wtg.UpdatedDt, wtg.CreatedDt) DESC) [WorkTypeSortOrderDesc]
				FROM Scan.WorkTypes wtg
			)
			UPDATE wt
				SET wt.WorkTypeSortOrder = 
					CASE
						WHEN EXISTS (SELECT 1 FROM SortOrder WHERE WorkTypeId = @WorkTypeId AND WorkTypeSortOrderAsc = @WorkTypeSortOrder)
							THEN so.WorkTypeSortOrderAsc
						ELSE so.WorkTypeSortOrderDesc
					END
			FROM Scan.WorkTypes wt
			INNER JOIN SortOrder so
				ON wt.WorkTypeId = so.WorkTypeId

			/****************************************
				Set 5: Update Request Types.
			****************************************/
			DECLARE @InsertedRequestTypes TABLE (WorkTypeId INT, RequestTypeId INT);
			MERGE Scan.WorkTypeRequestTypes AS t
			USING
			(
				SELECT
					@WorkTypeId [WorkTypeId]
					,Val [RequestTypeId]
				FROM Common.udf_JsonArrayToTable(@WorkTypeRequestTypeIds)
			) AS s
			ON (t.WorkTypeId = s.WorkTypeId AND t.RequestTypeId = s.RequestTypeId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (WorkTypeId, RequestTypeId)
				VALUES (WorkTypeId, RequestTypeId)
			WHEN NOT MATCHED BY SOURCE AND t.WorkTypeId = @WorkTypeId THEN
				DELETE
			OUTPUT INSERTED.WorkTypeId, INSERTED.RequestTypeId
			INTO @InsertedRequestTypes;

			/****************************************
				Set 6: Update Vehicle Makes.
			****************************************/
			DECLARE @InsertedVehicleMakes TABLE (WorkTypeId INT, VehicleMakeId INT);
			MERGE Scan.WorkTypeVehicleMakes AS t
			USING
			(
				SELECT
					@WorkTypeId [WorkTypeId]
					,Val [VehicleMakeId]
				FROM Common.udf_JsonArrayToTable(@WorkTypeVehicleMakeIds)
			) AS s
			ON (t.WorkTypeId = s.WorkTypeId AND t.VehicleMakeId = s.VehicleMakeId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (WorkTypeId, VehicleMakeId)
				VALUES (WorkTypeId, VehicleMakeId)
			WHEN NOT MATCHED BY SOURCE AND t.WorkTypeId = @WorkTypeId THEN
				DELETE
			OUTPUT INSERTED.WorkTypeId, INSERTED.VehicleMakeId
			INTO @InsertedVehicleMakes;
		
		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

	SELECT @WorkTypeId;

	RETURN @WorkTypeId;

END
GO