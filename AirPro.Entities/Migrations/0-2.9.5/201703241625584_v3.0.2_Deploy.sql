
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Access' AND ROUTINE_NAME = 'usp_NotificationUsers')
	DROP PROCEDURE Access.usp_NotificationUsers

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Michael Sanders
-- Create date: 3/24/2017
-- Description:	Return Notification Permissions.
-- =============================================
CREATE PROCEDURE Access.usp_NotificationUsers
	@NotificationType VARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

	WITH Asigned
	AS
	(
		SELECT
			a.AccountGuid
			,NULL [ShopGuid]
			,ua.UserGuid
		FROM Access.Accounts a
		INNER JOIN Access.UserAccounts ua
			ON a.AccountGuid = ua.AccountGuid

		UNION ALL

		SELECT 
			s.AccountGuid
			,s.ShopGuid
			,us.UserGuid
		FROM Access.Shops s
		INNER JOIN Access.UserShops us
			ON s.ShopGuid = us.ShopGuid
	)
	,AllowAll
	AS
	(
		SELECT
			a.AccountGuid
			,NULL [ShopGuid]
			,u.UserGuid
		FROM Access.Accounts a
		OUTER APPLY
		(
			SELECT DISTINCT
				u.UserGuid
			FROM Access.Users u
			INNER JOIN Access.UserRoles ur
				ON u.UserGuid = ur.UserGuid
			INNER JOIN Access.Roles r
				ON ur.RoleGuid = r.RoleGuid
					AND (r.Name = 'AccountShowAll')
		) u

		UNION ALL

		SELECT
			s.AccountGuid
			,s.ShopGuid
			,u.UserGuid
		FROM Access.Shops s
		OUTER APPLY
		(
			SELECT DISTINCT
				u.UserGuid
			FROM Access.Users u
			INNER JOIN Access.UserRoles ur
				ON u.UserGuid = ur.UserGuid
			INNER JOIN Access.Roles r
				ON ur.RoleGuid = r.RoleGuid
					AND (r.Name = 'ShopShowAll')
		) u
	)
	,Perms
	AS
	(
		SELECT
			assign.AccountGuid
			,assign.ShopGuid
			,assign.UserGuid
			,CAST(1 AS BIT) [Assigned]
		FROM Asigned assign

		UNION ALL

		SELECT
			allow.AccountGuid
			,allow.ShopGuid
			,allow.UserGuid
			,CAST(0 AS BIT) [Assigned]
		FROM AllowAll allow
		LEFT JOIN Asigned assign
			ON allow.AccountGuid = assign.AccountGuid
				AND (allow.ShopGuid = assign.ShopGuid OR allow.ShopGuid IS NULL AND assign.ShopGuid IS NULL)
				AND allow.UserGuid = assign.UserGuid
		WHERE assign.UserGuid IS NULL
	)

	SELECT
		p.AccountGuid
		,p.ShopGuid
		,p.UserGuid
		,p.Assigned
		,u.ShopBillingNotification
		,u.ShopReportNotification
		,u.EmailConfirmed
		,u.Email
		,u.PhoneNumberConfirmed
		,u.PhoneNumber
		,u.ConnectionId
	FROM Perms p
	INNER JOIN Access.Users u
		ON p.UserGuid = u.UserGuid
			AND (u.LockoutEnabled = 0 OR ISNULL(u.LockoutEndDateUtc, GETUTCDATE() -1) < GETUTCDATE())
	INNER JOIN Access.UserRoles ur
		ON u.UserGuid = ur.UserGuid
	INNER JOIN Notification.TypeRoles tr
		ON ur.RoleGuid = tr.RoleGuid
	INNER JOIN Notification.Types t
		ON tr.TypeGuid = t.TypeGuid
			AND t.Name = @NotificationType

END
GO