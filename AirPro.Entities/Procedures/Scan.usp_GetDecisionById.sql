IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME ='usp_GetDecisionById')
	DROP PROCEDURE Scan.usp_GetDecisionById
GO

CREATE PROCEDURE Scan.usp_GetDecisionById
	@UserGuid UNIQUEIDENTIFIER
	,@DecisionId INT
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZoneId VARCHAR(50);
	SET @UserTimeZoneId = Common.udf_GetUserTimeZoneId(@UserGuid);

	/**************************************************
		Return Decision.
	**************************************************/
	SELECT
		d.DecisionId
		,d.DecisionText
		,d.DefaultTextSeverity
		,d.ActiveInd
		,d.CreatedByUserGuid
		,cu.DisplayName [CreatedByUserDisplay]
		,CAST(d.CreatedDt AT TIME ZONE @UserTimeZoneId AS DATETIME) [CreatedDt]
		,d.UpdatedByUserGuid
		,uu.DisplayName [UpdatedByUserDisplay]
		,CAST(d.UpdatedDt AT TIME ZONE @UserTimeZoneId AS DATETIME) [UpdatedDt]
	FROM Scan.Decisions d
	INNER JOIN Access.Users cu
		ON d.CreatedByUserGuid = cu.UserGuid
	LEFT JOIN Access.Users uu
		ON d.UpdatedByUserGuid = uu.UserGuid
	WHERE d.DecisionId = @DecisionId

	/**************************************************
		Return Vehicle Makes for Decision.
	**************************************************/
	SELECT
		vm.VehicleMakeId
		,vm.VehicleMakeName
		,CAST(CASE WHEN dvm.VehicleMakeId IS NOT NULL THEN 1 ELSE 0 END AS BIT) [SelectedInd]
		,CAST(ISNULL(dvm.PreSelectedInd, 0) AS BIT) [PreSelectedInd]
	FROM Repair.VehicleMakes vm
	LEFT JOIN Scan.DecisionVehicleMakes dvm
		ON dvm.DecisionId = @DecisionId
			AND vm.VehicleMakeId = dvm.VehicleMakeId
	ORDER BY vm.VehicleMakeName

	/**************************************************
		Return Request Types for Decision.
	**************************************************/
	SELECT
		rt.RequestTypeId
		,rt.TypeName [RequestTypeName]
		,CAST(CASE WHEN drt.RequestTypeId IS NOT NULL THEN 1 ELSE 0 END AS BIT) [SelectedInd]
		,CAST(ISNULL(drt.PreSelectedInd, 0) AS BIT) [PreSelectedInd]
	FROM Scan.RequestTypes rt
	LEFT JOIN Scan.DecisionRequestTypes drt
		ON drt.DecisionId = @DecisionId
			AND rt.RequestTypeId = drt.RequestTypeId
	ORDER BY rt.SortOrder

	/**************************************************
		Return Request Categories for Decision.
	**************************************************/
	SELECT
		rc.RequestCategoryId
		,rc.RequestCategoryName
		,CAST(CASE WHEN drc.RequestCategoryId IS NOT NULL THEN 1 ELSE 0 END AS BIT) [SelectedInd]
		,CAST(ISNULL(drc.PreSelectedInd, 0) AS BIT) [PreSelectedInd]
	FROM Scan.RequestCategories rc
	LEFT JOIN Scan.DecisionRequestCategories drc
		ON drc.DecisionId = @DecisionId
			AND rc.RequestCategoryId = drc.RequestCategoryId
	WHERE rc.IsActive = 1
	ORDER BY rc.[Order]

END
GO