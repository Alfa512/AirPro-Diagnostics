CREATE TYPE Access.udt_ShopVehicleMakesPricing AS TABLE 
(
	ShopGuid UNIQUEIDENTIFIER NULL, 
	VehicleMakeId INT NULL,
	PricingPlanId INT NULL
)
GO