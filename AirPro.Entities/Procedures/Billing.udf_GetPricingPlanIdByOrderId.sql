
DROP FUNCTION IF EXISTS Billing.udf_GetPricingPlanIdByOrderId;
GO

CREATE FUNCTION Billing.udf_GetPricingPlanIdByOrderId 
(
	@OrderId INT
)
RETURNS INT
AS
BEGIN

	DECLARE @Result INT;

	WITH PricingPlans
	AS
	(
		SELECT pp.PricingPlanId
		FROM Billing.PricingPlans pp
		WHERE pp.PricingPlanActiveInd = 1
	)

	SELECT @Result = COALESCE(ipp.PricingPlanId, vpp.PricingPlanId, spp.PricingPlanId)
	FROM Repair.Orders o
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Access.Shops s
		LEFT JOIN PricingPlans spp
			ON s.PricingPlanId = spp.PricingPlanId
		ON o.ShopGuid = s.ShopGuid
	LEFT JOIN Billing.ShopInsuranceCompaniesPricing sicp
		LEFT JOIN PricingPlans ipp
			ON sicp.PricingPlanId = ipp.PricingPlanId
		ON o.ShopGuid = sicp.ShopId
			AND o.InsuranceCompanyId = sicp.InsuranceCompanyId
	LEFT JOIN Billing.ShopVehicleMakesPricing svmp
		LEFT JOIN PricingPlans vpp
			ON svmp.PricingPlanId = vpp.PricingPlanId
		ON o.ShopGuid = svmp.ShopId
			AND v.VehicleMakeId = svmp.VehicleMakeId
	WHERE o.OrderId = @OrderId

	RETURN @Result

END
GO

