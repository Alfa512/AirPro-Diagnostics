
CREATE PROCEDURE Notification.usp_GetRequestNotification
	@RequestId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		o.OrderId [RepairId]
		,r.RequestId
		,rt.TypeName [RequestType]
		,r.ProblemDescription
		,v.VehicleVIN
		,v.VehicleMakeId
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,o.ShopReferenceNumber [ShopRONumber]
		,s.ShopGuid
		,s.Name [ShopName]
		,s.Phone [ShopPhone]
		,rpt.CompletedInd [ReportCompletedInd]
		,CAST(CASE WHEN r.RequestCategoryId = 1 AND s.AllowSelfScanAssessment = 1 AND s.EstimatePlanId IS NOT NULL THEN 1 ELSE 0 END AS BIT) [EstimatePlanInd]
	FROM Scan.Requests r
	INNER JOIN Scan.RequestTypes rt
		ON r.RequestTypeId = rt.RequestTypeId
	INNER JOIN Repair.Orders o
		ON r.OrderId = o.OrderId
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Access.Shops s
		ON o.ShopGuid = s.ShopGuid
	LEFT JOIN Scan.Reports rpt
		ON r.ReportId = rpt.ReportId
	WHERE r.RequestId = @RequestId

END