
CREATE PROCEDURE Reporting.usp_GetVolumeSummaryReport
	@UserGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @TimeZone NVARCHAR(100);
	SELECT TOP 1 @TimeZone = TimeZoneInfoId
	FROM Access.Users
	WHERE UserGuid = ISNULL(@UserGuid, Common.udf_GetEmptyGuid());

	WITH BaseData
	AS
	(
		SELECT DISTINCT
			rd.ShopGuid
			,rd.RequestId
			,rd.RequestTypeId
			,rd.RequestType
			,rd.ReportCompletedDt
		FROM Reporting.ReportData rd
		INNER JOIN Scan.Reports rpt WITH (NOLOCK)
			ON rd.ReportId = rpt.ReportId
				AND rpt.CanceledInd = 0
		WHERE rd.RequestId IS NOT NULL
			AND rd.ReportCompletedDt IS NOT NULL
	),
	ReportData
	AS
	(
		SELECT
			ShopGuid
			,RequestTypeId
			,RequestType
			,DATEPART(MONTH, ReportCompletedDt AT TIME ZONE @TimeZone) [ReportCompletedMonth]
			,DATEPART(YEAR, ReportCompletedDt AT TIME ZONE @TimeZone) [ReportCompletedYear]
		FROM BaseData
	),
	SummaryData
	AS
	(
		SELECT
			ReportCompletedYear
			,ReportCompletedMonth
			,RequestTypeId
			,RequestType
			,ShopGuid
			,COUNT(1) [RequestCount]
		FROM ReportData
		GROUP BY
			ReportCompletedYear
			,ReportCompletedMonth
			,RequestTypeId
			,RequestType
			,ShopGuid
	),
	ShopCount
	AS
	(
		SELECT
			sd.ReportCompletedYear
			,sd.ReportCompletedMonth
			,COUNT(DISTINCT ShopGuid) [DistinctShopCount]
			,SUM(sd.RequestCount) [RequestCount]
		FROM SummaryData sd
		GROUP BY
			ReportCompletedYear
			,ReportCompletedMonth
	),
	RequestPivot
	AS
	(
		SELECT
			ReportCompletedYear
			,ReportCompletedMonth
			,[Quick Scan] [QuickScan]
			,[Diagnostic Scan] [DiagnosticScan]
			,[Completion Scan] [CompletionScan]
			,[Follow Up Scan] [FollowUpScan]
			,[Inspection Scan] [InspectionScan]
			,[Self Scan] [SelfScan]
			,[Scan Analysis] [ScanAnalysis]
			,[Demo Scan] [DemoScan]
		FROM
		(SELECT ReportCompletedYear, ReportCompletedMonth, RequestCount, RequestType FROM SummaryData) p
		PIVOT
		(
			SUM(RequestCount)
			FOR RequestType IN
				([Quick Scan], [Diagnostic Scan], [Completion Scan], [Follow Up Scan], [Inspection Scan], [Self Scan], [Scan Analysis], [Demo Scan])
		) AS pvt
	)

	SELECT
		CONVERT(CHAR(10), DATEFROMPARTS(sc.ReportCompletedYear, sc.ReportCompletedMonth, 1), 101) [RequestDate]
		,sc.RequestCount
		,sc.DistinctShopCount
		,sc.RequestCount / sc.DistinctShopCount [AvgRequestPerShop]
		,rp.QuickScan
		,rp.DiagnosticScan
		,rp.CompletionScan
		,rp.FollowUpScan
		,rp.InspectionScan
		,rp.SelfScan
		,rp.ScanAnalysis
		,rp.DemoScan
	FROM RequestPivot rp
	LEFT JOIN ShopCount sc
		ON sc.ReportCompletedYear = rp.ReportCompletedYear
			AND sc.ReportCompletedMonth = rp.ReportCompletedMonth
	ORDER BY
		sc.ReportCompletedYear
		,sc.ReportCompletedMonth

END