CREATE TABLE [Technician].[ProfileSchedules] (
    [ScheduleId]  INT              IDENTITY (1, 1) NOT NULL,
    [UserGuid]    UNIQUEIDENTIFIER NOT NULL,
    [DayOfWeek]   INT              NOT NULL,
    [StartTime]   TIME (7)         NULL,
    [EndTime]     TIME (7)         NULL,
    [BreakStart] TIME (7)         NULL,
    [BreakEnd]   TIME (7)         NULL,
    CONSTRAINT [PK_Technician.ProfileSchedules] PRIMARY KEY CLUSTERED ([ScheduleId] ASC),
    CONSTRAINT [FK_Technician.ProfileSchedules_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.ProfileSchedules_Technician.Profiles_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Technician].[Profiles] ([UserGuid])
);






GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Technician].[ProfileSchedules]([UserGuid] ASC);

