CREATE TABLE [Technician].[ProfileTimeOffEntries] (
    [TimeOffEntryId]    INT                IDENTITY (1, 1) NOT NULL,
    [UserGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [StartDate]         DATETIMEOFFSET (7) NOT NULL,
    [EndDate]           DATETIMEOFFSET (7) NOT NULL,
    [Reason]            NVARCHAR (MAX)     NULL,
    [DeleteInd]         BIT                NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Technician.ProfileTimeOffEntries] PRIMARY KEY CLUSTERED ([TimeOffEntryId] ASC),
    CONSTRAINT [FK_Technician.ProfileTimeOffEntries_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.ProfileTimeOffEntries_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.ProfileTimeOffEntries_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.ProfileTimeOffEntries_Technician.Profiles_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Technician].[Profiles] ([UserGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Technician].[ProfileTimeOffEntries]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Technician].[ProfileTimeOffEntries]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Technician].[ProfileTimeOffEntries]([UserGuid] ASC);

