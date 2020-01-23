
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Service' AND ROUTINE_NAME = 'usp_GetMitchellReport')
	DROP PROCEDURE Service.usp_GetMitchellReport
GO

CREATE PROCEDURE Service.usp_GetMitchellReport
	@RequestId INT
	,@UserGuid UNIQUEIDENTIFIER = NULL
	,@TimestampOffset DATETIMEOFFSET = NULL
AS
BEGIN

	SET NOCOUNT ON;

	/**************************************************
		Set Defaults
	**************************************************/
	SET @UserGuid = ISNULL(@UserGuid, Common.udf_GetEmptyGuid());
	SET @TimestampOffset = ISNULL(@TimestampOffset, GETUTCDATE());
	DECLARE @UserTimeZoneId VARCHAR(100);
	SELECT TOP 1 @UserTimeZoneId = u.TimeZoneInfoId
	FROM Access.Users u WHERE u.UserGuid = @UserGuid
	
	/**************************************************
		Store Report Lookup.
	**************************************************/
	IF EXISTS (SELECT 1 FROM Scan.Requests r
					INNER JOIN Repair.Orders o ON r.OrderId = o.OrderId
					INNER JOIN Access.Shops s ON o.ShopGuid = s.ShopGuid
					WHERE r.RequestId = @RequestId AND s.SendToMitchellInd = 1)
		INSERT INTO Service.MitchellReports (RequestId, RequestUserGuid, RequestDt)
		VALUES (@RequestId, @UserGuid, @TimestampOffset)

	/**************************************************
		Return Report.
	**************************************************/
	SELECT
		'2.10.0.14' [Version]
		,'AirPro' [ScanMaker]
		,'AirPro' [ScanTool]
		,rpt.CompletedDt AT TIME ZONE 'UTC' [ScanTimeStamp]
		,r.CreatedDt AT TIME ZONE @UserTimeZoneId [UploadTimeStamp]
		,rpt.CompletedDt AT TIME ZONE @UserTimeZoneId [LocalTimeStamp]
		,CAST(o.ShopGuid AS VARCHAR(36)) [ScanToolId]
		,r.RequestId [ScanReportId]
		,CASE r.RequestCategoryId
			WHEN 1 THEN 'PRE'
			WHEN 2 THEN 'POST'
			ELSE 'OTHER'
		END [ScanDesignation]
		,'Full' [ScanType]
		,NULL [ScanFullOrPartial]
		,o.ShopReferenceNumber [RONumber]
		,NULL [RepairOrder]
		,NULL [PrePostScan]
		,rt.TypeName [ScanSubType]

		,v.VehicleVIN [Vin]
		,v.Year
		,vm.VehicleMakeName [Make]
		,v.Model
		,NULL [Body]
		,NULL [Engine]
		,CASE WHEN v.VehicleLookupId IS NOT NULL THEN 'Y' ELSE 'N' END [VinVerified]
		,c.AlphaCode2 [CountryCode]
	FROM Scan.Requests r
	INNER JOIN Repair.Orders o
		INNER JOIN Access.Shops s
			INNER JOIN Common.States st
				INNER JOIN Common.Countries c
					ON st.CountryId = c.CountryId
				ON s.StateId = st.StateId
			ON o.ShopGuid = s.ShopGuid
				AND s.SendToMitchellInd = 1
		ON r.OrderId = o.OrderId
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Repair.VehicleMakes vm
		ON v.VehicleMakeId = vm.VehicleMakeId
	INNER JOIN Scan.RequestTypes rt
		ON r.RequestTypeId = rt.RequestTypeId
	LEFT JOIN Scan.RequestCategories rc
		ON r.RequestCategoryId = rc.RequestCategoryId
	LEFT JOIN Scan.Reports rpt
		ON r.ReportId = rpt.ReportId
	WHERE r.RequestId = @RequestId

END
GO