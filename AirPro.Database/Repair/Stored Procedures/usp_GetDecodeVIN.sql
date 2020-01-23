
CREATE PROCEDURE Repair.usp_GetDecodeVIN
	@DecodeVIN VARCHAR(16)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE
		@VehicleMakeIdentifier CHAR(3) = LEFT(@DecodeVIN, 3),
		@VehicleModelIdentifier CHAR(3) = RIGHT(LEFT(@DecodeVIN, 6), 3),
		@VehicleYearIdentifier CHAR(1) = RIGHT(LEFT(@DecodeVIN, 10), 1);

	WITH Vehicles
	AS
	(
		SELECT
			v.VehicleVIN
			,v.VehicleMakeId
			,v.Model [VehicleModelName]
			,v.Year [VehicleYear]
		FROM Repair.Vehicles v
		WHERE v.VehicleLookupId IS NOT NULL
		GROUP BY
			v.VehicleVIN
			,v.VehicleMakeId
			,v.Model
			,v.Year
	),
	VehicleMakes
	AS
	(
		SELECT
			LEFT(v.VehicleVIN, 3) [VehicleMakeIdentifier]
			,v.VehicleMakeId
			,COUNT(DISTINCT(v.VehicleVIN)) [VehicleMakeConfidence]
		FROM Vehicles v
		GROUP BY
			LEFT(v.VehicleVIN, 3)
			,v.VehicleMakeId
	),
	VehicleModels
	AS
	(
		SELECT
			RIGHT(LEFT(v.VehicleVIN, 6), 3) [VehicleModelIdentifer]
			,v.VehicleMakeId
			,v.VehicleModelName
			,COUNT(DISTINCT(v.VehicleVIN)) [VehicleModelConfidence]
		FROM Vehicles v
		GROUP BY
			RIGHT(LEFT(v.VehicleVIN, 6), 3)
			,v.VehicleMakeId
			,v.VehicleModelName
	),
	VehicleYears
	AS
	(
		SELECT
			RIGHT(LEFT(v.VehicleVIN, 10), 1) [VehicleYearIdentifier]
			,v.VehicleMakeId
			,v.VehicleYear
			,COUNT(DISTINCT(v.VehicleVIN)) [VehicleYearConfidence]
		FROM Vehicles v
		GROUP BY
			RIGHT(LEFT(v.VehicleVIN, 10), 1)
			,v.VehicleMakeId
			,v.VehicleYear
	)

	SELECT TOP (1)
		@DecodeVIN [VehicleVIN]
		,vm.VehicleMakeId
		,ISNULL(rvm.VehicleMakeName, 'Unknown') [VehicleMakeName]
		,ISNULL(vmo.VehicleModelName, 'Unknown') [VehicleModel]
		,ISNULL(vy.VehicleYear, '9999') [VehicleYear]
		,'Unknown' [VehicleTransmission]
		,rvm.VehicleMakeTypeId [VehicleMakeTypeId]
		,ISNULL(rvm.VehicleMakeName, 'Unknown') [VehicleMakeTypeName]
		,CAST(3 AS INT) [LookupService]
		,CAST(0 AS BIT) [ManualEntryInd]
	FROM VehicleMakes vm
	INNER JOIN Repair.VehicleMakes rvm
		ON vm.VehicleMakeId = rvm.VehicleMakeId
	LEFT JOIN VehicleModels vmo
		ON vm.VehicleMakeId = vmo.VehicleMakeId
	LEFT JOIN VehicleYears vy
		ON vm.VehicleMakeId = vy.VehicleMakeId
	WHERE vm.VehicleMakeIdentifier = @VehicleMakeIdentifier
		AND vmo.VehicleModelIdentifer = @VehicleModelIdentifier
		AND vy.VehicleYearIdentifier = @VehicleYearIdentifier
	ORDER BY
		vm.VehicleMakeConfidence DESC
		,vmo.VehicleModelConfidence DESC
		,vy.VehicleYearConfidence DESC

END