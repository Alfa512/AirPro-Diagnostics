
CREATE PROCEDURE Reporting.usp_GetInvoiceReportDataSource
	@InvoiceId INT
	,@Offset CHAR(10) = '-00:00'
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		s.DisplayName [ShopName]
		,s.Phone [ShopPhone]
		,s.Fax [ShopFax]
		,s.Address1 [ShopAddress1]
		,s.Address2 [ShopAddress2]
		,s.City [ShopCity]
		,st.Abbreviation [ShopState]
		,s.Zip [ShopZip]
		,o.OrderId [RepairOrderID]
		,o.ShopReferenceNumber [RepairShopReferenceNumber]
		,CASE
			WHEN o.InsuranceCompanyId IS NULL OR o.InsuranceCompanyId = 1
				THEN o.InsuranceCompanyOther
			ELSE ic.InsuranceCompanyName
		END [RepairInsuranceCompany]
		,o.InsuranceReferenceNumber [RepairInsuranceClaimNumber]
		,o.Odometer [RepairOdometer]
		,o.AirBagsDeployed [RepairAirBagsDeployed]
		,v.VehicleVIN [VehicleVIN]
		,v.Make [VehicleMake]
		,v.Model [VehicleModel]
		,v.Year [VehicleYear]
		,v.Transmission [VehicleTransmission]
		,i.CustomerMemo [CustomerMemo]
		,Common.udf_GetLocalDateTime(i.InvoicedDt, @Offset) [InvoicedDt]
		,ISNULL(c.Name, 'USD') [InvoiceCurrency]
	FROM Repair.Invoices i
	LEFT JOIN Billing.Currencies c
		ON i.CurrencyId = c.CurrencyId
	INNER JOIN Repair.Orders o
		ON i.InvoiceId = o.OrderId
	INNER JOIN Access.Shops s
		INNER JOIN Common.States st
			ON s.StateId = st.StateId
		ON o.ShopGuid = s.ShopGuid
	INNER JOIN Repair.Vehicles v
		ON o.VehicleVIN = v.VehicleVIN
	INNER JOIN Repair.InsuranceCompanies ic
		ON o.InsuranceCompanyId = ic.InsuranceCompanyId
	WHERE i.InvoiceId = @InvoiceId

	SELECT
		rq.ReportId,
		rt.TypeName [WorkPerfomed]
		,ISNULL(rpt.InvoiceAmount, 0) [InvoiceAmount]
		,cu.DisplayName [RequestedBy]
		,ISNULL(rc.RequestCategoryName, '') [RequestCategoryName]
	FROM Scan.Requests rq
	INNER JOIN Scan.RequestTypes rt
		ON rq.RequestTypeId = rt.RequestTypeId
	LEFT JOIN Scan.RequestCategories rc
		ON rq.RequestCategoryId = rc.RequestCategoryId
	INNER JOIN Scan.Reports rpt
		ON rq.ReportId = rpt.ReportId
			AND rpt.InvoicedInd = 1
	INNER JOIN Access.Users cu
		ON rq.CreatedByUserGuid = cu.UserGuid
	WHERE rq.OrderId = @InvoiceId

	SELECT 
		wt.WorkTypeName	[WorkPerfomed]
		,rwt.InvoiceAmount [InvoiceAmount]
		,rwt.ReportId
	FROM Scan.Requests r
	INNER JOIN Scan.RequestTypes rt
		ON r.RequestTypeId = rt.RequestTypeId
	INNER JOIN Scan.Reports rpt
		ON r.ReportId = rpt.ReportId
	INNER JOIN Scan.ReportWorkTypes rwt
		ON rpt.ReportId = rwt.ReportId
			AND rwt.InvoicedInd = 1
	INNER JOIN Scan.WorkTypes wt
		ON rwt.WorkTypeId = wt.WorkTypeId
	WHERE r.OrderId = @InvoiceId
	ORDER BY
		r.ReportId ASC
		,wt.WorkTypeSortOrder DESC

END