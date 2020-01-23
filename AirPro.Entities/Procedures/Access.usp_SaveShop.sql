IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Access' AND ROUTINE_NAME = 'usp_SaveShop')
	DROP PROCEDURE Access.usp_SaveShop
GO

CREATE PROCEDURE Access.usp_SaveShop
	@ShopGuid UNIQUEIDENTIFIER OUTPUT
	,@Name NVARCHAR(MAX)
	,@AccountGuid UNIQUEIDENTIFIER
	,@Address1 NVARCHAR(MAX)
	,@Address2 NVARCHAR(MAX)
	,@City NVARCHAR(MAX)
	,@State NVARCHAR(MAX)
	,@Zip NVARCHAR(MAX) NULL
	,@Phone NVARCHAR(MAX) NULL
	,@Fax NVARCHAR(MAX) NULL
	,@Notes NVARCHAR(MAX) NULL
	,@CCCShopId NVARCHAR(128) NULL
	,@AverageVehiclesPerMonth INT NULL
	,@AllowAutoRepairClose BIT
	,@AllowScanAnalysis BIT
	,@AllowDemoScan BIT
	,@DefaultInsuranceCompanyId INT NULL
	,@AllowSelfScanAssessment BIT
	,@PricingPlanId INT NULL
	,@ShopFixedPriceInd BIT
	,@FirstScanCost DECIMAL(18,2)
	,@AdditionalScanCost DECIMAL(18,2)
	,@AutomaticRepairCloseDays INT NULL
	,@HideFromReports BIT
	,@CurrencyId INT
	,@BillingCycleId INT
	,@AllowScanAnalysisAutoClose BIT
	,@SendToMitchellInd BIT
	,@AllowAllRepairAutoClose BIT
	,@DisableShopStatementNotification BIT
	,@DisableShopBillingNotification BIT
	,@ActiveInd BIT
	,@EstimatePlanId INT
	,@DiscountPercentage INT
	,@AllowSelfScan BIT
	,@UserGuid UNIQUEIDENTIFIER NULL
	,@ShopContacts Access.udt_ShopContacts NULL READONLY
	,@ShopInsurancePlansPricingPlans Access.udt_ShopInsuranceCompaniesPricingPlans NULL  READONLY
	,@ShopInsurancePlansEstimatePlans Access.udt_ShopInsuranceCompaniesEstimatePlans NULL READONLY
	,@ShopVehicleMakesPricing Access.udt_ShopVehicleMakesPricing NULL READONLY
	,@InsuranceCompanyIds NVARCHAR(MAX)
	,@VehicleMakeIds NVARCHAR(MAX)
	,@RequestTypeIds NVARCHAR(MAX)
	,@EmployeeGuid UNIQUEIDENTIFIER
	,@AutomaticInvoicesInd BIT
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			/**********************************************************
				Step 1: Load Ids.
			**********************************************************/
			SET @ShopGuid = NULLIF(@ShopGuid, Common.udf_GetEmptyGuid());
			DECLARE @StateId INT
			SELECT TOP 1 @StateId = StateId
			FROM Common.States
			WHERE Abbreviation = @State
				OR Name = @State;

			/**********************************************************
				Step 2: Insert/Update Shop.
			**********************************************************/
			IF (@ShopGuid IS NULL)
				BEGIN
					SET @ShopGuid = NEWID();
					INSERT INTO [Access].[Shops]
					   ([ShopGuid]
					   ,[AccountGuid]
					   ,[Name]
					   ,[Phone]
					   ,[Fax]
					   ,[Address1]
					   ,[Address2]
					   ,[City]
					   ,[StateId]
					   ,[Zip]
					   ,[Notes]
					   ,[DiscountPercentage]
					   ,[CCCShopId]
					   ,[AllowSelfScan]
					   ,[PricingPlanId]
					   ,[AverageVehiclesPerMonth]
					   ,[EstimatePlanId]
					   ,[AllowAutoRepairClose]
					   ,[AllowScanAnalysis]
					   ,[DefaultInsuranceCompanyId]
					   ,[AllowSelfScanAssessment]
					   ,[ShopFixedPriceInd]
					   ,[FirstScanCost]
					   ,[AdditionalScanCost]
					   ,[AutomaticRepairCloseDays]
					   ,[ActiveInd]
					   ,[HideFromReports]
					   ,[AllowDemoScan]
					   ,[CurrencyId]
					   ,[CreatedByUserGuid]
					   ,[CreatedDt]
					   ,[BillingCycleId]
					   ,[AllowScanAnalysisAutoClose]
					   ,[SendToMitchellInd]
					   ,[AllowAllRepairAutoClose]
					   ,[DisableShopBillingNotification]
					   ,[DisableShopStatementNotification]
					   ,[EmployeeGuid]
					   ,[AutomaticInvoicesInd])
					VALUES
					   (@ShopGuid
					   ,@AccountGuid
					   ,@Name
					   ,@Phone
					   ,@Fax
					   ,@Address1
					   ,@Address2
					   ,@City
					   ,@StateId
					   ,@Zip
					   ,@Notes
					   ,@DiscountPercentage
					   ,@CCCShopId
					   ,@AllowSelfScan
					   ,@PricingPlanId
					   ,@AverageVehiclesPerMonth
					   ,@EstimatePlanId
					   ,@AllowAutoRepairClose
					   ,@AllowScanAnalysis
					   ,NULLIF(@DefaultInsuranceCompanyId, 0)
					   ,@AllowSelfScanAssessment
					   ,@ShopFixedPriceInd
					   ,@FirstScanCost
					   ,@AdditionalScanCost
					   ,@AutomaticRepairCloseDays
					   ,@ActiveInd
					   ,@HideFromReports
					   ,@AllowDemoScan
					   ,@CurrencyId
					   ,@UserGuid
					   ,GETUTCDATE()
					   ,@BillingCycleId
					   ,@AllowScanAnalysisAutoClose
					   ,@SendToMitchellInd
					   ,@AllowAllRepairAutoClose
					   ,@DisableShopBillingNotification
					   ,@DisableShopStatementNotification
					   ,@EmployeeGuid
					   ,@AutomaticInvoicesInd)
				END
			ELSE
				BEGIN
					UPDATE Access.Shops
						SET AccountGuid = @AccountGuid
							,Name = @Name
							,Phone = @Phone
							,Fax = @Fax
							,Address1 = @Address1
							,Address2 = @Address2
							,City = @City
							,StateId = @StateId
							,Zip = @Zip
							,Notes = @Notes
							,DiscountPercentage = @DiscountPercentage
							,CCCShopId = @CCCShopId
							,AllowSelfScan = @AllowSelfScan
							,PricingPlanId = @PricingPlanId
							,AverageVehiclesPerMonth = @AverageVehiclesPerMonth
							,EstimatePlanId = @EstimatePlanId
							,AllowAutoRepairClose = @AllowAutoRepairClose
							,AllowScanAnalysis = @AllowScanAnalysis
							,DefaultInsuranceCompanyId = NULLIF(@DefaultInsuranceCompanyId, 0)
							,AllowSelfScanAssessment = @AllowSelfScanAssessment
							,ShopFixedPriceInd = @ShopFixedPriceInd
							,FirstScanCost = @FirstScanCost
							,AdditionalScanCost = @AdditionalScanCost
							,AutomaticRepairCloseDays = @AutomaticRepairCloseDays
							,ActiveInd = @ActiveInd
							,HideFromReports = @HideFromReports
							,AllowDemoScan = @AllowDemoScan
							,CurrencyId = @CurrencyId
							,BillingCycleId = @BillingCycleId
							,AllowScanAnalysisAutoClose = @AllowScanAnalysisAutoClose
							,SendToMitchellInd = @SendToMitchellInd
							,AllowAllRepairAutoClose = @AllowAllRepairAutoClose
							,DisableShopBillingNotification = @DisableShopBillingNotification
							,DisableShopStatementNotification = @DisableShopStatementNotification
							,UpdatedByUserGuid = @UserGuid
							,UpdatedDt = GETUTCDATE()
							,EmployeeGuid = @EmployeeGuid
							,AutomaticInvoicesInd = @AutomaticInvoicesInd
					WHERE ShopGuid = @ShopGuid
				END

			/**********************************************************
				Step 3: Add/Update/Delete Shop Insurance Companies Pricing Plans.
			**********************************************************/
			MERGE Billing.ShopInsuranceCompaniesPricing AS t
			USING
			(
				SELECT
					r.InsuranceCompanyId
					,r.PricingPlanId
				FROM @ShopInsurancePlansPricingPlans r
			) AS s
			ON (t.ShopId = @ShopGuid AND s.InsuranceCompanyId = t.InsuranceCompanyId AND s.PricingPlanId = t.PricingPlanId)
			WHEN NOT MATCHED THEN
				INSERT (ShopId, InsuranceCompanyId, PricingPlanId)
				VALUES (@ShopGuid, InsuranceCompanyId, PricingPlanId)
			WHEN MATCHED THEN
				UPDATE SET t.PricingPlanId = s.PricingPlanId
			WHEN NOT MATCHED BY SOURCE AND t.ShopId = @ShopGuid THEN
				DELETE
			OUTPUT INSERTED.*;

			/**********************************************************
				Step 4: Add/Update/Delete Shop Insurance Companies Estimate Plans.
			**********************************************************/
			MERGE Billing.ShopInsuranceCompaniesEstimate AS t
			USING
			(
				SELECT
					@ShopGuid [ShopGuid]
					,r.InsuranceCompanyId
					,r.EstimatePlanId
				FROM @ShopInsurancePlansEstimatePlans r
			) AS s
			ON (s.ShopGuid = t.ShopId AND s.InsuranceCompanyId = t.InsuranceCompanyId AND s.EstimatePlanId = t.EstimatePlanId)
			WHEN NOT MATCHED THEN
				INSERT (ShopId, InsuranceCompanyId, EstimatePlanId)
				VALUES (@ShopGuid, InsuranceCompanyId, EstimatePlanId)
			WHEN MATCHED THEN
				UPDATE SET t.EstimatePlanId = s.EstimatePlanId
			WHEN NOT MATCHED BY SOURCE AND t.ShopId = @ShopGuid THEN
				DELETE
			OUTPUT INSERTED.*;

			/**********************************************************
				Step 5: Add/Update/Delete Shop Vehicle Makes Prcing Plans.
			**********************************************************/
			MERGE Billing.ShopVehicleMakesPricing AS t
			USING
			(
				SELECT
					@ShopGuid [ShopGuid]
					,r.VehicleMakeId
					,r.PricingPlanId
				FROM @ShopVehicleMakesPricing r
			) AS s
			ON (s.ShopGuid = t.ShopId AND s.VehicleMakeId = t.VehicleMakeId AND s.PricingPlanId = t.PricingPlanId)
			WHEN NOT MATCHED THEN
				INSERT (ShopId, VehicleMakeId, PricingPlanId)
				VALUES (@ShopGuid, VehicleMakeId, PricingPlanId)
			WHEN MATCHED THEN
				UPDATE SET t.PricingPlanId = s.PricingPlanId
			WHEN NOT MATCHED BY SOURCE AND t.ShopId = @ShopGuid THEN
				DELETE
			OUTPUT INSERTED.*;

			/**********************************************************
				Step 6: Add/Update/Delete Shop Contacts.
			**********************************************************/
			MERGE Access.ShopContacts AS t
			USING
			(
				SELECT
					r.FirstName
					,r.LastName
					,r.ShopGuid
					,r.ShopContactGuid
					,r.PhoneNumber
				FROM @ShopContacts r
			) AS s
			ON (s.ShopGuid = t.ShopGuid AND s.ShopContactGuid = t.ShopContactGuid)
			WHEN NOT MATCHED THEN
				INSERT (FirstName, LastName, ShopGuid, ShopContactGuid, PhoneNumber)
				VALUES (FirstName, LastName, @ShopGuid, NEWID(), PhoneNumber)
			WHEN MATCHED THEN
				UPDATE SET
					t.FirstName = s.FirstName
					,t.LastName = s.LastName
					,t.PhoneNumber = s.PhoneNumber
			WHEN NOT MATCHED BY SOURCE AND t.ShopGuid = @ShopGuid THEN
				UPDATE SET DeletedInd = 1
			OUTPUT INSERTED.*;

			/**********************************************************
				Step 7: Update Shop Insurance Companies.
			**********************************************************/
			MERGE Access.ShopInsuranceCompanies AS t
			USING
			(
				SELECT
					@ShopGuid [ShopGuid]
					,Val [InsuranceCompanyId]
				FROM Common.udf_JsonArrayToTable(@InsuranceCompanyIds)
			) AS s
			ON (t.ShopId = s.ShopGuid AND t.InsuranceCompanyId = s.InsuranceCompanyId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ShopId, InsuranceCompanyId)
				VALUES (ShopGuid, InsuranceCompanyId)
			WHEN NOT MATCHED BY SOURCE AND t.ShopId = @ShopGuid THEN
				DELETE
			OUTPUT INSERTED.*;

			/**********************************************************
				Step 8: Update Shop Vehicle Makes.
			**********************************************************/
			MERGE Access.ShopVehicleMakes AS t
			USING
			(
				SELECT
					@ShopGuid [ShopGuid]
					,Val [VehicleMakeId]
				FROM Common.udf_JsonArrayToTable(@VehicleMakeIds)
			) AS s
			ON (t.ShopId = s.ShopGuid AND t.VehicleMakeId = s.VehicleMakeId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ShopId, VehicleMakeId)
				VALUES (ShopGuid, VehicleMakeId)
			WHEN NOT MATCHED BY SOURCE AND t.ShopId = @ShopGuid THEN
				DELETE
			OUTPUT INSERTED.*;

			EXEC Repair.usp_CreateFromCCCEstimates;
			
			/**********************************************************
				Step 9: Update Shop Request Types.
			**********************************************************/
			MERGE Access.ShopRequestTypes AS t
			USING
			(
				SELECT
					@ShopGuid [ShopGuid]
					,Val [RequestTypeId]
				FROM Common.udf_JsonArrayToTable(@RequestTypeIds)
			) AS s
			ON (t.ShopGuid = s.ShopGuid AND t.RequestTypeId = s.RequestTypeId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (ShopGuid, RequestTypeId)
				VALUES (ShopGuid, RequestTypeId)
			WHEN NOT MATCHED BY SOURCE AND t.ShopGuid = @ShopGuid THEN
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