CREATE TRIGGER [Scan].[trgScanReportsArchive] ON [Scan].[Reports]
AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO Scan.ReportsArchive
	(
		ArchiveDt
		,ReportId
		,TechnicianNotes
		,ReportNotes
		,ReportFooterHTML
		,CompletedInd
		,CanceledInd
		,CancellationNotes
		,CompletedDt
		,CompletedByUserGuid
		,InvoicedInd
		,InvoicedByUserGuid
		,InvoicedDt
		,InvoiceAmount
		,ResponsibleTechnicianUserGuid
		,ResponsibleSetDt
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		GETUTCDATE()
		,ReportId
		,TechnicianNotes
		,ReportNotes
		,ReportFooterHTML
		,CompletedInd
		,CanceledInd
		,CancellationNotes
		,CompletedDt
		,CompletedByUserGuid
		,InvoicedInd
		,InvoicedByUserGuid
		,InvoicedDt
		,InvoiceAmount
		,ResponsibleTechnicianUserGuid
		,ResponsibleSetDt
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM DELETED

END
GO

ALTER TABLE [Scan].[Reports] ENABLE TRIGGER [trgScanReportsArchive]
GO