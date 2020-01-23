
-- =============================================
-- Author:		Michael Sanders
-- Create date: 04/30/2017
-- Description:	Display Current User Rights.
-- =============================================
CREATE PROCEDURE [Access].[usp_ViewUserGroupRoles]
	@UserEmail VARCHAR(250) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		u.Email
		,g.Name [GroupName]
	FROM [Access].[Users] u
	INNER JOIN [Access].[UserGroups] ug
		ON u.UserGuid = ug.UserGuid
	INNER JOIN [Access].[Groups] g
		ON ug.GroupGuid = g.GroupGuid
	WHERE u.Email = ISNULL(@UserEmail, u.Email)

	SELECT
		u.Email
		,g.Name [GroupName]
		,r.Name [RoleName]
	FROM [Access].[Users] u
	INNER JOIN [Access].[UserGroups] ug
		ON u.UserGuid = ug.UserGuid
	INNER JOIN [Access].[Groups] g
		ON ug.GroupGuid = g.GroupGuid
	INNER JOIN [Access].[GroupRoles] gr
		ON g.GroupGuid = gr.GroupGuid
	INNER JOIN [Access].[Roles] r
		ON gr.RoleGuid = r.RoleGuid
	WHERE u.Email = ISNULL(@UserEmail, u.Email)

	SELECT
		u.Email
		,r.Name [RoleName]
	FROM [Access].[Users] u
	INNER JOIN [Access].[UserRoles] ur
		ON u.UserGuid = ur.UserGuid
	INNER JOIN [Access].[Roles] r
		ON ur.RoleGuid = r.RoleGuid
	WHERE u.Email = ISNULL(@UserEmail, u.Email)

END