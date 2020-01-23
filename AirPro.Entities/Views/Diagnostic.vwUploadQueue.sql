
DROP VIEW IF EXISTS Diagnostic.vwUploadQueue;
GO

CREATE VIEW Diagnostic.vwUploadQueue
AS

	SELECT
		r.ResultId
		,ISNULL(NULLIF(r.VehicleVin, ''), 'Not Reported') [VehicleVin]
		,ISNULL(NULLIF(r.VehicleMake, ''), 'Not Reported') [VehicleMake]
		,ISNULL(NULLIF(r.VehicleModel, ''), 'Not Reported') [VehicleModel]
		,ISNULL(NULLIF(r.VehicleYear, ''), 'Not Reported') [VehicleYear]
		,r.ScanDateTime
		,r.CreatedDt
		,TRY_CAST(NULLIF(RTRIM(r.ShopName), '') AS UNIQUEIDENTIFIER) [SearchGuid]
	FROM Diagnostic.Results r
	LEFT JOIN Scan.ReportDiagnosticResults rdr
		ON r.ResultId = rdr.DiagnosticResultId
	WHERE r.DeletedInd = 0
		AND r.RequestId IS NULL
		AND rdr.ReportId IS NULL

GO