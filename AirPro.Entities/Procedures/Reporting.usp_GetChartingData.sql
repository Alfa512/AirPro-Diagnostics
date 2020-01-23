
CREATE PROCEDURE Reporting.usp_GetChartingData
	@ShopGuid UNIQUEIDENTIFIER = NULL
	,@Offset CHAR(10) = '-00:00'
	,@TimeFrame CHAR(5) = 'WEEK'
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @StartDate DATE =
		CASE @TimeFrame
			WHEN 'WEEK' THEN DATEADD(WEEK, -1, Common.udf_GetLocalDateTime(GETUTCDATE(), @Offset))
			WHEN 'MONTH' THEN DATEADD(MONTH, -1, Common.udf_GetLocalDateTime(GETUTCDATE(), @Offset))
			WHEN 'YEAR' THEN DATEADD(YEAR, -1, Common.udf_GetLocalDateTime(GETUTCDATE(), @Offset))
		END

	SELECT
		bd.RequestDt
		,bd.RequestTypeName
		,Common.udf_GetDisplayName(u.LastName, u.FirstName) [RequestTechnician]
		,AVG(bd.CycleTimeMinutes) [AvgCycleTimeMinutes]
		,SUM(bd.InvoiceAmount) [SumInvoiceAmount]
	FROM
	(
		SELECT
			CASE @TimeFrame
				WHEN 'WEEK' THEN CAST(Common.udf_GetLocalDateTime(req.CreatedDt, @Offset) AS DATE)
				WHEN 'MONTH' THEN CAST(Common.udf_GetLocalDateTime(req.CreatedDt, @Offset) AS DATE)
				WHEN 'YEAR' THEN CAST(Common.udf_GetLastDayOfWeek(Common.udf_GetLocalDateTime(req.CreatedDt, @Offset)) AS DATE)
			END [RequestDt]
			,rt.TypeName [RequestTypeName]
			,COALESCE(rpt.ResponsibleTechnicianUserGuid, rpt.UpdatedByUserGuid, rpt.CreatedByUserGuid) [RequestTechUserGuid]
			,DATEDIFF(MINUTE, Common.udf_GetLocalDateTime(req.CreatedDt, @Offset), Common.udf_GetLocalDateTime(rpt.CompletedDt, @Offset)) [CycleTimeMinutes]
			,ISNULL(rpt.InvoiceAmount, 0) [InvoiceAmount]
		FROM Scan.Requests req
		INNER JOIN Scan.RequestTypes rt
			ON req.RequestTypeId = rt.RequestTypeId
		INNER JOIN Scan.Reports rpt
			ON req.ReportId = rpt.ReportId
				AND rpt.CompletedInd = 1
		INNER JOIN Repair.Orders o
			ON req.OrderId = o.OrderId
				AND o.ShopGuid = ISNULL(@ShopGuid, o.ShopGuid)
		WHERE req.CreatedDt BETWEEN @StartDate AND CAST(GETUTCDATE() AS DATE)
	) bd
	INNER JOIN Access.Users u
		ON bd.RequestTechUserGuid = u.UserGuid
	GROUP BY
		bd.RequestDt
		,bd.RequestTypeName
		,Common.udf_GetDisplayName(u.LastName, u.FirstName)

END
GO