
-- =============================================
-- Author:		Michael Sanders
-- Create date: 05/01/2017
-- Description:	Return Notification Permissions.
-- =============================================
CREATE PROCEDURE [Notification].[usp_GetNotificationUsers]
	@ShopGuid UNIQUEIDENTIFIER
	,@NotificationType VARCHAR(150)
AS
BEGIN

	SET NOCOUNT ON;

	-- Select Users w/Roles.
	SELECT DISTINCT
		ur.UserGuid
		,u.Email
		,u.EmailConfirmed
		,u.PhoneNumber
		,u.PhoneNumberConfirmed
		,cl.ConnectionGuid
		,cl.ConnectionStartDt
	FROM [Notification].[Types] t
	INNER JOIN [Notification].[TypeRoles] tr
		ON t.TypeGuid = tr.TypeGuid
	INNER JOIN [Access].[UserRoles] ur
		ON tr.RoleGuid = ur.RoleGuid
	INNER JOIN [Access].[Users] u
		ON ur.UserGuid = u.UserGuid
	LEFT JOIN 
	(
		-- Users By Shop.
		SELECT
			us.UserGuid
		FROM [Access].[UserShops] us
		WHERE us.ShopGuid = @ShopGuid

		UNION ALL

		-- Users By Shop Account.
		SELECT
			ua.UserGuid
		FROM [Access].[Shops] s
		INNER JOIN [Access].[UserAccounts] ua
			ON s.AccountGuid = ua.AccountGuid
		WHERE s.ShopGuid = @ShopGuid

		UNION ALL 

		-- Users w/ Show All Shop or Account.
		SELECT
			ur.UserGuid
		FROM [Access].[Roles] r
		INNER JOIN [Access].[UserRoles] ur
			ON r.RoleGuid = ur.RoleGuid
		WHERE r.Name IN ('AccountShowAll', 'ShopShowAll')
	) a
		ON ur.UserGuid = a.UserGuid
	LEFT JOIN [Notification].[UserOptOuts] uoo
		ON u.UserGuid = uoo.UserGuid
			AND t.TypeGuid = uoo.TypeGuid
	LEFT JOIN [Support].[ConnectionLogs] cl
		ON a.UserGuid = cl.UserGuid
			AND cl.ConnectionEndDt IS NULL
	WHERE t.Name = @NotificationType
		AND a.UserGuid IS NOT NULL
		AND uoo.UserGuid IS NULL

END
GO