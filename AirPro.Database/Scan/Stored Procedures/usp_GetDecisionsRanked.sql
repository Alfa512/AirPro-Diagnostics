CREATE PROCEDURE Scan.usp_GetDecisionsRanked
	@UserGuid UNIQUEIDENTIFIER
	,@RequestId INT
	,@SearchPhrase VARCHAR(MAX)
AS
BEGIN

	DECLARE
		@VehicleMakeId INT
		,@RequestTypeId INT
		,@RequestCategoryId INT

	SELECT
		@VehicleMakeId = v.VehicleMakeId
		,@RequestTypeId = r.RequestTypeId
		,@RequestCategoryId = r.RequestCategoryId
	FROM Scan.Requests r
	INNER JOIN Repair.Orders o
		ON r.OrderId = o.OrderId
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	WHERE r.RequestId = @RequestId

	SELECT
		d.DecisionId
		,d.DecisionText
		,d.DefaultTextSeverity
		,ISNULL(SUM(VehicleMakeMatch) / COUNT(1), 0) [VehicleMakeUsage]
		,ISNULL(SUM(RequestTypeMatch) / COUNT(1), 0) [RequestTypeUsage]
		,ISNULL(SUM(RequestCategoryMatch) / COUNT(1), 0) [RequestCategoryUsage]
	FROM Scan.Decisions d
	LEFT JOIN 
	(
		SELECT
			v.VehicleMakeId
			,CASE WHEN v.VehicleMakeId = @VehicleMakeId THEN 1 ELSE 0 END [VehicleMakeMatch]
			,rq.RequestTypeId
			,CASE WHEN rq.RequestTypeId = @RequestTypeId THEN 1 ELSE 0 END [RequestTypeMatch]
			,rq.RequestCategoryId
			,CASE WHEN rq.RequestCategoryId = @RequestCategoryId THEN 1 ELSE 0 END [RequestCategoryMatch]
			,rd.DecisionId
		FROM Scan.ReportDecisions rd
		INNER JOIN Scan.Reports rpt
			ON rd.ReportId = rpt.ReportId
		INNER JOIN Scan.Requests rq
			ON rpt.ReportId = rq.ReportId
		INNER JOIN Repair.Orders o
			ON rq.OrderId = o.OrderId
		INNER JOIN Repair.Vehicles v
			ON o.VehicleVIN = v.VehicleVIN
	) s
		ON d.DecisionId = s.DecisionId
	WHERE d.ActiveInd = 1
		AND d.DecisionText LIKE '%' + @SearchPhrase + '%'
	GROUP BY
		d.DecisionId
		,d.DecisionText
		,d.DefaultTextSeverity

END