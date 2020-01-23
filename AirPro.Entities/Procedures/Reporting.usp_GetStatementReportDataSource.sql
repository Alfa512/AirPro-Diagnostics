DROP PROCEDURE IF EXISTS Reporting.usp_GetStatementReportDataSource;
GO

CREATE PROCEDURE Reporting.usp_GetStatementReportDataSource
	@PaymentId INT
	,@Offset CHAR(10) = '-00:00'
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		p.PaymentID [PaymentId]
		,p.PaymentDt
		,pt.PaymentTypeName [PaymentType]
		,p.PaymentReferenceNumber [PaymentRefNumber]
		,ISNULL(c.Name, 'USD') [PaymentCurrency]
		,p.PaymentAmount [PaymentTotalAmount]
		,CAST(ISNULL(p.DiscountPercentage, 0) AS DECIMAL(5,2)) / 100 [PaymentDiscountAmount]
		,p.PaymentMemo
		,s.DisplayName [ShopName]
		,s.Phone [ShopPhone]
		,s.Fax [ShopFax]
		,s.Address1 [ShopAddress1]
		,s.Address2 [ShopAddress2]
		,s.City [ShopCity]
		,st.Abbreviation [ShopState]
		,s.Zip [ShopZip]
	FROM Billing.Payments p
	LEFT JOIN Billing.Currencies c
		ON p.CurrencyId = c.CurrencyId
	INNER JOIN Billing.PaymentTypes pt
		ON p.PaymentTypeId = pt.PaymentTypeId
	INNER JOIN Access.Shops s
		INNER JOIN Common.States st
			ON s.StateId = st.StateId
		ON p.PaymentReceivedFromShopGuid = s.ShopGuid
	WHERE p.PaymentID = @PaymentId
		AND p.PaymentVoidInd = 0

	SELECT
		pt.InvoiceId
		,o.ShopReferenceNumber [ShopRONumber]
		,o.VehicleVIN
		,Common.udf_GetLocalDateTime(invoice.CreatedDt, @Offset) [InvoiceCreatedDt]
		,ISNULL(i.InvoiceAmount, 0) + ISNULL(wti.InvoiceWorkTypesTotalAmount, 0) [InvoiceAmount]
		,pt.DiscountAmountApplied [DiscountAmount]
		,pt.InvoiceAmountApplied [PaidAmount]
		,Common.udf_GetDisplayName(ocu.LastName, ocu.FirstName) [RepairCreatedBy]
	FROM Billing.PaymentTransactions pt
	INNER JOIN Repair.Invoices invoice ON pt.InvoiceId = invoice.InvoiceId
	INNER JOIN Repair.Orders o
		INNER JOIN Access.Users ocu
			ON o.CreatedByUserGuid = ocu.UserGuid
		ON pt.InvoiceId = o.OrderId
	OUTER APPLY
	(
		SELECT
			rq.OrderId [InvoiceId]
			,SUM(rpt.InvoiceAmount) [InvoiceAmount]
		FROM Scan.Requests rq
		INNER JOIN Scan.Reports rpt
			ON rq.ReportId = rpt.ReportId
				AND rpt.InvoicedInd = 1
		WHERE rq.OrderId = o.OrderId
		GROUP BY rq.OrderId
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
	WHERE pt.PaymentId = @PaymentId
		AND pt.PaymentTransactionVoidInd = 0

END
GO