CREATE TRIGGER [Scan].[trgScanRequestsArchive] ON [Scan].[Requests]
AFTER UPDATE
AS
BEGIN

	INSERT INTO [Scan].[RequestsArchive]
	(
		ArchiveDt
		,RequestId
		,RequestTypeId
		,OrderId
		,ReportId
		,OtherWarningInfo
		,ProblemDescription
		,Notes
		,Contact
		,RequestCategoryId
		,SeatRemovedInd
		,ToolId
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,ContactUserGuid
		,ShopContactGuid
	)
	SELECT
		GETUTCDATE()
		,RequestId
		,RequestTypeId
		,OrderId
		,ReportId
		,OtherWarningInfo
		,ProblemDescription
		,Notes
		,Contact
		,RequestCategoryId
		,SeatRemovedInd
		,ToolId
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,ContactUserGuid
		,ShopContactGuid
	FROM deleted

END
GO

ALTER TABLE [Scan].[Requests] ENABLE TRIGGER [trgScanRequestsArchive]
GO