
CREATE VIEW [Billing].[vwPaymentTransactions]
AS

	SELECT
		p.PaymentReceivedFromShopGuid [ShopGuid]
		,s.Name [ShopName]
		,o.ShopReferenceNumber [ShopRO]
		,o.VehicleVIN
		,o.OrderId [RepairOrderId]
		,t.InvoiceId
		,t.InvoiceAmountApplied [InvoiceAppliedAmount]
		,p.PaymentId
		,t.PaymentTransactionId
		,p.PaymentTypeId
		,pt.PaymentTypeName
		,p.PaymentAmount
		,p.PaymentReferenceNumber
		,u.LastName + ', ' + u.FirstName [PaymentCreatedBy]
		,CAST(p.CreatedDt AS DATETIME) [PaymentCreatedDateTime]
	FROM Billing.Payments p
	INNER JOIN Access.Shops s
		ON p.PaymentReceivedFromShopGuid = s.ShopGuid
	INNER JOIN Access.Users u
		ON p.CreatedByUserGuid = u.UserGuid
	INNER JOIN Billing.PaymentTypes pt
		ON p.PaymentTypeID = pt.PaymentTypeID
	LEFT JOIN Billing.PaymentTransactions t
		INNER JOIN Repair.Orders o
			ON o.OrderId = t.InvoiceId
		ON p.PaymentID = t.PaymentID
			AND t.PaymentTransactionVoidInd = 0
	WHERE p.PaymentVoidInd = 0

GO