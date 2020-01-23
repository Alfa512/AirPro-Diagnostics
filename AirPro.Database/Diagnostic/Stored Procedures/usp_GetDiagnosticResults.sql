
CREATE PROCEDURE Diagnostic.usp_GetDiagnosticResults
	@ResultId INT = NULL
	,@RequestId INT = NULL
	,@ShopGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET @RequestId = NULLIF(@RequestId, 0);
	SET @ShopGuid = NULLIF(@ShopGuid, Common.udf_GetEmptyGuid());

	IF @ResultId IS NULL AND @RequestId IS NULL AND @ShopGuid IS NULL
		THROW 50000, 'Minimum 1 Search Parameter is Required.', 1;

	WITH
	Tools
	AS
	(
		SELECT CAST(ToolKey AS NVARCHAR(150)) [ToolKey]
		FROM Inventory.AirProTools apt
		INNER JOIN
		(
			SELECT apta.ToolId
			FROM Inventory.AirProToolAccounts apta
			INNER JOIN Access.Shops s
				ON apta.AccountGuid = s.AccountGuid
			WHERE s.ShopGuid = @ShopGuid
			UNION
			SELECT apts.ToolId
			FROM Inventory.AirProToolShops apts
			WHERE apts.ShopGuid = @ShopGuid
		) a ON apt.ToolId = a.ToolId
	),
	Results
	AS
	(
		SELECT
			r.ResultId
			,r.RequestId
			,r.DiagnosticToolId [DiagnosticTool]
			,r.CustomerFirstName
			,r.CustomerLastName
			,r.CustomerRo
			,r.ScanDateTime
			,r.ShopName
			,r.ShopAddress
			,r.ShopEmail
			,r.ShopFax
			,r.ShopPhone
			,r.VehicleVin
			,r.VehicleMake
			,r.VehicleModel
			,r.VehicleYear
			,r.TestabilityIssues
			,r.CreatedDt
		FROM Diagnostic.Results r
		LEFT JOIN Tools t
			ON t.ToolKey = r.ShopName
		WHERE r.DeletedInd = 0
			AND (@ResultId IS NULL OR r.ResultId = @ResultId) -- Search Result Id if Provided
			AND (@ResultId IS NOT NULL OR @RequestId IS NULL OR r.RequestId = @RequestId) -- Search Request Id if Provided
		
			AND (@ResultId IS NOT NULL OR @RequestId IS NOT NULL -- If Not Result or Request Id Search for Shop Guid
				OR @ShopGuid IS NULL OR r.ShopName = CAST(@ShopGuid AS NVARCHAR(150)) OR t.ToolKey IS NOT NULL)
			AND (@ShopGuid IS NULL OR r.RequestId IS NULL) -- If Searching Shop Guid Required Empty Request Id
			AND (@ResultId IS NOT NULL OR @RequestId IS NOT NULL OR r.DeletedInd = 0) -- Filter Deleted if Not Pulling by Id
	),
	Uploads
	AS
	(
		SELECT 
			u.ResultId
			,u.UploadFileTypeId [DiagnosticFileType]
			,u.UploadText [DiagnosticFileText]
		FROM Diagnostic.Uploads u
		WHERE u.ResultId IN (SELECT ResultId FROM Results)
	)

	SELECT
		r.ResultId
		,r.RequestId
		,r.DiagnosticTool
		,u.DiagnosticFileType
		,u.DiagnosticFileText
		,r.CustomerFirstName
		,r.CustomerLastName
		,r.CustomerRo
		,r.ScanDateTime
		,r.ShopName
		,r.ShopAddress
		,r.ShopEmail
		,r.ShopFax
		,r.ShopPhone
		,r.VehicleVin
		,r.VehicleMake
		,r.VehicleModel
		,r.VehicleYear
		,r.TestabilityIssues

		,ISNULL(rtc.ControllerId, rff.ControllerId) [ControllerId]
		,c.ControllerName

		,rtc.TroubleCodeId [DiagnosticTroubleCodeId]
		,tc.TroubleCode [DiagnosticTroubleCode]
		,tc.TroubleCodeDescription [DiagnosticTroubleCodeDescription]
		,rtc.TroubleCodeInformation [DiagnosticTroubleCodeInformation]

		,rff.FreezeFrameTroubleCode [FreezeFrameDiagnosticTroubleCode]
		,rff.SensorGroupsJson
	FROM Results r
	INNER JOIN Uploads u
		ON r.ResultId = u.ResultId
	LEFT JOIN Diagnostic.ResultTroubleCodes rtc
		ON r.ResultId = rtc.ResultId
	LEFT JOIN Diagnostic.TroubleCodes tc
		ON rtc.TroubleCodeId = tc.TroubleCodeId
	LEFT JOIN Diagnostic.ResultFreezeFrames rff
		ON r.ResultId = rff.ResultId
			AND rtc.ControllerId = rff.ControllerId
	LEFT JOIN Diagnostic.Controllers c
		ON c.ControllerId = ISNULL(rtc.ControllerId, rff.ControllerId)
	ORDER BY r.CreatedDt DESC

END