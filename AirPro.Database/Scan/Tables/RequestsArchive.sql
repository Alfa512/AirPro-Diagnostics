CREATE TABLE [Scan].[RequestsArchive] (
    [RequestArchiveId]   INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]          DATETIMEOFFSET (7) NOT NULL,
    [RequestId]          INT                NOT NULL,
    [RequestTypeId]      INT                NOT NULL,
    [OrderId]            INT                NOT NULL,
    [ReportId]           INT                NULL,
    [OtherWarningInfo]   NVARCHAR (MAX)     NULL,
    [ProblemDescription] NVARCHAR (MAX)     NULL,
    [Notes]              NVARCHAR (MAX)     NULL,
    [Contact]            NVARCHAR (MAX)     NULL,
    [RequestCategoryId]  INT                NULL,
    [SeatRemovedInd]     BIT                NOT NULL,
    [ToolId]             INT                NULL,
    [CreatedByUserGuid]  UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]          DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]  UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]          DATETIMEOFFSET (7) NULL,
    [ContactUserGuid]    UNIQUEIDENTIFIER   NULL,
    [ShopContactGuid]    UNIQUEIDENTIFIER   NULL,
    CONSTRAINT [PK_Scan.RequestsArchive] PRIMARY KEY CLUSTERED ([RequestArchiveId] ASC),
    CONSTRAINT [FK_Scan.RequestsArchive_Access.Users_ContactUserGuid] FOREIGN KEY ([ContactUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.RequestsArchive_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.RequestsArchive_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.RequestsArchive_Scan.Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Scan].[Requests] ([RequestId])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[RequestsArchive]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[RequestsArchive]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestId]
    ON [Scan].[RequestsArchive]([RequestId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContactUserGuid]
    ON [Scan].[RequestsArchive]([ContactUserGuid] ASC);

