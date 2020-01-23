CREATE PROCEDURE Reporting.usp_GetReportTemplates
	@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		t.TemplateId
		,t.TemplateName
		,t.TemplateDescription
		,t.TemplateSortOrder
	FROM Reporting.ReportTemplates t
	WHERE t.ActiveInd = 1
		AND NULLIF(t.AccessRoles, '') IS NULL
			OR EXISTS
			(
				SELECT 1
				FROM Common.udf_CommaListToTable(t.AccessRoles) ar
				INNER JOIN Access.Roles r
					INNER JOIN Access.UserRoles ur
						ON r.RoleGuid = ur.RoleGuid
							AND ur.UserGuid = @UserGuid
					ON ar.Val = r.Name
			)
	ORDER BY
		t.TemplateSortOrder
		,t.TemplateName

END