CREATE TYPE Access.udt_ShopInsuranceCompaniesPricingPlans AS TABLE 
(
	ShopGuid UNIQUEIDENTIFIER NULL, 
	InsuranceCompanyId INT NULL,
	PricingPlanId INT NULL
)
GO