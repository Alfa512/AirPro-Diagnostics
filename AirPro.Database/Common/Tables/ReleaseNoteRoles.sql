CREATE TABLE [Common].[ReleaseNoteRoles] (
    [ReleaseNoteId]     INT                NOT NULL,
    [RoleGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Common.ReleaseNoteRoles] PRIMARY KEY CLUSTERED ([ReleaseNoteId] ASC, [RoleGuid] ASC),
    CONSTRAINT [FK_Common.ReleaseNoteRoles_Access.Roles_RoleGuid] FOREIGN KEY ([RoleGuid]) REFERENCES [Access].[Roles] ([RoleGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Common.ReleaseNoteRoles_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Common.ReleaseNoteRoles_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Common.ReleaseNoteRoles_Common.ReleaseNotes_ReleaseNoteId] FOREIGN KEY ([ReleaseNoteId]) REFERENCES [Common].[ReleaseNotes] ([ReleaseNoteId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Common].[ReleaseNoteRoles]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Common].[ReleaseNoteRoles]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleGuid]
    ON [Common].[ReleaseNoteRoles]([RoleGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReleaseNoteId]
    ON [Common].[ReleaseNoteRoles]([ReleaseNoteId] ASC);

