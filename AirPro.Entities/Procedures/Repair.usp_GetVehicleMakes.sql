
CREATE PROCEDURE [Repair].[usp_GetVehicleMakes]
	@search VARCHAR(MAX) = NULL
	, @vehicleMakeId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SET @Search = '%' + ISNULL(@Search, '') + '%';
	SET @vehicleMakeId = NULLIF(@vehicleMakeId, 0);

	SELECT VM.VehicleMakeId, VM.VehicleMakeName, VM.VehicleMakeTypeId, VMT.VehicleMakeTypeName, VM.ProgramName, VM.ProgramInstructions
	FROM Repair.VehicleMakes VM WITH(NOLOCK)
		INNER JOIN Repair.VehicleMakeTypes VMT WITH(NOLOCK) ON VMT.VehicleMakeTypeId = VM.VehicleMakeTypeId
	WHERE (@vehicleMakeId IS NOT NULL AND VM.VehicleMakeId = @vehicleMakeId)
		OR
		(
			@vehicleMakeId IS NULL
			AND
			(
				VM.VehicleMakeName LIKE @Search
				OR VMT.VehicleMakeTypeName LIKE @Search
			)
		)
END
GO