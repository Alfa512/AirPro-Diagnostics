
DROP PROCEDURE IF EXISTS Repair.usp_GetRepairsByUser;
GO

CREATE PROCEDURE [Repair].[usp_GetRepairsByUser]
	@UserGuid UNIQUEIDENTIFIER

	,@RepairId INT = NULL
	,@AgingRepairs BIT = 0
	,@Search NVARCHAR(200) = NULL

	,@RepairStatusId INT = 1
	,@ShopGuid UNIQUEIDENTIFIER = NULL

	,@CurrentPage INT = 1
	,@RowCount INT = 25
	,@SortCol NVARCHAR(100) = 'CreatedDt'
	,@SortDir CHAR(4) = 'DESC'
AS
BEGIN

	SET NOCOUNT ON;

	/**************************************************
		Update Variables.
	**************************************************/
	SET @Search = '"' + ISNULL(NULLIF(@Search, '') + '*', '') + '"';
	SET @ShopGuid = NULLIF(@ShopGuid, Common.udf_GetEmptyGuid());
	SET @SortCol = ISNULL(NULLIF(@SortCol, ''), 'CreatedDt');
	SET @SortDir = ISNULL(NULLIF(@SortDir, ''), 'DESC');
	SET @RowCount = CASE WHEN @RowCount > 1000 THEN 1000 WHEN @RowCount < 1 THEN 25 ELSE @RowCount END;

	DECLARE @UserTimeZoneId VARCHAR(50) = Common.udf_GetUserTimeZoneId(@UserGuid);

	/**************************************************
		Search Repairs.
	**************************************************/
	DROP TABLE IF EXISTS #Repairs;
	SELECT TOP 0
		od.ShopGuid
		,od.ShopName
		,od.RepairId
		,od.RepairStatusId
		,od.RepairStatusName
		,od.ShopRONumber
		,od.InsuranceCompanyId
		,od.InsuranceCompanyOther
		,od.InsuranceCompanyDisplay
		,od.InsuranceReferenceNumber
		,od.VehicleVIN
		,od.VehicleMake
		,od.VehicleModel
		,od.VehicleYear
		,od.VehicleTransmission
		,od.VehicleManualEntryInd
		,od.Odometer
		,od.AirBagsDeployed
		,od.AirBagsVisualDeployments
		,od.DrivableInd
		,od.CreatedBy
		,od.CreatedDt
	INTO #Repairs
	FROM Repair.vwOrderDetails od

	IF (ISNULL(@AgingRepairs, 0) = 0)
	BEGIN
		INSERT INTO #Repairs
		SELECT
			od.ShopGuid
			,od.ShopName
			,od.RepairId
			,od.RepairStatusId
			,od.RepairStatusName
			,od.ShopRONumber
			,od.InsuranceCompanyId
			,od.InsuranceCompanyOther
			,od.InsuranceCompanyDisplay
			,od.InsuranceReferenceNumber
			,od.VehicleVIN
			,od.VehicleMake
			,od.VehicleModel
			,od.VehicleYear
			,od.VehicleTransmission
			,od.VehicleManualEntryInd
			,od.Odometer
			,od.AirBagsDeployed
			,od.AirBagsVisualDeployments
			,od.DrivableInd
			,od.CreatedBy
			,od.CreatedDt
		FROM Repair.vwOrderDetails od
		WHERE (od.RepairId = @RepairId -- Filter By Repair Id.
				OR (NULLIF(@RepairId, 0) IS NULL -- Apply Search IF Repair Id NULL.
					AND (@ShopGuid IS NULL OR od.ShopGuid = @ShopGuid) -- Filter By Shop Guid.
					AND (NULLIF(@Search, '""') IS NOT NULL OR od.RepairStatusId = @RepairStatusId)
					AND (NULLIF(@Search, '""') IS NULL OR CONTAINS(od.SearchText, @Search)))) -- Filter By Repair Status OR Search.
			AND od.ShopGuid IN (SELECT ShopGuid FROM Access.vwUserMemberships WHERE UserGuid = @UserGuid) -- Filter By User Access.
	END
	ELSE
	BEGIN
		INSERT INTO #Repairs
		SELECT
			od.ShopGuid
			,od.ShopName
			,od.RepairId
			,od.RepairStatusId
			,od.RepairStatusName
			,od.ShopRONumber
			,od.InsuranceCompanyId
			,od.InsuranceCompanyOther
			,od.InsuranceCompanyDisplay
			,od.InsuranceReferenceNumber
			,od.VehicleVIN
			,od.VehicleMake
			,od.VehicleModel
			,od.VehicleYear
			,od.VehicleTransmission
			,od.VehicleManualEntryInd
			,od.Odometer
			,od.AirBagsDeployed
			,od.AirBagsVisualDeployments
			,od.DrivableInd
			,od.CreatedBy
			,od.CreatedDt
		FROM Repair.vwOrderDetails od
		INNER JOIN Access.Shops s
			ON od.ShopGuid = s.ShopGuid
		WHERE od.RepairStatusId = 1 -- Filter By Active Repairs.
			AND od.ShopGuid IN (SELECT ShopGuid FROM Access.vwUserMemberships WHERE UserGuid = @UserGuid AND MembershipType IN ('Account', 'Shop')) -- Filter By Direct Access.
			AND DATEDIFF(DAY, od.CreatedDt, GETUTCDATE()) >= s.AutomaticRepairCloseDays -- Filter By Repair Age.
			AND (@Search = '""' OR CONTAINS(od.SearchText, @Search)) -- Filter By Repair Status OR Search.
	END

	/**************************************************
		Record Total Records from Search.
	**************************************************/
	DECLARE @TotalCount INT = 0;
	SELECT @TotalCount = COUNT(1)
	FROM #Repairs;

	/**************************************************
		Filter Current Page.
	**************************************************/
	DROP TABLE IF EXISTS #Results;
	SELECT
		ShopGuid
		,ShopName
		,RepairId
		,RepairStatusId
		,RepairStatusName
		,ShopRONumber
		,InsuranceCompanyId
		,InsuranceCompanyOther
		,InsuranceCompanyDisplay
		,InsuranceReferenceNumber
		,VehicleVIN
		,VehicleMake
		,VehicleModel
		,VehicleYear
		,VehicleTransmission
		,VehicleManualEntryInd
		,Odometer
		,AirBagsDeployed
		,AirBagsVisualDeployments
		,DrivableInd
		,CreatedBy
		,CreatedDt
	INTO #Results
	FROM #Repairs
	ORDER BY
		CASE WHEN @SortCol = 'RepairId' AND @SortDir = 'ASC' THEN RepairId END ASC
		,CASE WHEN @SortCol = 'RepairId' AND @SortDir = 'DESC' THEN RepairId END DESC

		,CASE WHEN @SortCol = 'ShopName' AND @SortDir = 'ASC' THEN ShopName END ASC
		,CASE WHEN @SortCol = 'ShopName' AND @SortDir = 'DESC' THEN ShopName END DESC

		,CASE WHEN @SortCol = 'RepairStatusName' AND @SortDir = 'ASC' THEN RepairStatusName END ASC
		,CASE WHEN @SortCol = 'RepairStatusName' AND @SortDir = 'DESC' THEN RepairStatusName END DESC

		,CASE WHEN @SortCol = 'ShopRONumber' AND @SortDir = 'ASC' THEN ShopRONumber END ASC
		,CASE WHEN @SortCol = 'ShopRONumber' AND @SortDir = 'DESC' THEN ShopRONumber END DESC

		,CASE WHEN @SortCol = 'InsuranceCompanyDisplay' AND @SortDir = 'ASC' THEN InsuranceCompanyDisplay END ASC
		,CASE WHEN @SortCol = 'InsuranceCompanyDisplay' AND @SortDir = 'DESC' THEN InsuranceCompanyDisplay END DESC

		,CASE WHEN @SortCol = 'InsuranceReferenceNumber' AND @SortDir = 'ASC' THEN InsuranceReferenceNumber END ASC
		,CASE WHEN @SortCol = 'InsuranceReferenceNumber' AND @SortDir = 'DESC' THEN InsuranceReferenceNumber END DESC

		,CASE WHEN @SortCol = 'VehicleVIN' AND @SortDir = 'ASC' THEN VehicleVIN END ASC
		,CASE WHEN @SortCol = 'VehicleVIN' AND @SortDir = 'DESC' THEN VehicleVIN END DESC

		,CASE WHEN @SortCol = 'VehicleMake' AND @SortDir = 'ASC' THEN VehicleMake END ASC
		,CASE WHEN @SortCol = 'VehicleMake' AND @SortDir = 'DESC' THEN VehicleMake END DESC

		,CASE WHEN @SortCol = 'VehicleModel' AND @SortDir = 'ASC' THEN VehicleModel END ASC
		,CASE WHEN @SortCol = 'VehicleModel' AND @SortDir = 'DESC' THEN VehicleModel END DESC

		,CASE WHEN @SortCol = 'VehicleYear' AND @SortDir = 'ASC' THEN VehicleYear END ASC
		,CASE WHEN @SortCol = 'VehicleYear' AND @SortDir = 'DESC' THEN VehicleYear END DESC

		,CASE WHEN @SortCol = 'VehicleTransmission' AND @SortDir = 'ASC' THEN VehicleTransmission END ASC
		,CASE WHEN @SortCol = 'VehicleTransmission' AND @SortDir = 'DESC' THEN VehicleTransmission END DESC

		,CASE WHEN @SortCol = 'Odometer' AND @SortDir = 'ASC' THEN Odometer END ASC
		,CASE WHEN @SortCol = 'Odometer' AND @SortDir = 'DESC' THEN Odometer END DESC

		,CASE WHEN @SortCol = 'CreatedBy' AND @SortDir = 'ASC' THEN CreatedBy END ASC
		,CASE WHEN @SortCol = 'CreatedBy' AND @SortDir = 'DESC' THEN CreatedBy END DESC

		,CASE WHEN @SortCol = 'CreatedDt' AND @SortDir = 'ASC' THEN CreatedDt END ASC
		,CASE WHEN @SortCol = 'CreatedDt' AND @SortDir = 'DESC' THEN CreatedDt END DESC
	OFFSET @RowCount * (@CurrentPage - 1) ROW
	FETCH NEXT @RowCount ROWS ONLY;
	DROP TABLE IF EXISTS #Repairs;

	/**************************************************
		Record Final Record Count.
	**************************************************/
	DECLARE @ResultCount INT = 0;
	SELECT @ResultCount = COUNT(1)
	FROM #Results

	/**************************************************
		Return Grid Statistics.
	**************************************************/
	SELECT
		@CurrentPage [Current]
		,@ResultCount [RowCount]
		,@TotalCount [Total];

	/**************************************************
		Return Repair Data.
	**************************************************/
	WITH Reports
	AS
	(
		SELECT
			r.OrderId
			,r.RequestTypeId
		FROM Scan.Requests r
		INNER JOIN Scan.Reports rpt
			ON r.ReportId = rpt.ReportId
		WHERE r.RequestTypeId IN (1, 3, 5, 6)
			AND rpt.CompletedInd = 1
	)

	SELECT
		r.ShopGuid
		,r.ShopName
		,r.RepairId
		,r.RepairStatusId
		,r.RepairStatusName
		,r.ShopRONumber
		,r.InsuranceCompanyId
		,r.InsuranceCompanyOther
		,r.InsuranceCompanyDisplay
		,r.InsuranceReferenceNumber
		,r.VehicleVIN
		,r.VehicleMake
		,r.VehicleModel
		,r.VehicleYear
		,r.VehicleTransmission
		,r.VehicleManualEntryInd
		,r.Odometer
		,r.AirBagsDeployed
		,r.AirBagsVisualDeployments
		,r.DrivableInd
		,r.CreatedBy
		,CAST(r.CreatedDt AT TIME ZONE @UserTimeZoneId AS DATETIME) [CreatedDt]

		,rtc.ShopRequestTypeCount [AllowedShopRequestTypesCount]

		,ar.RequestId [ActiveRequestId]
		,ar.RequestType [ActiveRequestType]
		,ar.InProgressInd [ActiveRequestInProgressInd]
		,ar.RequestTechnician [ActiveRequestTechnician]

		,CAST(ISNULL(e.EstimateInd, 0) AS BIT) [EstimateInd]

		,poi.PointOfImpactIdList
	FROM #Results r
	OUTER APPLY
	(
		SELECT TOP 1
			rq.RequestId
			,rt.TypeName [RequestType]
			,ISNULL(tp.DisplayName, ru.DisplayName) [RequestTechnician]
			,CASE WHEN rpt.ResponsibleTechnicianUserGuid IS NOT NULL OR EXISTS (SELECT TOP 1 1 FROM Scan.ReportDiagnosticResults WHERE ReportId = rpt.ReportId) THEN 1 ELSE 0 END [InProgressInd]
		FROM Scan.Requests rq
		INNER JOIN Scan.RequestTypes rt
			ON rq.RequestTypeId = rt.RequestTypeId
		LEFT JOIN Scan.Reports rpt
			LEFT JOIN Access.Users ru
				LEFT JOIN Technician.Profiles tp
					ON ru.UserGuid = tp.UserGuid
				ON rpt.ResponsibleTechnicianUserGuid = ru.UserGuid
			ON rq.ReportId = rpt.ReportId
		WHERE rq.OrderId = r.RepairId
			AND ISNULL(rpt.CompletedInd, 0) = 0
	) ar
	OUTER APPLY
	(
		SELECT COUNT(1) [ShopRequestTypeCount]
		FROM Access.ShopRequestTypes srt
		WHERE srt.ShopGuid = r.ShopGuid
			AND srt.RequestTypeId NOT IN ('6', '7')
		GROUP BY ShopGuid
	) rtc
	OUTER APPLY
	(
		SELECT 
			CASE
				WHEN r.RepairStatusId = 1
					AND ISNULL(ar.InProgressInd, 0) = 0
					AND EXISTS (SELECT 1 FROM Reports WHERE RepairId = r.RepairId AND (RequestTypeId IN (1, 5) OR (RequestTypeId = 6 AND s.AllowSelfScanAssessment = 1)))
					AND NOT EXISTS (SELECT 1 FROM Reports WHERE RepairId = r.RepairId AND RequestTypeId = 3)
				THEN 1
				ELSE 0
			END [EstimateInd]
		FROM Access.Shops s
		WHERE s.ShopGuid = r.ShopGuid
			AND s.EstimatePlanId IS NOT NULL
	) e
	OUTER APPLY
	(
		SELECT STUFF((
			SELECT ',' + CAST(PointOfImpactId AS VARCHAR(MAX))
			FROM Repair.OrderPointOfImpacts
			WHERE OrderID = r.RepairId
			FOR XML PATH('')
		), 1, 1, '') [PointOfImpactIdList]
	) poi;

	/**************************************************
		Return Download Selection.
	**************************************************/
	WITH ReportDownloads
	AS
	(
		SELECT
			r.RepairId
			,CAST(rq.RequestId AS VARCHAR(100)) [DownloadId]
			,CASE rpt.CanceledInd WHEN 1 THEN 'Cancelled' ELSE 'Report' END [DownloadType]
			,rt.TypeName [DisplayName]
			,ROW_NUMBER() OVER (PARTITION BY r.RepairId ORDER BY rq.RequestId) [SortOrder]
		FROM #Results r
		INNER JOIN Scan.Requests rq
			INNER JOIN Scan.RequestTypes rt
				ON rq.RequestTypeId = rt.RequestTypeId
			INNER JOIN Scan.Reports rpt
				ON rq.ReportId = rpt.ReportId
					AND rpt.CompletedInd = 1
			ON r.RepairId = rq.OrderId
	)

	SELECT
		RepairId
		,DownloadId
		,DownloadType
		,DisplayName
		,SortOrder
	FROM ReportDownloads

	UNION 

	SELECT
		d.RepairId
		,u.UploadId [DownloadId]
		,'File' [DownloadType]
		,u.UploadFileName + ISNULL('.' + u.UploadFileExtension, '') [DisplayName]
		,ROW_NUMBER() OVER (PARTITION BY d.RepairId ORDER BY u.CreatedDt) [SortOrder]
	FROM ReportDownloads d
	INNER JOIN Common.Uploads u
		INNER JOIN Common.UploadTypes ut
			ON u.UploadTypeId = ut.UploadTypeId
				AND ut.UploadTypeName = 'ScanRequests'
		ON d.DownloadId = TRY_CAST(u.UploadKey AS INT)
	WHERE u.UploadDeletedInd = 0

	UNION

	SELECT
		r.RepairId
		,u.UploadId [DownloadId]
		,'File' [DownloadType]
		,u.UploadFileName + ISNULL('.' + u.UploadFileExtension, '')
		,ROW_NUMBER() OVER (PARTITION BY r.RepairId ORDER BY u.CreatedDt) [SortOrder]
	FROM #Results r
	INNER JOIN Repair.Vehicles v
		INNER JOIN Repair.VehicleMakes vm
			ON v.VehicleMakeId = vm.VehicleMakeId
		ON r.VehicleVIN = v.VehicleVIN
	INNER JOIN Common.Uploads u
		INNER JOIN Common.UploadTypes ut
			ON u.UploadTypeId = ut.UploadTypeId
				AND ut.UploadTypeName = 'VehicleMakes'
		ON vm.VehicleMakeId = TRY_CAST(u.UploadKey AS INT)
	WHERE u.UploadDeletedInd = 0

END
GO