
CREATE PROCEDURE Billing.usp_GetOutstandingInvoices
	@ShopGuid UNIQUEIDENTIFIER = NULL
	,@CurrencyId INT = NULL
	,@Search VARCHAR(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;
	SET @Search = '%' + NULLIF(RTRIM(@Search), '') + '%';

	SELECT
		s.ShopGuid
		,s.DisplayName [ShopName]
		,o.ShopReferenceNumber [ShopRO]
		,o.CreatedDt [RepairCreatedDateTime]
		,p.InvoiceId
		,p.InvoiceDt [InvoiceDateTime]
		,p.InvoiceReportCount
		,p.InvoiceTotalAmount + ISNULL(p.InvoiceWorkTypesTotalAmount, 0) [InvoiceTotalAmount]
		,ISNULL(p.PaymentsCount, 0) [PaymentsCount]
		,ISNULL(p.PaymentsTotalAmount, 0) [PaymentsTotalAmount]
		,p.InvoiceTotalAmount + ISNULL(p.InvoiceWorkTypesTotalAmount, 0) - ISNULL(p.PaymentsTotalAmount, 0) [InvoiceBalanceAmount]
		,p.CurrencyId
	FROM Repair.Orders o
	INNER JOIN Access.Shops s
		ON o.ShopGuid = s.ShopGuid
	INNER JOIN
	(
		SELECT
			i.InvoiceId
			,i.CurrencyId
			,i.InvoiceDt
			,i.InvoiceReportCount
			,i.InvoiceTotalAmount
			,p.PaymentsCount
			,p.PaymentsTotalAmount
			,wti.InvoiceWorkTypesTotalAmount
		FROM
		(
			SELECT
				i.InvoiceId
				,i.CurrencyId
				,i.InvoicedDt [InvoiceDt]
				,COUNT(rpt.ReportId) [InvoiceReportCount]
				,SUM(ISNULL(rpt.InvoiceAmount, 0)) [InvoiceTotalAmount]
			FROM Repair.Orders o
			LEFT JOIN Scan.Requests rq
				INNER JOIN Scan.Reports rpt
					ON rq.ReportId = rpt.ReportId
						AND rpt.InvoicedInd = 1
				ON o.OrderId = rq.OrderId
			INNER JOIN Repair.Invoices i
				ON o.OrderId = i.InvoiceId
					AND i.InvoicedInd = 1
			GROUP BY i.InvoiceId, i.CurrencyId, i.InvoicedDt
		) i
		LEFT JOIN
		(
			SELECT i.InvoiceId, SUM(ISNULL(rwt.InvoiceAmount, 0)) [InvoiceWorkTypesTotalAmount]
			FROM Repair.Orders o
			LEFT JOIN Scan.Requests rq ON o.OrderId = rq.OrderId
			INNER JOIN Scan.Reports rpt ON rq.ReportId = rpt.ReportId
			INNER JOIN Repair.Invoices i ON o.OrderId = i.InvoiceId	AND i.InvoicedInd = 1
			INNER JOIN Scan.ReportWorkTypes rwt ON rpt.ReportId = rwt.ReportId AND rwt.InvoicedInd = 1
			GROUP BY i.InvoiceId 
		) wti ON i.InvoiceId = wti.InvoiceId 
		LEFT JOIN
		(
			SELECT
				pt.InvoiceId
				,COUNT(p.PaymentID) [PaymentsCount]
				,SUM(ISNULL(pt.InvoiceAmountApplied, 0) + ISNULL(pt.DiscountAmountApplied, 0)) [PaymentsTotalAmount]
			FROM Billing.Payments p
			INNER JOIN Billing.PaymentTransactions pt
				ON p.PaymentID = pt.PaymentId
					AND pt.PaymentTransactionVoidInd = 0
			WHERE p.PaymentVoidInd = 0
			GROUP BY pt.InvoiceId
		) p
			ON i.InvoiceId = p.InvoiceId
		WHERE p.PaymentsCount IS NULL OR (i.InvoiceTotalAmount + ISNULL(wti.InvoiceWorkTypesTotalAmount, 0)) != p.PaymentsTotalAmount
	) p
		ON o.OrderId = p.InvoiceId
	WHERE s.ShopGuid = ISNULL(@ShopGuid, s.ShopGuid)
		AND (p.CurrencyId = @CurrencyId OR @CurrencyId IS NULL)
		AND (@Search IS NULL
			OR
			(
				p.InvoiceId = TRY_CAST(REPLACE(@Search, '%', '') AS INT)
				OR s.ShopGuid LIKE @Search
				OR s.DisplayName LIKE @Search
				OR o.ShopReferenceNumber LIKE @Search
				OR o.CreatedDt LIKE @Search
				OR p.InvoiceDt LIKE @Search
				OR p.InvoiceReportCount LIKE @Search
				OR (p.InvoiceTotalAmount + ISNULL(p.InvoiceWorkTypesTotalAmount, 0)) LIKE @Search
				OR ISNULL(p.PaymentsCount, 0) LIKE @Search
				OR ISNULL(p.PaymentsTotalAmount, 0) LIKE @Search
				OR ((p.InvoiceTotalAmount + ISNULL(p.InvoiceWorkTypesTotalAmount, 0)) - ISNULL(p.PaymentsTotalAmount, 0)) LIKE @Search
			))
	ORDER BY p.InvoiceDt

END