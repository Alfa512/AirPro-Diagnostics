IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Billing' AND TABLE_NAME = 'vwPaymentTransactions')
	DROP VIEW Billing.vwPaymentTransactions
GO

CREATE VIEW Billing.vwPaymentTransactions
AS

	SELECT
		p.PaymentID
		,t.PaymentTransactionID
		,p.PaymentReceivedFromShopID
		,s.Name [PaymentReceivedFromShopName]
		,p.PaymentTypeID
		,pt.PaymentTypeName
		,p.PaymentAmount
		,p.PaymentReferenceNumber
		,t.InvoiceID
		,t.InvoiceAmountApplied
		,ISNULL(u.LastName, '') + ', ' + ISNULL(u.FirstName, '') [PaymentCreatedBy]
		,p.CreatedDt [PaymentCreatedDateTime]
	FROM Billing.Payments p
	INNER JOIN Access.Shops s
		ON p.PaymentReceivedFromShopID = s.ShopId
	INNER JOIN Access.Users u
		ON p.CreatedByUserGuid = u.UserGuid
	INNER JOIN Billing.PaymentTypes pt
		ON p.PaymentTypeID = pt.PaymentTypeID
	LEFT JOIN Billing.PaymentTransactions t
		ON p.PaymentID = t.PaymentID
			AND t.PaymentTransactionVoidInd = 0
	WHERE p.PaymentVoidInd = 0

GO


IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Billing' AND TABLE_NAME = 'vwInvoiceByShop')
	DROP VIEW Billing.vwInvoiceByShop
GO

CREATE VIEW Billing.vwInvoiceByShop
AS
	SELECT
		s.ShopId [ShopID]
		,s.Name [ShopName]
		,s.Phone [ShopPhone]
		,o.ShopReferenceNumber [ShopRONumber]
		,CASE WHEN o.InsuranceCompanyID = 1 THEN o.InsuranceCompanyOther ELSE ins.InsuranceCompanyName END [InsuranceCo]
		,o.VehicleVIN [VehicleVIN]
		,v.Year [VehicleYear]
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,i.InvoiceId [InvoiceNumber]
		,CAST(o.CreatedDt AS SMALLDATETIME) [RepairCreatedDt]
		,CAST(i.CreatedDt AS SMALLDATETIME) [InvoicedDt]
		,DATEDIFF(MINUTE, o.CreatedDt, i.CreatedDt) [CycleTimeTotalMinutes]
		,DATEDIFF(HOUR, o.CreatedDt, i.CreatedDt) [CycleTimeTotalHours]
		,DATEDIFF(DAY, o.CreatedDt, i.CreatedDt) [CycleTimeTotalDays]
		,DATEDIFF(WEEK, o.CreatedDt, i.CreatedDt) [CycleTimeTotalWeeks]
		,r.RequestID
		,rt.TypeName [RequestType]
		,rpt.InvoiceAmount [InvoicedAmount]
	FROM Access.Shops s
	INNER JOIN Repair.Orders o
		LEFT JOIN Repair.InsuranceCompanies ins
			ON o.InsuranceCompanyID = ins.InsuranceCompanyID
		ON s.ShopId = o.ShopId
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Repair.Invoices i
		ON o.OrderId = i.InvoiceID
			AND i.InvoicedInd = 1
	INNER JOIN Scan.Requests r
		INNER JOIN Scan.RequestTypes rt
			ON r.RequestTypeId = rt.RequestTypeID
		ON o.OrderId = r.OrderId
	INNER JOIN Scan.Reports rpt
		ON r.ReportID = rpt.ReportID
			AND rpt.InvoicedInd = 1

GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'vwRequestsByShop')
	DROP VIEW Scan.vwRequestsByShop
GO

CREATE VIEW Scan.vwRequestsByShop
AS
	SELECT
		[RequestCreated]
		,[ShopName]
		,[Quick Scan]
		,[Diagnostic Scan]
		,[Completion Scan]
		,[Follow Up Scan]
		,[Inspection Scan]
	FROM
	(
		SELECT 
			CAST(r.CreatedDt AS DATE) [RequestCreated]
			,s.Name [ShopName]
			,r.RequestTypeId [TypeOfScan]
			,rt.TypeName [ScanType]
		FROM [Access].[Shops] s
		INNER JOIN [Repair].[Orders] o
			INNER JOIN [Scan].[Requests] r
				INNER JOIN [Scan].[RequestTypes] rt
					ON r.RequestTypeId = rt.RequestTypeID
				ON o.OrderId = r.OrderId
			ON s.ShopId = o.ShopId
	) p
	PIVOT
	(
		COUNT(TypeOfScan)
		FOR ScanType IN ([Quick Scan], [Diagnostic Scan], [Completion Scan], [Follow Up Scan], [Inspection Scan])
	) AS pvt

GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Billing' AND TABLE_NAME = 'vwInvoiceBalances')
	DROP VIEW Billing.vwInvoiceBalances
GO

CREATE VIEW Billing.vwInvoiceBalances
AS

	WITH Invoices
	AS
	(
		SELECT
			i.InvoiceID
			,o.ShopId [ShopID]
			,s.Name [ShopName]
			,COUNT(rpt.ReportID) [ReportCount]
			,o.CreatedDt [RepairCreatedDate]
			,i.InvoicedDt [InvoicedDate]
			,ISNULL(SUM(rpt.InvoiceAmount), 0) [InvoicedTotal]
		FROM Repair.Orders o
		INNER JOIN Access.Shops s
			ON o.ShopId = s.ShopId
		INNER JOIN Repair.Invoices i
			ON o.OrderId = i.InvoiceId
				AND i.InvoicedInd = 1
		LEFT JOIN Scan.Requests rq
			ON o.OrderId = rq.OrderId
		LEFT JOIN Scan.Reports rpt
			ON rq.ReportID = rpt.ReportID
				AND rpt.InvoicedInd = 1
		GROUP BY
			i.InvoiceID
			,o.ShopId
			,s.Name
			,i.InvoicedDt
			,o.CreatedDt
	),
	Payments
	AS
	(
		SELECT
			t.InvoiceID
			,COUNT(t.PaymentTransactionID) [PaymentsApplied]
			,SUM(ISNULL(t.InvoiceAmountApplied, 0)) [PaymentsAppliedTotal]
		FROM Billing.PaymentTransactions t
		INNER JOIN Billing.Payments p
			ON t.PaymentID = p.PaymentID
				AND p.PaymentVoidInd = 0
		WHERE t.PaymentTransactionVoidInd = 0
		GROUP BY t.InvoiceID
	)

	SELECT
		i.InvoiceID
		,i.ShopID
		,i.ShopName
		,i.ReportCount
		,i.RepairCreatedDate
		,i.InvoicedDate
		,i.InvoicedTotal
		,ISNULL(p.PaymentsApplied, 0) [PaymentsApplied]
		,ISNULL(p.PaymentsAppliedTotal, 0) [PaymentsAppliedTotal]
		,(i.InvoicedTotal - ISNULL(p.PaymentsAppliedTotal, 0)) [InvoiceBalance]
	FROM Invoices i
	LEFT JOIN Payments p
		ON i.InvoiceID = p.InvoiceID

GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetOutstandingInvoices')
	DROP PROCEDURE Billing.usp_GetOutstandingInvoices
GO

CREATE PROCEDURE [Billing].[usp_GetOutstandingInvoices]
	@ShopID INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		i.InvoiceID
		,i.ShopID
		,i.ShopName
		,i.ReportCount
		,i.InvoicedDate
		,i.InvoicedTotal
		,i.PaymentsApplied
		,i.PaymentsAppliedTotal
		,i.InvoiceBalance
	FROM vwInvoiceBalances i
	WHERE i.ShopID = ISNULL(@ShopID, i.ShopID)
		AND (i.PaymentsApplied = 0 OR i.InvoiceBalance > 0)
	ORDER BY i.InvoicedDate

END

GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetPaymentTransactions')
	DROP PROCEDURE Billing.usp_GetPaymentTransactions
GO

CREATE PROCEDURE Billing.usp_GetPaymentTransactions
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		PaymentID
		,PaymentTransactionID
		,PaymentReceivedFromShopID
		,PaymentReceivedFromShopName
		,PaymentTypeID
		,PaymentTypeName
		,PaymentAmount
		,PaymentReferenceNumber
		,InvoiceID
		,InvoiceAmountApplied
		,PaymentCreatedBy
		,PaymentCreatedDateTime
	FROM Billing.vwPaymentTransactions pt
	ORDER BY pt.PaymentCreatedDateTime DESC

END

GO

