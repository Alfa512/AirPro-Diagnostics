CREATE PROCEDURE Notification.usp_GetScanToolIssuesNotification
	@RequestId INT
AS
BEGIN

	SET NOCOUNT ON;
		
	SELECT
		o.OrderId [RepairId]
		,CASE o.Status
			WHEN 1 THEN 'Active'
			WHEN 2 THEN 'Canceled'
			WHEN 3 THEN 'Completed'
			WHEN 4 THEN 'Invoiced'
			WHEN 5 THEN 'Paid'
		END [RepairStatus]
		,s.Name [ShopName]
		,s.ShopGuid
		,s.Phone [ShopPhone]
		,o.ShopReferenceNumber [ShopRONumber]
		,o.VehicleVIN
		,vm.VehicleMakeName [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,r.ReportId
		,rq.RequestId
		,r.CancellationNotes
		,r.ReportNotes
		,r.TechnicianNotes
	FROM Repair.Orders o
	INNER JOIN Access.Shops s
		ON o.ShopGuid = s.ShopGuid
	INNER JOIN Scan.Requests rq
		ON o.OrderId = rq.OrderId
	INNER JOIN Scan.Reports r
		ON rq.ReportId = r.ReportId
	INNER JOIN Repair.Vehicles v
		INNER JOIN Repair.VehicleMakes vm ON v.VehicleMakeId = vm.VehicleMakeId ON o.VehicleVIN = v.VehicleVIN
	WHERE rq.RequestId = @RequestId
END
GO