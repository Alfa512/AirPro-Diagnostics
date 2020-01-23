
CREATE PROCEDURE Notification.usp_GetUserMemberships
	@ShopGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
		m.AccessType
		,m.AccountGuid
		,a.Name [AccountName]
		,m.ShopGuid
		,s.Name [ShopName]
		,m.UserGuid
		,u.FirstName
		,u.LastName
		,u.TimeZoneInfoId
		,u.Email
		,u.EmailConfirmed
		,u.PhoneNumber
		,u.PhoneNumberConfirmed
		,u.ShopBillingNotification
		,u.ShopReportNotification
		,u.ShopStatementNotification
		,s.DisableShopBillingNotification
		,s.DisableShopStatementNotification
	FROM
	(
		SELECT DISTINCT
			m.AccountGuid
			,m.ShopGuid
			,u.UserGuid
			,r.Name [AccessType]
		FROM Access.Users u
		INNER JOIN Access.UserRoles ur
			ON u.UserGuid = ur.UserGuid
		INNER JOIN Access.Roles r
			ON ur.RoleGuid = r.RoleGuid
		CROSS APPLY
		(
			SELECT
				a.AccountGuid
				,s.ShopGuid
			FROM Access.Accounts a
			INNER JOIN Access.Shops s
				ON a.AccountGuid = s.AccountGuid
		) m
		WHERE r.Name IN ('AccountShowAll', 'ShopShowAll')

		UNION

		SELECT DISTINCT
			ua.AccountGuid
			,s.ShopGuid
			,ua.UserGuid
			,'AccountMembership' [AccessType]
		FROM Access.UserAccounts ua
		INNER JOIN Access.Shops s
			ON ua.AccountGuid = s.AccountGuid

		UNION

		SELECT DISTINCT
			s.AccountGuid
			,us.ShopGuid
			,us.UserGuid
			,'ShopMembership' [AccessType]
		FROM Access.UserShops us
		INNER JOIN Access.Shops s
			ON us.ShopGuid = s.ShopGuid
	) m
	INNER JOIN Access.Users u
		ON m.UserGuid = u.UserGuid
			AND (u.LockoutEnabled = 0
				OR ISNULL(u.LockoutEndDateUtc, DATEADD(SECOND, -1, GETUTCDATE())) < GETUTCDATE())
	INNER JOIN Access.Accounts a
		ON m.AccountGuid = a.AccountGuid
	INNER JOIN Access.Shops s
		ON m.ShopGuid = s.ShopGuid
	WHERE m.ShopGuid = ISNULL(@ShopGuid, m.ShopGuid)

END