
CREATE PROCEDURE Reporting.usp_GetAccountShopRepsReport
	@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @TimeZone NVARCHAR(100) = Common.udf_GetUserTimeZoneId(ISNULL(@UserGuid, Common.udf_GetEmptyGuid()));

	/****************************************
		Load Memberships and Requests.
	****************************************/
	WITH Memberships
	AS
	(
		SELECT
			ShopGuid
			,AccountGuid
		FROM Access.vwUserMemberships
		WHERE UserGuid = @UserGuid
		GROUP BY
			ShopGuid
			,AccountGuid
	)
	,Requests
	AS
	(
		SELECT
			o.ShopGuid
			,r.CreatedDt
			,DENSE_RANK() OVER (PARTITION BY o.ShopGuid ORDER BY r.CreatedDt DESC) [CreatedRank]
		FROM Repair.Orders o
		INNER JOIN Scan.Requests r
			ON o.OrderId = r.OrderId
	)

	/**************************************************
		Return Account And Shop with associated Reps.
	**************************************************/
	SELECT 
		a.Name [AccountName]
		,a.Phone [AccountPhone]
		,ast.Abbreviation [AccountState]
		,ae.DisplayName [AccountRepName]
		,s.Name [ShopName]
		,s.Phone [ShopPhone]
		,sst.Abbreviation [ShopState]
		,se.DisplayName [ShopRepName]
		,CONVERT(CHAR(10), CAST(r.CreatedDt AT TIME ZONE @TimeZone AS DATETIME), 101) [LastScanRequested]
	FROM Memberships m
	INNER JOIN Access.Shops s
		INNER JOIN Common.States sst
			ON s.StateId = sst.StateId
		LEFT JOIN Access.Users se
			ON s.EmployeeGuid = se.UserGuid
		ON m.ShopGuid = s.ShopGuid
			AND s.ActiveInd = 1
	INNER JOIN Access.Accounts a
		INNER JOIN Common.States ast
			ON a.StateId = ast.StateId
		LEFT JOIN Access.Users ae
			ON a.EmployeeGuid = ae.UserGuid
		ON s.AccountGuid = a.AccountGuid
			AND a.ActiveInd = 1
	LEFT JOIN Requests r
		ON s.ShopGuid = r.ShopGuid
			AND r.CreatedRank = 1

END