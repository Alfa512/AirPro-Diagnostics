CREATE TABLE [Notification].[TypeRoles] (
    [TypeGuid] UNIQUEIDENTIFIER NOT NULL,
    [RoleGuid] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Notification.TypeRoles] PRIMARY KEY CLUSTERED ([TypeGuid] ASC, [RoleGuid] ASC),
    CONSTRAINT [FK_Notification.TypeRoles_Access.Roles_RoleGuid] FOREIGN KEY ([RoleGuid]) REFERENCES [Access].[Roles] ([RoleGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Notification.TypeRoles_Notification.Types_TypeGuid] FOREIGN KEY ([TypeGuid]) REFERENCES [Notification].[Types] ([TypeGuid]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleGuid]
    ON [Notification].[TypeRoles]([RoleGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TypeGuid]
    ON [Notification].[TypeRoles]([TypeGuid] ASC);

