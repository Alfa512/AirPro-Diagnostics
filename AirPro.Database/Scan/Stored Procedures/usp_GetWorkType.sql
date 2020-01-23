
CREATE PROCEDURE Scan.usp_GetWorkType
	@WorkTypeId INT
AS
BEGIN

	SET NOCOUNT ON;

	/**************************************************
		Step 1: Load Work Type.
	**************************************************/
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
	WHERE wt.WorkTypeId = @WorkTypeId

	/**************************************************
		Step 2: Load Request Type Selection.
	**************************************************/
	SELECT
		rt.RequestTypeId
		,rt.TypeName [RequestTypeName]
		,CASE WHEN wtrt.RequestTypeId IS NOT NULL THEN 1 ELSE 0 END [SelectedInd]
	FROM Scan.RequestTypes rt
	LEFT JOIN
	(
		SELECT RequestTypeId
		FROM Scan.WorkTypeRequestTypes
		WHERE WorkTypeId = @WorkTypeId
	) wtrt
		ON rt.RequestTypeId = wtrt.RequestTypeId
	WHERE rt.ActiveFlag = 1
	ORDER BY rt.SortOrder

	/**************************************************
		Step 3: Load Vehicle Make Selection.
	**************************************************/
	SELECT
		vm.VehicleMakeId
		,vm.VehicleMakeName
		,CASE WHEN wtvm.VehicleMakeId IS NOT NULL THEN 1 ELSE 0 END [SelectedInd]
	FROM Repair.VehicleMakes vm
	LEFT JOIN
	(
		SELECT VehicleMakeId
		FROM Scan.WorkTypeVehicleMakes
		WHERE WorkTypeId = @WorkTypeId
	) wtvm
		ON vm.VehicleMakeId = wtvm.VehicleMakeId
	ORDER BY vm.VehicleMakeName

END