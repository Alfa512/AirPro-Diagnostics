CREATE PROCEDURE [Access].[usp_GetRegistration]
	@UserGuid UNIQUEIDENTIFIER
	,@RegistrationId UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZone NVARCHAR(MAX)
	SELECT @UserTimeZone = u.TimeZoneInfoId
	FROM Access.Users u
	WHERE u.UserGuid = @UserGuid
	
	SELECT
		 r.RegistrationId
        ,r.RegistrationStatus
	    ,r.Email
	    ,r.CreatedByUserGuid [CreatedByUserGuid]
		,r.AccountGuid
		,r.CallbackUrl
		,r.ClientUserGuid
		,r.CompletedByUserGuid
		,r.CreatedByUserGuid
		,r.DifferentShopInfo
		,r.ShopGuid
		,r.StatusUpdateByUserGuid
		,r.UpdatedByUserGuid
	    ,CAST(r.UpdatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [UpdatedDt]
	    ,CAST(r.CreatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [CreatedDt]
		,CAST(r.StatusUpdateDt AT TIME ZONE @UserTimeZone AS DATETIME) [StatusUpdateDt]
		,CAST(r.CompletedDt AT TIME ZONE @UserTimeZone AS DATETIME) [CompletedDt]
	    ,createdBy.DisplayName [CreatedBy]
		,completedBy.DisplayName [CompletedBy]
		,statusUpdatedBy.DisplayName [StatusUpdateBy]
		,client.DisplayName [CreatedUser]
		,shop.Name [CreatedShop]
		,account.Name [CreatedAccount]
		,r.PassedStep [PassedStep]
		,a.*
		,s.[RegistrationShopId]
        ,s.[Name]
        ,s.[Phone]
        ,s.[Fax]
        ,s.[Address1]
        ,s.[Address2]
        ,s.[City]
        ,s.[StateId]
        ,s.[Zip]
        ,s.[SendToMitchellInd]
        ,s.[DiscountPercentage]
        ,s.[CCCShopId]
        ,s.[AllowAllRepairAutoClose]
        ,s.[AllowAutoRepairClose]
        ,s.[AllowScanAnalysisAutoClose]
        ,s.[ShopFixedPriceInd]
        ,s.[FirstScanCost]
        ,s.[AdditionalScanCost]
        ,s.[AutomaticRepairCloseDays]
        ,s.[HideFromReports]
        ,s.[DisableShopBillingNotification]
        ,s.[DisableShopStatementNotification]
        ,s.[DefaultInsuranceCompanyId]
        ,s.[AverageVehiclesPerMonth]
        ,s.[CurrencyId]
        ,s.[PricingPlanId]
        ,s.[EstimatePlanId]
        ,s.[BillingCycleId]
        ,s.[InsuranceCompaniesIds] [InsuranceCompaniesString]
        ,s.[InsuranceCompaniesPricingPlansJson]
        ,s.[InsuranceCompaniesEstimatePlansJson]
        ,s.[VehicleMakesIds] [VehicleMakesString]
		,s.AllowedRequestTypeIds [AllowedRequestTypesString]
        ,s.[VehicleMakesPricingPlansJson]
		,s.[AllowSelfScanAssessment]
		,u.[RegistrationUserId]
        ,u.[FirstName]
        ,u.[LastName]
        ,u.[JobTitle]
        ,u.[ContactNumber]
        ,u.[PhoneNumber]
        ,u.[AccessGroupIds] [AccessGroupIdsString]
        ,u.[ShopBillingNotification]
        ,u.[ShopReportNotification]
        ,u.[TimeZoneInfoId]

		FROM Access.Registrations r
		INNER JOIN Access.RegistrationAccounts a ON r.RegistrationAccountId = a.RegistrationAccountId
		INNER JOIN Access.RegistrationUsers u ON r.RegistrationUserId = u.RegistrationUserId
		INNER JOIN Access.RegistrationShops s ON r.RegistrationShopId = s.RegistrationShopId
		INNER JOIN Access.Users createdBy ON r.CreatedByUserGuid = createdBy.UserGuid
		LEFT JOIN Access.Users completedBy ON r.CompletedByUserGuid = completedBy.UserGuid
		LEFT JOIN Access.Users statusUpdatedBy ON r.StatusUpdateByUserGuid = statusUpdatedBy.UserGuid
		LEFT JOIN Access.Users client ON r.ClientUserGuid = client.UserGuid
		LEFT JOIN Access.Shops shop ON r.ShopGuid = shop.ShopGuid
		LEFT JOIN Access.Accounts account ON r.AccountGuid = account.AccountGuid
		WHERE RegistrationId = @RegistrationId

END