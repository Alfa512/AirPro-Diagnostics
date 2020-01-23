
CREATE PROCEDURE [Reporting].[usp_BuildReportData]
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @LoadId INT;

	-- Create Log Table.
	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'ReportDataLoads')
		BEGIN
			CREATE TABLE Reporting.ReportDataLoads
			(
				ReportDataLoadId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY
				,ReportDataCount INT NOT NULL DEFAULT(0)
				,ReportViewCount INT NOT NULL DEFAULT(0)
				,InsertedCount INT NOT NULL DEFAULT(0)
				,DeletedCount INT NOT NULL DEFAULT(0)
				,LogStartDt DATETIMEOFFSET NOT NULL DEFAULT(GETUTCDATE())
				,LogEndDt DATETIMEOFFSET NULL
				,SuccessInd BIT NOT NULL DEFAULT(0)
			);
		END

	BEGIN TRAN

		BEGIN TRY

			-- Clean Old Logs.
			UPDATE Reporting.ReportDataLoads
				SET LogEndDt = GETUTCDATE()
			WHERE LogEndDt IS NULL

			-- Create Log.
			INSERT INTO Reporting.ReportDataLoads (LogStartDt) VALUES (GETUTCDATE())
			SET @LoadId = SCOPE_IDENTITY()

			-- Validate Report Data Table.
			IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'ReportData')
				BEGIN

					SELECT
						NEWID() [DataGuid]
						,@LoadId [DataLoadId]
						,CHECKSUM(*) [DataChecksum]
						,*
					INTO Reporting.ReportData
					FROM Reporting.vwReportData
					
					UPDATE Reporting.ReportDataLoads
						SET
							InsertedCount = @@ROWCOUNT
							,ReportViewCount = @@ROWCOUNT
					WHERE ReportDataLoadId = @LoadId

					ALTER TABLE Reporting.ReportData ALTER COLUMN DataGuid UNIQUEIDENTIFIER NOT NULL;
					ALTER TABLE Reporting.ReportData ADD CONSTRAINT DF_DataGuid DEFAULT (NEWSEQUENTIALID()) FOR DataGuid;
					ALTER TABLE Reporting.ReportData ADD CONSTRAINT PK_ReportData PRIMARY KEY (DataGuid);

					ALTER TABLE Reporting.ReportData ALTER COLUMN DataChecksum INT NOT NULL;
					CREATE INDEX IX_DataChecksum ON Reporting.ReportData (DataChecksum);

					ALTER TABLE Reporting.ReportData ADD CONSTRAINT FK_DataLoadId FOREIGN KEY (DataLoadId) REFERENCES Reporting.ReportDataLoads (ReportDataLoadId);
					
					CREATE INDEX IX_ReportDumpSearch ON Reporting.ReportData (ShopGuid, AccountGuid, RequestTypeId, RepairStatusId);

				END
			ELSE
				BEGIN

					-- Load Updates.
					IF (OBJECT_ID('tempdb..#Updates') IS NOT NULL)
						DROP TABLE #Updates;
					SELECT *, CHECKSUM(*) [DataChecksum]
					INTO #Updates
					FROM Reporting.vwReportData vrd
					DECLARE @ViewCount INT = @@ROWCOUNT

					-- Index for Searching.
					CREATE INDEX IX_ReportDataUpdates ON #Updates (DataChecksum)

					-- Insert Updates.
					INSERT INTO Reporting.ReportData
					(
						DataLoadId
						,AccountGuid
						,AccountName
						,AccountAddress1
						,AccountAddress2
						,AccountCity
						,AccountState
						,AccountZip
						,AccountPhone
						,AccountFax
						,AccountDiscountPercentage
						,AccountRepUser
						,ShopGuid
						,ShopName
						,ShopNumber
						,ShopPhone
						,ShopFax
						,ShopAddress1
						,ShopAddress2
						,ShopCity
						,ShopState
						,ShopZip
						,ShopNotes
						,ShopDiscountPercentage
						,ShopBillingCycleId
						,ShopBillingCycleName
						,ShopCCCId
						,ShopSelfScan
						,ShopRepUser
						,ShopAutomaticInvoicesInd
						,RepairOrderId
						,RepairStatusId
						,RepairStatus
						,RepairRONumber
						,RepairInsuranceCompany
						,RepairInsuranceClaimNumber
						,RepairVehicleVIN
						,RepairVehicleMake
						,RepairVehicleModel
						,RepairVehicleYear
						,RepairVehicleTransmission
						,RepairVehicleMakeType
						,RepairVehicleFound
						,RepairVehicleOdometer
						,RepairVehicleAirBagsDeployed
						,RepairVehicleDrivableInd
						,RepairCreateByCCC
						,RepairCreatedByUser
						,RepairCreatedDt
						,RepairLastUpdatedByUser
						,RepairLastUpdatedDt
						,RequestId
						,RequestTypeId
						,RequestType
						,RequestTypeCategory
						,RequestOtherWarningInfo
						,RequestSeatRemovedInd
						,RequestProblemDescription
						,RequestNotes
						,RequestCreatedByUser
						,RequestCreatedDt
						,RequestLastUpdatedByUser
						,RequestUpdatedDt
						,ReportId
						,ReportCreatedByUser
						,ReportCreatedDt
						,ReportUpdatedByUser
						,ReportUpdatedDt
						,ReportTechUser
						,ReportTechUserProfileInd
						,ReportTechAssignedDt
						,ReportCompletedByUser
						,ReportCompletedDt
						,ReportCancelled
						,ReportInvoicedInd
						,ReportInvoicedByUser
						,ReportInvoicedDt
						,ReportInvoicedAmount
						,ReportInvoiceDiscountAmount
						,RepairInvoiceId
						,RepairInvoiceMemo
						,RepairInvoiceAmountApplied
						,RepairInvoiceDiscountAmount
						,RepairInvoiceCreatedByUser
						,RepairInvoiceCreatedDt
						,PaymentID
						,PaymentType
						,PaymentDiscountPercentage
						,PaymentTotalAmount
						,PaymentRefNumber
						,PaymentMemo
						,PaymentCreatedByUser
						,PaymentCreatedDt
						,PaymentUpdatedByUser
						,PaymentUpdatedDt
						,DataChecksum
						,ReportCancelReasonTypeId
						,ReportCancelReasonTypeName
					)
					--OUTPUT INSERTED.*
					SELECT 
						@LoadId
						,u.AccountGuid
						,u.AccountName
						,u.AccountAddress1
						,u.AccountAddress2
						,u.AccountCity
						,u.AccountState
						,u.AccountZip
						,u.AccountPhone
						,u.AccountFax
						,u.AccountDiscountPercentage
						,u.AccountRepUser
						,u.ShopGuid
						,u.ShopName
						,u.ShopNumber
						,u.ShopPhone
						,u.ShopFax
						,u.ShopAddress1
						,u.ShopAddress2
						,u.ShopCity
						,u.ShopState
						,u.ShopZip
						,u.ShopNotes
						,u.ShopDiscountPercentage
						,u.ShopBillingCycleId
						,u.ShopBillingCycleName
						,u.ShopCCCId
						,u.ShopSelfScan
						,u.ShopRepUser
						,u.ShopAutomaticInvoicesInd
						,u.RepairOrderId
						,u.RepairStatusId
						,u.RepairStatus
						,u.RepairRONumber
						,u.RepairInsuranceCompany
						,u.RepairInsuranceClaimNumber
						,u.RepairVehicleVIN
						,u.RepairVehicleMake
						,u.RepairVehicleModel
						,u.RepairVehicleYear
						,u.RepairVehicleTransmission
						,u.RepairVehicleMakeType
						,u.RepairVehicleFound
						,u.RepairVehicleOdometer
						,u.RepairVehicleAirBagsDeployed
						,u.RepairVehicleDrivableInd
						,u.RepairCreateByCCC
						,u.RepairCreatedByUser
						,u.RepairCreatedDt
						,u.RepairLastUpdatedByUser
						,u.RepairLastUpdatedDt
						,u.RequestId
						,u.RequestTypeId
						,u.RequestType
						,u.RequestTypeCategory
						,u.RequestOtherWarningInfo
						,u.RequestSeatRemovedInd
						,u.RequestProblemDescription
						,u.RequestNotes
						,u.RequestCreatedByUser
						,u.RequestCreatedDt
						,u.RequestLastUpdatedByUser
						,u.RequestUpdatedDt
						,u.ReportId
						,u.ReportCreatedByUser
						,u.ReportCreatedDt
						,u.ReportUpdatedByUser
						,u.ReportUpdatedDt
						,u.ReportTechUser
						,u.ReportTechUserProfileInd
						,u.ReportTechAssignedDt
						,u.ReportCompletedByUser
						,u.ReportCompletedDt
						,u.ReportCancelled
						,u.ReportInvoicedInd
						,u.ReportInvoicedByUser
						,u.ReportInvoicedDt
						,u.ReportInvoicedAmount
						,u.ReportInvoiceDiscountAmount
						,u.RepairInvoiceId
						,u.RepairInvoiceMemo
						,u.RepairInvoiceAmountApplied
						,u.RepairInvoiceDiscountAmount
						,u.RepairInvoiceCreatedByUser
						,u.RepairInvoiceCreatedDt
						,u.PaymentID
						,u.PaymentType
						,u.PaymentDiscountPercentage
						,u.PaymentTotalAmount
						,u.PaymentRefNumber
						,u.PaymentMemo
						,u.PaymentCreatedByUser
						,u.PaymentCreatedDt
						,u.PaymentUpdatedByUser
						,u.PaymentUpdatedDt
						,u.DataChecksum
						,u.ReportCancelReasonTypeId
						,u.ReportCancelReasonTypeName
					FROM #Updates u
					LEFT JOIN Reporting.ReportData rd
						ON u.DataChecksum = rd.DataChecksum
					WHERE rd.DataChecksum IS NULL
					DECLARE @InsertedCount INT = @@ROWCOUNT

					-- Delete Old.
					DELETE rd
					--OUTPUT DELETED.*
					FROM Reporting.ReportData rd
					LEFT JOIN #Updates u
						ON rd.DataChecksum = u.DataChecksum
					WHERE u.DataChecksum IS NULL
					DECLARE @DeletedCount INT = @@ROWCOUNT

					-- Update Log.
					UPDATE Reporting.ReportDataLoads
						SET ReportViewCount = @ViewCount
							,InsertedCount = @InsertedCount
							,DeletedCount = @DeletedCount
					WHERE ReportDataLoadId = @LoadId

				END

		END TRY
		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

	UPDATE STATISTICS Reporting.ReportData;

	UPDATE Reporting.ReportDataLoads
		SET ReportDataCount = (SELECT COUNT(1) FROM Reporting.ReportData)
			,LogEndDt = GETUTCDATE()
			,SuccessInd = 1
	WHERE ReportDataLoadId = @LoadId

END
GO