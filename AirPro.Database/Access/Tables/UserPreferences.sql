CREATE TABLE [Access].[UserPreferences] (
    [UserGuid]     UNIQUEIDENTIFIER   NOT NULL,
    [ControlId]    NVARCHAR (128)     NOT NULL,
    [SettingsJson] NVARCHAR (MAX)     NULL,
    [UpdatedDt]    DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_Access.UserPreferences] PRIMARY KEY CLUSTERED ([UserGuid] ASC, [ControlId] ASC)
);

