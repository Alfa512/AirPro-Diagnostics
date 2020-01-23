
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetPaymentsGrid')
	DROP PROCEDURE Billing.usp_GetPaymentsGrid
GO

CREATE PROCEDURE Billing.usp_GetPaymentsGrid
	 @UserGuid UNIQUEIDENTIFIER
	,@ShopGuid VARCHAR(MAX) = NULL
	,@Search VARCHAR(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SET @Search = '%' + ISNULL(@Search, '') + '%';
	DECLARE @SearchFloat FLOAT = TRY_CAST(REPLACE(@Search, '%', '') AS FLOAT);

	DECLARE @UserTimeZone VARCHAR(100) = Common.udf_GetUserTimeZoneId(@UserGuid);

	WITH Access
	AS
	(
		SELECT
			um.ShopGuid
			,s.DisplayName [ShopName]
		FROM Access.vwUserMemberships um
		INNER JOIN Access.Shops s
			ON um.ShopGuid = s.ShopGuid
		WHERE um.UserGuid = @UserGuid
			AND (NULLIF(@ShopGuid, Common.udf_GetEmptyGuid()) IS NULL OR um.ShopGuid = @ShopGuid)
		GROUP BY
			um.ShopGuid
			,s.DisplayName
	),
	Payments
	AS
	(
		SELECT
			p.PaymentID [PaymentId]
			,p.PaymentReceivedFromShopGuid [ShopGuid]
			,a.ShopName
			,pt.PaymentTypeName [PaymentTypeName]
			,p.PaymentAmount
			,p.PaymentReferenceNumber
			,cu.DisplayName [PaymentCreatedBy]
			,p.CreatedDt [PaymentCreatedDateTime]
			,ptr.InvoiceId
			,o.VehicleVIN
		FROM Billing.Payments p
		INNER JOIN Access a
			ON p.PaymentReceivedFromShopGuid = a.ShopGuid
		INNER JOIN Billing.PaymentTypes pt
			ON p.PaymentTypeId = pt.PaymentTypeId
		INNER JOIN Access.Users cu
			ON p.CreatedByUserGuid = cu.UserGuid
		LEFT JOIN Billing.PaymentTransactions ptr
			LEFT JOIN Repair.Orders o
				ON ptr.InvoiceId = o.OrderId
			ON p.PaymentID = ptr.PaymentId
		WHERE p.PaymentVoidInd = 0
	)

	SELECT
		p.PaymentId
		,p.ShopGuid
		,p.ShopName
		,p.PaymentTypeName
		,p.PaymentAmount
		,p.PaymentReferenceNumber
		,p.PaymentCreatedBy
		,CAST(p.PaymentCreatedDateTime AT TIME ZONE @UserTimeZone AS DATETIME) [PaymentCreatedDateTime]
		,p.InvoiceId
	FROM Payments p
	WHERE NULLIF(@Search, '%%') IS NULL
		OR (p.PaymentId = @SearchFloat
			OR p.InvoiceId = @SearchFloat
			OR p.PaymentAmount = @SearchFloat
			OR p.ShopName LIKE @Search
			OR p.PaymentTypeName LIKE @Search
			OR p.PaymentReferenceNumber LIKE @Search
			OR p.PaymentCreatedBy LIKE @Search
			OR p.VehicleVIN LIKE @Search)
	ORDER BY p.PaymentId DESC

END
GO