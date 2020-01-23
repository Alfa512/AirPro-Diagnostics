
CREATE PROCEDURE Billing.usp_GetBillingInvoice
	@UserGuid UNIQUEIDENTIFIER,
	@RepairId INT
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZone NVARCHAR(MAX) = Common.udf_GetUserTimeZoneId(@UserGuid);
	DECLARE @PricingPlanId INT = Billing.udf_GetPricingPlanIdByOrderId(@RepairId);

	DECLARE @NonCancelledRequestCount INT
	SELECT @NonCancelledRequestCount = COUNT(r.OrderId)
	FROM Scan.Requests r
	INNER JOIN Scan.Reports rpt ON r.ReportId = rpt.ReportId
	WHERE rpt.CanceledInd = 0 AND r.OrderId = @RepairId

	SELECT
		o.OrderId [RepairId]
		,o.Status [RepairStatus]
		,o.ShopReferenceNumber [ShopRoNumber]
		,ISNULL(ic.InsuranceCompanyName, o.InsuranceCompanyOther) [InsuranceCompanyName]
		,o.InsuranceReferenceNumber [InsuranceClaimNumber]
		,CASE o.UpdatedDt WHEN '0001-01-01 00:00:00.0000000 +00:00' THEN NULL ELSE CAST(o.UpdatedDt AT TIME ZONE @UserTimeZone AS DATETIME) END [RepairLastUpdatedDt]

		,v.VehicleVIN
		,vm.VehicleMakeName [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,v.Transmission [VehicleTransmission]

		,s.DisplayName [ShopName]
		,s.Phone [ShopPhone]
		,s.Fax [ShopFax]
		,s.Address1 [ShopAddress1]
		,s.Address2 [ShopAddress2]
		,s.City [ShopCity]
		,st.Abbreviation [ShopState]
		,s.Zip [ShopZip]
		,s.Notes [ShopNotes]
		,s.ShopFixedPriceInd
		,s.FirstScanCost [ShopFixedPriceFirst]
		,s.AdditionalScanCost [ShopFixedPriceAdditional]

		,i.InvoiceId
		,CASE
			WHEN i.CurrencyId IS NULL AND s.ShopFixedPriceInd = 1 THEN s.CurrencyId
			ELSE ISNULL(i.CurrencyId, pp.CurrencyId)
		END [InvoiceCurrencyId]
		,i.CustomerMemo [InvoiceCustomerMemo]
		,i.InvoicedInd [InvoiceCompleteInd]
		,iu.LastName + ', ' + iu.FirstName [InvoicedByUserName]
		,CAST(i.InvoicedDt AT TIME ZONE @UserTimeZone AS DATETIME) [InvoicedDt]

		,li.RequestId
		,li.ReportId
		,li.InvoicedInd
		,li.InvoicedAmount
		,li.RequestTypeId
		,li.RequestTypeName
		,li.CanceledInd
		,li.CancellationNotes
		,li.RequestCreatedByName
		,li.TechnicianName
		,li.RequestSortOrder
		,li.RequestGeneratedMemo
		,DENSE_RANK() OVER (PARTITION BY li.CanceledInd ORDER BY li.RequestSortOrder) [LineItemRank]
	INTO #Invoice
	FROM Repair.Orders o
	INNER JOIN Repair.Vehicles v
		INNER JOIN Repair.VehicleMakes vm
			ON v.VehicleMakeId = vm.VehicleMakeId
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Access.Shops s
		INNER JOIN Common.States st
			ON s.StateId = st.StateId
		LEFT JOIN Billing.PricingPlans spp
			ON s.PricingPlanId = spp.PricingPlanId
		ON o.ShopGuid = s.ShopGuid
	LEFT JOIN Repair.Invoices i
		LEFT JOIN Access.Users iu
			ON i.InvoicedByUserGuid = iu.UserGuid
		ON o.OrderId = i.InvoiceId
	LEFT JOIN Repair.InsuranceCompanies ic
		ON o.InsuranceCompanyId = ic.InsuranceCompanyId
			AND ic.InsuranceCompanyId > 1
	LEFT JOIN Billing.PricingPlans pp
		ON pp.PricingPlanId = @PricingPlanId
	OUTER APPLY
	(
		SELECT
			r.RequestId
			,rpt.ReportId
			,CASE
				WHEN rpt.InvoiceAmount IS NULL
					AND rt.BillableFlag = 1
					AND ISNULL(rpt.CanceledInd , 0) = 0 THEN 1
				ELSE rpt.InvoicedInd
			END [InvoicedInd]
			,COALESCE(rpt.InvoiceAmount, pprt.LineItemCost, rt.DefaultPrice) [InvoicedAmount]
			,r.RequestTypeId
			,rt.TypeName [RequestTypeName]
			,rpt.CanceledInd
			,ISNULL(NULLIF(RTRIM(rpt.CancellationNotes), ''), 'No notes available.') [CancellationNotes]
			,rtu.LastName + ', ' + rtu.FirstName [TechnicianName]
			,rcu.LastName + ', ' + rcu.FirstName [RequestCreatedByName]
			,ROW_NUMBER() OVER (PARTITION BY r.OrderId ORDER BY rt.SortOrder, r.CreatedDt) [RequestSortOrder]
			,CASE
				WHEN ISNULL(rpt.CanceledInd, 0) = 1 AND @NonCancelledRequestCount = 0 THEN '<p>Scan Services Cancelled, no diagnostics work or calibrations performed.</p>'
				WHEN ISNULL(rpt.CanceledInd, 0) = 1 AND @NonCancelledRequestCount > 0 THEN ''
				ELSE '<p>' + rt.InvoiceMemo + '</p>' + REPLACE(ISNULL('<ul><li>' + wt.WorkTypeDescriptions + '</li></ul>', ''), '|', '</li><li>')
			 END
			 [RequestGeneratedMemo]
		FROM Scan.Requests r
		INNER JOIN Scan.RequestTypes rt
			ON r.RequestTypeId = rt.RequestTypeId
		INNER JOIN Scan.Reports rpt
			ON r.ReportId = rpt.ReportId
		OUTER APPLY
		(
			SELECT STUFF((SELECT DISTINCT '|' + t.WorkTypeDescription
						FROM Scan.WorkTypes t
						INNER JOIN Scan.ReportWorkTypes rwt
							ON t.WorkTypeId = rwt.WorkTypeId
								AND r.ReportId = rwt.ReportId
						WHERE t.WorkTypeActiveInd = 1
							AND NULLIF(RTRIM(t.WorkTypeDescription), '') IS NOT NULL
						FOR XML PATH('')), 1, 1, '') [WorkTypeDescriptions]
		) wt
		INNER JOIN Access.Users rcu
			ON r.CreatedByUserGuid = rcu.UserGuid
		INNER JOIN Access.Users rtu
			ON rpt.ResponsibleTechnicianUserGuid = rtu.UserGuid
		LEFT JOIN Billing.PricingPlanRequestTypes pprt
			ON pp.PricingPlanId = pprt.PricingPlanId
				AND r.RequestTypeId = pprt.RequestTypeId
				AND vm.VehicleMakeTypeId = pprt.VehicleMakeTypeId
		WHERE r.OrderId = o.OrderId
	) li
	WHERE o.OrderId = @RepairId

	/************************************************************
		Set Fixed Pricing.
	************************************************************/
	UPDATE i
		SET i.InvoicedAmount = CASE
				WHEN i.CanceledInd = 1 THEN 0
				WHEN i.LineItemRank = 1 THEN i.ShopFixedPriceFirst
				ELSE i.ShopFixedPriceAdditional
			END
	FROM #Invoice i
	WHERE i.ShopFixedPriceInd = 1
		AND i.InvoiceId IS NULL

	/************************************************************
		Set Default Price when ONLY Quick or Scan Analysis.
	************************************************************/
	IF EXISTS (SELECT 1 FROM #Invoice WHERE RequestTypeId IN (1, 7))
		AND NOT EXISTS (SELECT 1 FROM #Invoice WHERE RequestTypeId NOT IN (1, 7))
		BEGIN
			UPDATE i
				SET i.InvoicedAmount = rt.DefaultPrice
			FROM #Invoice i
			INNER JOIN Scan.RequestTypes rt
				ON i.RequestTypeId = rt.RequestTypeId
		END

	SELECT
		RepairId
		,RepairStatus
		,ShopRoNumber
		,InsuranceCompanyName
		,InsuranceClaimNumber
		,RepairLastUpdatedDt
		,VehicleVIN
		,VehicleMake
		,VehicleModel
		,VehicleYear
		,VehicleTransmission
		,ShopName
		,ShopPhone
		,ShopFax
		,ShopAddress1
		,ShopAddress2
		,ShopCity
		,ShopState
		,ShopZip
		,ShopNotes
		,InvoiceId
		,InvoiceCurrencyId
		,InvoiceCustomerMemo
		,InvoiceCompleteInd
		,InvoicedByUserName
		,InvoicedDt
		,RequestId
		,ReportId
		,InvoicedInd
		,InvoicedAmount
		,RequestTypeId
		,RequestTypeName
		,CanceledInd
		,CancellationNotes
		,RequestCreatedByName
		,TechnicianName
		,RequestSortOrder
		,RequestGeneratedMemo
	FROM #Invoice
	ORDER BY RequestSortOrder

	/************************************************************
		Load Work Type Pricing.
	************************************************************/
	SELECT
		wt.WorkTypeId
		,wt.WorkTypeName
		,COALESCE(rwt.InvoiceAmount, ppwt.LineItemCost, 0) [InvoicedAmount]
		,wt.WorkTypeSortOrder [SortOrder]
		,r.RequestId
		,CASE
			WHEN rpt.InvoiceAmount IS NULL
				AND ISNULL(rpt.CanceledInd , 0) = 0 THEN 1
			ELSE rwt.InvoicedInd
		END [InvoicedInd]
	FROM Repair.Orders o
	INNER JOIN Repair.Vehicles v
		INNER JOIN Repair.VehicleMakes vm
			ON v.VehicleMakeId = vm.VehicleMakeId
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Scan.Requests r
		INNER JOIN Scan.Reports rpt
			INNER JOIN Scan.ReportWorkTypes rwt
				INNER JOIN Scan.WorkTypes wt
					ON rwt.WorkTypeId = wt.WorkTypeId
				ON rpt.ReportId = rwt.ReportId
			ON r.ReportId = rpt.ReportId
		ON o.OrderId = r.OrderId
	LEFT JOIN Billing.PricingPlanWorkTypes ppwt
		ON ppwt.WorkTypeId = wt.WorkTypeId
			AND ppwt.PricingPlanId = @PricingPlanId
			AND ppwt.VehicleMakeTypeId = vm.VehicleMakeTypeId
	WHERE r.OrderId = @RepairId

END