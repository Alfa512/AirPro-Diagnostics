
CREATE  PROCEDURE [Access].[usp_GetShops]
	@UserGuid UNIQUEIDENTIFIER,
	@ShopGuid UNIQUEIDENTIFIER = NULL,
	@Search NVARCHAR(200) = NULL,
	@ShopName NVARCHAR(200) = NULL,
	@NotShopGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @StartTime DATETIMEOFFSET;
	SET @Search = '%' + ISNULL(@Search, '') + '%';
	DECLARE @UserTimeZone NVARCHAR(MAX) = Common.udf_GetUserTimeZoneId(@UserGuid);

	/************************************************************
		Step 0: Load Shops.
	************************************************************/
	SET @StartTime = GETUTCDATE();

	DECLARE @Shops TABLE (ShopGuid UNIQUEIDENTIFIER)
	INSERT INTO @Shops
	SELECT DISTINCT ShopGuid
	FROM Access.vwUserMemberships
	WHERE UserGuid = @UserGuid AND (@ShopGuid IS NULL OR ShopGuid = @ShopGuid) AND (@ShopName IS NULL OR ShopName = @ShopName) AND (@NotShopGuid IS NULL OR ShopGuid <> @NotShopGuid)

	PRINT 'Step 0: Load Shops. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 1: Load Shop Data.
	************************************************************/
	SET @StartTime = GETUTCDATE();

	SELECT
		s.ShopGuid
		,s.ShopNumber
		,s.AccountGuid
		,s.EmployeeGuid
		,a.EmployeeGuid [AccountEmployeeGuid]
		,shopEmployee.DisplayName [ShopRep]
		,accountEmployee.DisplayName [AccountRep]
		,a.Name [AccountName]
		,s.Address1
		,s.Address2
		,s.City
		,s.Fax
		,s.Name
		,s.DisplayName
		,s.Notes
		,s.DiscountPercentage
		,s.Phone
		,st.Abbreviation [State]
		,s.Zip

		,s.CCCShopId
		,s.AllowAllRepairAutoClose
		,s.AllowSelfScan
		,s.AllowAutoRepairClose
		,s.AllowScanAnalysisAutoClose
		,s.AllowSelfScanAssessment
		,s.AllowDemoScan
		,s.AllowScanAnalysis
		,s.DefaultInsuranceCompanyId
		,s.PricingPlanId
		,s.EstimatePlanId
		,s.BillingCycleId
		,s.AverageVehiclesPerMonth
		,s.ShopFixedPriceInd
		,s.HideFromReports
		,s.FirstScanCost
		,s.AdditionalScanCost
		,s.AutomaticRepairCloseDays
		,s.ActiveInd
		,s.CurrencyId
		,s.SendToMitchellInd
		,s.DisableShopBillingNotification
		,s.DisableShopStatementNotification
		,s.AutomaticInvoicesInd
	FROM Access.Shops s
	INNER JOIN @Shops fs ON fs.ShopGuid = s.ShopGuid
	LEFT JOIN Access.Accounts a ON s.AccountGuid = a.AccountGuid
	LEFT JOIN Repair.InsuranceCompanies ic ON s.DefaultInsuranceCompanyId = ic.InsuranceCompanyId
	LEFT JOIN Access.Users shopEmployee ON s.EmployeeGuid = shopEmployee.UserGuid
	LEFT JOIN Access.Users accountEmployee ON a.EmployeeGuid = accountEmployee.UserGuid
	INNER JOIN Common.States st ON s.StateId = st.StateId
	WHERE NULLIF(@Search, '%%') IS NULL
				OR s.DisplayName LIKE @Search
				OR s.Address1 LIKE @Search
				OR s.Zip LIKE @Search
				OR s.Fax LIKE @Search
				OR s.CCCShopId LIKE @Search
				OR st.Name LIKE @Search
				OR ic.InsuranceCompanyName LIKE @Search
				OR s.CCCShopId LIKE @Search
				OR shopEmployee.DisplayName LIKE @Search
				OR accountEmployee.DisplayName LIKE @Search

	PRINT 'Step 1: Load Shop Data. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'
	
	/************************************************************
		Step 2: Load Users.
	************************************************************/
	SET @StartTime = GETUTCDATE();

	SELECT
		u.UserGuid
		,u.PasswordHash
		,u.FirstName
		,u.LastName
		,u.DisplayName
		,u.JobTitle
		,u.ContactNumber
		,u.Email
		,u.EmailConfirmed
		,u.PhoneNumber
		,u.PhoneNumberConfirmed
		,u.TwoFactorEnabled
		,u.AccessFailedCount
		,u.ShopBillingNotification
		,u.ShopReportNotification
		,u.ShopStatementNotification
		,u.TimeZoneInfoId
		,u.LockoutEnabled [AccountLocked]
		,us.ShopGuid
	FROM Access.Users u
	INNER JOIN Access.UserShops us ON u.UserGuid = us.UserGuid
	INNER JOIN @Shops fs ON fs.ShopGuid = us.ShopGuid

	PRINT 'Step 2: Load Users. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 3: Load Account Users.
	************************************************************/
	SET @StartTime = GETUTCDATE();

	SELECT
		u.UserGuid
		,u.PasswordHash
		,u.FirstName
		,u.LastName
		,u.DisplayName
		,u.JobTitle
		,u.ContactNumber
		,u.Email
		,u.EmailConfirmed
		,u.PhoneNumber
		,u.PhoneNumberConfirmed
		,u.TwoFactorEnabled
		,u.AccessFailedCount
		,u.ShopBillingNotification
		,u.ShopReportNotification
		,u.ShopStatementNotification
		,u.TimeZoneInfoId
		,u.LockoutEnabled [AccountLocked]
		,s.ShopGuid
	FROM Access.Shops s
	INNER JOIN @Shops fs ON fs.ShopGuid = s.ShopGuid
	INNER JOIN Access.Accounts a ON s.AccountGuid = a.AccountGuid
	INNER JOIN Access.UserAccounts ua ON a.AccountGuid = ua.AccountGuid
	INNER JOIN Access.Users u ON ua.UserGuid = u.UserGuid

	PRINT 'Step 3: Load Account Users. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 4: Load AirPro Tools.
	************************************************************/
	SET @StartTime = GETUTCDATE();

	SELECT
		apt.ToolId
		,apt.ToolKey
		,apt.ToolName
		,apt.ToolPassword

		,apt.AutoEnginuityNum
		,apt.AutoEnginuityVersion
		,apt.DGNum
		,apt.TeamViewerId
		,apt.TeamViewerPassword
		,apt.WindowsVersion
		,apt.TabletModel
		,apt.HubModel
		,apt.IPV6DisabledInd
		,apt.OneDriveSyncEnabledInd
		,apt.UpdatesServiceInd
		,apt.MeteredConnectionInd
		,apt.SelfScanEnabledInd
		,apt.OBD2YConnector
		,apt.AELatestCode
		,apt.ChargerStyle
		,apt.TabletSerialNumber
		,apt.WifiCard
		,apt.WifiHardwareId
		,apt.WifiDriverDate
		,apt.WifiDriverVersion
		,apt.WifiMacAddress
		,apt.ImageVersion
		,apt.CellularActiveInd
		,apt.CellularProvider
		,apt.CellularIMEI
		,apt.HondaVersion
		,apt.FJDSVersion
		,apt.TechstreamVersion
		,apt.J2534Brand
		,apt.J2534Model
		,apt.J2534Serial
		,apt.Type
		,apts.ShopGuid
	FROM Inventory.AirProTools apt
	INNER JOIN Inventory.AirProToolShops apts ON apt.ToolId = apts.ToolId
	INNER JOIN @Shops fs ON fs.ShopGuid = apts.ShopGuid

	PRINT 'Step 4: Load AirPro Tools. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 5: Load Account AirPro Tools.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT
		apt.ToolId
		,apt.ToolKey
		,apt.ToolName
		,apt.ToolPassword

		,apt.AutoEnginuityNum
		,apt.AutoEnginuityVersion
		,apt.DGNum
		,apt.TeamViewerId
		,apt.TeamViewerPassword
		,apt.WindowsVersion
		,apt.TabletModel
		,apt.HubModel
		,apt.IPV6DisabledInd
		,apt.OneDriveSyncEnabledInd
		,apt.UpdatesServiceInd
		,apt.MeteredConnectionInd
		,apt.SelfScanEnabledInd
		,apt.OBD2YConnector
		,apt.AELatestCode
		,apt.ChargerStyle
		,apt.TabletSerialNumber
		,apt.WifiCard
		,apt.WifiHardwareId
		,apt.WifiDriverDate
		,apt.WifiDriverVersion
		,apt.WifiMacAddress
		,apt.ImageVersion
		,apt.CellularActiveInd
		,apt.CellularProvider
		,apt.CellularIMEI
		,apt.HondaVersion
		,apt.FJDSVersion
		,apt.TechstreamVersion
		,apt.J2534Brand
		,apt.J2534Model
		,apt.J2534Serial
		,apt.Type
		,s.ShopGuid
	FROM Access.Shops s
	INNER JOIN @Shops fs ON fs.ShopGuid = s.ShopGuid
	INNER JOIN Access.Accounts a ON s.AccountGuid = a.AccountGuid
	INNER JOIN Inventory.AirProToolAccounts apta ON a.AccountGuid = apta.AccountGuid
	INNER JOIN Inventory.AirProTools apt ON apta.ToolId = apt.ToolId

	PRINT 'Step 5: Load Account AirPro Tools. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 6: Load Shop Contacts.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT
		sc.ShopContactGuid
		,FirstName
		,LastName
		,PhoneNumber
		,sc.ShopGuid
		,COUNT(r.RequestId) [HasRequests]
	FROM Access.ShopContacts sc
	INNER JOIN @Shops fs ON fs.ShopGuid = sc.ShopGuid
	LEFT JOIN Scan.Requests r ON sc.ShopContactGuid = r.ContactUserGuid
	WHERE DeletedInd = 0
	GROUP BY sc.ShopContactGuid, FirstName, LastName, PhoneNumber, sc.ShopGuid

	PRINT 'Step 6: Load Shop Contacts. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 7: Load Shop Vehicle Makes.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT svm.VehicleMakeId [Id], svm.ShopId [ShopGuid]
	FROM Access.ShopVehicleMakes svm
	INNER JOIN @Shops fs ON fs.ShopGuid = svm.ShopId

	PRINT 'Step 7: Load Shop Vehicle Makes. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 8: Load Shop Insurance Companies.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT suc.InsuranceCompanyId [Id], suc.ShopId [ShopGuid]
	FROM Access.ShopInsuranceCompanies suc
	INNER JOIN @Shops fs ON fs.ShopGuid = suc.ShopId

	PRINT 'Step 8: Load Shop Insurance Companies. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

	/************************************************************
		Step 9: Load Shop Insurance Company Pricing Plans.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT 
		sicp.ShopId [ShopGuid]
		,sicp.InsuranceCompanyId
		,sicp.PricingPlanId [PlanId]
	FROM Billing.ShopInsuranceCompaniesPricing sicp
	INNER JOIN @Shops fs ON fs.ShopGuid = sicp.ShopId

	PRINT 'Step 9: Load Shop Insurance Company Pricing Plans. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

	/************************************************************
		Step 10: Load Shop Insurance Company Estimate Plans.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT 
		sice.ShopId [ShopGuid]
		,sice.InsuranceCompanyId
		,sice.EstimatePlanId [PlanId]
	FROM Billing.ShopInsuranceCompaniesEstimate sice
	INNER JOIN @Shops fs ON fs.ShopGuid = sice.ShopId

	PRINT 'Step 10: Load Shop Insurance Company Estimate Plans. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

	/************************************************************
		Step 11: Load Vehicle Makes Pricing Plans.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT 
		svmp.ShopId [ShopGuid]
		,svmp.VehicleMakeId
		,svmp.PricingPlanId
	FROM Billing.ShopVehicleMakesPricing svmp
	INNER JOIN @Shops fs ON fs.ShopGuid = svmp.ShopId

	PRINT 'Step 11: Load Vehicle Makes Pricing Plans. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms';

	/************************************************************
		Step 12: Load Shop Request Types.
	************************************************************/
	SET @StartTime = GETUTCDATE();
	
	SELECT srt.RequestTypeId [Id], srt.ShopGuid
	FROM Access.ShopRequestTypes srt
	INNER JOIN @Shops fs on fs.ShopGuid = srt.ShopGuid

	PRINT 'Step 12: Load Shop Request Types. => ' + CAST(DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE()) AS VARCHAR(10)) + 'ms'

END