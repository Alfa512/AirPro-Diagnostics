
CREATE PROCEDURE Notification.usp_GetRepairNotification
	@RepairId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		o.OrderId [RepairId]
		,v.VehicleVIN
		,v.VehicleMakeId
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,o.ShopReferenceNumber [ShopRONumber]
		,s.ShopGuid
		,s.Name [ShopName]
		,s.Phone [ShopPhone]
		,i.InvoicedInd
		,ISNULL(SUM(rpt.InvoiceAmount), 0) [InvoiceTotal]
	FROM Repair.Orders o
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Access.Shops s
		ON o.ShopGuid = s.ShopGuid
	LEFT JOIN Repair.Invoices i
		ON o.OrderId = i.InvoiceId
	LEFT JOIN Scan.Requests r
		INNER JOIN Scan.Reports rpt
			ON r.ReportId = rpt.ReportId
				AND rpt.InvoicedInd = 1
		ON o.OrderId = r.OrderId
	WHERE o.OrderId = @RepairId
	GROUP BY
		o.OrderId
		,v.VehicleVIN
		,v.VehicleMakeId
		,v.Make
		,v.Model
		,v.Year
		,o.ShopReferenceNumber
		,s.ShopGuid
		,s.Name
		,s.Phone
		,i.InvoicedInd

END
GO