CREATE PROCEDURE [Reporting].[usp_GetEstimateReportDataSource]
	@RepairId INT
	,@Offset CHAR(10) = '-00:00'
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		s.DisplayName [ShopName]
		,s.Phone [ShopPhone]
		,s.Fax [ShopFax]
		,s.Address1 [ShopAddress1]
		,s.Address2 [ShopAddress2]
		,s.City [ShopCity]
		,st.Abbreviation [ShopState]
		,s.Zip [ShopZip]
		,o.OrderId [RepairOrderID]
		,o.ShopReferenceNumber [RepairShopReferenceNumber]
		,CASE
			WHEN o.InsuranceCompanyId IS NULL OR o.InsuranceCompanyId = 1
				THEN o.InsuranceCompanyOther
			ELSE ic.InsuranceCompanyName
		END [RepairInsuranceCompany]
		,o.InsuranceReferenceNumber [RepairInsuranceClaimNumber]
		,o.Odometer [RepairOdometer]
		,o.AirBagsDeployed [RepairAirBagsDeployed]
		,v.VehicleVIN [VehicleVIN]
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,v.Transmission [VehicleTransmission]
		,COALESCE(pp.CurrencyName, c.Name, 'USD') [CurrencyName]
	FROM Repair.Orders o
	INNER JOIN Access.Shops s
		INNER JOIN Common.States st
			ON s.StateId = st.StateId
		ON o.ShopGuid = s.ShopGuid
	INNER JOIN Billing.Currencies c ON s.CurrencyId = c.CurrencyId
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Repair.InsuranceCompanies ic
		ON o.InsuranceCompanyId = ic.InsuranceCompanyId
	LEFT JOIN (SELECT pp.PricingPlanId, c.Name [CurrencyName] FROM Billing.PricingPlans pp INNER JOIN Billing.Currencies c ON pp.CurrencyId = c.CurrencyId) pp ON s.PricingPlanId = pp.PricingPlanId
	WHERE o.OrderId = @RepairId

	SELECT
		TypeOfScan
		,EstimateAmount
	FROM
	(
		SELECT
			rt.TypeName [TypeOfScan]
			,ISNULL(Ppicrt.LineItemCost, pprt.LineItemCost) [EstimateAmount]
			,rt.SortOrder
		FROM Repair.Orders o
		INNER JOIN Access.Shops s
			ON s.ShopGuid = o.ShopGuid
		INNER JOIN Repair.Vehicles v
			INNER JOIN Repair.VehicleMakes vm
				ON vm.VehicleMakeId = v.VehicleMakeId
			ON o.VehicleVIN = v.VehicleVIN
		INNER JOIN Scan.Requests rq
			INNER JOIN Scan.RequestTypes rt
				ON rt.RequestTypeId = rq.RequestTypeId
					AND rt.BillableFlag = 1
			ON rq.OrderId = o.OrderId
		INNER JOIN Billing.PricingPlans pp
			ON pp.PricingPlanId = s.PricingPlanId
				AND pp.PricingPlanActiveInd = 1
		INNER JOIN Billing.PricingPlanRequestTypes pprt
			ON pprt.PricingPlanId = pp.PricingPlanId
				AND pprt.RequestTypeId = rq.RequestTypeId
				AND pprt.VehicleMakeTypeId = vm.VehicleMakeTypeId
		LEFT JOIN Billing.ShopInsuranceCompaniesPricing Sp
			ON Sp.InsuranceCompanyId = O.InsuranceCompanyId
				AND Sp.ShopId = O.ShopGuid
				LEFT JOIN Billing.PricingPlans Ppic ON Ppic.PricingPlanId = Sp.PricingPlanId
				LEFT JOIN Billing.PricingPlanRequestTypes Ppicrt ON Ppicrt.RequestTypeId = rq.RequestTypeId
					AND Ppicrt.PricingPlanId = Ppic.PricingPlanId 
					AND Ppicrt.VehicleMakeTypeId = vm.VehicleMakeTypeId
		WHERE o.OrderId = @RepairId

		UNION

		SELECT
			'Completion Scan' [TypeOfScan]
			,ISNULL(ISNULL(oepv.CompletionCost, epv.CompletionCost), pprt.LineItemCost) [EstimateAmount]
			,99 [SortOrder]
		FROM Repair.Orders o
		INNER JOIN Access.Shops s
			ON s.ShopGuid = o.ShopGuid
		INNER JOIN Repair.Vehicles v
			INNER JOIN Repair.VehicleMakes vm
				ON vm.VehicleMakeId = v.VehicleMakeId
			ON v.VehicleVIN = o.VehicleVIN
		INNER JOIN Billing.PricingPlanRequestTypes pprt
			ON pprt.PricingPlanId = s.PricingPlanId
				AND pprt.VehicleMakeTypeId = vm.VehicleMakeTypeId
				AND pprt.RequestTypeId = 3
		LEFT JOIN Billing.ShopInsuranceCompaniesEstimate Sp
			ON Sp.InsuranceCompanyId = O.InsuranceCompanyId
				AND Sp.ShopId = O.ShopGuid
				LEFT JOIN Billing.EstimatePlans Ppic ON Ppic.EstimatePlanId = Sp.EstimatePlanId
				LEFT JOIN Billing.EstimatePlanVehicles oepv
					ON oepv.EstimatePlanId = Ppic.EstimatePlanId
						AND oepv.VehicleMakeId = v.VehicleMakeId
		LEFT JOIN Billing.EstimatePlans ep
			ON ep.EstimatePlanId = s.EstimatePlanId
				AND ep.ActiveInd = 1
		LEFT JOIN Billing.EstimatePlanVehicles epv
			ON epv.EstimatePlanId = s.EstimatePlanId
				AND epv.VehicleMakeId = v.VehicleMakeId
		WHERE o.OrderId = @RepairId
	) e
	ORDER BY e.SortOrder
END