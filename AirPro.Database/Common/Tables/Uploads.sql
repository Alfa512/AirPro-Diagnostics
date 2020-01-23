CREATE TABLE [Common].[Uploads] (
    [UploadId]                INT                IDENTITY (1, 1) NOT NULL,
    [UploadKey]               NVARCHAR (50)      NOT NULL,
    [UploadTypeId]            INT                NOT NULL,
    [UploadFileName]          NVARCHAR (150)     NULL,
    [UploadFileExtension]     NVARCHAR (10)      NULL,
    [UploadFileSizeBytes]     BIGINT             NOT NULL,
    [UploadStorageName]       NVARCHAR (50)      NULL,
    [UploadMimeType]          NVARCHAR (100)     NULL,
    [UploadDeletedInd]        BIT                NOT NULL,
    [UploadDeletedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UploadDeletedDt]         DATETIMEOFFSET (7) NULL,
    [CreatedByUserGuid]       UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]               DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]       UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]               DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Common.Uploads] PRIMARY KEY CLUSTERED ([UploadId] ASC),
    CONSTRAINT [FK_Common.Uploads_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.Uploads_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.Uploads_Access.Users_UploadDeletedByUserGuid] FOREIGN KEY ([UploadDeletedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.Uploads_Common.UploadTypes_UploadTypeId] FOREIGN KEY ([UploadTypeId]) REFERENCES [Common].[UploadTypes] ([UploadTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Common].[Uploads]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Common].[Uploads]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UploadDeletedByUserGuid]
    ON [Common].[Uploads]([UploadDeletedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UploadTypeId]
    ON [Common].[Uploads]([UploadTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CommonUploads_UploadDeletedInd]
    ON [Common].[Uploads]([UploadDeletedInd] ASC)
    INCLUDE([UploadKey], [UploadTypeId], [UploadFileName], [UploadFileExtension], [CreatedDt]);

