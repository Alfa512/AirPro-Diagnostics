
CREATE PROCEDURE [Scan].[usp_GetReportByRequestId]
	@UserGuid UNIQUEIDENTIFIER
	,@RequestId INT
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @StartTime DATETIMEOFFSET;
	DECLARE @UserTimeZone NVARCHAR(100) = Common.udf_GetUserTimeZoneId(@UserGuid);

	/************************************************************
		Step 1: Load Ids for Lookups.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	DECLARE
		@RequestTypeId INT
		,@RequestCategoryId INT
		,@ReportId INT
		,@ShopGuid UNIQUEIDENTIFIER
		,@VehicleMakeId INT
		,@VehicleVIN VARCHAR(20)
		,@RepairId INT

	SELECT
		@RequestTypeId = r.RequestTypeId
		,@RequestCategoryId = r.RequestCategoryId
		,@VehicleVIN = o.VehicleVIN
		,@VehicleMakeId = v.VehicleMakeId
		,@ReportId = r.ReportId
		,@ShopGuid = o.ShopGuid
		,@RepairId = o.OrderId
	FROM Scan.Requests r
	INNER JOIN Repair.Orders o
		INNER JOIN Repair.Vehicles v
			ON o.VehicleVIN = v.VehicleVIN
		ON r.OrderId = o.OrderId
	LEFT JOIN Scan.Reports rpt
		ON r.ReportId = rpt.ReportId
	WHERE r.RequestId = @RequestId
	PRINT 'Step 1: Load Ids for Lookups. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 2: Load Request/Report Data.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		rq.RequestId [RequestId]
		,rq.RequestTypeId
		,rq.RequestCategoryId

		,rq.ToolId [AirProToolId]
		,t.ToolName [AirProToolName]
		,t.ToolPassword [AirProToolPassword]

		,rpt.ReportNotes [ReportHeaderHTML]
		,rpt.ReportFooterHTML
		,rpt.TechnicianNotes [TechnicianNotes]

		,rpt.CompletedInd [CompleteReport]

		,rpt.CanceledInd [CancelReport]
		,rpt.CancellationNotes [CancelNotes]
		,rpt.CancelReasonTypeId [CancelReasonTypeId]

		,rpt.ResponsibleTechnicianUserGuid [ResponsibleTechUserId]
		,rt.FirstName + ' ' + rt.LastName [ResponsibleTech]
		,CAST(rpt.ResponsibleSetDt AT TIME ZONE @UserTimeZone AS DATETIME) [ResponsibleDt]

		,cu.FirstName + ' ' + cu.LastName [CreatedBy]
		,CAST(rpt.CreatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [CreatedDt]

		,uu.FirstName + ' ' + uu.LastName [UpdatedBy]
		,CAST(rpt.UpdatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [UpdatedDt]

		,CAST(CASE WHEN rpt.ResponsibleTechnicianUserGuid = @UserGuid AND o.Status = 1 THEN 1 ELSE 0 END AS BIT) [AllowEdit]
		,CAST(CASE WHEN o.Status > 1 THEN 1 ELSE 0 END AS BIT) [RepairComplete]

		,rpt.ReportVersion [ReportVersion]
		
		,vm.ProgramInstructions [VehicleInstructions]
		,CASE WHEN vm.VehicleMakeName IS NULL THEN 'False' ELSE 'True' END [OEMCertifiedInd]
	FROM Scan.Requests rq
	INNER JOIN Repair.Orders o
		ON rq.OrderId = o.OrderId
	LEFT JOIN Scan.Reports rpt
		LEFT JOIN Access.Users cu
			ON rpt.CreatedByUserGuid = cu.UserGuid
		LEFT JOIN Access.Users uu
			ON rpt.UpdatedByUserGuid = uu.UserGuid
		LEFT JOIN Access.Users rt
			ON rpt.ResponsibleTechnicianUserGuid = rt.UserGuid
		ON rq.ReportId = rpt.ReportId
	LEFT JOIN Inventory.AirProTools t
		ON rq.ToolId = t.ToolId
	LEFT JOIN Repair.VehicleMakes vm
		INNER JOIN Access.ShopVehicleMakes svm
			ON svm.VehicleMakeId = vm.VehicleMakeId AND svm.ShopId = @ShopGuid
		ON vm.VehicleMakeId = @VehicleMakeId
	WHERE rq.RequestId = @RequestId
	PRINT 'Step 2: Load Request/Report Data. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 3: Load Request Type Selections.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		ISNULL(rc.RequestCategoryId, 0) [RequestCategoryId]
		,ISNULL(rc.RequestCategoryName, 'Other') [RequestCategoryName]
		,rt.RequestTypeId
		,rt.TypeName [RequestTypeName]
	FROM Scan.RequestTypes rt
	LEFT JOIN Scan.RequestCategoryTypes rct
		ON rt.RequestTypeId = rct.RequestTypeId
	LEFT JOIN Scan.RequestCategories rc
		ON rct.RequestCategoryId = rc.RequestCategoryId
	LEFT JOIN Access.ShopRequestTypes srt
		ON rt.RequestTypeId = srt.RequestTypeId
			AND srt.ShopGuid = @ShopGuid
	WHERE (rt.ActiveFlag = 1 AND srt.RequestTypeId IS NOT NULL)
		OR rt.RequestTypeId = @RequestTypeId
	ORDER BY
		ISNULL(rc.[Order], 3)
		,rt.SortOrder
	PRINT 'Step 3: Load Request Type Selections. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 4: Load AirPro Tool Selections.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		apt.ToolId [AirProToolId]
		,apt.ToolName [AirProToolName]
		,apt.ToolPassword [AirProToolPassword]
	FROM Inventory.AirProTools apt
	INNER JOIN
	(
		SELECT apta.ToolId
		FROM Inventory.AirProToolAccounts apta
		INNER JOIN Access.Shops s
			ON apta.AccountGuid = s.AccountGuid
		WHERE s.ShopGuid = @ShopGuid

		UNION

		SELECT apts.ToolId
		FROM Inventory.AirProToolShops apts
		WHERE apts.ShopGuid = @ShopGuid
	) m ON apt.ToolId = m.ToolId
	ORDER BY apt.ToolId
	PRINT 'Step 4: Load AirPro Tool Selections. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 5: Load Responsibility History.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		CASE
			WHEN ResponsibleTechnicianUserGuid = Common.udf_GetEmptyGuid() THEN 'Unassigned'
			ELSE u.LastName + ', ' + u.FirstName
		END [ResponsibleTech]
		,CAST(ResponsibleStartDt AT TIME ZONE @UserTimeZone AS DATETIME) [ResponsibleStartDt]
		,CAST(LAG(ResponsibleStartDt, 1, NULL) OVER (ORDER BY ResponsibleStartDt DESC) AT TIME ZONE @UserTimeZone AS DATETIME) [ResponsibleEndDt]
	FROM
	(
		SELECT
			ISNULL(ResponsibleTechnicianUserGuid, Common.udf_GetEmptyGuid()) [ResponsibleTechnicianUserGuid]
			,ISNULL(ResponsibleSetDt, UpdatedDt) [ResponsibleStartDt]
		FROM Scan.Reports
		WHERE ReportId = @ReportId

		UNION

		SELECT
			ISNULL(ResponsibleTechnicianUserGuid, Common.udf_GetEmptyGuid()) [ResponsibleTechnicianUserGuid]
			,ISNULL(ResponsibleSetDt, UpdatedDt) [ResponsibleStartDt]
		FROM Scan.ReportsArchive
		WHERE ReportId = @ReportId
	) l
	LEFT JOIN Access.Users u
		ON l.ResponsibleTechnicianUserGuid = u.UserGuid
	ORDER BY ResponsibleStartDt DESC
	PRINT 'Step 5: Load Responsibility History. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 6: Internal Note History.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		r.RequestId
		,rpt.TechnicianNotes [TechnicianNotes]
		,u.LastName + ', ' + u.FirstName [UpdatedByUserDisplay]
		,rpt.UpdatedByUserGuid
		,CAST(rpt.UpdatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [UpdatedDt]
	FROM Scan.Requests r
	INNER JOIN Scan.Reports rpt
		ON r.ReportId = rpt.ReportId
	INNER JOIN Access.Users u
		ON rpt.UpdatedByUserGuid = u.UserGuid	
	WHERE r.OrderId = @RepairId
	ORDER BY r.RequestId DESC
	PRINT 'Step 6: Internal Note History. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 7: Load Work Types.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		wtg.WorkTypeGroupName
		,wt.WorkTypeId
		,wt.WorkTypeName
		,CAST(CASE WHEN rwt.WorkTypeId IS NULL THEN 0 ELSE 1 END AS BIT) [WorkTypeSelected]
	FROM Scan.WorkTypes wt
	INNER JOIN Scan.WorkTypeGroups wtg
		ON wt.WorkTypeGroupId = wtg.WorkTypeGroupId
	LEFT JOIN Scan.ReportWorkTypes rwt
		ON wt.WorkTypeId = rwt.WorkTypeId
			AND rwt.ReportId = @ReportId
	WHERE wt.WorkTypeActiveInd = 1
		AND wtg.WorkTypeGroupActiveInd = 1
		AND wt.WorkTypeId IN
			(
				SELECT WorkTypeId
				FROM Scan.ReportWorkTypes rwt
				WHERE rwt.ReportId = @ReportId

				UNION

				SELECT wtrt.WorkTypeId
				FROM Scan.WorkTypeRequestTypes wtrt
				INNER JOIN Scan.WorkTypeVehicleMakes wtvm
					ON wtrt.WorkTypeId = wtvm.WorkTypeId
				WHERE wtrt.RequestTypeId = @RequestTypeId
					AND wtvm.VehicleMakeId = @VehicleMakeId
			)
	ORDER BY
		wtg.WorkTypeGroupSortOrder
		,wt.WorkTypeSortOrder
	PRINT 'Step 7: Load Work Types. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 8: Load Decisions.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		d.DecisionId
		,d.DecisionText
		,CAST(CASE
			WHEN @ReportId IS NULL THEN ISNULL(drc.PreSelectedInd, 0) | ISNULL(drt.PreSelectedInd, 0) | ISNULL(dvm.PreSelectedInd, 0)
			WHEN rd.ReportDecisionId IS NOT NULL THEN 1
			ELSE 0
		 END AS BIT) [DecisionSelected]
		 ,ISNULL(rd.TextSeverity, d.DefaultTextSeverity) [DecisionTextSeverity]
	FROM Scan.Decisions d
	LEFT JOIN Scan.DecisionRequestCategories drc
		ON d.DecisionId = drc.DecisionId
			AND drc.RequestCategoryId = @RequestCategoryId
	LEFT JOIN Scan.DecisionRequestTypes drt
		ON d.DecisionId = drt.DecisionId
			AND drt.RequestTypeId = @RequestTypeId
	LEFT JOIN Scan.DecisionVehicleMakes dvm
		ON d.DecisionId = dvm.DecisionId
			AND dvm.VehicleMakeId = @VehicleMakeId
	LEFT JOIN Scan.ReportDecisions rd
		ON d.DecisionId = rd.DecisionId
			AND rd.ReportId = @ReportId
	WHERE d.ActiveInd = 1
		AND (rd.ReportDecisionId IS NOT NULL
			OR (drc.DecisionRequestCategoryId IS NOT NULL
				AND drt.DecisionRequestTypeId IS NOT NULL
				AND dvm.DecisionVehicleMakeId IS NOT NULL))
	ORDER BY
		ISNULL(rd.TextSeverity, d.DefaultTextSeverity) DESC
		,d.DecisionText
	PRINT 'Step 8: Load Decisions. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 9: Load Diagnostic Uploads.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	UPDATE r
		SET r.RequestId = @RequestId
	FROM Diagnostic.Results r
	WHERE r.ResultId IN (SELECT DiagnosticResultId FROM Scan.ReportDiagnosticResults WHERE ReportId = @ReportId)
		AND r.RequestId != @RequestId

	SELECT
		dr.ResultId
		,dr.VehicleVin
		,dr.VehicleMake
		,dr.VehicleModel
		,dr.VehicleYear

		,ISNULL(c.ControllerCount, 0) [ControllerCount]
		,ISNULL(c.TroubleCodeCount, 0) [TroubleCodeCount]
	
		,CAST(dr.ScanDateTime AT TIME ZONE @UserTimeZone AS DATETIME) [ScanPerformedDt]
		,CAST(dr.CreatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [ScanUploadedDt]

		,dr.VinMatchInd
		,dr.AssignedToRequestInd
		,dr.SelectedForReportInd
	FROM
	(
		SELECT
			r.ResultId
			,r.VehicleVin
			,r.VehicleMake
			,r.VehicleModel
			,r.VehicleYear
			,r.ScanDateTime
			,r.CreatedDt
			,CAST(CASE WHEN r.VehicleVin = @VehicleVIN THEN 1 ELSE 0 END AS BIT) [VinMatchInd]
			,CAST(CASE WHEN r.RequestId = @RequestId THEN 1 ELSE 0 END AS BIT) [AssignedToRequestInd]
			,CAST(CASE WHEN rdr.DiagnosticResultId IS NOT NULL THEN 1 ELSE 0 END AS BIT) [SelectedForReportInd]
		FROM Diagnostic.Results r
		LEFT JOIN Scan.ReportDiagnosticResults rdr
			ON r.ResultId = rdr.DiagnosticResultId
		WHERE r.RequestId = @RequestId
			OR rdr.ReportId = @ReportId

		UNION

		SELECT
			uq.ResultId
			,uq.VehicleVin
			,uq.VehicleMake
			,uq.VehicleModel
			,uq.VehicleYear
			,uq.ScanDateTime
			,uq.CreatedDt
			,CAST(CASE WHEN uq.VehicleVin = @VehicleVIN THEN 1 ELSE 0 END AS BIT) [VinMatchInd]
			,0 [AssignedToRequestInd]
			,0 [SelectedForReportInd]
		FROM Diagnostic.vwUploadQueue uq
		WHERE uq.SearchGuid = @ShopGuid
			OR uq.SearchGuid IN
			(
				SELECT apt.ToolKey
				FROM Inventory.AirProTools apt
				WHERE apt.ToolId IN
				(
					SELECT apta.ToolId
					FROM Inventory.AirProToolAccounts apta
					INNER JOIN Access.Shops s
						ON apta.AccountGuid = s.AccountGuid
					WHERE s.ShopGuid = @ShopGuid

					UNION

					SELECT apts.ToolId
					FROM Inventory.AirProToolShops apts
					WHERE apts.ShopGuid = @ShopGuid
				)
			)
	) dr
	OUTER APPLY
	(
		SELECT
			COUNT(DISTINCT ControllerId) [ControllerCount]
			,SUM(CASE WHEN tc.TroubleCode IS NOT NULL THEN 1 ELSE 0 END) [TroubleCodeCount]
		FROM Diagnostic.ResultTroubleCodes rtc
		LEFT JOIN Diagnostic.TroubleCodes tc
			ON rtc.TroubleCodeId = tc.TroubleCodeId
		WHERE ResultId = dr.ResultId
	) c
	ORDER BY dr.ResultId DESC
	PRINT 'Step 9: Load Diagnostic Uploads. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 10: Load Frequent Recommendations.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	WITH TroubleCodeRecommendationRanking
	AS
	(
		SELECT
			rotc.ControllerId
			,rotc.TroubleCodeId
			,v.VehicleMakeId
			,rtcr.TroubleCodeRecommendationId
			,DENSE_RANK() OVER (PARTITION BY rotc.ControllerId, rotc.TroubleCodeId, v.VehicleMakeId ORDER BY COUNT(1) DESC) [TroubleCodeRecommendationRank]
		FROM Scan.ReportOrderTroubleCodes rotc
		INNER JOIN Repair.Orders o
			INNER JOIN Repair.Vehicles v
				ON o.VehicleVIN = v.VehicleVIN
					AND v.VehicleMakeId = @VehicleMakeId
			ON rotc.OrderId = o.OrderId
		INNER JOIN Scan.ReportTroubleCodeRecommendations rtcr
			ON rotc.ReportOrderTroubleCodeId = rtcr.ReportOrderTroubleCodeId
		WHERE rtcr.TroubleCodeRecommendationId IS NOT NULL
		GROUP BY
			rotc.ControllerId
			,rotc.TroubleCodeId
			,v.VehicleMakeId
			,rtcr.TroubleCodeRecommendationId
	)

	SELECT
		tcrr.ControllerId
		,tcrr.TroubleCodeId
		,tcrr.TroubleCodeRecommendationId
		,tcr.TroubleCodeRecommendationText
		,tcrr.TroubleCodeRecommendationRank
	FROM Scan.ReportOrderTroubleCodes rotc
	INNER JOIN TroubleCodeRecommendationRanking tcrr
		ON rotc.ControllerId = tcrr.ControllerId
			AND rotc.TroubleCodeId = tcrr.TroubleCodeId
			AND tcrr.TroubleCodeRecommendationRank <= 5
	INNER JOIN Scan.TroubleCodeRecommendations tcr
		ON tcrr.TroubleCodeRecommendationId = tcr.TroubleCodeRecommendationId
			AND tcr.ActiveInd = 1
	WHERE rotc.OrderId = @RepairId
	ORDER BY 
		tcrr.ControllerId
		,tcrr.TroubleCodeId
		,tcrr.VehicleMakeId
		,tcrr.TroubleCodeRecommendationRank
	PRINT 'Step 10: Load Frequent Recommendations. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

	/************************************************************
		Step 11: Load Report Trouble Codes.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		rotc.ReportOrderTroubleCodeId
		,rotc.ControllerId
		,c.ControllerName
		,rotc.ControllerIdOrig
		,co.ControllerName [ControllerNameOrig]
		,rotc.TroubleCodeId
		,tc.TroubleCode
		,tc.TroubleCodeDescription
		,rotc.TroubleCodeIdOrig
		,tco.TroubleCode [TroubleCodeOrig]
		,tco.TroubleCodeDescription [TroubleCodeDescriptionOrig]
		,rotc.CreatedByUserGuid [OverrideCreatedByUserGuid]
		,ocu.DisplayName [OverrideCreatedByUserDisplay]
		,CAST(rotc.CreatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [OverrideCreatedDt]
		,rotc.UpdatedByUserGuid [OverrideUpdatedByUserGuid]
		,ouu.DisplayName [OverrideUpdatedByUserDisplay]
		,CAST(rotc.UpdatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [OverrideUpdatedDt]

		,r.RequestId
		,r.RequestTypeId
		,r.RequestCategoryId
		,CAST(CASE WHEN r.RequestId = @RequestId THEN 1 ELSE 0 END AS BIT) [CurrentRequestInd]
		,DENSE_RANK() OVER (PARTITION BY r.OrderId ORDER BY r.RequestId) [RequestVersion]
		,rpt.CanceledInd [RequestCanceleInd]
		,rpt.CancellationNotes [RequestCancelNotes]
		,rpt.ReportId
		
		,rtc.DiagnosticResultId
		,rtc.ResultTroubleCodeId
		,rtc.TroubleCodeInformation

		,rtcr.InformCustomerInd
		,rtcr.AccidentRelatedInd
		,rtcr.ExcludeFromReportInd
		,rtcr.CodeClearedInd
		,rtcr.TroubleCodeNoteText
		,rtcr.TroubleCodeRecommendationId
		,tcr.TroubleCodeRecommendationText
		,rtcr.TextSeverity [RecommendationTextSeverity]

		,rtcr.CreatedByUserGuid [RecommendationCreatedByUserGuid]
		,rcu.DisplayName [RecommendationCreatedByUserDisplay]
		,CAST(rtcr.CreatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [RecommendationCreatedDt]
		,rtcr.UpdatedByUserGuid [RecommendationUpdatedByUserGuid]
		,ruu.DisplayName [RecommendationUpdatedByUserDisplay]
		,CAST(rtcr.UpdatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [RecommendationUpdatedDt]
	FROM Scan.ReportOrderTroubleCodes rotc
	INNER JOIN Scan.Requests r
		ON rotc.OrderId = r.OrderId
	INNER JOIN Diagnostic.TroubleCodes tc
		ON rotc.TroubleCodeId = tc.TroubleCodeId
	INNER JOIN Diagnostic.TroubleCodes tco	
		ON rotc.TroubleCodeIdOrig = tco.TroubleCodeId
	INNER JOIN Diagnostic.Controllers c
		ON rotc.ControllerId = c.ControllerId
	INNER JOIN Diagnostic.Controllers co
		ON rotc.ControllerIdOrig = co.ControllerId
	LEFT JOIN Access.Users ocu
		ON rotc.CreatedByUserGuid = ocu.UserGuid
	LEFT JOIN Access.Users ouu
		ON rotc.UpdatedByUserGuid = ouu.UserGuid
	LEFT JOIN Scan.Reports rpt
		ON r.ReportId = rpt.ReportId
	LEFT JOIN
	(
		SELECT
			MIN(rtc.ResultId) [DiagnosticResultId]
			,MIN(rtc.ResultTroubleCodeId) [ResultTroubleCodeId]
			,rtc.ControllerId
			,rtc.TroubleCodeId
			,rtc.TroubleCodeInformation
		FROM Scan.ReportDiagnosticResults rdr
		INNER JOIN Diagnostic.ResultTroubleCodes rtc
			ON rdr.DiagnosticResultId = rtc.ResultId
		WHERE ReportId = @ReportId
		GROUP BY
			rtc.ControllerId
			,rtc.TroubleCodeId
			,rtc.TroubleCodeInformation
	) rtc
		ON rotc.ControllerIdOrig = rtc.ControllerId
			AND rotc.TroubleCodeIdOrig = rtc.TroubleCodeId
	LEFT JOIN Scan.ReportTroubleCodeRecommendations rtcr
		LEFT JOIN Scan.TroubleCodeRecommendations tcr
			ON rtcr.TroubleCodeRecommendationId = tcr.TroubleCodeRecommendationId
		LEFT JOIN Access.Users rcu
			ON rtcr.CreatedByUserGuid = rcu.UserGuid
		LEFT JOIN Access.Users ruu
			ON rtcr.UpdatedByUserGuid = ruu.UserGuid
		ON rotc.ReportOrderTroubleCodeId = rtcr.ReportOrderTroubleCodeId
			AND r.ReportId = rtcr.ReportId
	WHERE rotc.OrderId = @RepairId
		AND ISNULL(rotc.RequestId, @RequestId) <= @RequestId
	ORDER BY
		c.ControllerName
		,tc.TroubleCode
		,tc.TroubleCodeDescription
		,r.RequestId
	PRINT 'Step 11: Load Report Trouble Codes. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

	/************************************************************
		Step 12: Load Possible Missing Controllers.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		vc.ControllerId
		,c.ControllerName
	FROM Repair.Vehicles v
	INNER JOIN Diagnostic.VehicleControllers vc
		ON v.VehicleMakeId = vc.VehicleMakeId
			AND v.Model = vc.VehicleModelName
			AND v.Year = vc.VehicleYear
	INNER JOIN Diagnostic.Controllers c
		ON vc.ControllerId = c.ControllerId
	WHERE v.VehicleVIN = @VehicleVIN
		AND vc.ControllerId NOT IN (SELECT ControllerId FROM Scan.ReportOrderTroubleCodes WHERE OrderId = @RepairId)
	GROUP BY
		vc.ControllerId
		,c.ControllerName
	ORDER BY
		c.ControllerName
	PRINT 'Step 12: Load Possible Missing Controllers. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

	/************************************************************
		Step 13: Cancel Reason Types.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		crt.CancelReasonTypeId
		,crt.Name
	FROM Scan.CancelReasonTypes crt
	ORDER BY crt.[Order] ASC
	PRINT 'Step 13: Cancel Reson Types. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 14: Load Vehicle Make Tools.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		vmt.Name
		,vmt.VehicleMakeToolId
		,CASE WHEN rvmt.VehicleMakeToolId IS NULL THEN 'False' ELSE 'True' END [CheckedInd]
		,rvmt.ToolVersion
	FROM Repair.VehicleMakeTools vmt
	LEFT JOIN Scan.ReportVehicleMakeTools rvmt
		ON vmt.VehicleMakeToolId = rvmt.VehicleMakeToolId
			AND rvmt.ReportId = @ReportId
	WHERE vmt.VehicleMakeId = @VehicleMakeId 
	PRINT 'Step 14: Load Vehicle Make Tools. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

	/************************************************************
		Step 15: Load Validation Rules.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	SELECT
		vr.ValidationRuleId
		,vr.ValidationRuleText
		,vr.ValidationRuleDetails
		,vr.ValidationRuleSortOrder
		,rvr.ValidationRuleResultInd
		,CAST(ISNULL(rvr.ResultAcknowledgedInd, 0) AS BIT) [ResultAcknowledgedInd]
		,rvr.ResultAcknowledgedByUserGuid [ResultAcknowledgedByUserGuid]
		,au.DisplayName [ResultAcknowledgedByUserDisplay]
	FROM Scan.ReportValidationRules rvr
	INNER JOIN Scan.ValidationRules vr
		ON rvr.ValidationRuleId = vr.ValidationRuleId
			AND vr.ValidationRuleActiveInd = 1
	LEFT JOIN Access.Users au
		ON rvr.ResultAcknowledgedByUserGuid = au.UserGuid
	WHERE rvr.ReportId = @ReportId
	ORDER BY vr.ValidationRuleSortOrder
	PRINT 'Step 15: Load Validation Rules. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

END