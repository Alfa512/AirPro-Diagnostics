
CREATE VIEW [Access].[vwUserMemberships]
AS

	SELECT
		a.AccountGuid
		,a.NAME [AccountName]
		,s.ShopGuid
		,s.NAME [ShopName]
		,u.UserGuid
		,u.Email
		,u.LastName
		,u.FirstName
		,m.MembershipType
	FROM
	(
		SELECT
			s.AccountGuid
			,s.ShopGuid
			,us.UserGuid
			,'Shop' [MembershipType]
		FROM ACCESS.UserShops us
		INNER JOIN ACCESS.Shops s
			ON us.ShopGuid = s.ShopGuid

		UNION 

		SELECT
			ua.AccountGuid
			,s.ShopGuid
			,ua.UserGuid
			,'Account' [MembershipType]
		FROM ACCESS.UserAccounts ua
		INNER JOIN ACCESS.Shops s
			ON ua.AccountGuid = s.AccountGuid

		UNION 

		SELECT
			s.AccountGuid
			,s.ShopGuid
			,ur.UserGuid
			,'ShowAllRole' [MembershipType]
		FROM
		(
			SELECT
				s.ShopGuid
				,ur.UserGuid
			FROM Access.UserRoles ur, (SELECT ShopGuid FROM Access.Shops) s
			WHERE ur.RoleGuid IN ('EEDE84E3-6F34-48AA-B729-243D9A62A7D9', '982C936C-149D-475D-AC0A-4DE4D7BB18D5')
		) ur
		INNER JOIN ACCESS.Shops s
			ON ur.ShopGuid = s.ShopGuid
	) m
	INNER JOIN ACCESS.Users u
		ON m.UserGuid = u.UserGuid
			AND (u.LockoutEnabled = 0
				OR (u.LockoutEnabled = 1
					AND ISNULL(u.LockoutEndDateUtc, GETUTCDATE() - 1) < GETUTCDATE()))
	INNER JOIN ACCESS.Shops s
		ON m.ShopGuid = s.ShopGuid
	INNER JOIN ACCESS.Accounts a
		ON m.AccountGuid = a.AccountGuid

GO