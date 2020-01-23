
CREATE PROCEDURE Service.usp_SaveMitchellRequest
	@ShopGuid UNIQUEIDENTIFIER
	,@VehicleVIN VARCHAR(128)
	,@MitchellRecId VARCHAR(128)
	,@ShopRONum VARCHAR(128)
	,@InsuranceCoName VARCHAR(128)
	,@Odometer INT
	,@DrivableInd BIT
	,@AirBagsDeployedInd BIT
	,@RequestBody VARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			INSERT INTO Service.MitchellRequests
			(
				ShopGuid
				,MitchellRecId
				,VehicleVIN
				,ShopRONum
				,InsuranceCoName
				,Odometer
				,DrivableInd
				,AirBagsDeployedInd
				,RequestBody
				,RequestDt
			)
			SELECT
				@ShopGuid
				,@MitchellRecId
				,@VehicleVIN
				,@ShopRONum
				,@InsuranceCoName
				,@Odometer
				,@DrivableInd
				,@AirBagsDeployedInd
				,@RequestBody
				,GETUTCDATE()

			MERGE Repair.Orders AS t
			USING
			(
				SELECT
					mr.RequestId
					,mr.ShopGuid
					,mr.VehicleVIN
					,mr.ShopRONum
					,mr.InsuranceCoName
					,ISNULL(mr.Odometer, 0)
					,CAST(ISNULL(mr.AirBagsDeployedInd, 0) AS BIT)
					,CAST(ISNULL(mr.DrivableInd, 1) AS BIT)
				FROM Service.MitchellRequests mr
				WHERE mr.RequestId IN
				(
					SELECT
						r.RequestId
					FROM
					(
						SELECT MAX(RequestId) [RequestId], ShopGuid, VehicleVIN
						FROM Service.MitchellRequests mr
						GROUP BY ShopGuid, VehicleVIN
					) r
					INNER JOIN Access.Shops s
						ON r.ShopGuid = s.ShopGuid
					INNER JOIN Repair.Vehicles v
						ON r.VehicleVIN = v.VehicleVIN
					LEFT JOIN Repair.Orders o
						ON r.ShopGuid = o.ShopGuid
							AND r.VehicleVIN = o.VehicleVIN
					WHERE o.OrderId IS NULL
				)
			) AS s (RequestId, ShopGuid, VehicleVIN, ShopRONum, InsuranceCoName, Odometer, AirBagsDeployedInd, DrivableInd)
			ON (t.ShopGuid = s.ShopGuid AND t.VehicleVIN = s.VehicleVIN)
			WHEN NOT MATCHED
				THEN
					INSERT
					(
						Status
						,ShopGuid
						,VehicleVIN
						,ShopReferenceNumber
						,InsuranceCompanyId
						,InsuranceCompanyOther
						,Odometer
						,AirBagsDeployed
						,DrivableInd
						,CreatedByUserGuid
						,CreatedDt
						,MitchellRequestId
					)
					VALUES
					(
						1
						,ShopGuid
						,VehicleVIN
						,ShopRONum
						,1
						,InsuranceCoName
						,Odometer
						,AirBagsDeployedInd
						,DrivableInd
						,Common.udf_GetEmptyGuid()
						,GETUTCDATE()
						,RequestId
					)
			OUTPUT inserted.*;

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH
	
	IF @@TRANCOUNT > 0 COMMIT TRAN;

END