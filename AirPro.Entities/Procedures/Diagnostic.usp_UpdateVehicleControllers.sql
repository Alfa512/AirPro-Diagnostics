IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Diagnostic' AND ROUTINE_NAME = 'usp_UpdateVehicleControllers')
	DROP PROCEDURE Diagnostic.usp_UpdateVehicleControllers
GO

CREATE PROCEDURE Diagnostic.usp_UpdateVehicleControllers
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			WITH VehicleControllers
			AS
			(
				SELECT
					o.VehicleVIN
					,rotc.ControllerId
					,MAX(rotc.CreatedDt) [LastCreatedDt]
				FROM Repair.Orders o
				INNER JOIN Scan.ReportOrderTroubleCodes rotc
					INNER JOIN Scan.ReportTroubleCodeRecommendations rtcr
						ON rotc.ReportOrderTroubleCodeId = rtcr.ReportOrderTroubleCodeId
							AND rtcr.ExcludeFromReportInd = 0
					ON o.OrderId = rotc.OrderId
				INNER JOIN Access.Shops s
					ON o.ShopGuid = s.ShopGuid
						AND s.HideFromReports = 0
				GROUP BY
					o.VehicleVIN
					,rotc.ControllerId
			)

			MERGE Diagnostic.VehicleControllers AS t
			USING
			(
				SELECT
					v.VehicleMakeId
					,v.Model
					,v.Year
					,vc.ControllerId
					,CAST(MAX(vc.LastCreatedDt) AS DATE) [LastRecordedDt]
				FROM VehicleControllers vc
				INNER JOIN Repair.Vehicles v
					ON vc.VehicleVIN = v.VehicleVIN
				GROUP BY
					v.VehicleMakeId
					,v.Model
					,v.Year
					,vc.ControllerId
			) AS s
			ON (t.VehicleMakeId = s.VehicleMakeId AND t.VehicleModelName = s.Model AND t.VehicleYear = s.Year AND t.ControllerId = s.ControllerId)
			WHEN MATCHED AND t.LastRecordedDt <> s.LastRecordedDt THEN
				UPDATE SET t.LastRecordedDt = s.LastRecordedDt
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (VehicleMakeId, VehicleModelName, VehicleYear, ControllerId, LastRecordedDt)
				VALUES (VehicleMakeId, Model, Year, ControllerId, LastRecordedDt)
			WHEN NOT MATCHED BY SOURCE THEN
				DELETE
			OUTPUT INSERTED.*;

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END
GO
