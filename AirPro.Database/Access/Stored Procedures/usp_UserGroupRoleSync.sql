-- =============================================
-- Author:		Michael Sanders
-- Create date: 3/21/2017
-- Description:	Update User Roles based on Group Roles.
-- =============================================
CREATE PROCEDURE [Access].[usp_UserGroupRoleSync] 
AS
BEGIN

	SET NOCOUNT ON;

	IF (OBJECT_ID('tempdb..#Actions') IS NOT NULL)
		DROP TABLE #Actions

	-- Compare Group Roles to User Roles.
	SELECT
		ISNULL(groupRoles.UserGuid, userRoles.UserGuid) [UserGuid]
		,ISNULL(groupRoles.RoleGuid, userRoles.RoleGuid) [RoleGuid]
		,CASE
			WHEN groupRoles.UserGuid IS NULL THEN 'DEL'
			WHEN userRoles.UserGuid IS NULL THEN 'ADD'
			ELSE 'NONE'
		END [Action]
	INTO #Actions
	FROM
	(
		SELECT
			ug.UserGuid
			,gr.RoleGuid
		FROM Access.UserGroups ug
		INNER JOIN Access.GroupRoles gr
			ON ug.GroupGuid = gr.GroupGuid
	) groupRoles
	FULL JOIN Access.UserRoles userRoles
		ON groupRoles.UserGuid = userRoles.UserGuid
			AND groupRoles.RoleGuid = userRoles.RoleGuid

	-- Add Missing Roles.
	INSERT INTO Access.UserRoles (UserGuid, RoleGuid)
	SELECT DISTINCT a.UserGuid, a.RoleGuid
	FROM #Actions a
	WHERE a.Action = 'ADD'

	-- Del Extra Roles.
	DELETE ur
	FROM Access.UserRoles ur
	INNER JOIN #Actions a
		ON ur.UserGuid = a.UserGuid
			AND ur.RoleGuid = a.RoleGuid
	WHERE a.Action = 'DEL'

	DROP TABLE #Actions

END