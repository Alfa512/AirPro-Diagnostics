
DROP PROCEDURE IF EXISTS Scan.usp_GetRequestsByUser;
GO
DROP PROCEDURE IF EXISTS Scan.usp_GetRequestsByUser;
GO

CREATE PROCEDURE Scan.usp_GetRequestsByUser
	@UserGuid UNIQUEIDENTIFIER
	,@CompletedInd BIT = 0
	,@Search NVARCHAR(200) = NULL
	,@CurrentPage INT = 1
	,@RowCount INT = 25
	,@SortCol NVARCHAR(100) = 'RequestCreatedDt'
	,@SortDir CHAR(4) = 'ASC'
AS
BEGIN

	SET NOCOUNT ON;

	/**************************************************
		Update Variables.
	**************************************************/
	SET @Search = NULLIF(@Search, '');
	SET @SortDir = ISNULL(NULLIF(@SortDir, ''), 'ASC');
	SET @SortCol = ISNULL(NULLIF(@SortCol, ''), 'RequestCreatedDt');
	SET @RowCount = CASE WHEN @RowCount > 1000 THEN 1000 WHEN @RowCount < 1 THEN 25 ELSE @RowCount END;

	DECLARE @UserTimeZoneId VARCHAR(50) = Common.udf_GetUserTimeZoneId(@UserGuid);
	DECLARE @FullTextSearch NVARCHAR(200) = '"' + ISNULL(NULLIF(@Search, '') + '*', '') + '"';

	/**************************************************
		Search Requests.
	**************************************************/
	DROP TABLE IF EXISTS #Requests;
	SELECT
		r.RequestId
		,r.RequestTypeId
		,r.RequestTypeName
		,r.RequestCategoryId
		,rc.RequestCategoryName
		,r.RequestCreatedDt
	
		,o.RepairId
		,o.RepairStatusName
		,o.ShopGuid
		,o.ShopName
		,o.InsuranceCompanyDisplay
		,o.VehicleVIN
		,o.VehicleMake [VehicleMakeName]
		,o.VehicleModel [VehicleModelName]
		,o.VehicleYear

		,rpt.ResponsibleTechnicianUserGuid
		,ISNULL(rpt.CompletedInd, 0) [CompletedInd]
		,ISNULL(rpt.CanceledInd, 0) [CancelledInd]
	INTO #Requests
	FROM Scan.vwRequestDetails r
	INNER JOIN Repair.vwOrderDetails o
		INNER JOIN
		(
			SELECT ShopGuid
			FROM Access.vwUserMemberships
			WHERE UserGuid = @UserGuid
			GROUP BY ShopGuid
		) s ON s.ShopGuid = o.ShopGuid
		ON r.RepairId = o.RepairId
	LEFT JOIN Scan.RequestCategories rc
		ON r.RequestCategoryId = rc.RequestCategoryId
	LEFT JOIN Scan.Reports rpt
		LEFT JOIN
		(
			SELECT u.UserGuid
			FROM Access.Users u
			LEFT JOIN Technician.Profiles p
				ON u.UserGuid = p.UserGuid
			WHERE u.FirstName LIKE @Search + '%'
				OR u.LastName LIKE @Search + '%'
				OR p.DisplayName LIKE @Search + '%'
		) us ON rpt.ResponsibleTechnicianUserGuid = us.UserGuid
		ON r.ReportId = rpt.ReportId
	WHERE (@Search IS NULL AND ISNULL(rpt.CompletedInd, 0) = @CompletedInd)
			OR (@Search IS NOT NULL
				AND (CONTAINS(r.SearchText, @FullTextSearch)
					OR CONTAINS(o.SearchText, @FullTextSearch)
					OR us.UserGuid IS NOT NULL))

	/**************************************************
		Record Total Records from Search.
	**************************************************/
	DECLARE @TotalCount INT = 0;
	SELECT @TotalCount = COUNT(1)
	FROM #Requests;

	/**************************************************
		Filter Current Page.
	**************************************************/
	DROP TABLE IF EXISTS #Results;
	SELECT
		RequestId
		,RequestTypeId
		,RequestTypeName
		,RequestCategoryId
		,RequestCategoryName
		,RequestCreatedDt
		,RepairId
		,RepairStatusName
		,ShopGuid
		,ShopName
		,VehicleVIN
		,VehicleMakeName
		,VehicleModelName
		,VehicleYear
		,InsuranceCompanyDisplay
		,ResponsibleTechnicianUserGuid
		,CompletedInd
		,CancelledInd
	INTO #Results
	FROM #Requests
	ORDER BY
		CASE WHEN @SortCol = 'RequestId' AND @SortDir = 'ASC' THEN RequestId END ASC
		,CASE WHEN @SortCol = 'RequestId' AND @SortDir = 'DESC' THEN RequestId END DESC
			
		,CASE WHEN @SortCol = 'RequestTypeName' AND @SortDir = 'ASC' THEN RequestTypeName END ASC
		,CASE WHEN @SortCol = 'RequestTypeName' AND @SortDir = 'DESC' THEN RequestTypeName END DESC

		,CASE WHEN @SortCol = 'RequestCategoryName' AND @SortDir = 'ASC' THEN RequestCategoryName END ASC
		,CASE WHEN @SortCol = 'RequestCategoryName' AND @SortDir = 'DESC' THEN RequestCategoryName END DESC

		,CASE WHEN @SortCol = 'RequestCreatedDt' AND @SortDir = 'ASC' THEN RequestCreatedDt END ASC
		,CASE WHEN @SortCol = 'RequestCreatedDt' AND @SortDir = 'DESC' THEN RequestCreatedDt END DESC

		,CASE WHEN @SortCol = 'RepairId' AND @SortDir = 'ASC' THEN RepairId END ASC
		,CASE WHEN @SortCol = 'RepairId' AND @SortDir = 'DESC' THEN RepairId END DESC

		,CASE WHEN @SortCol = 'RepairStatusName' AND @SortDir = 'ASC' THEN RepairStatusName END ASC
		,CASE WHEN @SortCol = 'RepairStatusName' AND @SortDir = 'DESC' THEN RepairStatusName END DESC

		,CASE WHEN @SortCol = 'ShopName' AND @SortDir = 'ASC' THEN ShopName END ASC
		,CASE WHEN @SortCol = 'ShopName' AND @SortDir = 'DESC' THEN ShopName END DESC

		,CASE WHEN @SortCol = 'VehicleVIN' AND @SortDir = 'ASC' THEN VehicleVIN END ASC
		,CASE WHEN @SortCol = 'VehicleVIN' AND @SortDir = 'DESC' THEN VehicleVIN END DESC

		,CASE WHEN @SortCol = 'VehicleMakeName' AND @SortDir = 'ASC' THEN VehicleMakeName END ASC
		,CASE WHEN @SortCol = 'VehicleMakeName' AND @SortDir = 'DESC' THEN VehicleMakeName END DESC

		,CASE WHEN @SortCol = 'VehicleModelName' AND @SortDir = 'ASC' THEN VehicleModelName END ASC
		,CASE WHEN @SortCol = 'VehicleModelName' AND @SortDir = 'DESC' THEN VehicleModelName END DESC

		,CASE WHEN @SortCol = 'VehicleYear' AND @SortDir = 'ASC' THEN VehicleYear END ASC
		,CASE WHEN @SortCol = 'VehicleYear' AND @SortDir = 'DESC' THEN VehicleYear END DESC
	OFFSET @RowCount * (@CurrentPage - 1) ROW
	FETCH NEXT @RowCount ROWS ONLY
	DROP TABLE IF EXISTS #Requests;

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
		Return Request Data.
	**************************************************/
	SELECT
		r.RequestId
		,r.RequestTypeId
		,r.RequestTypeName
		,r.RequestCategoryId
		,r.RequestCategoryName
		,r.RepairId
		,r.RepairStatusName
		,r.ShopGuid
		,r.ShopName
		,r.VehicleVIN
		,r.VehicleMakeName
		,r.VehicleModelName
		,r.VehicleYear
		,r.InsuranceCompanyDisplay [InsuranceCompanyName]

		,CAST(r.RequestCreatedDt AT TIME ZONE 'UTC' AS DATETIME) [RequestCreateDtUtc]
		,CAST(r.RequestCreatedDt AT TIME ZONE @UserTimeZoneId AS DATETIME) [RequestCreateDt]
		
		,r.CompletedInd
		,r.CancelledInd

		,ISNULL(p.DisplayName, CAST(ru.DisplayName AS NVARCHAR(128))) [TechnicianName]
		,CAST(ru.ContactNumber AS NVARCHAR(100)) [TechnicianContactNumber]
		,CAST(ru.PhoneNumber AS NVARCHAR(100)) [TechnicianMobileNumber]

		,rq.ProblemDescription

		,CAST(u.CreatedDt AT TIME ZONE @UserTimeZoneId AS DATETIME) [ScanUploadDt]
	FROM #Results r
	LEFT JOIN Access.Users ru
		LEFT JOIN Technician.Profiles p
			ON ru.UserGuid = p.UserGuid
		ON r.ResponsibleTechnicianUserGuid = ru.UserGuid
	LEFT JOIN Scan.Requests rq
		ON r.RequestId = rq.RequestId
	OUTER APPLY
	(
		SELECT TOP (1) dr.CreatedDt
		FROM Diagnostic.Results dr
		WHERE dr.RequestId = r.RequestId
		ORDER BY dr.ResultId DESC
	) u
	DROP TABLE IF EXISTS #Results;

END
GO