CREATE VIEW [Reporting].[vwReportData]
AS

	WITH
	Users
	AS
	(
		SELECT
			UserGuid
			,LastName + ', ' + FirstName [DisplayName]
		FROM Access.Users
	)

	SELECT
		a.AccountGuid
		,a.Name [AccountName]
		,a.Address1 [AccountAddress1]
		,a.Address2 [AccountAddress2]
		,a.City [AccountCity]
		,ast.Abbreviation + ' - ' + ast.Name [AccountState]
		,a.Zip [AccountZip]
		,a.Phone [AccountPhone]
		,a.Fax [AccountFax]
		,a.DiscountPercentage [AccountDiscountPercentage]
		,accountEmployee.DisplayName [AccountRepUser]
		,s.ShopGuid
		,s.Name [ShopName]
		,s.ShopNumber
		,s.Phone [ShopPhone]
		,s.Fax [ShopFax]
		,s.Address1 [ShopAddress1]
		,s.Address2 [ShopAddress2]
		,s.City [ShopCity]
		,ss.Abbreviation + ' - ' + ss.Name [ShopState]
		,s.Zip [ShopZip]
		,s.Notes [ShopNotes]
		,s.DiscountPercentage [ShopDiscountPercentage]
		,s.BillingCycleId [ShopBillingCycleId]
		,bc.CycleName [ShopBillingCycleName]
		,s.CCCShopId [ShopCCCId]
		,s.AllowSelfScan [ShopSelfScan]
		,shopEmployee.DisplayName [ShopRepUser]
		,s.AutomaticInvoicesInd [ShopAutomaticInvoicesInd]

		,o.OrderId [RepairOrderId]
		,o.Status [RepairStatusId]
		,CASE o.Status
			WHEN 1 THEN 'Active'
			WHEN 2 THEN 'Canceled'
			WHEN 3 THEN 'Completed'
			WHEN 4 THEN 'Invoiced'
			WHEN 5 THEN 'Paid'
		END [RepairStatus]
		,o.ShopReferenceNumber [RepairRONumber]
		,CASE o.InsuranceCompanyId WHEN 1 THEN o.InsuranceCompanyOther ELSE ic.InsuranceCompanyName END [RepairInsuranceCompany]
		,o.InsuranceReferenceNumber [RepairInsuranceClaimNumber]
		,o.VehicleVIN [RepairVehicleVIN]
		,v.Make [RepairVehicleMake]
		,v.Model [RepairVehicleModel]
		,v.Year [RepairVehicleYear]
		,v.Transmission [RepairVehicleTransmission]
		,vmt.VehicleMakeTypeName [RepairVehicleMakeType]
		,ISNULL(vl.RequestSuccess, 0) [RepairVehicleFound]
		,o.Odometer [RepairVehicleOdometer]
		,o.AirBagsDeployed [RepairVehicleAirBagsDeployed]
		,o.DrivableInd [RepairVehicleDrivableInd]
		,CASE WHEN o.CCCDocumentGuid IS NULL THEN 0 ELSE 1 END [RepairCreateByCCC]
		,ocu.DisplayName [RepairCreatedByUser]
		,o.CreatedDt [RepairCreatedDt]
		,ouu.DisplayName [RepairLastUpdatedByUser]
		,o.UpdatedDt [RepairLastUpdatedDt]

		,rq.RequestId
		,rt.RequestTypeId
		,rt.TypeName [RequestType]
		,rc.RequestCategoryName [RequestTypeCategory]
		,rq.OtherWarningInfo [RequestOtherWarningInfo]
		,rq.SeatRemovedInd [RequestSeatRemovedInd]
		,rq.ProblemDescription [RequestProblemDescription]
		,rq.Notes [RequestNotes]
		,rqcu.DisplayName [RequestCreatedByUser]
		,rq.CreatedDt [RequestCreatedDt]
		,rquu.DisplayName [RequestLastUpdatedByUser]
		,rq.UpdatedDt [RequestUpdatedDt]

		,rp.ReportId
		,rpcru.DisplayName [ReportCreatedByUser]
		,rp.CreatedDt [ReportCreatedDt]
		,rpuu.DisplayName [ReportUpdatedByUser]
		,rp.UpdatedDt [ReportUpdatedDt]
		,rprtu.DisplayName [ReportTechUser]
		,CASE WHEN rprtu.TechProfileGuid IS NOT NULL THEN CONVERT(bit, 1) ELSE CONVERT(bit, 0) END [ReportTechUserProfileInd]
		,rp.ResponsibleSetDt [ReportTechAssignedDt]
		,rpcmu.DisplayName [ReportCompletedByUser]
		,rp.CompletedDt [ReportCompletedDt]

		,rp.CanceledInd [ReportCancelled]
		,rp.CancelReasonTypeId [ReportCancelReasonTypeId]
		,crt.Name [ReportCancelReasonTypeName]

		,rp.InvoicedInd [ReportInvoicedInd]
		,rpiu.DisplayName [ReportInvoicedByUser]
		,rp.InvoicedDt [ReportInvoicedDt]
		,CASE WHEN rp.InvoicedInd = 1 THEN rp.InvoiceAmount ELSE 0.00 END [ReportInvoicedAmount]
		,CASE WHEN rp.InvoicedInd = 1 THEN CAST((ISNULL(p.DiscountPercentage, 0) / CAST(100 AS DECIMAL(5,2))) * rp.InvoiceAmount AS DECIMAL(18, 3)) ELSE 0.00 END [ReportInvoiceDiscountAmount]

		,inv.InvoiceId [RepairInvoiceId]
		,inv.CustomerMemo [RepairInvoiceMemo]
		,pt.InvoiceAmountApplied [RepairInvoiceAmountApplied]
		,pt.DiscountAmountApplied [RepairInvoiceDiscountAmount]
		,invu.DisplayName [RepairInvoiceCreatedByUser]
		,inv.InvoicedDt [RepairInvoiceCreatedDt]

		,p.PaymentID
		,ptp.PaymentTypeName [PaymentType]
		,p.DiscountPercentage [PaymentDiscountPercentage]
		,p.PaymentAmount [PaymentTotalAmount]
		,p.PaymentReferenceNumber [PaymentRefNumber]
		,p.PaymentMemo
		,pcu.DisplayName [PaymentCreatedByUser]
		,p.CreatedDt [PaymentCreatedDt]
		,puu.DisplayName [PaymentUpdatedByUser]
		,p.UpdatedDt [PaymentUpdatedDt]
	FROM Repair.Orders o
	INNER JOIN Repair.InsuranceCompanies ic
		ON o.InsuranceCompanyId = ic.InsuranceCompanyId
	INNER JOIN Repair.Vehicles v
		INNER JOIN Repair.VehicleMakes vm
			INNER JOIN Repair.VehicleMakeTypes vmt
				ON vm.VehicleMakeTypeId = vmt.VehicleMakeTypeId
			ON v.VehicleMakeId = vm.VehicleMakeId
		LEFT JOIN Repair.VehicleLookups vl
			ON v.VehicleLookupId = vl.VehicleLookupId
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN (SELECT UserGuid, DisplayName FROM Users) ocu
		ON o.CreatedByUserGuid = ocu.UserGuid
	LEFT JOIN (SELECT UserGuid, DisplayName FROM Users) ouu
		ON o.UpdatedByUserGuid = ouu.UserGuid
	INNER JOIN Access.Shops s
		INNER JOIN Common.States ss
			ON s.StateId = ss.StateId
		INNER JOIN Access.Accounts a
			INNER JOIN Common.States ast
				ON a.StateId = ast.StateId
			ON s.AccountGuid = a.AccountGuid
		LEFT JOIN Billing.Cycles bc
			ON s.BillingCycleId = bc.CycleId
		ON o.ShopGuid = s.ShopGuid
			AND s.HideFromReports = 0
	LEFT JOIN Scan.Requests rq
		INNER JOIN Scan.RequestTypes rt
			ON rq.RequestTypeId = rt.RequestTypeId
		LEFT JOIN Scan.RequestCategories rc
			ON rc.RequestCategoryId = rq.RequestCategoryId
		INNER JOIN (SELECT UserGuid, DisplayName FROM Users) rqcu
			ON rq.CreatedByUserGuid = rqcu.UserGuid
		LEFT JOIN (SELECT UserGuid, DisplayName FROM Users) rquu
			ON rq.UpdatedByUserGuid = rquu.UserGuid
		ON o.OrderId = rq.OrderId
	LEFT JOIN Scan.Reports rp
		INNER JOIN (SELECT UserGuid, DisplayName FROM Users) rpcru
			ON rp.CreatedByUserGuid = rpcru.UserGuid
		LEFT JOIN (SELECT UserGuid, DisplayName FROM Users) rpuu
			ON rp.UpdatedByUserGuid = rpuu.UserGuid
		LEFT JOIN (SELECT u.UserGuid, u.DisplayName, tp.UserGuid [TechProfileGuid] FROM Users u LEFT JOIN Technician.Profiles tp ON u.UserGuid = tp.UserGuid AND tp.ActiveInd = 1) rprtu
			ON rp.ResponsibleTechnicianUserGuid = rprtu.UserGuid
		LEFT JOIN (SELECT UserGuid, DisplayName FROM Users) rpcmu
			ON rp.CompletedByUserGuid = rpcmu.UserGuid
		LEFT JOIN (SELECT UserGuid, DisplayName FROM Users) rpiu
			ON rp.InvoicedByUserGuid = rpiu.UserGuid
		ON rq.ReportId = rp.ReportId
	LEFT JOIN Repair.Invoices inv
		INNER JOIN (SELECT UserGuid, DisplayName FROM Users) invu
			ON inv.InvoicedByUserGuid = invu.UserGuid
		LEFT JOIN Billing.PaymentTransactions pt
			LEFT JOIN Billing.Payments p
				INNER JOIN Billing.PaymentTypes ptp
					ON p.PaymentTypeId = ptp.PaymentTypeId
				INNER JOIN (SELECT UserGuid, DisplayName FROM Users) pcu
					ON p.CreatedByUserGuid = pcu.UserGuid
				LEFT JOIN (SELECT UserGuid, DisplayName FROM Users) puu
					ON p.UpdatedByUserGuid = puu.UserGuid
				ON pt.PaymentID = p.PaymentID
					AND p.PaymentVoidInd = 0
			ON inv.InvoiceId = pt.InvoiceId
				AND pt.PaymentTransactionVoidInd = 0
		ON o.OrderId = inv.InvoiceId
			AND inv.InvoicedInd = 1
	LEFT JOIN Access.Users shopEmployee ON s.EmployeeGuid = shopEmployee.UserGuid
	LEFT JOIN Access.Users accountEmployee ON a.EmployeeGuid = accountEmployee.UserGuid
	LEFT JOIN Scan.CancelReasonTypes crt ON rp.CancelReasonTypeId = crt.CancelReasonTypeId
GO