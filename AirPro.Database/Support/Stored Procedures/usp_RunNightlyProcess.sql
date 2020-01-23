
CREATE PROCEDURE Support.usp_RunNightlyProcess
AS
BEGIN

	SET NOCOUNT ON;

	EXEC Diagnostic.usp_UpdateVehicleControllers;

	EXEC Service.usp_UpdateCCCInsuranceCompanies;

	EXEC Reporting.usp_BuildReportData;

END