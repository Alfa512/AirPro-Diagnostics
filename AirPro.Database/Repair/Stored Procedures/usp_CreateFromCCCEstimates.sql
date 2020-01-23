
CREATE PROCEDURE Repair.usp_CreateFromCCCEstimates
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	BEGIN TRAN

		BEGIN TRY

			/****************************************
				Step 1: Update Processed.
			****************************************/
			DECLARE @CreateQueue TABLE
			(
				EstimateId INT
				,DocumentGuid UNIQUEIDENTIFIER
			)

			UPDATE ce
				SET ce.ProcessedInd = 1
					,ce.ProcessedDt = GETUTCDATE()
			OUTPUT INSERTED.EstimateId, INSERTED.DocumentGuid
			INTO @CreateQueue
			FROM Service.CCCEstimates ce
			WHERE ce.ProcessedInd = 0

			/****************************************
				Step 2: Load New Repairs.
			****************************************/
			SELECT
				CAST(NULL AS INT) [OrderId]
				,1 [Status]
				,ce.ShopRoNumber [ShopReferenceNumber]
				,CASE WHEN ic.RepairInsuranceCompanyId IS NULL THEN ISNULL(ce.InsuranceCompanyName, 'Unknown') ELSE NULL END [InsuranceCompanyOther]
				,NULL [InsuranceReferenceNumber]
				,ISNULL(ce.VehicleOdometer, 0) [Odometer]
				,0 [AirBagsDeployed]
				,ce.VehicleVin [VehicleVIN]
				,s.ShopGuid
				,'00000000-0000-0000-0000-000000000000' [CreatedByUserGuid]
				,GETUTCDATE() [CreatedDt]
				,CASE WHEN ic.RepairInsuranceCompanyId IS NULL THEN 1 ELSE ic.RepairInsuranceCompanyId END [InsuranceCompanyId]
				,ce.VehicleDrivable [DrivableInd]
				,ce.DocumentGuid [CCCDocumentGuid]
				,ce.EstimateId
			INTO #NewRepairs
			FROM Service.CCCEstimates ce
			INNER JOIN
			(
				SELECT
					e.EstimateId
					,e.DocumentGuid
				FROM
				(
					SELECT
						EstimateId
						,DocumentGuid
						,ROW_NUMBER() OVER (PARTITION BY DocumentGuid ORDER BY EstimateId DESC) [RowNumber]
					FROM @CreateQueue
				) e
				WHERE e.RowNumber = 1
			) cer
				ON ce.EstimateId = cer.EstimateId
			INNER JOIN Access.Shops s
				ON ce.ShopId = s.CCCShopId
			INNER JOIN Repair.Vehicles v
				ON ce.VehicleVIN = v.VehicleVIN
			LEFT JOIN Service.CCCInsuranceCompanies ic
				ON ce.InsuranceCompanyId = ic.CCCInsuranceCompanyId
			LEFT JOIN Repair.Orders r
				ON ce.DocumentGuid = r.CCCDocumentGuid AND r.Status = 1
			WHERE r.OrderId IS NULL

			/****************************************
				Step 3: Create New Repairs.
			****************************************/
			DECLARE @Inserted TABLE
			(
				OrderId INT
				,CCCDocumentGuid UNIQUEIDENTIFIER
				,VehicleVIN VARCHAR(100)
			)

			INSERT INTO Repair.Orders
			(
				[Status]
				,[ShopReferenceNumber]
				,[InsuranceCompanyOther]
				,[InsuranceReferenceNumber]
				,[Odometer]
				,[AirBagsDeployed]
				,[VehicleVIN]
				,[ShopGuid]
				,[CreatedByUserGuid]
				,[CreatedDt]
				,[InsuranceCompanyId]
				,[DrivableInd]
				,[CCCDocumentGuid]
			)
			OUTPUT
				INSERTED.OrderId
				,INSERTED.CCCDocumentGuid
				,INSERTED.VehicleVIN
			INTO @Inserted
			SELECT
				[Status]
				,[ShopReferenceNumber]
				,[InsuranceCompanyOther]
				,[InsuranceReferenceNumber]
				,[Odometer]
				,[AirBagsDeployed]
				,[VehicleVIN]
				,[ShopGuid]
				,[CreatedByUserGuid]
				,[CreatedDt]
				,[InsuranceCompanyId]
				,[DrivableInd]
				,[CCCDocumentGuid]
			FROM #NewRepairs

			UPDATE nr
				SET nr.OrderId = i.OrderId
			FROM #NewRepairs nr
			INNER JOIN @Inserted i
				ON nr.CCCDocumentGuid = i.CCCDocumentGuid
					AND nr.VehicleVIN = i.VehicleVIN

			/****************************************
				Step 4: Update Repair Point of Impacts.
			****************************************/
			INSERT INTO Repair.OrderPointOfImpacts
			(
				OrderID
				,PointOfImpactId
			)
			SELECT
				nr.OrderId
				,poi.PointOfImpactId
			FROM #NewRepairs nr
			INNER JOIN Repair.Orders o
				ON nr.OrderId = o.OrderId
			INNER JOIN Service.CCCEstimates c
				ON nr.EstimateId = c.EstimateId
			OUTER APPLY
			(
				SELECT Attribute
				FROM Common.udf_XmlToTable(c.RawXml)
				WHERE Element = 'POICode'
				GROUP BY Attribute
			) p
			INNER JOIN Repair.PointOfImpacts poi
				ON p.Attribute = poi.PointOfImpactId

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END