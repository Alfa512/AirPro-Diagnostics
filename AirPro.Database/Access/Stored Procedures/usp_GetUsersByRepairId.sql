
CREATE PROCEDURE Access.usp_GetUsersByRepairId
	@RepairId INT,
	@ShopGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	SELECT @ShopGuid = ISNULL(@ShopGuid, ShopGuid)
	FROM Repair.Orders o
	WHERE o.OrderId = @RepairId;

	WITH Users
	AS
	(
		SELECT
			um.UserGuid [Id]
			,um.LastName
			,um.FirstName
		FROM Access.vwUserMemberships um
		WHERE @ShopGuid IS NOT NULL
			AND um.ShopGuid = @ShopGuid
			AND um.MembershipType IN ('Account', 'Shop')

		UNION 

		SELECT
			sc.ShopContactGuid [Id]
			,sc.LastName
			,sc.FirstName
		FROM Access.ShopContacts sc
		WHERE DeletedInd = 0
			AND @ShopGuid IS NOT NULL
			AND sc.ShopGuid = @ShopGuid
	)

	SELECT
		u.Id
		,u.LastName
		,u.FirstName
	FROM Users u
	ORDER BY u.LastName

END