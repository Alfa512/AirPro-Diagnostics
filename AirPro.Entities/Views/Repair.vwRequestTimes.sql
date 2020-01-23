
CREATE VIEW [Repair].[vwRequestTimes]
AS

	SELECT
		s.ShopGuid
		,s.Name [ShopName]
		,o.ShopReferenceNumber [ShopRO]
		,o.VehicleVIN
		,o.OrderId [RepairOrderId]
		,rq.RequestId
		,rt.TypeName [RequestType]
		,CAST(o.CreatedDt AS DATETIME) [RepairCreatedDateTime]
		,CAST(rq.CreatedDt AS DATETIME) [RequestCreatedDateTime]
		,CAST(rpt.ResponsibleSetDt AS DATETIME) [RequestResponsibleSetDateTime]
		,ur.LastName + ', ' + ur.FirstName [RequestResponsibleBy]
		,CAST(rpt.CompletedDt AS DATETIME) [RequestCompletedDateTime]
		,u.LastName + ', ' + u.FirstName [RequestCompletedBy]
		,rpt.InvoicedInd [RequestInvoicedInd]
		,CAST(rpt.InvoicedDt AS DATETIME) [RequestInvoicedDateTime]
		,ui.LastName + ', ' + ui.FirstName [RequestInvoicedBy]
		,CASE rpt.InvoicedInd WHEN 1 THEN rpt.InvoiceAmount ELSE NULL END [RequestInvoicedAmount]
	FROM Scan.Requests rq
	INNER JOIN Scan.RequestTypes rt
		ON rq.RequestTypeId = rt.RequestTypeId
	INNER JOIN Scan.Reports rpt
		INNER JOIN Access.Users u
			ON rpt.CompletedByUserGuid = u.UserGuid
		LEFT JOIN Access.Users ui
			ON rpt.InvoicedByUserGuid = ui.UserGuid
		LEFT JOIN Access.Users ur
			ON rpt.ResponsibleTechnicianUserGuid = ur.UserGuid
		ON rq.ReportId = rpt.ReportId
			AND rpt.CompletedInd = 1
	INNER JOIN Repair.Orders o
		INNER JOIN Access.Shops s
			ON o.ShopGuid = s.ShopGuid
		ON rq.OrderId = o.OrderId

GO