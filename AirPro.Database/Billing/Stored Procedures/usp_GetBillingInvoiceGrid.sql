
CREATE PROCEDURE [Billing].[usp_GetBillingInvoiceGrid]
	@UserGuid UNIQUEIDENTIFIER
	,@Status INT
	,@Search VARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;

	SET @Search = '%' + NULLIF(RTRIM(@Search), '') + '%';
	DECLARE @UserTimeZone NVARCHAR(MAX) = Common.udf_GetUserTimeZoneId(@UserGuid);

	WITH Invoices
	AS
	(
		SELECT
			o.OrderId [RepairId]
			,o.Status [RepairStatus]
			,o.ShopReferenceNumber [ShopRoNumber]
			,ISNULL(ic.InsuranceCompanyName, o.InsuranceCompanyOther) [InsuranceCompanyName]
			,o.InsuranceReferenceNumber [InsuranceClaimNumber]
			,CAST(ISNULL(NULLIF(o.UpdatedDt, '0001-01-01 00:00:00.0000000 +00:00'), o.CreatedDt) AT TIME ZONE @UserTimeZone AS DATETIME) [RepairLastUpdatedDt]

			,v.VehicleVIN
			,vm.VehicleMakeName [VehicleMake]
			,v.Model [VehicleModel]
			,v.Year [VehicleYear]
			,v.Transmission [VehicleTransmission]

			,s.DisplayName [ShopName]
			,s.Phone [ShopPhone]
			,s.Fax [ShopFax]
			,s.Address1 [ShopAddress1]
			,s.Address2 [ShopAddress2]
			,s.City [ShopCity]
			,st.Abbreviation [ShopState]
			,s.Zip [ShopZip]

			,CAST(i.InvoicedDt AT TIME ZONE @UserTimeZone AS DATETIME) [InvoicedDt]
		FROM Repair.Orders o
		INNER JOIN Repair.Vehicles v
			INNER JOIN Repair.VehicleMakes vm
				ON v.VehicleMakeId = vm.VehicleMakeId
			ON o.VehicleVIN = v.VehicleVIN
		INNER JOIN Access.Shops s
			INNER JOIN Common.States st
				ON s.StateId = st.StateId
			LEFT JOIN Billing.PricingPlans spp
				ON s.PricingPlanId = spp.PricingPlanId
			ON o.ShopGuid = s.ShopGuid
		LEFT JOIN Repair.Invoices i
			LEFT JOIN Access.Users iu
				ON i.InvoicedByUserGuid = iu.UserGuid
			ON o.OrderId = i.InvoiceId
		LEFT JOIN Repair.InsuranceCompanies ic
			ON o.InsuranceCompanyId = ic.InsuranceCompanyId
				AND ic.InsuranceCompanyId > 1
		WHERE o.ShopGuid IN (SELECT ShopGuid FROM Access.vwUserMemberships WHERE UserGuid = @UserGuid)
			AND
			(
				(@Search IS NULL AND o.Status = @Status)
				OR
				(@Search IS NOT NULL AND o.Status IN (3, 4))
			)
	)

	SELECT
		i.RepairId
		,i.RepairStatus
		,i.ShopRoNumber
		,i.InsuranceCompanyName
		,i.InsuranceClaimNumber
		,i.RepairLastUpdatedDt
		,i.VehicleVIN
		,i.VehicleMake
		,i.VehicleModel
		,i.VehicleYear
		,i.VehicleTransmission
		,i.ShopName
		,i.ShopPhone
		,i.ShopFax
		,i.ShopAddress1
		,i.ShopAddress2
		,i.ShopCity
		,i.ShopState
		,i.ShopZip
		,i.InvoicedDt
	FROM Invoices i
	WHERE @Search IS NULL
		OR
		(
			i.RepairId = TRY_CAST(REPLACE(@Search, '%', '') AS INT)
			OR i.ShopRoNumber LIKE @Search
			OR i.InsuranceCompanyName LIKE @Search
			OR i.InsuranceClaimNumber LIKE @Search
			OR i.VehicleVIN LIKE @Search
			OR i.VehicleMake LIKE @Search
			OR i.VehicleModel LIKE @Search
			OR i.VehicleYear LIKE @Search
			OR i.VehicleTransmission LIKE @Search
			OR i.ShopName LIKE @Search
			OR i.ShopPhone LIKE @Search
			OR i.ShopFax LIKE @Search
			OR i.ShopAddress1 LIKE @Search
			OR i.ShopAddress2 LIKE @Search
			OR i.ShopCity LIKE @Search
			OR i.ShopState LIKE @Search
			OR i.ShopZip LIKE @Search
		)
END