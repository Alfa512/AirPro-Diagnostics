
UPDATE i
	SET i.InvoicedDt = COALESCE(UpdatedDt, CreatedDt),
		i.InvoicedByID = COALESCE(UpdatedBy_Id, CreatedBy_Id)
FROM [Repair].[Invoices] i
WHERE InvoicedInd = 1

UPDATE r
	SET r.CompletedDt = COALESCE(UpdatedDt, CreatedDt),
		r.CompletedByID = COALESCE(UpdatedBy_Id, CreatedBy_Id)
FROM [Scan].[Reports] r
WHERE r.CompletedInd = 1
	AND r.CompletedDt IS NULL

UPDATE r
	SET r.InvoicedByID = COALESCE(rq.UpdatedBy_Id, rq.CreatedBy_Id)
FROM [Scan].[Reports] r
INNER JOIN [Scan].[Requests] rq ON r.ReportID = rq.ReportID
WHERE r.InvoicedInd = 1
	AND InvoicedByID IS NULL