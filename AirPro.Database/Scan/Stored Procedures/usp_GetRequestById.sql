
CREATE PROCEDURE Scan.usp_GetRequestById
	@UserGuid UNIQUEIDENTIFIER
	,@RequestId INT
AS
BEGIN

	DECLARE @UserTimeZone NVARCHAR(MAX)
	SELECT TOP 1 @UserTimeZone = u.TimeZoneInfoId
	FROM Access.Users u
	WHERE u.UserGuid = @UserGuid

	SELECT
		o.OrderId [RepairId]
		,CASE o.Status
			WHEN 1 THEN 'Active'
			WHEN 2 THEN 'Canceled'
			WHEN 3 THEN 'Completed'
			WHEN 4 THEN 'Invoiced'
			WHEN 5 THEN 'Paid'
		END [RepairStatusName]

		,rq.RequestId [RequestId]
		,rq.RequestTypeId [RequestTypeId]
		,rt.TypeName [RequestTypeName]
		,rq.RequestCategoryId [RequestCategoryId]
		,rc.RequestCategoryName [RequestCategoryName]

		,s.Name + ' (' + CAST(s.ShopNumber AS VARCHAR(10)) + ')' [ShopName]
		,COALESCE(NULLIF(RTRIM(LTRIM(cu.DisplayName + ' ' + COALESCE(cu.PhoneNumber, cu.ContactNumber, cu.Email))), '')
			,NULLIF(RTRIM(LTRIM(sc.FirstName + ' ' + sc.LastName + ' ' + sc.PhoneNumber)), '')
			,rq.Contact) [Contact]
		,o.ShopReferenceNumber [ShopRONumber]
		,ISNULL(s.Phone, a.Phone) [ShopContact]

		,o.DrivableInd [DrivableInd]
		,o.AirBagsDeployed [AirBagsDeployed]
		,rq.SeatRemovedInd [SeatRemovedInd]
		
		,ISNULL(NULLIF(RTRIM(o.InsuranceCompanyOther), ''), ic.InsuranceCompanyName) [InsuranceCompanyDisplay]
		,o.InsuranceReferenceNumber [InsuranceReferenceNumber]

		,rq.OtherWarningInfo [OtherWarningInfo]
		,rq.ProblemDescription [ProblemDescription]
		,rq.Notes [Notes]

		,CAST(CASE WHEN v.VehicleLookupId IS NULL THEN 1 ELSE 0 END AS BIT) [VehicleManualEntryInd]
		,vl.ResponseContent [VehicleLookupInfo]
		,o.VehicleVIN [VehicleVIN]
		,ISNULL(vm.VehicleMakeName, v.Make) [VehicleMakeName]
		,v.Model [VehicleModelName]
		,v.Year [VehicleYear]
		,v.Transmission [VehicleTransmission]
		,o.Odometer [Odometer]

		,CAST(rq.CreatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [RequestCreateDt]
		,CAST(rq.CreatedDt AT TIME ZONE 'UTC' AS DATETIME) [RequestCreateDtUtc]
		,COALESCE(ru.FirstName + ' ' + ru.LastName, 'Unassigned') [TechnicianName]
		,ru.ContactNumber [TechnicianContactNumber]
		,ru.PhoneNumber [TechnicianMobileNumber]
		,CAST((SELECT MIN(r.CreatedDt) FROM Diagnostic.Results r WHERE r.RequestId = @RequestId) AT TIME ZONE @UserTimeZone AS DATETIME) [ScanUploadDt]
	FROM Scan.Requests rq
	INNER JOIN Repair.Orders o
		ON rq.OrderId = o.OrderId
	INNER JOIN Access.Shops s
		ON o.ShopGuid = s.ShopGuid
	INNER JOIN Access.Accounts a
		ON s.AccountGuid = a.AccountGuid
	LEFT JOIN Scan.Reports rpt
		LEFT JOIN Access.Users ru
			ON rpt.ResponsibleTechnicianUserGuid = ru.UserGuid
		ON rq.ReportId = rpt.ReportId
	INNER JOIN Repair.Vehicles v
		LEFT JOIN Repair.VehicleLookups vl
			ON v.VehicleLookupId = vl.VehicleLookupId
		LEFT JOIN Repair.VehicleMakes vm
			ON v.VehicleMakeId = vm.VehicleMakeId
		ON o.VehicleVIN = v.VehicleVIN
	LEFT JOIN Scan.RequestTypes rt
		ON rq.RequestTypeId = rt.RequestTypeId
	LEFT JOIN Scan.RequestCategories rc
		ON rq.RequestCategoryId = rc.RequestCategoryId
	LEFT JOIN Repair.InsuranceCompanies ic
		ON o.InsuranceCompanyId = ic.InsuranceCompanyId
	LEFT JOIN Access.Users cu
		ON rq.ContactUserGuid = cu.UserGuid
	LEFT JOIN Access.ShopContacts sc
		ON rq.ShopContactGuid = sc.ShopContactGuid
	WHERE rq.RequestId = @RequestId

	SELECT wi.Name [WarningIndicatorName]
	FROM Scan.RequestWarningIndicators rwi
	INNER JOIN Scan.WarningIndicators wi
		ON rwi.WarningIndicatorId = wi.WarningIndicatorId
	WHERE RequestId = @RequestId

	SELECT opoi.PointOfImpactId
	FROM Scan.Requests rq
	INNER JOIN Repair.OrderPointOfImpacts opoi
		ON rq.OrderId = opoi.OrderID
	WHERE rq.RequestId = @RequestId

END