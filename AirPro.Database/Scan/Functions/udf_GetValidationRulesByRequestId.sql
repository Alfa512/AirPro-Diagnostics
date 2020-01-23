
CREATE FUNCTION Scan.udf_GetValidationRulesByRequestId
(
	@RequestId INT
)
RETURNS @Result TABLE 
(
	ValidationRuleId INT
	,ValidationRuleResultInd BIT
)
AS
BEGIN

	DECLARE
		@RepairId INT
		,@ReportId INT
		,@RequestTypeId INT
		,@TechnicianNotes NVARCHAR(MAX);

	SELECT
		@RepairId = r.OrderId
		,@ReportId = r.ReportId
		,@RequestTypeId = r.RequestTypeId
		,@TechnicianNotes = rpt.TechnicianNotes
	FROM Scan.Requests r
	LEFT JOIN Scan.Reports rpt
		ON r.ReportId = rpt.ReportId
	WHERE r.RequestId = @RequestId;

	WITH RepairRequests
	AS
	(
		SELECT
			r.RequestCategoryId
			,COUNT(1) [RequestCategoryCount]
		FROM Scan.Requests r
		WHERE r.OrderId = @RepairId
			AND r.RequestId <> @RequestId
		GROUP BY
			r.RequestCategoryId
	),
	RequestCodes
	AS
	(
		SELECT
			ISNULL(SUM(CASE WHEN rtcr.CodeClearedInd = 0 THEN 1 ELSE 0 END), 0) [TroubleCodeCount]
			,ISNULL(SUM(CASE WHEN rtcr.CodeClearedInd = 1 THEN 1 ELSE 0 END), 0) [ClearedCount]
		FROM Scan.ReportOrderTroubleCodes rotc
		INNER JOIN Diagnostic.TroubleCodes tc
			ON tc.TroubleCodeId = rotc.TroubleCodeId
				AND NULLIF(RTRIM(tc.TroubleCode), '') IS NOT NULL
		LEFT JOIN Scan.ReportTroubleCodeRecommendations rtcr
			ON rtcr.ReportOrderTroubleCodeId = rotc.ReportOrderTroubleCodeId
				AND rtcr.ReportId = @ReportId
		WHERE rotc.OrderId = @RepairId
			AND rotc.RequestId <= @RequestId
			AND ISNULL(rtcr.ExcludeFromReportInd, 0) = 0
	)

	INSERT INTO @Result
	(
	    ValidationRuleId,
	    ValidationRuleResultInd
	)
	SELECT
		rtvr.ValidationRuleId
		,CAST(CASE rtvr.ValidationRuleId
			WHEN 1 THEN -- Pre Scan Performed
				CASE WHEN ISNULL((SELECT SUM(RequestCategoryCount) FROM RepairRequests WHERE RequestCategoryId = 1), 0) > 0 THEN 1 ELSE 0 END
			WHEN 2 THEN -- Post Scan Performed
				CASE WHEN ISNULL((SELECT SUM(RequestCategoryCount) FROM RepairRequests WHERE RequestCategoryId = 2), 0) > 0 THEN 1 ELSE 0 END
			WHEN 3 THEN -- Notes Entered
				CASE WHEN NULLIF(RTRIM(@TechnicianNotes), '') IS NOT NULL THEN 1 ELSE 0 END
			WHEN 4 THEN -- Codes Exist
				CASE WHEN ISNULL((SELECT SUM(TroubleCodeCount + ClearedCount) FROM RequestCodes), 0) > 0 THEN 1 ELSE 0 END
			WHEN 5 THEN -- NO Codes Exist
				CASE WHEN ISNULL((SELECT SUM(TroubleCodeCount + ClearedCount) FROM RequestCodes), 0) = 0 THEN 1 ELSE 0 END
			WHEN 6 THEN -- Codes Cleared (if Exist)
				CASE WHEN ISNULL((SELECT SUM(TroubleCodeCount) FROM RequestCodes), 0) = 0 THEN 1 ELSE 0 END
			WHEN 7 THEN -- Work Types Selected
				CASE WHEN ISNULL((SELECT COUNT(1) FROM Scan.ReportWorkTypes WHERE ReportId = @ReportId), 0) > 0 THEN 1 ELSE 0 END
			WHEN 8 THEN -- Decisions Selected
				CASE WHEN ISNULL((SELECT COUNT(1) FROM Scan.ReportDecisions WHERE ReportId = @ReportId), 0) > 0 THEN 1 ELSE 0 END
			ELSE 0
		END AS BIT) [ValidationRuleResultInd]
	FROM Scan.RequestTypeValidationRules rtvr
	INNER JOIN Scan.ValidationRules vr
		ON vr.ValidationRuleId = rtvr.ValidationRuleId
			AND vr.ValidationRuleActiveInd = 1
	WHERE rtvr.RequestTypeId = @RequestTypeId

	RETURN

END