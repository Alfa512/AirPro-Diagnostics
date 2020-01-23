
CREATE PROCEDURE Reporting.usp_GetEstimatedOutstandingRevenueReport
	@UserGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	WITH RepairPricingPlans
	AS
	(
		-- Load Repairs & Pricing Plans.
		SELECT
			o.OrderId
			,o.ShopGuid
			,s.ShopFixedPriceInd
			,s.FirstScanCost
			,s.AdditionalScanCost
			,vm.VehicleMakeTypeId
			,Billing.udf_GetPricingPlanIdByOrderId(o.OrderId) [PricingPlanId]
		FROM Repair.Orders o
		INNER JOIN Repair.Vehicles v
			INNER JOIN Repair.VehicleMakes vm
				ON v.VehicleMakeId = vm.VehicleMakeId
			ON o.VehicleVIN = v.VehicleVIN
		INNER JOIN Access.Shops s
			ON o.ShopGuid = s.ShopGuid
				AND s.ActiveInd = 1 -- ONLY Active Shops.
				AND s.HideFromReports = 0 -- ONLY Reporting Shops.
		WHERE o.Status IN (1, 3) -- Active & Completed Status.
			AND o.ShopGuid IN (SELECT ShopGuid FROM Access.vwUserMemberships WHERE UserGuid = @UserGuid)
	),
	OrderPricing
	AS
	(
		-- Calculate Fixed Pricing.
		SELECT
			rpp.OrderId
			,CAST(CASE WHEN ISNULL(rq.RequestCount, 0) > 0 THEN rpp.FirstScanCost ELSE 0.00 END AS DECIMAL(18, 2))
				+ CAST(CASE WHEN ISNULL(rq.RequestCount, 0) > 1 THEN (rq.RequestCount - 1) * rpp.AdditionalScanCost ELSE 0.00 END AS DECIMAL(18, 2)) [ItemCost]
		FROM RepairPricingPlans rpp
		OUTER APPLY
		(
			SELECT COUNT(1) [RequestCount]
			FROM Scan.Requests r
			WHERE r.OrderId = rpp.OrderId
		) rq
		WHERE rpp.ShopFixedPriceInd = 1

		UNION ALL

		-- Calculate Request Types by Pricing Plan.
		SELECT
			rpp.OrderId
			,ISNULL(pprt.LineItemCost, 0) [ItemCost]
		FROM RepairPricingPlans rpp
		LEFT JOIN Scan.Requests r
			ON rpp.OrderId = r.OrderId
		LEFT JOIN Billing.PricingPlanRequestTypes pprt
			ON rpp.PricingPlanId = pprt.PricingPlanId
				AND r.RequestTypeId = pprt.RequestTypeId
				AND rpp.VehicleMakeTypeId = pprt.VehicleMakeTypeId
		WHERE rpp.ShopFixedPriceInd = 0

		UNION ALL

		-- Calculate Work Types by Pricing Plan.
		SELECT
			rpp.OrderId
			,ISNULL(ppwt.LineItemCost, 0) [ItemCost]
		FROM RepairPricingPlans rpp
		LEFT JOIN Scan.Requests r
			LEFT JOIN Scan.ReportWorkTypes rwt
				ON r.ReportId = rwt.ReportId
			ON rpp.OrderId = r.OrderId
		LEFT JOIN Billing.PricingPlanWorkTypes ppwt
			ON rpp.PricingPlanId = ppwt.PricingPlanId
				AND rwt.WorkTypeId = ppwt.WorkTypeId
				AND rpp.VehicleMakeTypeId = ppwt.VehicleMakeTypeId
	)

	-- Compile Results.
	SELECT
		s.DisplayName [ShopName]
		,s.ShopNumber [ShopNumber]
		,f.ShopFixedPriceInd
		,f.PricingPlanId [AppliedPricingPlanId]
		,f.OrderId [RepairId]
		,CASE o.Status
			WHEN 1 THEN 'Active'
			WHEN 2 THEN 'Canceled'
			WHEN 3 THEN 'Completed'
			WHEN 4 THEN 'Invoiced'
			WHEN 5 THEN 'Paid'
		END [RepairStatus]
		,ISNULL(o.ShopReferenceNumber, '') [ShopRONumber]
		,o.VehicleVIN
		,vm.VehicleMakeName
		,v.Model [VehicleModelName]
		,v.Year [VehicleYear]
		,CONVERT(VARCHAR, o.CreatedDt, 101) [RepairCreatedDt]
		,rq.LastRequestDt
		,DATEDIFF(DAY, rq.LastRequestDt, GETDATE()) [LastRequestDays]
		,rq.RequestCount
		,f.EstimatedRepairCost
	FROM
	(
		SELECT
			op.OrderId
			,rpp.PricingPlanId
			,rpp.ShopFixedPriceInd
			,SUM(op.ItemCost) [EstimatedRepairCost]
		FROM OrderPricing op
		INNER JOIN RepairPricingPlans rpp
			ON op.OrderId = rpp.OrderId
		GROUP BY
			op.OrderId
			,rpp.PricingPlanId
			,rpp.ShopFixedPriceInd
	) f
	INNER JOIN Repair.Orders o
		INNER JOIN Access.Shops s
			ON o.ShopGuid = s.ShopGuid
		INNER JOIN Repair.Vehicles v
			INNER JOIN Repair.VehicleMakes vm
				ON v.VehicleMakeId = vm.VehicleMakeId
			ON o.VehicleVIN = v.VehicleVIN
		ON f.OrderId = o.OrderId
	OUTER APPLY
	(
		SELECT
			COUNT(1) [RequestCount]
			,CONVERT(VARCHAR, MAX(CreatedDt), 101) [LastRequestDt]
		FROM Scan.Requests
		WHERE OrderId = f.OrderId
	) rq
	ORDER BY
		f.EstimatedRepairCost DESC
		,o.CreatedDt

END