IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_SaveDecision')
	DROP PROCEDURE Scan.usp_SaveDecision
GO

CREATE PROCEDURE Scan.usp_SaveDecision
	@UserGuid UNIQUEIDENTIFIER
	,@DecisionId INT
	,@DecisionText NVARCHAR(MAX)
	,@DefaultTextSeverity INT
	,@ActiveInd BIT
	,@VehicleMakes AS Scan.udt_DecisionSettings READONLY
	,@RequestTypes AS Scan.udt_DecisionSettings READONLY
	,@RequestCategories AS Scan.udt_DecisionSettings READONLY
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			/**************************************************
				Lookup Decision ID.
			**************************************************/
			IF EXISTS (SELECT 1 FROM Scan.Decisions WHERE NULLIF(@DecisionId, 0) IS NULL AND DecisionText = @DecisionText)
				THROW 50000, 'Decision Text Found, please edit existing.', 1;

			/**************************************************
				Update/Insert Decision.
			**************************************************/
			MERGE Scan.Decisions AS t
			USING
			(
				SELECT
					@DecisionId
					,@DecisionText
					,@DefaultTextSeverity
					,@ActiveInd
			) AS s (DecisionId, DecisionText, DefaultTextSeverity, ActiveInd)
			ON (t.DecisionId = s.DecisionId)
			WHEN MATCHED THEN
				UPDATE
					SET DecisionText = s.DecisionText
						,DefaultTextSeverity = s.DefaultTextSeverity
						,ActiveInd = s.ActiveInd
						,UpdatedByUserGuid = @UserGuid
						,UpdatedDt = GETUTCDATE()
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (DecisionText, ActiveInd, DefaultTextSeverity, CreatedByUserGuid, CreatedDt)
				VALUES (DecisionText, ActiveInd, DefaultTextSeverity, @UserGuid, GETUTCDATE())
			OUTPUT INSERTED.*;

			SET @DecisionId = ISNULL(NULLIF(@DecisionId, 0), SCOPE_IDENTITY());

			/**************************************************
				Update/Insert Vehicle Make Selections.
			**************************************************/
			MERGE Scan.DecisionVehicleMakes AS t
			USING @VehicleMakes AS s
			ON (t.VehicleMakeId = s.TypeId AND t.DecisionId = @DecisionId)
			WHEN MATCHED THEN
				UPDATE
					SET PreSelectedInd = s.TypePreSelectedInd
						,UpdatedByUserGuid = @UserGuid
						,UpdatedDt = GETUTCDATE()
			WHEN NOT MATCHED BY SOURCE AND t.DecisionId = @DecisionId THEN
				DELETE
			WHEN NOT MATCHED BY TARGET THEN 
				INSERT (DecisionId, VehicleMakeId, PreSelectedInd, CreatedByUserGuid, CreatedDt)
				VALUES (@DecisionId, TypeId, TypePreSelectedInd, @UserGuid, GETUTCDATE())
			OUTPUT INSERTED.*;

			/**************************************************
				Update/Insert Request Type Selections.
			**************************************************/
			MERGE Scan.DecisionRequestTypes AS t
			USING @RequestTypes AS s
			ON (t.RequestTypeId = s.TypeId AND t.DecisionId = @DecisionId)
			WHEN MATCHED THEN
				UPDATE
					SET PreSelectedInd = s.TypePreSelectedInd
						,UpdatedByUserGuid = @UserGuid
						,UpdatedDt = GETUTCDATE()
			WHEN NOT MATCHED BY SOURCE AND t.DecisionId = @DecisionId THEN
				DELETE
			WHEN NOT MATCHED BY TARGET THEN 
				INSERT (DecisionId, RequestTypeId, PreSelectedInd, CreatedByUserGuid, CreatedDt)
				VALUES (@DecisionId, TypeId, TypePreSelectedInd, @UserGuid, GETUTCDATE())
			OUTPUT INSERTED.*;

			/**************************************************
				Update/Insert Request Type Selections.
			**************************************************/
			MERGE Scan.DecisionRequestCategories AS t
			USING @RequestCategories AS s
			ON (t.RequestCategoryId = s.TypeId AND t.DecisionId = @DecisionId)
			WHEN MATCHED THEN
				UPDATE
					SET PreSelectedInd = s.TypePreSelectedInd
						,UpdatedByUserGuid = @UserGuid
						,UpdatedDt = GETUTCDATE()
			WHEN NOT MATCHED BY SOURCE AND t.DecisionId = @DecisionId THEN
				DELETE
			WHEN NOT MATCHED BY TARGET THEN 
				INSERT (DecisionId, RequestCategoryId, PreSelectedInd, CreatedByUserGuid, CreatedDt)
				VALUES (@DecisionId, TypeId, TypePreSelectedInd, @UserGuid, GETUTCDATE())
			OUTPUT INSERTED.*;

			SELECT @DecisionId

		END TRY
		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END
GO