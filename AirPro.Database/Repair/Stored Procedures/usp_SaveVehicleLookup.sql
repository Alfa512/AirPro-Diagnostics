
CREATE PROCEDURE [Repair].[usp_SaveVehicleLookup]
	@VehicleVIN CHAR(17)
	,@Service INT
	,@RequestBaseURL VARCHAR(MAX)
	,@RequestString VARCHAR(MAX)
	,@RequestSuccess BIT
	,@ResponseStatusCode INT
	,@RequestMessage VARCHAR(MAX)
	,@ResponseContent VARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	INSERT INTO Repair.VehicleLookups
	(
		VehicleVIN
		,Service
		,RequestBaseURL
		,RequestString
		,RequestSuccess
		,ResponseStatusCode
		,RequestMessage
		,ResponseContent
		,RequestDt
	)
	VALUES
	(
		@VehicleVIN
		,@Service
		,@RequestBaseURL
		,@RequestString
		,@RequestSuccess
		,@ResponseStatusCode
		,@RequestMessage
		,@ResponseContent
		,GETUTCDATE()
	)

	SELECT SCOPE_IDENTITY()

END