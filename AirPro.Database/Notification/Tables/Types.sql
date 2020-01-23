CREATE TABLE [Notification].[Types] (
    [TypeGuid] UNIQUEIDENTIFIER NOT NULL,
    [Name]     NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Notification.Types] PRIMARY KEY CLUSTERED ([TypeGuid] ASC)
);

