

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'vwDTCResultsByScan')
	DROP VIEW Scan.vwDTCResultsByScan
GO

CREATE VIEW Scan.vwDTCResultsByScan
AS
	SELECT
		rt.TypeName [ScanType]
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,d.CONTROLLER_NAME [DTC_Controller]
		,d.DTC_DESCRIPTION [DTC_Description]
		,COUNT(1) [Count]
	FROM Repair.Orders o
	INNER JOIN Scan.Requests r
		INNER JOIN Scan.RequestTypes rt
			ON r.RequestTypeId = rt.RequestTypeID
		ON o.OrderID = r.OrderId
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Scan.UploadXmls u
		INNER JOIN Scan.Results sr
			INNER JOIN Scan.DTCResults d
				ON sr.SCAN_ID = d.SCAN_ID
			ON u.SCAN_UPLOAD_ID = sr.SCAN_UPLOAD_ID
		ON r.RequestID = u.RequestID
	GROUP BY
		rt.TypeName
		,v.Make
		,v.Model
		,v.Year
		,d.CONTROLLER_NAME
		,d.DTC_DESCRIPTION
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'vwDTCResultsByScan')
	DROP VIEW Scan.vwDTCResultsByScan
GO

CREATE VIEW Scan.vwDTCResultsByScan
AS
	SELECT
		rt.TypeName [ScanType]
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,d.CONTROLLER_NAME [DTC_Controller]
		,d.DTC_DESCRIPTION [DTC_Description]
		,COUNT(1) [Count]
	FROM Repair.Orders o
	INNER JOIN Scan.Requests r
		INNER JOIN Scan.RequestTypes rt
			ON r.RequestTypeId = rt.RequestTypeID
		ON o.OrderID = r.OrderId
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Scan.UploadXmls u
		INNER JOIN Scan.Results sr
			INNER JOIN Scan.DTCResults d
				ON sr.SCAN_ID = d.SCAN_ID
			ON u.SCAN_UPLOAD_ID = sr.SCAN_UPLOAD_ID
		ON r.RequestID = u.RequestID
	GROUP BY
		rt.TypeName
		,v.Make
		,v.Model
		,v.Year
		,d.CONTROLLER_NAME
		,d.DTC_DESCRIPTION
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