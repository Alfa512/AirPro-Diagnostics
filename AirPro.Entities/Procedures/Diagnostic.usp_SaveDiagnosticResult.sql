
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Diagnostic' AND ROUTINE_NAME = 'usp_SaveDiagnosticResult')
	DROP PROCEDURE Diagnostic.usp_SaveDiagnosticResult
GO

CREATE PROCEDURE Diagnostic.usp_SaveDiagnosticResult
	@UserGuid UNIQUEIDENTIFIER,
	@DiagnosticTool INT,
	@DiagnosticFileType INT,
	@DiagnosticFileText NVARCHAR(MAX),
	@ResultId INT = NULL,
	@RequestId INT = NULL,
	@CustomerFirstName NVARCHAR(50) = NULL,
	@CustomerLastName NVARCHAR(50) = NULL,
	@CustomerRo NVARCHAR(50) = NULL,
	@ScanDateTime DATETIME = NULL,
	@ShopName NVARCHAR(150) = NULL,
	@ShopAddress NVARCHAR(150) = NULL,
	@ShopEmail NVARCHAR(150) = NULL,
	@ShopFax NVARCHAR(50) = NULL,
	@ShopPhone NVARCHAR(50) = NULL,
	@VehicleVin NVARCHAR(50) = NULL,
	@VehicleMake NVARCHAR(50) = NULL,
	@VehicleModel NVARCHAR(100) = NULL,
	@VehicleYear NVARCHAR(10) = NULL,
	@TestabilityIssues NVARCHAR(MAX) = NULL,
	@DeletedInd BIT = 0,
	@TroubleCodes Diagnostic.udt_ResultTroubleCodes READONLY,
	@FreezeFrames Diagnostic.udt_ResultFreezeFrames READONLY
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			DECLARE @NewResult AS TABLE (ResultId INT)

			MERGE Diagnostic.Results AS t
			USING
			(
				SELECT
					@ResultId [ResultId]
					,ISNULL(NULLIF(@DiagnosticTool, 0), 1) [DiagnosticToolId]
					,@RequestId [RequestId]
					,@ScanDateTime [ScanDateTime]
					,@CustomerFirstName [CustomerFirstName]
					,@CustomerLastName [CustomerLastName]
					,@CustomerRo [CustomerRo]
					,@ShopName [ShopName]
					,@ShopAddress [ShopAddress]
					,@ShopEmail [ShopEmail]
					,@ShopFax [ShopFax]
					,@ShopPhone [ShopPhone]
					,@VehicleVin [VehicleVin]
					,@VehicleMake [VehicleMake]
					,@VehicleModel [VehicleModel]
					,@VehicleYear [VehicleYear]
					,@TestabilityIssues [TestabilityIssues]
					,@DeletedInd [DeletedInd]
			) AS s
			ON (t.ResultId = s.ResultId)
			WHEN NOT MATCHED THEN
				INSERT
				(
					DiagnosticToolId
					,RequestId
					,ScanDateTime
					,CustomerFirstName
					,CustomerLastName
					,CustomerRo
					,ShopName
					,ShopAddress
					,ShopEmail
					,ShopFax
					,ShopPhone
					,VehicleVin
					,VehicleMake
					,VehicleModel
					,VehicleYear
					,TestabilityIssues
					,DeletedInd
					,CreatedByUserGuid
					,CreatedDt
				)
				VALUES
				(
					DiagnosticToolId
					,RequestId
					,ScanDateTime
					,CustomerFirstName
					,CustomerLastName
					,CustomerRo
					,ShopName
					,ShopAddress
					,ShopEmail
					,ShopFax
					,ShopPhone
					,VehicleVin
					,VehicleMake
					,VehicleModel
					,VehicleYear
					,TestabilityIssues
					,DeletedInd
					,@UserGuid
					,GETUTCDATE()
				)
			WHEN MATCHED THEN
				UPDATE
					SET t.DiagnosticToolId = s.DiagnosticToolId
						,t.RequestId = s.RequestId
						,t.ScanDateTime = s.ScanDateTime
						,t.CustomerFirstName = s.CustomerFirstName
						,t.CustomerLastName = s.CustomerLastName
						,t.CustomerRo = s.CustomerRo
						,t.ShopName = s.ShopName
						,t.ShopAddress = s.ShopAddress
						,t.ShopEmail = s.ShopEmail
						,t.ShopFax = s.ShopFax
						,t.ShopPhone = s.ShopPhone
						,t.VehicleVin = s.VehicleVin
						,t.VehicleMake = s.VehicleMake
						,t.VehicleModel = s.VehicleModel
						,t.VehicleYear = s.VehicleYear
						,t.TestabilityIssues = s.TestabilityIssues
						,t.DeletedInd = s.DeletedInd
						,t.UpdatedByUserGuid = @UserGuid
						,t.UpdatedDt = GETUTCDATE()
			OUTPUT INSERTED.ResultId
			INTO @NewResult;

			SELECT TOP 1 @ResultId = ISNULL(ResultId, @ResultId) FROM @NewResult;

			DECLARE @NewUploads TABLE (UploadId INT)
			MERGE Diagnostic.Uploads AS t
			USING
			(
				SELECT
					@ResultId [ResultId]
					,ISNULL(NULLIF(@DiagnosticFileType, 0), 1) [UploadFileTypeId]
					,@DiagnosticFileText [UploadText]
			) AS s
			ON (t.ResultId = s.ResultId AND t.UploadFileTypeId = s.UploadFileTypeId)
			WHEN NOT MATCHED THEN
				INSERT
				(
					ResultId
					,UploadFileTypeId
					,UploadText
				)
				VALUES
				(
					ResultId
					,UploadFileTypeId
					,UploadText
				)
			WHEN MATCHED THEN
				UPDATE SET t.UploadText = s.UploadText
			OUTPUT INSERTED.UploadId
			INTO @NewUploads;

			DECLARE @NewControllers TABLE (ControllerId INT)
			MERGE Diagnostic.Controllers AS t
			USING
			(
				SELECT
					ControllerName
					,CHECKSUM(ControllerName) [ControllerHash]
				FROM
				(
					SELECT ControllerId, ControllerName
					FROM @TroubleCodes t
					UNION
					SELECT ControllerId, ControllerName
					FROM @FreezeFrames ff
					WHERE ControllerId IS NULL
				) c
				GROUP BY
					ControllerName
					,CHECKSUM(ControllerName)
			) AS s
			ON (t.ControllerHash = s.ControllerHash)
			WHEN NOT MATCHED THEN
				INSERT (ControllerName)
				VALUES (ControllerName)
			OUTPUT INSERTED.ControllerId
			INTO @NewControllers;

			DECLARE @NewTroubleCodes TABLE (TroubleCodeId INT)
			MERGE Diagnostic.TroubleCodes AS t
			USING
			(
				SELECT
					TroubleCode
					,TroubleCodeDescription
					,CHECKSUM(TroubleCode, TroubleCodeDescription) [TroubleCodeHash]
				FROM @TroubleCodes t
				WHERE TroubleCodeId IS NULL
				GROUP BY
					TroubleCode
					,TroubleCodeDescription
					,CHECKSUM(TroubleCode, TroubleCodeDescription)
			) AS s
			ON (t.TroubleCodeHash = s.TroubleCodeHash)
			WHEN NOT MATCHED THEN
				INSERT
				(
					TroubleCode,
					TroubleCodeDescription
				)
				VALUES
				(
					TroubleCode,
					TroubleCodeDescription
				)
			OUTPUT INSERTED.TroubleCodeId
			INTO @NewTroubleCodes;

			DECLARE @NewResultTroubleCodes TABLE (ResultTroubleCodeId INT)
			MERGE Diagnostic.ResultTroubleCodes AS t
			USING
			(
				SELECT
					@ResultId [ResultId]
					,c.ControllerId
					,tc.TroubleCodeId
					,t.TroubleCodeInformation
				FROM @TroubleCodes t
				INNER JOIN Diagnostic.Controllers c
					ON t.ControllerId = c.ControllerId
						OR (t.ControllerId IS NULL AND c.ControllerHash = CHECKSUM(t.ControllerName))
				INNER JOIN Diagnostic.TroubleCodes tc
					ON t.TroubleCodeId = tc.TroubleCodeId
						OR (t.TroubleCodeId IS NULL AND tc.TroubleCodeHash = CHECKSUM(t.TroubleCode, t.TroubleCodeDescription))
			) AS s
			ON (t.ResultId = s.ResultId AND t.ControllerId = s.ControllerId AND t.TroubleCodeId = s.TroubleCodeId)
			WHEN NOT MATCHED THEN
				INSERT
				(
					ResultId
					,ControllerId
					,TroubleCodeId
					,TroubleCodeInformation
				)
				VALUES
				(
					ResultId
					,ControllerId
					,TroubleCodeId
					,TroubleCodeInformation
				)
			OUTPUT INSERTED.ResultTroubleCodeId
			INTO @NewResultTroubleCodes;

			DECLARE @NewResultFreezeFrames TABLE (FreezeFrameId INT)
			MERGE Diagnostic.ResultFreezeFrames AS t
			USING
			(
				SELECT
					c.ControllerId
					,@ResultId [ResultId]
					,ff.FreezeFrameId
					,ff.FreezeFrameTroubleCode
					,ff.SensorGroupsJson
				FROM @FreezeFrames ff
				INNER JOIN Diagnostic.Controllers c
					ON ff.ControllerId = c.ControllerId
						OR (ff.ControllerId IS NULL AND c.ControllerHash = CHECKSUM(ff.ControllerName))
			) AS s
			ON (t.FreezeFrameId = s.FreezeFrameId AND t.ResultId = s.ResultId AND t.ControllerId = s.ControllerId AND t.FreezeFrameTroubleCode = s.FreezeFrameTroubleCode)
			WHEN NOT MATCHED THEN
				INSERT (ResultId, ControllerId, FreezeFrameTroubleCode, SensorGroupsJson)
				VALUES (ResultId, ControllerId, FreezeFrameTroubleCode, SensorGroupsJson)
			WHEN MATCHED THEN
				UPDATE SET t.SensorGroupsJson = s.SensorGroupsJson
			OUTPUT INSERTED.FreezeFrameId
			INTO @NewResultFreezeFrames;

			SELECT @ResultId;

		END TRY
		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END
GO