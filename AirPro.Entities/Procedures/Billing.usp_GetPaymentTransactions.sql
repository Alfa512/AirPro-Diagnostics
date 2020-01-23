
CREATE PROCEDURE [Billing].[usp_GetPaymentTransactions]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		p.PaymentReceivedFromShopGuid [ShopGuid]
		,s.DisplayName [ShopName]
		,o.ShopReferenceNumber [ShopRO]
		,t.InvoiceId
		,t.InvoiceAmountApplied [InvoiceAppliedAmount]
		,p.PaymentId
		,t.PaymentTransactionId
		,pt.PaymentTypeName
		,p.PaymentAmount
		,p.PaymentReferenceNumber
		,u.LastName + ', ' + u.FirstName [PaymentCreatedBy]
		,p.CreatedDt [PaymentCreatedDateTime]
	FROM Billing.Payments p
	INNER JOIN Access.Shops s
		ON p.PaymentReceivedFromShopGuid = s.ShopGuid
	INNER JOIN Access.Users u
		ON p.CreatedByUserGuid = u.UserGuid
	INNER JOIN Billing.PaymentTypes pt
		ON p.PaymentTypeID = pt.PaymentTypeID
	INNER JOIN Billing.PaymentTransactions t
		INNER JOIN Repair.Orders o
			ON o.OrderId = t.InvoiceId
		ON p.PaymentID = t.PaymentID
			AND t.PaymentTransactionVoidInd = 0
	WHERE p.PaymentVoidInd = 0
	ORDER BY p.CreatedDt DESC

END
GO