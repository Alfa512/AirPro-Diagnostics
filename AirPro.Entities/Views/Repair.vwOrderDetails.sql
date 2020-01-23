DROP VIEW IF EXISTS Repair.vwOrderDetails;
GO

CREATE VIEW Repair.vwOrderDetails
WITH SCHEMABINDING
AS

	SELECT
		o.OrderId [RepairId]
		,o.Status [RepairStatusId]
		,o.ShopGuid
		,o.ShopReferenceNumber [ShopRONumber]
		,o.InsuranceCompanyId
		,o.InsuranceCompanyOther [InsuranceCompanyOther]
		,o.InsuranceReferenceNumber [InsuranceReferenceNumber]
		,o.VehicleVIN
		,o.Odometer
		,o.AirBagsDeployed
		,o.AirBagsVisualDeployments [AirBagsVisualDeployments]
		,o.DrivableInd
		,o.CreatedDt

		,os.StatusName [RepairStatusName]

		,s.DisplayName [ShopName]

		,v.Model [VehicleModel]
		,CAST(LEFT(v.Year, 4) AS CHAR(4)) [VehicleYear]
		,v.Transmission [VehicleTransmission]
		,CAST(CASE WHEN v.VehicleLookupId IS NULL THEN 1 ELSE 0 END AS BIT) [VehicleManualEntryInd]

		,vm.VehicleMakeName [VehicleMake]

		,ISNULL(NULLIF(ic.InsuranceCompanyName, 'Other'), o.InsuranceCompanyOther) [InsuranceCompanyDisplay]

		,CAST(cu.DisplayName AS VARCHAR(200)) [CreatedBy]

		,CAST(o.OrderId AS NVARCHAR(MAX)) + '|' +
			ISNULL(os.StatusName, '') + '|' +
			ISNULL(s.DisplayName, '') + '|' +
			ISNULL(o.ShopReferenceNumber, '') + '|' +
			ISNULL(ic.InsuranceCompanyName, '') + '|' +
			ISNULL(o.InsuranceCompanyOther, '') + '|' +
			ISNULL(o.InsuranceReferenceNumber, '') + '|' +
			ISNULL(o.VehicleVIN, '') + '|' +
			ISNULL(vm.VehicleMakeName, '') + '|' +
			ISNULL(v.Model, '') + '|' +
			ISNULL(v.Year, '') + '|' +
			ISNULL(v.Transmission, '') + '|' +
			ISNULL(o.AirBagsVisualDeployments, '') + '|' +
			ISNULL(cu.DisplayName, '') [SearchText]
	FROM Repair.Orders o
	INNER JOIN Repair.OrderStatuses os
		ON o.Status = os.StatusId
	INNER JOIN Access.Shops s
		ON o.ShopGuid = s.ShopGuid
	INNER JOIN Repair.Vehicles v
		INNER JOIN Repair.VehicleMakes vm
			ON v.VehicleMakeId = vm.VehicleMakeId
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Repair.InsuranceCompanies ic
		ON o.InsuranceCompanyId = ic.InsuranceCompanyId
	INNER JOIN Access.Users cu
		ON o.CreatedByUserGuid = cu.UserGuid

GO

CREATE UNIQUE CLUSTERED INDEX PK_RepairOrderDetails ON Repair.vwOrderDetails (RepairId);
GO

CREATE FULLTEXT INDEX ON Repair.vwOrderDetails (SearchText) KEY INDEX PK_RepairOrderDetails;
GO