
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetTroubleCodeRecommendations')
	DROP PROCEDURE Scan.usp_GetTroubleCodeRecommendations
GO

CREATE PROCEDURE Scan.usp_GetTroubleCodeRecommendations
	@UserGuid UNIQUEIDENTIFIER
	,@RecommendationId INT = NULL
	,@Search VARCHAR(100) = NULL
AS
BEGIN

	-- Set Parameters.
	SET @Search = NULLIF('%' + @Search + '%', '%%');
	SET @RecommendationId = NULLIF(@RecommendationId, 0);

	-- Get Timezone.
	DECLARE @UserTimezone VARCHAR(100) = Common.udf_GetUserTimeZoneId(@UserGuid);

	-- Return Results.
	SELECT
		tcr.TroubleCodeRecommendationId
		,tcr.TroubleCodeRecommendationText
		,tcr.CreatedByUserGuid
		,cu.DisplayName [CreatedByUserDisplay]
		,CAST(tcr.CreatedDt AT TIME ZONE @UserTimezone AS DATETIME) [CreatedDt]
		,tcr.UpdatedByUserGuid
		,uu.DisplayName [UpdatedByUserDisplay]
		,CAST(tcr.UpdatedDt AT TIME ZONE @UserTimezone AS DATETIME) [UpdatedDt]
		,tcr.ActiveInd
		,r.VehicleMakeId
		,vm.VehicleMakeName
		,r.ControllerId
		,c.ControllerName
		,r.TroubleCodeId
		,tc.TroubleCode
		,tc.TroubleCodeDescription
		,r.UsageCount
	FROM Scan.TroubleCodeRecommendations tcr
	LEFT JOIN
	(
		SELECT
			rotc.ControllerId
			,rotc.TroubleCodeId
			,v.VehicleMakeId
			,rtcr.TroubleCodeRecommendationId
			,COUNT(1) [UsageCount]
		FROM Scan.ReportOrderTroubleCodes rotc
		INNER JOIN Scan.ReportTroubleCodeRecommendations rtcr
			ON rotc.ReportOrderTroubleCodeId = rtcr.ReportOrderTroubleCodeId
		INNER JOIN Repair.Orders o
			INNER JOIN Repair.Vehicles v
				ON o.VehicleVIN = v.VehicleVIN
			ON rotc.OrderId = o.OrderId
		WHERE rtcr.TroubleCodeRecommendationId IS NOT NULL
		GROUP BY
			rotc.ControllerId
			,rotc.TroubleCodeId
			,v.VehicleMakeId
			,rtcr.TroubleCodeRecommendationId
	) r ON tcr.TroubleCodeRecommendationId = r.TroubleCodeRecommendationId
	LEFT JOIN Diagnostic.Controllers c
		ON r.ControllerId = c.ControllerId
	LEFT JOIN Diagnostic.TroubleCodes tc
		ON r.TroubleCodeId = tc.TroubleCodeId
	LEFT JOIN Repair.VehicleMakes vm
		ON r.VehicleMakeId = vm.VehicleMakeId
	INNER JOIN Access.Users cu
		ON tcr.CreatedByUserGuid = cu.UserGuid
	LEFT JOIN Access.Users uu
		ON tcr.UpdatedByUserGuid = uu.UserGuid
	WHERE (@RecommendationId IS NULL OR tcr.TroubleCodeRecommendationId = @RecommendationId)
		AND (@Search IS NULL
			OR (tcr.TroubleCodeRecommendationText LIKE @Search
				OR c.ControllerName LIKE @Search
				OR tc.TroubleCode LIKE @Search
				OR tc.TroubleCodeDescription LIKE @Search
				OR vm.VehicleMakeName LIKE @Search))

END
GO

DECLARE @UserGuid UNIQUEIDENTIFIER, @UserTimeZone VARCHAR(100);
SELECT TOP 1 @UserGuid = UserGuid, @UserTimeZone = TimeZoneInfoId
FROM Access.Users WHERE Email IN ('sandersmw@unimatrixdesigns.com', 'dev@umd.tech');

EXEC Scan.usp_GetTroubleCodeRecommendations @UserGuid, 0, 'new'