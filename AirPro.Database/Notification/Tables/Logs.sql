CREATE TABLE [Notification].[Logs] (
    [NotificationLogId] INT                IDENTITY (1, 1) NOT NULL,
    [Destination]       NVARCHAR (MAX)     NULL,
    [Subject]           NVARCHAR (MAX)     NULL,
    [Body]              NVARCHAR (MAX)     NULL,
    [StatusMessage]     NVARCHAR (MAX)     NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_Support.NotificationLog] PRIMARY KEY CLUSTERED ([NotificationLogId] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_NotificationLogs_Created]
    ON [Notification].[Logs]([CreatedDt] ASC)
    INCLUDE([Destination]);

