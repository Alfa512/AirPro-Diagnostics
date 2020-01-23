
CREATE PROCEDURE Scan.usp_GetRequestTypes
	@UserGuid UNIQUEIDENTIFIER
	,@RequestTypeId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZoneId VARCHAR(50) = Common.udf_GetUserTimeZoneId(@UserGuid);

	SELECT 
		rt.RequestTypeId
		,rt.TypeName [RequestTypeName]

		,rt.ActiveFlag
		,rt.BillableFlag

		,rt.SortOrder

		,rt.Instructions

		,rt.InvoiceMemo
		,rt.DefaultPrice

		,rt.CreatedByUserGuid
		,cu.DisplayName [CreatedByUserDisplay]
		,CAST(rt.CreatedDt AT TIME ZONE @UserTimeZoneId AS DATETIME) [CreatedDt]

		,rt.UpdatedByUserGuid
		,uu.DisplayName [UpdatedByUserDisplay]
		,CAST(rt.UpdatedDt AT TIME ZONE @UserTimeZoneId AS DATETIME) [UpdatedDt]

		,rc.RequestCategoryIdList
		,vr.ValidationRuleIdList
	FROM Scan.RequestTypes rt
	INNER JOIN Access.Users cu
		ON rt.CreatedByUserGuid = cu.UserGuid
	LEFT JOIN Access.Users uu
		ON rt.UpdatedByUserGuid = uu.UserGuid
	OUTER APPLY
	(
		SELECT STUFF((
			SELECT ',' + CAST(RequestCategoryId AS VARCHAR(MAX))
			FROM Scan.RequestCategoryTypes
			WHERE RequestTypeId = rt.RequestTypeId
			FOR XML PATH('')
		), 1, 1, '') [RequestCategoryIdList]
	) rc
	OUTER APPLY
	(
		SELECT STUFF((
			SELECT ',' + CAST(ValidationRuleId AS VARCHAR(MAX))
			FROM Scan.RequestTypeValidationRules
			WHERE RequestTypeId = rt.RequestTypeId
			FOR XML PATH('')
		), 1, 1, '') [ValidationRuleIdList]
	) vr
	WHERE NULLIF(@RequestTypeId, 0) IS NULL
		OR rt.RequestTypeId = @RequestTypeId
	ORDER BY rt.SortOrder

END