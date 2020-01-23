CREATE PROCEDURE [Repair].[usp_GetVehicleByVIN]
	@VehicleVIN CHAR(17) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		v.VehicleVIN
		,v.VehicleMakeId
		,vm.VehicleMakeName
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,v.Transmission [VehicleTransmission]
		,vm.VehicleMakeTypeId
		,vmt.VehicleMakeTypeName
		,vl.Service [LookupService]
		,CASE WHEN v.VehicleLookupId IS NULL THEN 1 ELSE 0 END [ManualEntryInd]
	FROM Repair.Vehicles v
	INNER JOIN Repair.VehicleMakes vm
		INNER JOIN Repair.VehicleMakeTypes vmt
			ON vm.VehicleMakeTypeId = vmt.VehicleMakeTypeId
		ON v.VehicleMakeId = vm.VehicleMakeId
	LEFT JOIN Repair.VehicleLookups vl
		ON v.VehicleLookupId = vl.VehicleLookupId
	WHERE v.VehicleVIN = ISNULL(@VehicleVIN, v.VehicleVIN)

END
GO