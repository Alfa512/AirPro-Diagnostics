
DROP PROCEDURE IF EXISTS Reporting.usp_GetScanReportDataSource;
GO

CREATE PROCEDURE Reporting.usp_GetScanReportDataSource
	@RequestId INT
	,@Offset CHAR(10) = '-00:00'
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		o.OrderId [RepairID]
		,CASE o.Status
			WHEN 1 THEN 'Active'
			WHEN 2 THEN 'Canceled'
			WHEN 3 THEN 'Completed'
			WHEN 4 THEN 'Invoiced'
			WHEN 5 THEN 'Paid'
		END [RepairStatus]
		,s.DisplayName [ShopName]
		,s.Address1 [ShopAddress]
		,s.City [ShopCity]
		,st.Abbreviation [ShopState]
		,s.Zip [ShopZip]
		,o.ShopReferenceNumber [RepairShopReferenceNumber]
		,CASE
			WHEN ic.InsuranceCompanyId IS NULL OR ic.InsuranceCompanyId = 1
				THEN o.InsuranceCompanyOther
			ELSE ic.InsuranceCompanyName
		END [RepairInsuranceCompany]
		,o.Odometer [RepairOdometer]
		,o.AirBagsDeployed [RepairAirBagsDeployed]
		,o.DrivableInd [RepairDrivable]
		,Common.udf_GetDisplayName(ocu.LastName, ocu.FirstName) [RepairCreatedBy]
		,v.VehicleVIN
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,v.Transmission [VehicleTransmission]
		,rq.RequestId [RequestID]
		,CASE WHEN NULLIF(rc.RequestCategoryName, '') IS NOT NULL THEN REPLACE(rc.RequestCategoryName, 'Scan', '') + rt.TypeName ELSE rt.TypeName END [RequestTypeOfScan]
		,i.RequestWarningIndicators
		,poi.RepairPointOfImpacts
		,rq.OtherWarningInfo [RequestOtherWarningInfo]
		,rq.ProblemDescription [RequestProblemDescription]
		,rq.Notes [RequestNotes]
		,Common.udf_GetDisplayName(rqcu.LastName, rqcu.FirstName) [RequestCreatedBy]
		,rpt.ReportId
		,rpt.ReportNotes [ReportTechnicianNotes]
		,rpt.CanceledInd
		,ISNULL(p.DisplayName, rptcu.DisplayName) [TechnicianName]
		,rptcu.ContactNumber [TechnicianContact]
		,Common.udf_GetLocalDateTime(rpt.CompletedDt, @Offset) [ReportCompletedDt]
	FROM Scan.Requests rq
	OUTER APPLY
	(
		SELECT STUFF((
			SELECT ',' + CAST(rwi.WarningIndicatorId AS VARCHAR(10))
			FROM Scan.RequestWarningIndicators rwi
			WHERE rwi.RequestId = rq.RequestId
			FOR XML PATH('')
			), 1, 1, '') [RequestWarningIndicators]
	) i
	INNER JOIN Access.Users rqcu
		ON rq.CreatedByUserGuid = rqcu.UserGuid
	INNER JOIN Scan.RequestTypes rt
		ON rq.RequestTypeId = rt.RequestTypeId
	LEFT JOIN Scan.RequestCategories rc
		ON rq.RequestCategoryId = rc.RequestCategoryId
	INNER JOIN Repair.Orders o
		INNER JOIN Repair.Vehicles v
			ON o.VehicleVIN = v.VehicleVIN
		INNER JOIN Access.Users ocu
			ON o.CreatedByUserGuid = ocu.UserGuid
		LEFT JOIN Repair.InsuranceCompanies ic
			ON o.InsuranceCompanyId = ic.InsuranceCompanyId
		ON rq.OrderId = o.OrderId
	OUTER APPLY
	(
		SELECT STUFF((
			SELECT ',' + CAST(opoi.PointOfImpactId AS VARCHAR(10))
			FROM Repair.OrderPointOfImpacts opoi
			WHERE o.OrderId = opoi.OrderID
			FOR XML PATH('')
			), 1, 1, '') [RepairPointOfImpacts]
	) poi
	INNER JOIN Access.Shops s
		INNER JOIN Common.States st
			ON s.StateId = st.StateId
		ON o.ShopGuid = s.ShopGuid
	LEFT JOIN Scan.Reports rpt
		LEFT JOIN Access.Users rptcu
			ON rpt.CompletedByUserGuid = rptcu.UserGuid
		LEFT JOIN Technician.Profiles p
			ON rpt.CompletedByUserGuid = p.UserGuid
				AND p.ActiveInd = 1
		ON rq.ReportId = rpt.ReportId
	WHERE rq.RequestId = @RequestId

END
GO