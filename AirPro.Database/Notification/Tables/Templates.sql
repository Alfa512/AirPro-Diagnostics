CREATE TABLE [Notification].[Templates] (
    [NotificationTemplateId] INT                IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (MAX)     NULL,
    [Options]                NVARCHAR (MAX)     NULL,
    [Subject]                NVARCHAR (MAX)     NULL,
    [EmailBody]              NVARCHAR (MAX)     NULL,
    [CreatedDt]              DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDt]              DATETIMEOFFSET (7) NULL,
    [CreatedByUserGuid]      UNIQUEIDENTIFIER   NOT NULL,
    [UpdatedByUserGuid]      UNIQUEIDENTIFIER   NULL,
    [TextMessage]            NVARCHAR (MAX)     NULL,
    CONSTRAINT [PK_Support.NotificationTemplates] PRIMARY KEY CLUSTERED ([NotificationTemplateId] ASC),
    CONSTRAINT [FK_Support.NotificationTemplates_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Support.NotificationTemplates_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Notification].[Templates]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Notification].[Templates]([CreatedByUserGuid] ASC);

