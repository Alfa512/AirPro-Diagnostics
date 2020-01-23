
CREATE VIEW [Billing].[vwInvoiceBalances]
AS

	WITH Invoices
	AS
	(
		SELECT
			i.InvoiceID
			,o.ShopGuid
			,s.Name [ShopName]
			,o.ShopReferenceNumber
			,o.VehicleVIN
			,o.OrderId
			,COUNT(rpt.ReportId) [InvoicedReportCount]
			,o.CreatedDt [RepairCreatedDate]
			,i.InvoicedDt [InvoicedDate]
			,u.LastName + ', ' + u.FirstName [InvoicedBy]
			,ISNULL(SUM(rpt.InvoiceAmount), 0) [InvoicedTotalAmount]
		FROM Repair.Orders o
		INNER JOIN Access.Shops s
			ON o.ShopGuid = s.ShopGuid
		INNER JOIN Repair.Invoices i
			INNER JOIN Access.Users u
				ON i.InvoicedByUserGuid = u.UserGuid
			ON o.OrderId = i.InvoiceId
				AND i.InvoicedInd = 1
		LEFT JOIN Scan.Requests rq
			ON o.OrderId = rq.OrderId
		LEFT JOIN Scan.Reports rpt
			ON rq.ReportID = rpt.ReportID
				AND rpt.InvoicedInd = 1
		GROUP BY
			i.InvoiceID
			,o.ShopGuid
			,o.ShopReferenceNumber
			,o.VehicleVIN
			,o.OrderId
			,s.Name
			,i.InvoicedDt
			,o.CreatedDt
			,u.LastName + ', ' + u.FirstName
	),
	Payments
	AS
	(
		SELECT
			t.InvoiceID
			,COUNT(t.PaymentTransactionID) [PaymentsApplied]
			,SUM(ISNULL(t.InvoiceAmountApplied, 0)) [PaymentsAppliedTotal]
			,SUM(ISNULL(t.DiscountAmountApplied, 0)) [DiscountsAppliedTotal]
		FROM Billing.PaymentTransactions t
		INNER JOIN Billing.Payments p
			ON t.PaymentID = p.PaymentID
				AND p.PaymentVoidInd = 0
		WHERE t.PaymentTransactionVoidInd = 0
		GROUP BY t.InvoiceID
	)

	SELECT
		i.ShopGuid
		,i.ShopName
		,i.ShopReferenceNumber [ShopRO]
		,i.VehicleVIN
		,i.OrderId [RepairOrderId]
		,CAST(i.RepairCreatedDate AS DATETIME) [RepairCreatedDateTime]
		,i.InvoiceId
		,i.InvoicedReportCount
		,i.InvoicedBy
		,CAST(i.InvoicedDate AS DATETIME) [InvoicedDateTime]
		,i.InvoicedTotalAmount
		,ISNULL(p.PaymentsApplied, 0) [PaymentsCount]
		,ISNULL(p.PaymentsAppliedTotal, 0) [PaymentsTotalAmount]
		,ISNULL(p.DiscountsAppliedTotal, 0) [DiscountsTotalAmount]
		,(i.InvoicedTotalAmount - ISNULL(p.PaymentsAppliedTotal, 0) - ISNULL(p.DiscountsAppliedTotal, 0)) [InvoiceBalanceAmount]
	FROM Invoices i
	LEFT JOIN Payments p
		ON i.InvoiceID = p.InvoiceID