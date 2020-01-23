
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetWorkTypeSearch')
	DROP PROCEDURE Scan.usp_SearchWorkTypes
GO

CREATE PROCEDURE Scan.usp_GetWorkTypeSearch
	@Search VARCHAR(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SET @Search = '%' + ISNULL(@Search, '') + '%';

	WITH SelectionSearch
	AS
	(
		SELECT
			wtrt.WorkTypeId
		FROM Scan.RequestTypes rt
		INNER JOIN Scan.WorkTypeRequestTypes wtrt
			ON rt.RequestTypeId = wtrt.RequestTypeId
		WHERE rt.TypeName LIKE @Search

		UNION

		SELECT
			wtvm.WorkTypeId
		FROM Repair.VehicleMakes vm
		INNER JOIN Scan.WorkTypeVehicleMakes wtvm
			ON vm.VehicleMakeId = wtvm.VehicleMakeId
		WHERE vm.VehicleMakeName LIKE @Search
	)

	SELECT
		wt.WorkTypeId
		,wt.WorkTypeName
		,wt.WorkTypeSortOrder
		,wt.WorkTypeDescription
		,wt.WorkTypeActiveInd
		,wtg.WorkTypeGroupId
		,wtg.WorkTypeGroupName
		,wtg.WorkTypeGroupSortOrder
		,wtg.WorkTypeGroupActiveInd
	FROM Scan.WorkTypes wt
	INNER JOIN Scan.WorkTypeGroups wtg
		ON wt.WorkTypeGroupId = wtg.WorkTypeGroupId
	WHERE (wt.WorkTypeId IN (SELECT WorkTypeId FROM SelectionSearch))
		OR
		(
			wt.WorkTypeName LIKE @Search
			OR wt.WorkTypeDescription LIKE @Search
			OR wtg.WorkTypeGroupName LIKE @Search
		)

END
GO