
CREATE PROCEDURE Scan.usp_SaveWorkTypeGroup
	@WorkTypeGroupId INT
	,@WorkTypeGroupName VARCHAR(MAX)
	,@WorkTypeGroupSortOrder INT
	,@WorkTypeGroupActiveInd BIT
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	-- Verify Work Type Group Id.
	IF NOT EXISTS (SELECT 1 FROM Scan.WorkTypeGroups WHERE WorkTypeGroupId = @WorkTypeGroupId) SET @WorkTypeGroupId = NULL
	SELECT @WorkTypeGroupId = ISNULL(@WorkTypeGroupId, wtg.WorkTypeGroupId)
	FROM Scan.WorkTypeGroups wtg
	WHERE wtg.WorkTypeGroupName = @WorkTypeGroupName

	-- Check Sort Order.
	SELECT @WorkTypeGroupSortOrder = ISNULL(@WorkTypeGroupSortOrder, COUNT(1) + 1)
	FROM Scan.WorkTypeGroups wtg

	BEGIN TRAN

		BEGIN TRY

			IF (@WorkTypeGroupId IS NULL)
				BEGIN

					INSERT INTO Scan.WorkTypeGroups (WorkTypeGroupName, WorkTypeGroupSortOrder, WorkTypeGroupActiveInd, CreatedByUserGuid, CreatedDt)
					SELECT @WorkTypeGroupName, @WorkTypeGroupSortOrder, @WorkTypeGroupActiveInd, @UserGuid, GETUTCDATE()

					SET @WorkTypeGroupId = SCOPE_IDENTITY()

				END
			ELSE
				BEGIN

					UPDATE wtg
						SET wtg.WorkTypeGroupName = @WorkTypeGroupName
							,wtg.WorkTypeGroupSortOrder = @WorkTypeGroupSortOrder
							,wtg.WorkTypeGroupActiveInd = @WorkTypeGroupActiveInd
							,wtg.UpdatedByUserGuid = @UserGuid
							,wtg.UpdatedDt = GETUTCDATE()
					FROM Scan.WorkTypeGroups wtg
					WHERE wtg.WorkTypeGroupId = @WorkTypeGroupId

				END

			;WITH SortOrder
			AS
			(
				SELECT
					wtg.WorkTypeGroupId
					,ROW_NUMBER() OVER(ORDER BY wtg.WorkTypeGroupSortOrder ASC, ISNULL(wtg.UpdatedDt, wtg.CreatedDt) ASC) [WorkTypeGroupSortOrderAsc]
					,ROW_NUMBER() OVER(ORDER BY wtg.WorkTypeGroupSortOrder ASC, ISNULL(wtg.UpdatedDt, wtg.CreatedDt) DESC) [WorkTypeGroupSortOrderDesc]
				FROM Scan.WorkTypeGroups wtg
			)
			UPDATE wtg
				SET wtg.WorkTypeGroupSortOrder = 
					CASE
						WHEN EXISTS (SELECT 1 FROM SortOrder WHERE WorkTypeGroupId = @WorkTypeGroupId AND WorkTypeGroupSortOrderAsc = @WorkTypeGroupSortOrder)
							THEN so.WorkTypeGroupSortOrderAsc
						ELSE so.WorkTypeGroupSortOrderDesc
					END
			FROM Scan.WorkTypeGroups wtg
			INNER JOIN SortOrder so
				ON wtg.WorkTypeGroupId = so.WorkTypeGroupId

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

	SELECT @WorkTypeGroupId;

	RETURN @WorkTypeGroupId;

END
GO