IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME ='usp_GetDecisionsByGridPage')
	DROP PROCEDURE Scan.usp_GetDecisionsByGridPage
GO

CREATE PROCEDURE Scan.usp_GetDecisionsByGridPage
	@UserGuid UNIQUEIDENTIFIER
	,@Search VARCHAR(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZoneId VARCHAR(50);
	SET @UserTimeZoneId = Common.udf_GetUserTimeZoneId(@UserGuid);

	/**************************************************
		Return Decisions.
	**************************************************/
	SELECT
		d.DecisionId
		,d.DecisionText
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
	WHERE NULLIF(@Search, '') IS NULL
		OR (cu.DisplayName LIKE '%' + @Search + '%'
			OR uu.DisplayName LIKE '%' + @Search + '%')
		OR (d.DecisionText LIKE '%' + @Search + '%'
			OR d.DecisionId IN (
						SELECT dvm.DecisionId
						FROM Scan.DecisionVehicleMakes dvm
						INNER JOIN Repair.VehicleMakes vm
							ON dvm.VehicleMakeId = vm.VehicleMakeId
						WHERE vm.VehicleMakeName LIKE '%' + @Search + '%'

						UNION

						SELECT drc.DecisionId
						FROM Scan.DecisionRequestCategories drc
						INNER JOIN Scan.RequestCategories rc
							ON drc.RequestCategoryId = rc.RequestCategoryId
						WHERE rc.RequestCategoryName LIKE '%' + @Search + '%'

						UNION

						SELECT drt.DecisionId
						FROM SCan.DecisionRequestTypes drt
						INNER JOIN Scan.RequestTypes rt
							ON drt.RequestTypeId = rt.RequestTypeId
						WHERE rt.RequestTypeId LIKE '%' + @Search + '%'))

END
GO