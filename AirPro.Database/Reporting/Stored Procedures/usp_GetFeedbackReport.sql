
CREATE PROCEDURE Reporting.usp_GetFeedbackReport
	@RepairId INT = NULL,
	@UserGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @TimeZone NVARCHAR(100) = Common.udf_GetUserTimeZoneId(ISNULL(@UserGuid, Common.udf_GetEmptyGuid()));
	
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
		,f.ResponseTimeRate [ResponseTime]
		,f.RequestTimeRate [RequestTime]
		,f.TechnicianKnowledgeRate [TechnicianKnowledge]
		,f.ReportCompletionRate [ReportCompletion]
		,f.ConcernsAddressedRate [ConcernsAddressed]
		,f.TechnicianCommunicationRate [TechnicianCommunication]
		,f.AdditionalFeedback [AdditionalFeedback]
		,CAST(o.UpdatedDt AT TIME ZONE @TimeZone AS DATETIME) [RepairLastUpdated]
		,(SELECT AVG(Col)
		 FROM (VALUES(f.ResponseTimeRate),
					 (f.RequestTimeRate),
					 (f.TechnicianKnowledgeRate),
					 (f.ReportCompletionRate),
					 (f.ConcernsAddressedRate),
					 (f.TechnicianCommunicationRate)) V(Col)) [Average]
	FROM Repair.Feedback f
	INNER JOIN Repair.Orders o
		ON f.RepairId = o.OrderId
	INNER JOIN Access.Shops s
		ON o.ShopGuid = s.ShopGuid
	INNER JOIN Repair.Vehicles v
		INNER JOIN Repair.VehicleMakes vm
			ON v.VehicleMakeId = vm.VehicleMakeId
		ON o.VehicleVIN = v.VehicleVIN
	WHERE @RepairId IS NULL OR f.RepairId = @RepairId

END