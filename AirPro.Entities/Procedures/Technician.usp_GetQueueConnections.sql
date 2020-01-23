
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_GetQueueConnections')
	DROP PROCEDURE Technician.usp_GetQueueConnections
GO

CREATE PROCEDURE Technician.usp_GetQueueConnections
	@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZone VARCHAR(100)
	SELECT @UserTimeZone = u.TimeZoneInfoId
	FROM Access.Users u
	WHERE u.UserGuid = @UserGuid

	SELECT
		u.UserGuid
		,u.Email [UserEmail]
		,u.LastName + ', ' + u.FirstName [UserFullName]
		,p.DisplayName [ProfileDisplayName]
		,c.ConnectionGuid
		,c.ConnectionStartDt AT TIME ZONE @UserTimeZone [ConnectionStartDt]
	FROM Support.Connections c
	INNER JOIN Access.Users u
		ON c.UserGuid = u.UserGuid
	INNER JOIN Technician.Profiles p
		ON c.UserGuid = p.UserGuid
			AND p.ActiveInd = 1
	WHERE c.PageUrl LIKE '/Request/Queue%'
	GROUP BY
		u.UserGuid
		,u.Email
		,u.LastName + ', ' + u.FirstName
		,p.DisplayName
		,c.ConnectionGuid
		,c.ConnectionStartDt
	ORDER BY
		u.LastName + ', ' + u.FirstName
		,c.ConnectionStartDt DESC

END
GO