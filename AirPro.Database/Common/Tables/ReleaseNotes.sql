CREATE TABLE [Common].[ReleaseNotes] (
    [ReleaseNoteId]     INT                IDENTITY (1, 1) NOT NULL,
    [Version]           NVARCHAR (MAX)     NULL,
    [Summary]           NVARCHAR (MAX)     NULL,
    [DevelopmentId]     NVARCHAR (MAX)     NULL,
    [ReleaseNote]       NVARCHAR (MAX)     NULL,
    [DeletedInd]        BIT                NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Common.ReleaseNotes] PRIMARY KEY CLUSTERED ([ReleaseNoteId] ASC),
    CONSTRAINT [FK_Common.ReleaseNotes_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.ReleaseNotes_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Common].[ReleaseNotes]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Common].[ReleaseNotes]([CreatedByUserGuid] ASC);

