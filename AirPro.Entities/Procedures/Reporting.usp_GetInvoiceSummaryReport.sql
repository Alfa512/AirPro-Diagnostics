IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetInvoiceSummaryReport')
	DROP PROCEDURE Reporting.usp_GetInvoiceSummaryReport
GO

CREATE PROCEDURE Reporting.usp_GetInvoiceSummaryReport
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
		SELECT
			rd.ShopGuid
			,rd.RequestId
			,rd.RequestTypeId
			,rd.RequestType
			,rd.ReportInvoicedDt
			,rd.ReportInvoicedAmount
			,MAX(rd.ReportInvoiceDiscountAmount) [ReportInvoiceDiscountAmount]
		FROM Reporting.ReportData rd
		WHERE rd.ReportInvoicedInd = 1
		GROUP BY
			rd.ShopGuid
			,rd.RequestId
			,rd.RequestTypeId
			,rd.RequestType
			,rd.ReportInvoicedDt
			,rd.ReportInvoicedAmount
	),
	ReportData
	AS
	(
		SELECT
			ShopGuid
			,RequestTypeId
			,RequestType
			,ReportInvoicedAmount
			,ReportInvoiceDiscountAmount
			,DATEPART(MONTH, ReportInvoicedDt AT TIME ZONE @TimeZone) [ReportInvoicedMonth]
			,DATEPART(YEAR, ReportInvoicedDt AT TIME ZONE @TimeZone) [ReportInvoicedYear]
		FROM BaseData
	),
	SummaryData
	AS
	(
		SELECT
			ReportInvoicedYear
			,ReportInvoicedMonth
			,RequestTypeId
			,RequestType
			,ShopGuid
			,SUM(ReportInvoicedAmount) [ReportInvoicedAmount]
			,SUM(ReportInvoiceDiscountAmount) [ReportInvoiceDiscountAmount]
			,COUNT(1) [RequestCount]
		FROM ReportData
		GROUP BY
			ReportInvoicedYear
			,ReportInvoicedMonth
			,RequestTypeId
			,RequestType
			,ShopGuid	
	),
	ShopCount
	AS
	(
		SELECT
			sd.ReportInvoicedYear
			,sd.ReportInvoicedMonth
			,COUNT(DISTINCT ShopGuid) [DistinctShopCount]
			,SUM(sd.RequestCount) [RequestCount]
		FROM SummaryData sd
		GROUP BY
			ReportInvoicedYear
			,ReportInvoicedMonth
	),
	InvoiceAmountPivot
	AS
	(
		SELECT
			ReportInvoicedYear
			,ReportInvoicedMonth
			,[Quick Scan] [QuickScanInvoicedTotal]
			,[Diagnostic Scan] [DiagnosticScanInvoicedTotal]
			,[Completion Scan] [CompletionScanInvoicedTotal]
			,[Follow Up Scan] [FollowUpScanInvoicedTotal]
			,[Inspection Scan] [InspectionScanInvoicedTotal]
			,[Self Scan] [SelfScanInvoicedTotal]
			,[Scan Analysis] [ScanAnalysisInvoicedTotal]
			,[Demo Scan] [DemoScanInvoicedTotal]
		FROM
		(SELECT ReportInvoicedYear, ReportInvoicedMonth, ReportInvoicedAmount, RequestType FROM SummaryData) p
		PIVOT
		(
			SUM(ReportInvoicedAmount)
			FOR RequestType IN
				([Quick Scan], [Diagnostic Scan], [Completion Scan], [Follow Up Scan], [Inspection Scan], [Self Scan], [Scan Analysis], [Demo Scan])
		) AS pvt
	),
	InvoiceDiscountPivot
	AS
	(
		SELECT
			ReportInvoicedYear
			,ReportInvoicedMonth
			,[Quick Scan] [QuickScanDiscountTotal]
			,[Diagnostic Scan] [DiagnosticScanDiscountTotal]
			,[Completion Scan] [CompletionScanDiscountTotal]
			,[Follow Up Scan] [FollowUpScanDiscountTotal]
			,[Inspection Scan] [InspectionScanDiscountTotal]
			,[Self Scan] [SelfScanDiscountTotal]
			,[Scan Analysis] [ScanAnalysisDiscountTotal]
			,[Demo Scan] [DemoScanDiscountTotal]
		FROM
		(SELECT ReportInvoicedYear, ReportInvoicedMonth, ReportInvoiceDiscountAmount, RequestType FROM SummaryData) p
		PIVOT
		(
			SUM(ReportInvoiceDiscountAmount)
			FOR RequestType IN
				([Quick Scan], [Diagnostic Scan], [Completion Scan], [Follow Up Scan], [Inspection Scan], [Self Scan], [Scan Analysis], [Demo Scan])
		) AS pvt
	)

	SELECT
		CONVERT(CHAR(10), DATEFROMPARTS(sc.ReportInvoicedYear, sc.ReportInvoicedMonth, 1), 101) [InvoiceDate]
		,sc.DistinctShopCount
		,sc.RequestCount
		,sc.RequestCount / sc.DistinctShopCount [AvgRequestPerShop]
		,ISNULL(iap.QuickScanInvoicedTotal, 0)
			+ ISNULL(iap.DiagnosticScanInvoicedTotal, 0)
			+ ISNULL(iap.CompletionScanInvoicedTotal, 0)
			+ ISNULL(iap.FollowUpScanInvoicedTotal, 0)
			+ ISNULL(iap.InspectionScanInvoicedTotal, 0)
			+ ISNULL(iap.SelfScanInvoicedTotal, 0)
			+ ISNULL(iap.ScanAnalysisInvoicedTotal, 0)
			+ ISNULL(iap.DemoScanInvoicedTotal, 0) [InvoicedTotal]
		,ISNULL(idp.QuickScanDiscountTotal, 0)
			+ ISNULL(idp.DiagnosticScanDiscountTotal, 0)
			+ ISNULL(idp.CompletionScanDiscountTotal, 0)
			+ ISNULL(idp.FollowUpScanDiscountTotal, 0)
			+ ISNULL(idp.InspectionScanDiscountTotal, 0)
			+ ISNULL(idp.SelfScanDiscountTotal, 0)
			+ ISNULL(idp.ScanAnalysisDiscountTotal, 0)
			+ ISNULL(idp.DemoScanDiscountTotal, 0) [DiscountTotal]
		,iap.QuickScanInvoicedTotal
		,idp.QuickScanDiscountTotal
		,iap.DiagnosticScanInvoicedTotal
		,idp.DiagnosticScanDiscountTotal
		,iap.CompletionScanInvoicedTotal
		,idp.CompletionScanDiscountTotal
		,iap.FollowUpScanInvoicedTotal
		,idp.FollowUpScanDiscountTotal
		,iap.InspectionScanInvoicedTotal
		,idp.InspectionScanDiscountTotal
		,iap.SelfScanInvoicedTotal
		,idp.SelfScanDiscountTotal
		,iap.ScanAnalysisInvoicedTotal
		,idp.ScanAnalysisDiscountTotal
		,iap.DemoScanInvoicedTotal
		,idp.DemoScanDiscountTotal
	FROM ShopCount sc
	LEFT JOIN InvoiceAmountPivot iap
		ON sc.ReportInvoicedYear = iap.ReportInvoicedYear
			AND sc.ReportInvoicedMonth = iap.ReportInvoicedMonth
	LEFT JOIN InvoiceDiscountPivot idp
		ON sc.ReportInvoicedYear = idp.ReportInvoicedYear
			AND sc.ReportInvoicedMonth = idp.ReportInvoicedMonth
	ORDER BY
		sc.ReportInvoicedYear
		,sc.ReportInvoicedMonth

END
GO