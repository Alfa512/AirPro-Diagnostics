
CREATE PROCEDURE [Access].[usp_GetRegistrationOptions]
AS
BEGIN

	SET NOCOUNT ON;
	
		SELECT s.Abbreviation [Key], s.Name [Value] FROM Common.States s
		ORDER BY s.CountryId, s.Name

		SELECT pp.PricingPlanId [Key], pp.CurrencyId, CASE pp.CurrencyId
			WHEN NULL THEN pp.PricingPlanName
			ELSE pp.PricingPlanName + ' (' + c.Name + ')'
		END [Value] FROM Billing.PricingPlans pp
		LEFT JOIN Billing.Currencies c ON pp.CurrencyId = c.CurrencyId
		WHERE pp.PricingPlanActiveInd = 1

		SELECT ep.EstimatePlanId [Key], ep.Name [Value] FROM Billing.EstimatePlans ep
		ORDER BY ep.Name

		SELECT ic.InsuranceCompanyName, ic.InsuranceCompanyId, ic.ProgramName FROM Repair.InsuranceCompanies ic
		WHERE ic.DisabledInd = 0
		ORDER BY ic.InsuranceCompanyName

		SELECT c.CycleId [Key], c.CycleName [Value] FROM Billing.Cycles c
		ORDER BY c.SortOrder

		EXEC Repair.usp_GetVehicleMakes

		SELECT g.GroupGuid [Key], g.Name [Value] FROM Access.Groups g

		SELECT rt.RequestTypeId [Key], rt.TypeName [Value] FROM Scan.RequestTypes rt
		ORDER BY rt.SortOrder

		SELECT c.CurrencyId [Key], c.Name [Value] FROM Billing.Currencies c
		ORDER BY c.CurrencyId

END