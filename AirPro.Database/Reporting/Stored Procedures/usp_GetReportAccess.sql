
CREATE PROCEDURE Reporting.usp_GetReportAccess
	@UserGuid UNIQUEIDENTIFIER
	,@RepairId INT = NULL
	,@RequestId INT = NULL
	,@PaymentId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @ShopGuid UNIQUEIDENTIFIER;
	SELECT @ShopGuid = o.ShopGuid
	FROM Repair.Orders o
	LEFT JOIN Scan.Requests r
		ON r.OrderId = o.OrderId
	LEFT JOIN Billing.PaymentTransactions pt
		INNER JOIN Billing.Payments p
			ON p.PaymentID = pt.PaymentId
		ON o.OrderId = pt.InvoiceId
	WHERE o.OrderId = @RepairId
		OR r.RequestId = @RequestId
		OR p.PaymentID = @PaymentId

	DECLARE @Result BIT = 0;
	SELECT @Result = 1
	FROM Access.vwUserMemberships um
	WHERE um.UserGuid = @UserGuid
		AND um.ShopGuid = @ShopGuid

	SELECT @Result [Result]

END