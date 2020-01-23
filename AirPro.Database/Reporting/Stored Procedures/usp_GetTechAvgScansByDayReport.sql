CREATE PROCEDURE Reporting.usp_GetTechAvgScansByDayReport
	@UserGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @TimeZone NVARCHAR(100);
	SELECT TOP 1 @TimeZone = TimeZoneInfoId
	FROM Access.Users
	WHERE UserGuid = ISNULL(@UserGuid, Common.udf_GetEmptyGuid());

	WITH Base
	AS
	(
		SELECT
			RequestId
			,RequestTypeId
			,ReportTechUser
			,CompletedDt
		FROM
		(
			SELECT
				rd.RequestId
				,rd.RequestTypeId
				,rd.ReportTechUser
				,CAST(rd.ReportCompletedDt AT TIME ZONE @TimeZone AS DATE) [CompletedDt]
			FROM Reporting.ReportData rd
			WHERE rd.RequestId IS NOT NULL
				AND rd.ReportTechUser IS NOT NULL
				AND rd.ReportCompletedDt IS NOT NULL
		) rd
		GROUP BY
			RequestId
			,RequestTypeId
			,ReportTechUser
			,CompletedDt
	),
	Daily
	AS
	(
		SELECT
			b.RequestTypeId
			,b.ReportTechUser
			,b.CompletedDt
			,COUNT(DISTINCT b.RequestId) [RequestCount]
		FROM Base b
		GROUP BY
			b.RequestTypeId
			,b.ReportTechUser
			,b.CompletedDt
	),
	OverAll
	AS
	(
		SELECT
			d.RequestTypeId
			,d.ReportTechUser
			,AVG(d.RequestCount) [LifetimeAvg]
		FROM Daily d
		GROUP BY
			d.RequestTypeId
			,d.ReportTechUser
	),
	SevenDay
	AS
	(
		SELECT
			d.RequestTypeId
			,d.ReportTechUser
			,AVG(d.RequestCount) [SevenDayAvg]
		FROM Daily d
		WHERE d.CompletedDt >= DATEADD(DAY, -7, GETDATE())
		GROUP BY
			d.RequestTypeId
			,d.ReportTechUser
	),
	Monthly
	AS
	(
		SELECT
			d.RequestTypeId
			,d.ReportTechUser
			,AVG(d.RequestCount) [MonthlyAvg]
		FROM Daily d
		WHERE d.CompletedDt >= DATEADD(DAY, -30, GETDATE())
		GROUP BY
			d.RequestTypeId
			,d.ReportTechUser
	)

	SELECT
		o.ReportTechUser
		,rt.TypeName [RequestType]
		,s.SevenDayAvg
		,m.MonthlyAvg
		,o.LifetimeAvg
		,t.TotalRequestCount
	FROM OverAll o
	LEFT JOIN SevenDay s
		ON o.RequestTypeId = s.RequestTypeId
			AND o.ReportTechUser = s.ReportTechUser
	LEFT JOIN Monthly m
		ON o.RequestTypeId = m.RequestTypeId
			AND o.ReportTechUser = m.ReportTechUser
	OUTER APPLY
	(
		SELECT SUM(d.RequestCount) [TotalRequestCount]
		FROM Daily d
		WHERE d.RequestTypeId = o.RequestTypeId
			AND d.ReportTechUser = o.ReportTechUser
	) t
	INNER JOIN Scan.RequestTypes rt
		ON o.RequestTypeId = rt.RequestTypeId
	ORDER BY
		o.ReportTechUser
		,rt.SortOrder

END