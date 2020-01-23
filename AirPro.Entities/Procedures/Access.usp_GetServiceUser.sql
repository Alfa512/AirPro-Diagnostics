
CREATE PROCEDURE [Access].[usp_GetServiceUser]
	@UserName NVARCHAR(256)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @ServiceGuid UNIQUEIDENTIFIER = CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)

	SELECT
		u.UserGuid
		,u.UserName
		,u.TimeZoneInfoId
		,r.UserRoleGuids
		,CAST(CASE WHEN u.LockoutEnabled = 1 AND u.LockoutEndDateUtc > GETUTCDATE() AND u.UserGuid != @ServiceGuid THEN 1 ELSE 0 END AS BIT) [UserLockedOut]
	FROM Access.Users u
	CROSS APPLY
	(
		SELECT STUFF((
			SELECT ',' + CAST(RoleGuid AS NVARCHAR(36))
			FROM Access.UserRoles ur
			WHERE ur.UserGuid = u.UserGuid
			FOR XML PATH('')
		), 1, 1, '') [UserRoleGuids]
	) r
	WHERE u.UserName = @UserName

END
GO