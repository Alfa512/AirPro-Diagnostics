CREATE TABLE [Common].[Notes] (
    [NoteId]                INT                IDENTITY (1, 1) NOT NULL,
    [NoteKey]               NVARCHAR (50)      NOT NULL,
    [NoteTypeId]            INT                NOT NULL,
    [NoteDescription]       NVARCHAR (MAX)     NULL,
    [NoteDeletedInd]        BIT                NOT NULL,
    [NoteDeletedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [NoteDeletedDt]         DATETIMEOFFSET (7) NULL,
    [CreatedByUserGuid]     UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]             DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]     UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]             DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Common.Notes] PRIMARY KEY CLUSTERED ([NoteId] ASC),
    CONSTRAINT [FK_Common.Notes_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.Notes_Access.Users_NoteDeletedByUserGuid] FOREIGN KEY ([NoteDeletedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.Notes_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.Notes_Common.NoteTypes_NoteTypeId] FOREIGN KEY ([NoteTypeId]) REFERENCES [Common].[NoteTypes] ([NoteTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Common].[Notes]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Common].[Notes]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NoteDeletedByUserGuid]
    ON [Common].[Notes]([NoteDeletedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NoteTypeId]
    ON [Common].[Notes]([NoteTypeId] ASC);

