
CREATE VIEW [Repair].[vwRequestTypeCounts]
AS

	SELECT
		ShopGuid
		,ShopName
		,ShopRO
		,VehicleVIN
		,OrderId [RepairOrderId]
		,RepairCreatedDt [RepairCreatedDateTime]
		,[Quick Scan] [QuickScanCount]
		,[Diagnostic Scan] [DiagnosticScanCount]
		,[Completion Scan] [CompletionScanCount]
		,[Follow Up Scan] [FollowUpScanCount]
		,[Inspection Scan] [InspectionScanCount]
		,[Self Scan] [SelfScanCount]
	FROM
	(
		SELECT
			s.ShopGuid
			,s.Name [ShopName]
			,o.OrderId
			,o.ShopReferenceNumber [ShopRO]
			,o.VehicleVIN
			,CAST(o.CreatedDt AS DATETIME) [RepairCreatedDt]
			,r.RequestTypeId [TypeOfScan]
			,rt.TypeName [ScanType]
		FROM [Access].[Shops] s
		INNER JOIN [Repair].[Orders] o
			INNER JOIN [Scan].[Requests] r
				INNER JOIN [Scan].[RequestTypes] rt
					ON r.RequestTypeId = rt.RequestTypeID
				ON o.OrderId = r.OrderId
			ON s.ShopGuid = o.ShopGuid
	) s
	PIVOT
	(
		COUNT(TypeOfScan)
		FOR ScanType IN ([Quick Scan], [Diagnostic Scan], [Completion Scan], [Follow Up Scan], [Inspection Scan], [Self Scan])
	) AS p