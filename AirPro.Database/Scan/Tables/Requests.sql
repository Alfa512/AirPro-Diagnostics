CREATE TABLE [Scan].[Requests] (
    [RequestId]          INT                IDENTITY (1, 1) NOT NULL,
    [RequestTypeId]      INT                NOT NULL,
    [CreatedDt]          DATETIMEOFFSET (7) NOT NULL,
    [CreatedByUserGuid]  UNIQUEIDENTIFIER   NOT NULL,
    [OrderId]            INT                NOT NULL,
    [UpdatedDt]          DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NULL,
    [UpdatedByUserGuid]  UNIQUEIDENTIFIER   NULL,
    [ReportId]           INT                NULL,
    [OtherWarningInfo]   NVARCHAR (MAX)     NULL,
    [ProblemDescription] NVARCHAR (MAX)     NULL,
    [Notes]              NVARCHAR (MAX)     NULL,
    [Contact]            NVARCHAR (MAX)     NULL,
    [RequestCategoryId]  INT                NULL,
    [SeatRemovedInd]     BIT                DEFAULT ((0)) NOT NULL,
    [ToolId]             INT                NULL,
    [ContactUserGuid]    UNIQUEIDENTIFIER   NULL,
    [ShopContactGuid]    UNIQUEIDENTIFIER   NULL,
    CONSTRAINT [PK_Scan.Requests] PRIMARY KEY CLUSTERED ([RequestId] ASC),
    CONSTRAINT [FK_Scan.Requests_Access.ShopContacts_ShopContactGuid] FOREIGN KEY ([ShopContactGuid]) REFERENCES [Access].[ShopContacts] ([ShopContactGuid]),
    CONSTRAINT [FK_Scan.Requests_Access.Users_ContactUserGuid] FOREIGN KEY ([ContactUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Requests_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Requests_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Requests_Inventory.AirProTools_ToolId] FOREIGN KEY ([ToolId]) REFERENCES [Inventory].[AirProTools] ([ToolId]),
    CONSTRAINT [FK_Scan.Requests_Repair.Orders_Repair_RepairOrderID] FOREIGN KEY ([OrderId]) REFERENCES [Repair].[Orders] ([OrderId]),
    CONSTRAINT [FK_Scan.Requests_Scan.Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId]),
    CONSTRAINT [FK_Scan.Requests_Scan.Reports_ScanReport_ReportID] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId]),
    CONSTRAINT [FK_Scan.Requests_Scan.RequestCategories_RequestTypeCategoryId] FOREIGN KEY ([RequestCategoryId]) REFERENCES [Scan].[RequestCategories] ([RequestCategoryId]),
    CONSTRAINT [FK_Scan.Requests_Scan.RequestTypes_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [Scan].[RequestTypes] ([RequestTypeId])
);






GO
CREATE NONCLUSTERED INDEX [IX_ReportID]
    ON [Scan].[Requests]([ReportID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[Requests]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Scan].[Requests]([RequestTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestId]
    ON [Scan].[Requests]([RequestId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RepairId]
    ON [Scan].[Requests]([OrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[Requests]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_RequestId_RequestTypeId_OrderId_ReportId_CreatedDt]
    ON [Scan].[Requests]([RequestId] ASC)
    INCLUDE([RequestTypeId], [OrderId], [ReportId], [CreatedDt]);

GO
CREATE NONCLUSTERED INDEX [IX_RequestCategoryId]
    ON [Scan].[Requests]([RequestCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ToolId]
    ON [Scan].[Requests]([ToolId] ASC);


GO
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
CREATE NONCLUSTERED INDEX [IX_ContactUserGuid]
    ON [Scan].[Requests]([ContactUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopContactGuid]
    ON [Scan].[Requests]([ShopContactGuid] ASC);

