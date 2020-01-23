CREATE TABLE [Technician].[Profiles] (
    [UserGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [DisplayName]       NVARCHAR (128)     NOT NULL,
    [EmployeeId]        NVARCHAR (128)     NOT NULL,
    [OtherNotes]        NVARCHAR (MAX)     NULL,
    [ActiveInd]         BIT                DEFAULT ((1)) NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    [LocationId]        INT                NULL,
    CONSTRAINT [PK_Technician.Profiles] PRIMARY KEY CLUSTERED ([UserGuid] ASC),
    CONSTRAINT [FK_Technician.Profiles_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.Profiles_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.Profiles_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.Profiles_Technician.Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Technician].[Locations] ([LocationId])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Technician].[Profiles]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Technician].[Profiles]([CreatedByUserGuid] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [Technician].[Profiles]([LocationId] ASC);

