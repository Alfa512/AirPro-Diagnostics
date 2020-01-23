DROP PROCEDURE IF EXISTS Scan.usp_SaveReport;
GO

CREATE PROCEDURE Scan.usp_SaveReport
	@RequestId INT
	,@RequestTypeId INT
	,@RequestCategoryId INT
	,@AirProToolId INT
	,@ReportFooterHTML NVARCHAR(MAX)
	,@ReportHeaderHTML NVARCHAR(MAX)
	,@TechnicianNotes NVARCHAR(MAX)
	,@CompleteReport BIT
	,@CancelReport BIT
	,@CancelNotes NVARCHAR(MAX)
	,@CancelReasonTypeId INT
	,@ResponsibleTechUserId UNIQUEIDENTIFIER
	,@ReportVersion VARBINARY(8)

	,@WorkTypeIds NVARCHAR(MAX)
	,@ResultIds NVARCHAR(MAX)
	,@SelectedResultIds NVARCHAR(MAX)
	,@ReviewedRuleIds NVARCHAR(MAX)

	,@Recommendations Scan.udt_ReportRecommendations READONLY
	,@Decisions Scan.udt_ReportDecisions READONLY
	,@ReportVehicleMakeTools Scan.udt_VehicleMakeTool READONLY
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'ReportSaveAudit')
		INSERT INTO Scan.ReportSaveAudit
		(
			RequestId
			,RequestTypeId
			,RequestCategoryId
			,AirProToolId
			,ReportFooterHTML
			,ReportHeaderHTML
			,TechnicianNotes
			,CompleteReport
			,CancelReport
			,CancelNotes
			,ResponsibleTechUserId
			,ReportVersion
			,WorkTypeIds
			,ResultIds
			,UserGuid
			,RecommendationJson
			,DecisionJson
		)
		VALUES
		(
			@RequestId
			,@RequestTypeId
			,@RequestCategoryId
			,@AirProToolId
			,@ReportFooterHTML
			,@ReportHeaderHTML
			,@TechnicianNotes
			,@CompleteReport
			,@CancelReport
			,@CancelNotes
			,@ResponsibleTechUserId
			,@ReportVersion
			,@WorkTypeIds
			,@ResultIds
			,@UserGuid
			,(SELECT * FROM @Recommendations FOR JSON PATH)
			,(SELECT * FROM @Decisions FOR JSON PATH)
		)
	
	BEGIN TRAN

		BEGIN TRY

			/**********************************************************
				Step 1: Load Ids.
			**********************************************************/
			DECLARE @ReportId INT
					,@RepairId INT
					,@ShopGuid UNIQUEIDENTIFIER
			SELECT TOP 1
				@ReportId = rpt.ReportId
				,@RepairId = r.OrderId
				,@ShopGuid = o.ShopGuid
			FROM Scan.Requests r
			INNER JOIN Repair.Orders o
				ON r.OrderId = o.OrderId
			LEFT JOIN Scan.Reports rpt
				ON r.ReportId = rpt.ReportId
			WHERE r.RequestId = @RequestId;

			/**********************************************************
				Step 2: Validate Report Version.
			**********************************************************/
			IF EXISTS (SELECT 1 FROM Scan.Reports WHERE ReportId = @ReportId AND ReportVersion != @ReportVersion)
				THROW 50000, 'Report Version Mismatch.', 1;

			IF EXISTS (SELECT 1 FROM Repair.Orders WHERE OrderId = @RepairId AND Status > 1)
				THROW 50000, 'Repair is NOT Active.', 1;

			/**********************************************************
				Step 3: Update Request.
			**********************************************************/
			UPDATE r
				SET r.RequestTypeId = @RequestTypeId
					,r.RequestCategoryId = NULLIF(@RequestCategoryId, 0)
					,r.ToolId = @AirProToolId
					,r.UpdatedByUserGuid = @UserGuid
					,r.UpdatedDt = GETUTCDATE()
			FROM Scan.Requests r
			WHERE r.RequestId = @RequestId
				AND (r.RequestTypeId != @RequestTypeId
					OR r.RequestCategoryId != @RequestCategoryId
					OR r.ToolId != @AirProToolId)

			/**********************************************************
				Step 4: Insert/Update Report.
			**********************************************************/
			IF (@ReportId IS NULL)
				BEGIN
					INSERT Scan.Reports
					(
						ReportFooterHTML
						,ReportNotes
						,TechnicianNotes
						,CompletedInd
						,CompletedByUserGuid
						,CompletedDt
						,CanceledInd
						,CancellationNotes
						,CancelReasonTypeId
						,ResponsibleTechnicianUserGuid
						,ResponsibleSetDt
						,CreatedByUserGuid
						,CreatedDt
					)
					VALUES
					(
						@ReportFooterHTML
						,@ReportHeaderHTML
						,@TechnicianNotes
						,@CompleteReport
						,CASE WHEN @CompleteReport = 1 THEN @UserGuid ELSE NULL END
						,CASE WHEN @CompleteReport = 1 THEN GETUTCDATE() ELSE NULL END
						,@CancelReport
						,@CancelNotes
						,@CancelReasonTypeId
						,@ResponsibleTechUserId
						,CASE WHEN @ResponsibleTechUserId IS NOT NULL THEN GETUTCDATE() ELSE NULL END
						,@UserGuid
						,GETUTCDATE()
					)
					SET @ReportId = SCOPE_IDENTITY();

					UPDATE r
						SET r.ReportId = @ReportId
					FROM Scan.Requests r
					WHERE r.RequestId = @RequestId
				END
			ELSE
				BEGIN
					UPDATE Scan.Reports
						SET ReportFooterHTML = @ReportFooterHTML
							,ReportNotes = @ReportHeaderHTML
							,TechnicianNotes = @TechnicianNotes
							,CompletedByUserGuid = 
								CASE 
									WHEN CompletedInd = 0 AND @CompleteReport = 1 THEN @UserGuid
									WHEN CompletedInd = 1 AND @CompleteReport = 0 THEN NULL
									ELSE CompletedByUserGuid
								END
							,CompletedDt =
								CASE
									WHEN CompletedInd = 0 AND @CompleteReport = 1 THEN GETUTCDATE()
									WHEN CompletedInd = 1 AND @CompleteReport = 0 THEN NULL
									ELSE CompletedDt
								END
							,CompletedInd = @CompleteReport
							,CanceledInd = @CancelReport
							,CancellationNotes = CASE WHEN @CancelReport = 0 THEN NULL ELSE @CancelNotes END
							,CancelReasonTypeId = CASE WHEN @CancelReport = 0 THEN NULL ELSE @CancelReasonTypeId END
							,ResponsibleSetDt = 
								CASE 
									WHEN NULLIF(@ResponsibleTechUserId, Common.udf_GetEmptyGuid()) IS NOT NULL AND ResponsibleTechnicianUserGuid IS NULL THEN GETUTCDATE()
									WHEN NULLIF(@ResponsibleTechUserId, Common.udf_GetEmptyGuid()) IS NULL AND ResponsibleTechnicianUserGuid IS NOT NULL THEN NULL
									ELSE ResponsibleSetDt
								END
							,ResponsibleTechnicianUserGuid = NULLIF(@ResponsibleTechUserId, Common.udf_GetEmptyGuid())
							,UpdatedByUserGuid = @UserGuid
							,UpdatedDt = GETUTCDATE()
					WHERE ReportId = @ReportId
				END

			/**********************************************************
				Step 5: Update Diagnostic Results for Report.
			**********************************************************/
			DECLARE @ReportDiagnosticResultUpdates TABLE
			(
				ReportId INT
				,DiagnosticResultId INT
			)

			MERGE Scan.ReportDiagnosticResults AS t
			USING
			(
				SELECT
					@ReportId [ReportId]
					,Val [DiagnosticResultId]
				FROM Common.udf_JsonArrayToTable(@SelectedResultIds)
			) AS s
			ON (t.ReportId = s.ReportId AND t.DiagnosticResultId = s.DiagnosticResultId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ReportId, DiagnosticResultId)
				VALUES (ReportId, DiagnosticResultId)
			WHEN NOT MATCHED BY SOURCE AND t.ReportId = @ReportId THEN
				DELETE
			OUTPUT INSERTED.*
			INTO @ReportDiagnosticResultUpdates;

			/**********************************************************
				Step 6: Generate Report Trouble Codes.
			**********************************************************/
			DECLARE @ReportTroubleCodeUpdates TABLE
			(
				OrderId INT
				,RequestId INT
				,ControllerId INT
				,TroubleCodeId INT
			)

			MERGE Scan.ReportOrderTroubleCodes AS t
			USING
			(
				SELECT
					r.OrderId
					,r.RequestId
					,rtc.ControllerId
					,rtc.TroubleCodeId
				FROM Scan.Requests r
				LEFT JOIN Scan.ReportDiagnosticResults rdr
					ON r.ReportId = rdr.ReportId
				LEFT JOIN Diagnostic.ResultTroubleCodes rtc
					ON rdr.DiagnosticResultId = rtc.ResultId
				WHERE r.OrderId = @RepairId
					AND rtc.ControllerId IS NOT NULL
					AND rtc.TroubleCodeId IS NOT NULL
				GROUP BY
					r.OrderId
					,r.RequestId
					,rtc.ControllerId
					,rtc.TroubleCodeId
			) AS s
			ON (s.OrderId = t.OrderId AND s.ControllerId = t.ControllerIdOrig AND s.TroubleCodeId = t.TroubleCodeIdOrig)
			WHEN NOT MATCHED THEN
				INSERT (OrderId, RequestId, ControllerId, ControllerIdOrig, TroubleCodeId, TroubleCodeIdOrig, CreatedByUserGuid, CreatedDt)
				VALUES (s.OrderId, s.RequestId, s.ControllerId, s.ControllerId, s.TroubleCodeId, s.TroubleCodeId, @UserGuid, GETUTCDATE())
			OUTPUT INSERTED.OrderId, INSERTED.RequestId, INSERTED.ControllerId, INSERTED.TroubleCodeId
			INTO @ReportTroubleCodeUpdates;

			/**********************************************************
				Step 7: Update Report Work Types.
			**********************************************************/
			DECLARE @WorkTypeUpdates TABLE
			(
				ReportId INT
				,WorkTypeId INT
			)

			MERGE Scan.ReportWorkTypes AS t
			USING
			(
				SELECT
					@ReportId [ReportId]
					,Val [WorkTypeId]
				FROM Common.udf_JsonArrayToTable(@WorkTypeIds)
			) AS s
			ON (t.ReportId = s.ReportId AND t.WorkTypeId = s.WorkTypeId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ReportId, WorkTypeId)
				VALUES (ReportId, WorkTypeId)
			WHEN NOT MATCHED BY SOURCE AND t.ReportId = @ReportId THEN
				DELETE
			OUTPUT INSERTED.ReportId, INSERTED.WorkTypeId
			INTO @WorkTypeUpdates;

			/**********************************************************
				Step 8: Add New Decisions.
			**********************************************************/
			DECLARE @ReportDecisions TABLE (DecisionId INT, DecisionTextSeverity INT)
			MERGE Scan.Decisions AS t
			USING
			(
				SELECT
					d.DecisionText
					,d.DecisionTextSeverity
				FROM @Decisions d
				WHERE NULLIF(d.DecisionId, 0) IS NULL
				GROUP BY
					d.DecisionText
					,d.DecisionTextSeverity
			) AS s
			ON (t.DecisionText = s.DecisionText)
			WHEN NOT MATCHED THEN
				INSERT (DecisionText, DefaultTextSeverity, ActiveInd, CreatedByUserGuid, CreatedDt)
				VALUES (DecisionText, DecisionTextSeverity, 1, @UserGuid, GETUTCDATE())
			OUTPUT INSERTED.DecisionId, INSERTED.DefaultTextSeverity
			INTO @ReportDecisions;

			INSERT INTO @ReportDecisions
			SELECT DecisionId, DecisionTextSeverity
			FROM @Decisions
			WHERE NULLIF(DecisionId, 0) IS NOT NULL

			/**********************************************************
				Step 9: Update Report Decisions.
			**********************************************************/
			DECLARE @DecisionUpdates TABLE
			(
				ReportId INT
				,DecisionId INT
			)

			MERGE Scan.ReportDecisions AS t
			USING
			(
				SELECT
					@ReportId [ReportId]
					,DecisionId
					,DecisionTextSeverity
				FROM @ReportDecisions
				GROUP BY
					DecisionId
					,DecisionTextSeverity
			) AS s
			ON (t.ReportId = s.ReportId AND t.DecisionId = s.DecisionId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ReportId, DecisionId, TextSeverity, CreatedByUserGuid, CreatedDt)
				VALUES (ReportId, DecisionId, DecisionTextSeverity, @UserGuid, GETUTCDATE())
			WHEN NOT MATCHED BY SOURCE AND t.ReportId = @ReportId THEN
				DELETE
			WHEN MATCHED THEN
				UPDATE
					SET TextSeverity = s.DecisionTextSeverity
						,UpdatedByUserGuid = @UserGuid
						,UpdatedDt = GETUTCDATE()
			OUTPUT INSERTED.ReportId, INSERTED.DecisionId
			INTO @DecisionUpdates;

			/**********************************************************
				Step 10: Update Diagnostic Results.
			**********************************************************/
			UPDATE r
				SET r.RequestId = NULL
					,r.ShopName = ISNULL(r.ShopName, @ShopGuid)
			FROM Diagnostic.Results r
			LEFT JOIN Common.udf_JsonArrayToTable(@ResultIds) u
				ON r.ResultId = u.Val
			WHERE r.RequestId = @RequestId
				AND u.Val IS NULL
			
			UPDATE r
				SET r.RequestId = @RequestId
			FROM Diagnostic.Results r
			WHERE r.ResultId IN (SELECT Val FROM Common.udf_JsonArrayToTable(@ResultIds))
				OR r.ResultId IN (SELECT Val FROM Common.udf_JsonArrayToTable(@SelectedResultIds))

			/**********************************************************
				Step 11: Add New Controllers.
			**********************************************************/
			DECLARE @NewControllers TABLE
			(
				ControllerId INT
				,ControllerName NVARCHAR(200)
			)

			MERGE Diagnostic.Controllers AS t
			USING
			(
				SELECT DISTINCT
					ControllerName
					,CHECKSUM(ControllerName) [ControllerHash]
				FROM @Recommendations r
				WHERE NULLIF(ControllerId, 0) IS NULL
			) AS s
			ON (s.ControllerHash = t.ControllerHash)
			WHEN NOT MATCHED THEN
				INSERT (ControllerName)
				VALUES (ControllerName)
			OUTPUT INSERTED.ControllerId, INSERTED.ControllerName
			INTO @NewControllers;

			/**********************************************************
				Step 12: Add New Trouble Codes.
			**********************************************************/
			DECLARE @NewTroubleCodes TABLE
			(
				TroubleCodeId INT
				,TroubleCode NVARCHAR(20)
				,TroubleCodeDescription NVARCHAR(1000)
			)

			INSERT INTO @NewTroubleCodes
			(
				TroubleCodeId
				,TroubleCode
				,TroubleCodeDescription
			)
			SELECT DISTINCT
				tc.TroubleCodeId
				,r.TroubleCode
				,r.TroubleCodeDescription
			FROM @Recommendations r
			LEFT JOIN Diagnostic.TroubleCodes tc
				ON tc.TroubleCodeHash = CHECKSUM(r.TroubleCode, r.TroubleCodeDescription)
			WHERE NULLIF(r.TroubleCodeId, 0) IS NULL
				AND tc.TroubleCodeId IS NOT NULL

			MERGE Diagnostic.TroubleCodes AS t
			USING
			(
				SELECT DISTINCT
					r.TroubleCode
					,r.TroubleCodeDescription
					,CHECKSUM(r.TroubleCode, r.TroubleCodeDescription) [TroubleCodeHash]
				FROM @Recommendations r
				LEFT JOIN @NewTroubleCodes ntc
					ON r.TroubleCode = ntc.TroubleCode
						AND r.TroubleCodeDescription = ntc.TroubleCodeDescription
				WHERE NULLIF(r.TroubleCodeId, 0) IS NULL
					AND ntc.TroubleCodeId IS NULL
			) AS s
			ON (s.TroubleCodeHash = t.TroubleCodeHash)
			WHEN NOT MATCHED THEN
				INSERT (TroubleCode, TroubleCodeDescription)
				VALUES (TroubleCode, TroubleCodeDescription)
			OUTPUT INSERTED.TroubleCodeId, INSERTED.TroubleCode, INSERTED.TroubleCodeDescription
			INTO @NewTroubleCodes;

			/**********************************************************
				Step 13: Update Report Order Trouble Codes.
			**********************************************************/
			DECLARE @NewOrderTroubleCodes TABLE
			(
				ReportOrderTroubleCodeId BIGINT
				,ControllerId INT
				,TroubleCodeId INT
			)

			MERGE Scan.ReportOrderTroubleCodes AS t
			USING 
			(
				SELECT
					r.ReportOrderTroubleCodeId
					,@RepairId [OrderId]
					,@RequestId [RequestId]
					,ISNULL(NULLIF(r.ControllerId, 0), nc.ControllerId) [ControllerId]
					,ISNULL(NULLIF(r.TroubleCodeId, 0), ntc.TroubleCodeId) [TroubleCodeId]
				FROM @Recommendations r
				LEFT JOIN @NewControllers nc
					ON r.ControllerName = nc.ControllerName
				LEFT JOIN @NewTroubleCodes ntc
					ON r.TroubleCode = ntc.TroubleCode
						AND r.TroubleCodeDescription = ntc.TroubleCodeDescription
			) AS s
			ON (s.ReportOrderTroubleCodeId = t.ReportOrderTroubleCodeId)
			WHEN NOT MATCHED THEN
				INSERT (OrderId, RequestId, ControllerId, ControllerIdOrig, TroubleCodeId, TroubleCodeIdOrig, CreatedByUserGuid, CreatedDt)
				VALUES (OrderId, RequestId, ControllerId, ControllerId, TroubleCodeId, TroubleCodeId, @UserGuid, GETUTCDATE())
			WHEN MATCHED AND s.ControllerId != t.ControllerId OR s.TroubleCodeId != t.TroubleCodeId THEN
				UPDATE
					SET ControllerId = s.ControllerId
						,TroubleCodeId = s.TroubleCodeId
						,UpdatedByUserGuid = @UserGuid
						,UpdatedDt = GETUTCDATE()
			OUTPUT INSERTED.ReportOrderTroubleCodeId, INSERTED.ControllerId, INSERTED.TroubleCodeId
			INTO @NewOrderTroubleCodes;

			/**********************************************************
				Step 14: Add New Recommendations.
			**********************************************************/
			DECLARE @NewRecommendations TABLE
			(
				TroubleCodeRecommendationId INT
				,TroubleCodeRecommendationText NVARCHAR(MAX)
			)

			MERGE Scan.TroubleCodeRecommendations AS t
			USING
			(
				SELECT
					r.TroubleCodeRecommendationText
					,CHECKSUM(r.TroubleCodeRecommendationText) [TroubleCodeRecommendationHash]
				FROM @Recommendations r
				WHERE NULLIF(r.TroubleCodeRecommendationId, 0) IS NULL
					AND NULLIF(r.TroubleCodeRecommendationText, '') IS NOT NULL
				GROUP BY r.TroubleCodeRecommendationText
					,CHECKSUM(r.TroubleCodeRecommendationText)
			) AS s
			ON (t.TroubleCodeRecommendationHash = s.TroubleCodeRecommendationHash)
			WHEN NOT MATCHED THEN
				INSERT (TroubleCodeRecommendationText, CreatedByUserGuid, CreatedDt)
				VALUES (TroubleCodeRecommendationText, @UserGuid, GETUTCDATE())
			OUTPUT
				INSERTED.TroubleCodeRecommendationId
				,INSERTED.TroubleCodeRecommendationText
			INTO @NewRecommendations;

			/**********************************************************
				Step 15: Update Report Recommendations.
			**********************************************************/
			DECLARE @RecommendationUpdates TABLE (ResultTroubleCodeId INT)
			MERGE Scan.ReportTroubleCodeRecommendations AS t
			USING
			(
				SELECT
					ISNULL(NULLIF(r.ReportOrderTroubleCodeId, 0), ntc.ReportOrderTroubleCodeId) [ReportOrderTroubleCodeId]
					,@ReportId [ReportId]
					,r.ResultTroubleCodeId
					,r.InformCustomerInd
					,r.AccidentRelatedInd
					,r.ExcludeFromReportInd
					,r.CodeClearedInd
					,r.TroubleCodeNoteText
					,ISNULL(nr.TroubleCodeRecommendationId, r.TroubleCodeRecommendationId) [TroubleCodeRecommendationId]
					,r.RecommendationTextSeverity
				FROM @Recommendations r
				LEFT JOIN @NewOrderTroubleCodes ntc
					ON r.ControllerId = ntc.ControllerId
						AND r.TroubleCodeId = ntc.TroubleCodeId
				LEFT JOIN @NewRecommendations nr
					ON r.TroubleCodeRecommendationText = nr.TroubleCodeRecommendationText
				WHERE ISNULL(NULLIF(r.ReportOrderTroubleCodeId, 0), ntc.ReportOrderTroubleCodeId) IS NOT NULL
			) AS s
			ON (t.ReportOrderTroubleCodeId = s.ReportOrderTroubleCodeId AND t.ReportId = s.ReportId)
			WHEN NOT MATCHED THEN
				INSERT (ReportOrderTroubleCodeId, ReportId, ResultTroubleCodeId, InformCustomerInd, AccidentRelatedInd, ExcludeFromReportInd, CodeClearedInd, TroubleCodeNoteText, TroubleCodeRecommendationId, TextSeverity, CreatedByUserGuid, CreatedDt)
				VALUES (ReportOrderTroubleCodeId, ReportId, ResultTroubleCodeId, InformCustomerInd, AccidentRelatedInd, ExcludeFromReportInd, CodeClearedInd, TroubleCodeNoteText, TroubleCodeRecommendationId, RecommendationTextSeverity, @UserGuid, GETUTCDATE())
			WHEN MATCHED THEN
				UPDATE SET
					t.ResultTroubleCodeId = s.ResultTroubleCodeId
					,t.InformCustomerInd = s.InformCustomerInd
					,t.AccidentRelatedInd = s.AccidentRelatedInd
					,t.ExcludeFromReportInd = s.ExcludeFromReportInd
					,t.CodeClearedInd = s.CodeClearedInd
					,t.TroubleCodeNoteText = s.TroubleCodeNoteText
					,t.TroubleCodeRecommendationId = s.TroubleCodeRecommendationId
					,t.TextSeverity = s.RecommendationTextSeverity
					,t.UpdatedByUserGuid = @UserGuid
					,t.UpdatedDt = GETUTCDATE()
			OUTPUT INSERTED.ResultTroubleCodeId
			INTO @RecommendationUpdates;

			/**********************************************************
				Step 15: Add/Update/Delete Report Vehicle Make Tool.
			**********************************************************/
			MERGE Scan.ReportVehicleMakeTools AS t
			USING
			(
				SELECT
					@ReportId [ReportId]
					,r.VehicleMakeToolId
					,r.ToolVersion
				FROM @ReportVehicleMakeTools r
			) AS s
			ON (s.VehicleMakeToolId = t.VehicleMakeToolId AND t.ReportId = s.ReportId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ReportId, VehicleMakeToolId, ToolVersion)
				VALUES (ReportId, VehicleMakeToolId, ToolVersion)
			WHEN MATCHED THEN
				UPDATE SET t.ToolVersion = s.ToolVersion
			WHEN NOT MATCHED BY SOURCE AND t.ReportId = @ReportId THEN
				DELETE;

			/**********************************************************
				Step 16: Run Report Validations.
			**********************************************************/
			MERGE Scan.ReportValidationRules AS t
			USING Scan.udf_GetValidationRulesByRequestId(@RequestId) AS s
			ON (t.ReportId = @ReportId AND t.ValidationRuleId = s.ValidationRuleId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ReportId, ValidationRuleId, ValidationRuleResultInd)
				VALUES (@ReportId, s.ValidationRuleId, s.ValidationRuleResultInd)
			WHEN MATCHED THEN
				UPDATE
					SET ValidationRuleResultInd = s.ValidationRuleResultInd
						,ResultAcknowledgedInd = CASE WHEN s.ValidationRuleResultInd = 1 THEN 0 ELSE ResultAcknowledgedInd END
						,ResultAcknowledgedByUserGuid = CASE WHEN s.ValidationRuleResultInd = 1 THEN NULL ELSE ResultAcknowledgedByUserGuid END
			WHEN NOT MATCHED BY SOURCE AND t.ReportId = @ReportId THEN
				DELETE;

			/**********************************************************
				Step 17: Update Validation Acknowledgements.
			**********************************************************/
			MERGE Scan.ReportValidationRules AS t
			USING Common.udf_JsonArrayToTable(@ReviewedRuleIds) AS s
			ON (t.ReportId = @ReportId AND t.ValidationRuleId = s.Val)
			WHEN MATCHED THEN
				UPDATE
					SET t.ResultAcknowledgedInd = CASE WHEN t.ValidationRuleResultInd = 0 THEN 1 ELSE 0 END
						,t.ResultAcknowledgedByUserGuid = CASE WHEN t.ValidationRuleResultInd = 0 THEN @UserGuid ELSE NULL END
			WHEN NOT MATCHED BY SOURCE AND t.ReportId = @ReportId THEN
				UPDATE
					SET t.ResultAcknowledgedInd = 0
						,t.ResultAcknowledgedByUserGuid = NULL;

			/**********************************************************
				Done: Return Request Id.
			**********************************************************/
			SELECT @RequestId [RequestId]

		END TRY
		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END
GO