
CREATE VIEW [Billing].[vwInvoiceLineItems]
AS
	SELECT
		s.ShopGuid
		,s.Name [ShopName]
		,o.ShopReferenceNumber [ShopRO]
		,o.VehicleVIN [VehicleVIN]
		,o.OrderId [RepairOrderId]
		,i.InvoiceId
		,r.RequestId
		,rt.TypeName [RequestType]
		,u.LastName + ', ' + u.FirstName [InvoicedBy]
		,CAST(i.CreatedDt AS SMALLDATETIME) [InvoicedDateTime]
		,rpt.InvoiceAmount [InvoicedAmount]
	FROM Access.Shops s
	INNER JOIN Repair.Orders o
		LEFT JOIN Repair.InsuranceCompanies ins
			ON o.InsuranceCompanyID = ins.InsuranceCompanyID
		ON s.ShopGuid = o.ShopGuid
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Repair.Invoices i
		INNER JOIN Access.Users u
			ON i.InvoicedByUserGuid = u.UserGuid
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