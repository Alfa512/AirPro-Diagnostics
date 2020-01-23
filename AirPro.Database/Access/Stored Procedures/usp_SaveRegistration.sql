
CREATE PROCEDURE [Access].[usp_SaveRegistration]
	@RegistrationId UNIQUEIDENTIFIER
	,@RegistrationShopId INT = 0
	,@RegistrationAccountId INT = 0
	,@RegistrationUserId INT = 0
	,@RegistrationStatus INT NULL = NULL
	,@CallbackUrl NVARCHAR(MAX) NULL = NULL
	,@Email NVARCHAR(MAX) NULL = NULL
	,@DifferentShopInfo NVARCHAR(MAX) NULL = NULL
	,@StatusUpdateDt DATETIME NULL = NULL
	,@CompletedDt DATETIME NULL = NULL
	,@CreatedDt DATETIME NULL = NULL
	,@UpdatedDt DATETIME NULL = NULL
	,@StatusUpdateByUserGuid UNIQUEIDENTIFIER NULL = NULL
	,@CompletedByUserGuid UNIQUEIDENTIFIER NULL = NULL
	,@CreatedByUserGuid UNIQUEIDENTIFIER NULL = NULL
	,@UpdatedByUserGuid UNIQUEIDENTIFIER NULL = NULL

	,@FirstName NVARCHAR(MAX) NULL = NULL
	,@LastName NVARCHAR(MAX) NULL = NULL
	,@JobTitle NVARCHAR(MAX) NULL = NULL
	,@ContactNumber NVARCHAR(MAX) NULL = NULL
	,@PhoneNumber NVARCHAR(MAX) NULL = NULL
	,@PasswordHash NVARCHAR(MAX) NULL = NULL
	,@TimeZoneInfoId NVARCHAR(128) NULL = NULL
	,@AccessGroupIds NVARCHAR(MAX) NULL = NULL
	,@ShopBillingNotification BIT NULL = NULL
	,@ShopReportNotification BIT NULL = NULL

	,@AccountName NVARCHAR(MAX) = NULL
	,@AccountCity NVARCHAR(MAX) = NULL
	,@AccountState NVARCHAR(MAX) = NULL
	,@AccountZip NVARCHAR(MAX) = NULL
	,@AccountPhone NVARCHAR(MAX) = NULL
	,@AccountFax NVARCHAR(MAX) = NULL
	,@AccountAddress1 NVARCHAR(MAX) = NULL
	,@AccountAddress2 NVARCHAR(MAX) = NULL
	,@DiscountPercentage INT = NULL

	,@ShopName NVARCHAR(MAX) NULL = NULL
	,@ShopAccountGuid UNIQUEIDENTIFIER NULL = NULL
	,@ShopAddress1 NVARCHAR(MAX) NULL = NULL
	,@ShopAddress2 NVARCHAR(MAX) NULL = NULL
	,@ShopCity NVARCHAR(MAX) NULL = NULL
	,@ShopState NVARCHAR(MAX) NULL = NULL
	,@ShopZip NVARCHAR(MAX) NULL = NULL
	,@ShopPhone NVARCHAR(MAX) NULL = NULL
	,@ShopFax NVARCHAR(MAX) NULL = NULL
	,@CCCShopId NVARCHAR(128) NULL = NULL
	,@AverageVehiclesPerMonth INT NULL = NULL
	,@AllowAutoRepairClose BIT NULL = NULL
	,@AllowScanAnalysisAutoClose BIT NULL = NULL
	,@AllowAllRepairAutoClose BIT NULL = NULL
	,@ShopDiscountPercentage INT = NULL

	,@ShopFixedPriceInd BIT = NULL
	,@FirstScanCost DECIMAL(18,2) = NULL
	,@AdditionalScanCost DECIMAL(18,2) = NULL
	,@AutomaticRepairCloseDays INT NULL = NULL
	,@HideFromReports BIT NULL = NULL
	,@DisableShopStatementNotification BIT NULL = NULL
	,@DisableShopBillingNotification BIT NULL = NULL
	,@SendToMitchellInd BIT NULL = NULL

	,@DefaultInsuranceCompanyId INT NULL = NULL
		
	,@CurrencyId INT NULL = NULL
	,@EstimatePlanId INT NULL = NULL
	,@PricingPlanId INT NULL = NULL
	,@BillingCycleId INT NULL = NULL

	,@AllowedRequestTypeIds NVARCHAR(MAX) NULL = NULL
	,@InsuranceCompaniesIds NVARCHAR(MAX) NULL = NULL
	,@InsuranceCompaniesPricingPlansJson NVARCHAR(MAX) NULL = NULL
	,@InsuranceCompaniesEstimatePlansJson NVARCHAR(MAX) NULL = NULL
	,@VehicleMakesIds NVARCHAR(MAX) NULL = NULL
	,@VehicleMakesPricingPlansJson NVARCHAR(MAX) NULL = NULL
	,@PassedStep INT NULL = NULL
	,@AllowSelfScanAssessment BIT = 0
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY
		IF (@RegistrationId = Common.udf_GetEmptyGuid())
			BEGIN
				INSERT INTO [Access].[RegistrationUsers]
				([FirstName]
				,[LastName]
				,[JobTitle]
				,[ContactNumber]
				,[PhoneNumber]
				,[AccessGroupIds]
				,[ShopBillingNotification]
				,[ShopReportNotification]
				,[TimeZoneInfoId])
				VALUES
				(@FirstName
				,@LastName
				,@JobTitle
				,@ContactNumber
				,@PhoneNumber
				,@AccessGroupIds
				,ISNULL(@ShopBillingNotification, 0)
				,ISNULL(@ShopReportNotification, 0)
				,@TimeZoneInfoId)

				SELECT @RegistrationUserId = SCOPE_IDENTITY()

				INSERT INTO [Access].[RegistrationAccounts]
				([Name]
				,[Phone]
				,[Fax]
				,[Address1]
				,[Address2]
				,[City]
				,[StateId]
				,[Zip]
				,[DiscountPercentage])
				VALUES
				(@AccountName
				,@AccountPhone
				,@AccountFax
				,@AccountAddress1
				,@AccountAddress2
				,@AccountCity
				,@AccountState
				,@AccountZip
				,@DiscountPercentage)

				SELECT @RegistrationAccountId = SCOPE_IDENTITY()

				INSERT INTO [Access].[RegistrationShops]
				([Name]
				,[Phone]
				,[Fax]
				,[Address1]
				,[Address2]
				,[City]
				,[StateId]
				,[Zip]
				,[SendToMitchellInd]
				,[DiscountPercentage]
				,[CCCShopId]
				,[AllowAllRepairAutoClose]
				,[AllowAutoRepairClose]
				,[AllowScanAnalysisAutoClose]
				,[ShopFixedPriceInd]
				,[FirstScanCost]
				,[AdditionalScanCost]
				,[AutomaticRepairCloseDays]
				,[HideFromReports]
				,[DisableShopBillingNotification]
				,[DisableShopStatementNotification]
				,[DefaultInsuranceCompanyId]
				,[AverageVehiclesPerMonth]
				,[CurrencyId]
				,[PricingPlanId]
				,[EstimatePlanId]
				,[BillingCycleId]
				,[AllowedRequestTypeIds]
				,[InsuranceCompaniesIds]
				,[InsuranceCompaniesPricingPlansJson]
				,[InsuranceCompaniesEstimatePlansJson]
				,[VehicleMakesIds]
				,[VehicleMakesPricingPlansJson]
				,[AllowSelfScanAssessment])
				VALUES
				(@ShopName
				,@ShopPhone
				,@ShopFax
				,@ShopAddress1
				,@ShopAddress2
				,@ShopCity
				,@ShopState
				,@ShopZip
				,@SendToMitchellInd
				,@ShopDiscountPercentage
				,@CCCShopId
				,ISNULL(@AllowAllRepairAutoClose, 0)
				,ISNULL(@AllowAutoRepairClose, 0)
				,ISNULL(@AllowScanAnalysisAutoClose, 0)
				,ISNULL(@ShopFixedPriceInd, 0)
				,@FirstScanCost
				,@AdditionalScanCost
				,@AutomaticRepairCloseDays
				,ISNULL(@HideFromReports, 0)
				,ISNULL(@DisableShopBillingNotification, 0)
				,ISNULL(@DisableShopStatementNotification, 0)
				,@DefaultInsuranceCompanyId
				,@AverageVehiclesPerMonth
				,@CurrencyId
				,@PricingPlanId
				,@EstimatePlanId
				,@BillingCycleId
				,@AllowedRequestTypeIds
				,@InsuranceCompaniesIds
				,@InsuranceCompaniesPricingPlansJson
				,@InsuranceCompaniesEstimatePlansJson
				,@VehicleMakesIds
				,@VehicleMakesPricingPlansJson
				,@AllowSelfScanAssessment)

				SELECT @RegistrationShopId = SCOPE_IDENTITY()
				SELECT @RegistrationId = NEWID()
				INSERT INTO [Access].[Registrations]
				([RegistrationId]
				,[RegistrationStatus]
				,[CallbackUrl]
				,[Email]
				,[DifferentShopInfo]
				,[RegistrationUserId]
				,[RegistrationAccountId]
				,[RegistrationShopId]
				,[PassedStep]
				,[CreatedByUserGuid]
				,[CreatedDt]
				,[StatusUpdateByUserGuid]
				,[StatusUpdateDt])
				VALUES
				(@RegistrationId
				,ISNULL(@RegistrationStatus, 0)
				,@CallbackUrl
				,@Email
				,ISNULL(@DifferentShopInfo, 0)
				,@RegistrationUserId
				,@RegistrationAccountId
				,@RegistrationShopId
				,@PassedStep
				,@UserGuid
				,GETUTCDATE()
				,@UserGuid
				,GETUTCDATE())
			END
		ELSE
			BEGIN
				IF (@PassedStep IS NULL)
					BEGIN
						DECLARE @OldRegistrationStatus INT
						SELECT TOP 1 @OldRegistrationStatus = r.RegistrationStatus FROM Access.Registrations r WHERE r.RegistrationId = @RegistrationId
						IF (@RegistrationStatus <> @OldRegistrationStatus)
							BEGIN
								UPDATE [Access].[Registrations]
								SET [RegistrationStatus] = @RegistrationStatus
								,[StatusUpdateDt] = GETUTCDATE()
								,[StatusUpdateByUserGuid] = @UserGuid
								WHERE RegistrationId = @RegistrationId

								IF (@RegistrationStatus = 2)
								BEGIN
									UPDATE [Access].[Registrations]
									SET CompletedDt = GETUTCDATE()
									,CompletedByUserGuid = @UserGuid
									WHERE RegistrationId = @RegistrationId
								END
							END
						UPDATE [Access].[RegistrationUsers]
						SET [FirstName] = @FirstName
						,[LastName] = @LastName
						,[JobTitle] = @JobTitle
						,[ContactNumber] = @ContactNumber
						,[PhoneNumber] = @PhoneNumber
						,[AccessGroupIds] = @AccessGroupIds
						,[ShopBillingNotification] = @ShopBillingNotification
						,[ShopReportNotification] = @ShopReportNotification
						,[TimeZoneInfoId] = @TimeZoneInfoId
						WHERE RegistrationUserId = @RegistrationUserId

						UPDATE [Access].[RegistrationAccounts]
						SET [Name] = @AccountName
						,[Phone] = @AccountPhone
						,[Fax] = @AccountFax
						,[Address1] = @AccountAddress1
						,[Address2] = @AccountAddress2
						,[City] = @AccountCity
						,[StateId] = @AccountState
						,[Zip] = @AccountZip
						,[DiscountPercentage] = @DiscountPercentage
						WHERE RegistrationAccountId = @RegistrationAccountId

						UPDATE [Access].[RegistrationShops]
						SET [Name] = @ShopName
						,[Phone] = @ShopPhone
						,[Fax] = @ShopFax
						,[Address1] = @ShopAddress1
						,[Address2] = @ShopAddress2
						,[City] = @ShopCity
						,[StateId] = @ShopState
						,[Zip] = @ShopZip
						,[SendToMitchellInd] = @SendToMitchellInd
						,[DiscountPercentage] = @ShopDiscountPercentage
						,[CCCShopId] = @CCCShopId
						,[AllowAllRepairAutoClose] = @AllowAllRepairAutoClose
						,[AllowAutoRepairClose] = @AllowAutoRepairClose
						,[AllowScanAnalysisAutoClose] = @AllowScanAnalysisAutoClose
						,[ShopFixedPriceInd] = @ShopFixedPriceInd
						,[FirstScanCost] = @FirstScanCost
						,[AdditionalScanCost] = @AdditionalScanCost
						,[AutomaticRepairCloseDays] = @AutomaticRepairCloseDays
						,[HideFromReports] = @HideFromReports
						,[DisableShopBillingNotification] = @DisableShopBillingNotification
						,[DisableShopStatementNotification] = @DisableShopStatementNotification
						,[DefaultInsuranceCompanyId] = @DefaultInsuranceCompanyId
						,[AverageVehiclesPerMonth] = @AverageVehiclesPerMonth
						,[CurrencyId] = @CurrencyId
						,[PricingPlanId] = @PricingPlanId
						,[EstimatePlanId] = @EstimatePlanId
						,[BillingCycleId] = @BillingCycleId
						,[AllowedRequestTypeIds] = @AllowedRequestTypeIds
						,[InsuranceCompaniesIds] = @InsuranceCompaniesIds
						,[InsuranceCompaniesPricingPlansJson] = @InsuranceCompaniesPricingPlansJson
						,[InsuranceCompaniesEstimatePlansJson] = @InsuranceCompaniesEstimatePlansJson
						,[VehicleMakesIds] = @VehicleMakesIds
						,[VehicleMakesPricingPlansJson] = @VehicleMakesPricingPlansJson
						,[AllowSelfScanAssessment] = @AllowSelfScanAssessment
						WHERE RegistrationShopId = @RegistrationShopId

						UPDATE [Access].[Registrations]
						SET [CallbackUrl] = @CallbackUrl
						,[Email] = @Email
						,[DifferentShopInfo] = @DifferentShopInfo
						,[RegistrationUserId] = @RegistrationUserId
						,[RegistrationAccountId] = @RegistrationAccountId
						,[RegistrationShopId] = @RegistrationShopId
						,[UpdatedByUserGuid] = @UserGuid
						,[UpdatedDt] = GETUTCDATE()
						WHERE RegistrationId = @RegistrationId
					END
				ELSE IF (@PassedStep = 2)
					BEGIN
						UPDATE [Access].[RegistrationUsers]
						SET [FirstName] = @FirstName
						,[LastName] = @LastName
						,[JobTitle] = @JobTitle
						,[ContactNumber] = @ContactNumber
						,[PhoneNumber] = @PhoneNumber
						,[ShopBillingNotification] = @ShopBillingNotification
						,[ShopReportNotification] = @ShopReportNotification
						,[TimeZoneInfoId] = @TimeZoneInfoId
						,[PasswordHash] = @PasswordHash
						WHERE RegistrationUserId = @RegistrationUserId

						UPDATE [Access].[Registrations]
						SET [PassedStep] = @PassedStep
						,[RegistrationStatus] = 3
						WHERE RegistrationId = @RegistrationId
					END
				ELSE IF (@PassedStep = 3)
					BEGIN
						UPDATE [Access].[RegistrationAccounts]
						SET [Name] = @AccountName
						,[Phone] = @AccountPhone
						,[Fax] = @AccountFax
						,[Address1] = @AccountAddress1
						,[Address2] = @AccountAddress2
						,[City] = @AccountCity
						,[StateId] = @AccountState
						,[Zip] = @AccountZip
						WHERE RegistrationAccountId = @RegistrationAccountId

						UPDATE [Access].[RegistrationShops]
						SET [BillingCycleId] = @BillingCycleId
						WHERE RegistrationShopId = @RegistrationShopId

						UPDATE [Access].[Registrations]
						SET [DifferentShopInfo] = @DifferentShopInfo
						,[PassedStep] = @PassedStep
						,[RegistrationStatus] = 3
						WHERE RegistrationId = @RegistrationId
					END
				ELSE IF (@PassedStep = 4 AND @DifferentShopInfo = 1)
					BEGIN
						UPDATE [Access].[RegistrationShops]
						SET [Name] = @ShopName
						,[Phone] = @ShopPhone
						,[Fax] = @ShopFax
						,[Address1] = @ShopAddress1
						,[Address2] = @ShopAddress2
						,[City] = @ShopCity
						,[StateId] = @ShopState
						,[Zip] = @ShopZip
						WHERE RegistrationShopId = @RegistrationShopId

						UPDATE [Access].[Registrations]
						SET [PassedStep] = @PassedStep
						,[RegistrationStatus] = 3
						WHERE RegistrationId = @RegistrationId
					END
				ELSE IF ((@PassedStep = 5 AND @DifferentShopInfo = 1) OR (@PassedStep = 4 AND @DifferentShopInfo = 0))
					BEGIN
						UPDATE [Access].[RegistrationShops]
						SET [AverageVehiclesPerMonth] = @AverageVehiclesPerMonth
						,[InsuranceCompaniesIds] = @InsuranceCompaniesIds
						,[VehicleMakesIds] = @VehicleMakesIds
						WHERE RegistrationShopId = @RegistrationShopId

						UPDATE [Access].[Registrations]
						SET [PassedStep] = @PassedStep
						,[RegistrationStatus] = 3
						WHERE RegistrationId = @RegistrationId
					END
				ELSE IF ((@PassedStep = 6 AND @DifferentShopInfo = 1) OR (@PassedStep = 5 AND @DifferentShopInfo = 0))
					BEGIN
						UPDATE [Access].[RegistrationShops]
						SET [SendToMitchellInd] = @SendToMitchellInd
						,[CCCShopId] = @CCCShopId
						WHERE RegistrationShopId = @RegistrationShopId

						UPDATE [Access].[Registrations]
						SET [PassedStep] = @PassedStep
						,[RegistrationStatus] = 4
						WHERE RegistrationId = @RegistrationId
					END
			END

			SELECT @RegistrationId

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END