CREATE TABLE [Notification].[UserOptOuts] (
    [TypeGuid] UNIQUEIDENTIFIER NOT NULL,
    [UserGuid] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Notification.UserOptOuts] PRIMARY KEY CLUSTERED ([TypeGuid] ASC, [UserGuid] ASC),
    CONSTRAINT [FK_Notification.UserOptOuts_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Notification.UserOptOuts_Notification.Types_TypeGuid] FOREIGN KEY ([TypeGuid]) REFERENCES [Notification].[Types] ([TypeGuid]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Notification].[UserOptOuts]([UserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TypeGuid]
    ON [Notification].[UserOptOuts]([TypeGuid] ASC);

