
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetRequestTypeCountByDate')
	DROP PROCEDURE Reporting.usp_GetRequestTypeCountByDate
GO

CREATE PROCEDURE Reporting.usp_GetRequestTypeCountByDate
	@UserGuid UNIQUEIDENTIFIER,
	@ShopGuid UNIQUEIDENTIFIER = NULL,
	@SearchDate DATETIME = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZoneId VARCHAR(100) = Common.udf_GetUserTimeZoneId(@UserGuid);

	DECLARE @Today DATE = CAST(GETUTCDATE() AT TIME ZONE @UserTimeZoneId AS DATE);

	WITH Requests
	AS
	(
		SELECT
			r.RequestId
			,CASE WHEN ISNULL(rpt.CanceledInd, 0) = 1 THEN 0 ELSE r.RequestTypeId END [RequestTypeId]
			,CAST(r.CreatedDt AT TIME ZONE @UserTimeZoneId AS DATE) [CreatedDt]
		FROM Scan.Requests r
		INNER JOIN Repair.Orders o
			ON r.OrderId = o.OrderId
				AND (@ShopGuid IS NULL OR o.ShopGuid = @ShopGuid)
		LEFT JOIN Scan.Reports rpt
			ON r.ReportId = rpt.ReportId
	),
	Summary
	AS
	(
		SELECT
			r.RequestTypeId
			,COUNT(1) [RequestTypeCount]
		FROM Requests r
		WHERE r.CreatedDt = CAST(ISNULL(@SearchDate, @Today) AS DATE)
		GROUP BY
			r.RequestTypeId
	)

	SELECT
		ISNULL(rt.TypeName, 'Canceled') [TypeName]
		,ISNULL(rt.SortOrder, 999) [TypeSortOrder]
		,s.RequestTypeCount [TypeCount]
	FROM Summary s
	LEFT JOIN Scan.RequestTypes rt
		ON s.RequestTypeId = rt.RequestTypeId
	ORDER BY ISNULL(rt.SortOrder, 999)

END
GO
