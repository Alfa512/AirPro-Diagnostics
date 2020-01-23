CREATE PROCEDURE [Access].[usp_CompleteRegistration]
	@RegistrationId UNIQUEIDENTIFIER
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

	BEGIN TRY

	DECLARE @NewUserGuid UNIQUEIDENTIFIER
	DECLARE @NewShopGuid UNIQUEIDENTIFIER
	DECLARE @NewAccountGuid UNIQUEIDENTIFIER
	SET @NewUserGuid = NEWID();
	SET @NewShopGuid = NEWID();
	SET @NewAccountGuid = NEWID();

	INSERT INTO Access.Users(ContactNumber, CreatedByUserGuid, CreatedDt, Email, EmailConfirmed, FirstName, LastName, JobTitle, PasswordHash, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, ShopBillingNotification, ShopReportNotification, TimeZoneInfoId, UserName, UserGuid, SecurityStamp)
    SELECT ru.ContactNumber, @UserGuid, GETUTCDATE(), r.Email, 1, ru.FirstName, ru.LastName, ru.JobTitle, ru.PasswordHash, ru.PhoneNumber, 0, 0, 1, 0, ru.ShopBillingNotification, ru.ShopReportNotification, ru.TimeZoneInfoId, r.Email, @NewUserGuid, NEWID()
    FROM Access.RegistrationUsers ru
	INNER JOIN Access.Registrations r ON ru.RegistrationUserId = r.RegistrationUserId
    WHERE r.RegistrationId = @RegistrationId
	PRINT @@ROWCOUNT
	INSERT INTO Access.UserGroups(GroupGuid,UserGuid,CreatedDt,CreatedByUserGuid)
	SELECT CONVERT(uniqueidentifier, value), @NewUserGuid, GETUTCDATE(),@UserGuid
	FROM STRING_SPLIT((SELECT TOP 1 AccessGroupIds FROM Access.RegistrationUsers ru INNER JOIN Access.Registrations r ON ru.RegistrationUserId = r.RegistrationUserId WHERE RegistrationId = @RegistrationId), ',')  
	WHERE RTRIM(value) <> ''; 
	PRINT @@ROWCOUNT
	INSERT INTO Access.Accounts(AccountGuid, ActiveInd, Address1, Address2, City, DiscountPercentage, CreatedByUserGuid, CreatedDt, Fax, Name, Phone, StateId, Zip)
    SELECT @NewAccountGuid, 1, ra.Address1, ra.Address2, ra.City, ISNULL(ra.DiscountPercentage, 0), @UserGuid, GETUTCDATE(), ra.Fax, ra.Name, ra.Phone, s.StateId, ra.Zip
    FROM Access.RegistrationAccounts ra
	INNER JOIN Access.Registrations r ON ra.RegistrationAccountId = r.RegistrationAccountId
	INNER JOIN Common.States s ON ra.StateId = s.Abbreviation
    WHERE r.RegistrationId = @RegistrationId
	PRINT @@ROWCOUNT
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
					   ,[SendToMitchellInd]
					   ,[DiscountPercentage]
					   ,[CCCShopId]
					   ,[PricingPlanId]
					   ,[AverageVehiclesPerMonth]
					   ,[EstimatePlanId]
					   ,[AllowAutoRepairClose]
					   ,[AllowScanAnalysisAutoClose]
					   ,[AllowAllRepairAutoClose]
					   ,[DefaultInsuranceCompanyId]
					   ,[ShopFixedPriceInd]
					   ,[FirstScanCost]
					   ,[AdditionalScanCost]
					   ,[AutomaticRepairCloseDays]
					   ,[ActiveInd]
					   ,[HideFromReports]
					   ,[CurrencyId]
					   ,[BillingCycleId]
					   ,[CreatedByUserGuid]
					   ,[CreatedDt]
					   ,[DisableShopBillingNotification]
					   ,[DisableShopStatementNotification]
					   ,[AllowSelfScanAssessment])
	SELECT 
		@NewShopGuid
		,@NewAccountGuid
        ,ISNULL(s.[Name], a.Name)
        ,ISNULL(s.[Phone], a.Phone)
        ,ISNULL(s.[Fax], a.Fax)
        ,ISNULL(s.[Address1], a.Address1)
        ,ISNULL(s.[Address2], a.Address2)
        ,ISNULL(s.[City], a.City)
        ,ISNULL(shopState.[StateId], accountState.StateId)
        ,ISNULL(s.[Zip], a.Zip)
        ,s.[SendToMitchellInd]
        ,ISNULL(s.[DiscountPercentage],0)
        ,s.[CCCShopId]
        ,s.[PricingPlanId]
        ,s.[AverageVehiclesPerMonth]
        ,s.[EstimatePlanId]
        ,s.[AllowAutoRepairClose]
        ,s.[AllowScanAnalysisAutoClose]
        ,s.[AllowAllRepairAutoClose]
        ,s.[DefaultInsuranceCompanyId]
        ,s.[ShopFixedPriceInd]
        ,ISNULL(s.[FirstScanCost], 0)
        ,ISNULL(s.[AdditionalScanCost], 0)
        ,s.[AutomaticRepairCloseDays]
		,1
        ,s.[HideFromReports]
        ,s.[CurrencyId]
        ,s.[BillingCycleId]		
		,@UserGuid
        ,GETUTCDATE()
        ,s.[DisableShopBillingNotification]
        ,s.[DisableShopStatementNotification]
		,s.[AllowSelfScanAssessment]
		FROM Access.Registrations r
		INNER JOIN Access.RegistrationShops s ON r.RegistrationShopId = s.RegistrationShopId
		INNER JOIN Access.RegistrationAccounts a ON r.RegistrationAccountId = a.RegistrationAccountId
		LEFT JOIN Common.States shopState ON s.StateId = shopState.Abbreviation
		LEFT JOIN Common.States accountState ON a.StateId = accountState.Abbreviation
		WHERE RegistrationId = @RegistrationId
	PRINT @@ROWCOUNT
	INSERT Access.UserAccounts (UserGuid,AccountGuid,CreatedByUserGuid,CreatedDt)
	SELECT @NewUserGuid, @NewAccountGuid, @UserGuid, GETUTCDATE()
	PRINT @@ROWCOUNT
	INSERT Access.UserShops(UserGuid,ShopGuid,CreatedByUserGuid,CreatedDt)
	SELECT @NewUserGuid, @NewShopGuid, @UserGuid, GETUTCDATE()
	PRINT @@ROWCOUNT
	INSERT INTO Access.ShopRequestTypes(ShopGuid,RequestTypeId)
	SELECT @NewShopGuid, CONVERT(INT, value) 
	FROM STRING_SPLIT((SELECT TOP 1 AllowedRequestTypeIds FROM Access.RegistrationShops rs INNER JOIN Access.Registrations r ON rs.RegistrationShopId = r.RegistrationShopId WHERE RegistrationId = @RegistrationId), ',')  
	WHERE RTRIM(value) <> '';
	PRINT @@ROWCOUNT
	INSERT INTO Access.ShopInsuranceCompanies(ShopId, InsuranceCompanyId)
	SELECT @NewShopGuid, CONVERT(INT, value) 
	FROM STRING_SPLIT((SELECT TOP 1 InsuranceCompaniesIds FROM Access.RegistrationShops rs INNER JOIN Access.Registrations r ON rs.RegistrationShopId = r.RegistrationShopId WHERE RegistrationId = @RegistrationId), ',')  
	WHERE RTRIM(value) <> ''; 
	PRINT @@ROWCOUNT
	INSERT INTO Access.ShopVehicleMakes(ShopId, VehicleMakeId)
	SELECT @NewShopGuid, CONVERT(INT, value) 
	FROM STRING_SPLIT((SELECT TOP 1 VehicleMakesIds FROM Access.RegistrationShops rs INNER JOIN Access.Registrations r ON rs.RegistrationShopId = r.RegistrationShopId WHERE RegistrationId = @RegistrationId), ',')  
	WHERE RTRIM(value) <> ''; 

	INSERT INTO Billing.ShopInsuranceCompaniesPricing (ShopId,PricingPlanId,InsuranceCompanyId)
	(SELECT @NewShopGuid, PlanId, InsuranceCompanyId
	FROM OPENJSON((SELECT TOP 1 InsuranceCompaniesPricingPlansJson FROM Access.RegistrationShops rs INNER JOIN Access.Registrations r ON rs.RegistrationShopId = r.RegistrationShopId WHERE RegistrationId = @RegistrationId)) 
	WITH (InsuranceCompanyId int 'strict $.InsuranceCompanyId', PlanId int '$.PlanId'))

	INSERT INTO Billing.ShopInsuranceCompaniesEstimate(ShopId,EstimatePlanId,InsuranceCompanyId)
	(SELECT @NewShopGuid, PlanId, InsuranceCompanyId
	FROM OPENJSON((SELECT TOP 1 InsuranceCompaniesEstimatePlansJson FROM Access.RegistrationShops rs INNER JOIN Access.Registrations r ON rs.RegistrationShopId = r.RegistrationShopId WHERE RegistrationId = @RegistrationId)) 
	WITH (InsuranceCompanyId int 'strict $.InsuranceCompanyId', PlanId int '$.PlanId'))

	INSERT INTO Billing.ShopVehicleMakesPricing(ShopId,VehicleMakeId,PricingPlanId)
	(SELECT @NewShopGuid, VehicleMakeId, PricingPlanId
	FROM OPENJSON((SELECT TOP 1 VehicleMakesPricingPlansJson FROM Access.RegistrationShops rs INNER JOIN Access.Registrations r ON rs.RegistrationShopId = r.RegistrationShopId WHERE RegistrationId = @RegistrationId)) 
	WITH (VehicleMakeId int 'strict $.VehicleMakeId', PricingPlanId int '$.PricingPlanId'))

	UPDATE Access.Registrations
	SET ShopGuid = @NewShopGuid, AccountGuid = @NewAccountGuid, ClientUserGuid = @NewUserGuid
	WHERE RegistrationId = @RegistrationId

	SELECT @NewUserGuid [UserGuid], @NewAccountGuid [AccountGuid], @NewShopGuid [ShopGuid]

	END TRY

	BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

	END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;
END