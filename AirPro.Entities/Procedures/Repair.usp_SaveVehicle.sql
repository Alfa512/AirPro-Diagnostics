
CREATE PROCEDURE [Repair].[usp_SaveVehicle]
	@VehicleVIN CHAR(17)
	,@VehicleMakeId INT
	,@VehicleMakeName VARCHAR(MAX)
	,@VehicleModel VARCHAR(MAX)
	,@VehicleYear CHAR(4)
	,@VehicleTransmission VARCHAR(MAX) = 'Unknown'
	,@VehicleLookupId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	SET @VehicleMakeId = NULLIF(@VehicleMakeId, 0);

	IF (LEN(RTRIM(@VehicleVIN)) < 17) THROW 50000, '@VehicleVIN Length < 17', 1;
	IF (LEN(RTRIM(@VehicleMakeName)) < 1 AND @VehicleMakeId IS NULL) THROW 50000, '@VehicleMakeName < 1 or @VehicleMakeId IS NULL', 1;
	IF (LEN(RTRIM(@VehicleModel)) < 1) THROW 50000, '@VehicleModel Length < 1', 1;
	IF (LEN(RTRIM(@VehicleYear)) < 4) THROW 50000, '@VehicleYear Length < 4', 1;

	BEGIN TRAN

		BEGIN TRY

			SELECT @VehicleMakeId = ISNULL(@VehicleMakeId, VehicleMakeId)
			FROM Repair.VehicleMakes
			WHERE VehicleMakeName = @VehicleMakeName
				OR LEFT(RTRIM(LTRIM(VehicleMakeName)), 4) = @VehicleMakeName

			SELECT @VehicleMakeName = ISNULL(@VehicleMakeName, VehicleMakeName)
			FROM Repair.VehicleMakes
			WHERE VehicleMakeId = @VehicleMakeId

			IF (@VehicleMakeId IS NULL AND NULLIF(@VehicleMakeName, '') IS NOT NULL)
			BEGIN
				INSERT INTO Repair.VehicleMakes
					(VehicleMakeName, VehicleMakeTypeId)
				SELECT @VehicleMakeName, VehicleMakeTypeId
				FROM Repair.VehicleMakeTypes
				WHERE VehicleMakeTypeName = 'Domestic'

				SET @VehicleMakeId = SCOPE_IDENTITY()
			END

			IF EXISTS (SELECT 1 FROM Repair.Vehicles WHERE VehicleVIN = @VehicleVIN)
				BEGIN

					UPDATE v
						SET v.VehicleMakeId = @VehicleMakeId
							,v.Make = @VehicleMakeName
							,v.Model = @VehicleModel
							,v.Year = @VehicleYear
							,v.Transmission = @VehicleTransmission
							,v.VehicleLookupId = ISNULL(@VehicleLookupId, v.VehicleLookupId)
					FROM Repair.Vehicles v
					WHERE v.VehicleVIN = @VehicleVIN

				END
			ELSE
				BEGIN

					INSERT INTO Repair.Vehicles
					(
						VehicleVIN
						,VehicleMakeId
						,Make
						,Model
						,Year
						,Transmission
						,VehicleLookupId
					)
					SELECT
						@VehicleVIN
						,@VehicleMakeId
						,@VehicleMakeName
						,@VehicleModel
						,@VehicleYear
						,@VehicleTransmission
						,@VehicleLookupId

				END

			IF @@TRANCOUNT > 0 COMMIT TRAN;

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

END
GO