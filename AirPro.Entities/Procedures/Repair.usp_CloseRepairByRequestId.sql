
DROP PROCEDURE IF EXISTS Repair.usp_CloseRepairByRequestId;
GO

CREATE PROCEDURE Repair.usp_CloseRepairByRequestId
	@RequestId INT
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			/************************************************************
				Load Repair ID to Close.
			************************************************************/
			SELECT o.OrderId [RepairId]
			FROM Scan.Requests r
			INNER JOIN Scan.Reports rpt
				ON r.ReportId = rpt.ReportId
					AND rpt.CompletedInd = 1
					AND rpt.CanceledInd = 0
			INNER JOIN Repair.Orders o
				ON r.OrderId = o.OrderId
					AND o.Status = 1
			INNER JOIN Access.Shops s
				ON o.ShopGuid = s.ShopGuid
			WHERE r.RequestId = @RequestId
				AND ((s.AllowAutoRepairClose = 1 AND r.RequestTypeId = 3)
					OR (s.AllowScanAnalysisAutoClose = 1 AND r.RequestTypeId = 7)
					OR s.AllowAllRepairAutoClose = 1)
		
		END TRY
		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END
GO