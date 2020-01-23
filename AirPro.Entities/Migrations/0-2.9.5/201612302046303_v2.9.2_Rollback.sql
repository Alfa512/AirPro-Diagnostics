IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Billing' AND TABLE_NAME = 'vwPaymentTransactions')
	DROP VIEW Billing.vwPaymentTransactions
GO

CREATE VIEW Billing.vwPaymentTransactions
AS

	SELECT
		p.PaymentID
		,t.PaymentTransactionID
		,p.PaymentReceivedFromShopID
		,s.Name [PaymentReceivedFromShopName]
		,p.PaymentTypeID
		,pt.PaymentTypeName
		,p.PaymentAmount
		,p.PaymentReferenceNumber
		,t.InvoiceID
		,t.InvoiceAmountApplied
		,ISNULL(u.LastName, '') + ', ' + ISNULL(u.FirstName, '') [PaymentCreatedBy]
		,p.CreatedDt [PaymentCreatedDateTime]
	FROM Billing.Payments p
	INNER JOIN Access.Shops s
		ON p.PaymentReceivedFromShopID = s.ShopId
	INNER JOIN Access.Users u
		ON p.CreatedByUserId = u.UserId
	INNER JOIN Billing.PaymentTypes pt
		ON p.PaymentTypeID = pt.PaymentTypeID
	LEFT JOIN Billing.PaymentTransactions t
		ON p.PaymentID = t.PaymentID
			AND t.PaymentTransactionVoidInd = 0
	WHERE p.PaymentVoidInd = 0

GO

IF (OBJECT_ID('Scan.trgScanReportsArchive') IS NOT NULL)
	DROP TRIGGER Scan.trgScanReportsArchive
GO

CREATE TRIGGER [Scan].[trgScanReportsArchive] ON [Scan].[Reports]
AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO [Scan].[ReportsArchive]
	(
		ReportID
		,CreatedDt
		,UpdatedDt
		,CreatedByUserId
		,UpdatedByUserId
		,ReportNotes
		,CompletedInd
		,InvoicedInd
		,InvoicedDt
		,CompletedByUserId
		,InvoiceAmount
		,CompletedDt
		,InvoicedByUserId
		,TechnicianNotes
		,ResponsibleTechnicianUserId
		,ResponsibleSetDt
		,ArchiveDt
	)
	SELECT
		ReportID
		,CreatedDt
		,UpdatedDt
		,CreatedByUserId
		,UpdatedByUserId
		,ReportNotes
		,CompletedInd
		,InvoicedInd
		,InvoicedDt
		,CompletedByUserId
		,InvoiceAmount
		,CompletedDt
		,InvoicedByUserId
		,TechnicianNotes
		,ResponsibleTechnicianUserId
		,ResponsibleSetDt
		,GETUTCDATE()
	FROM deleted

END

GO

