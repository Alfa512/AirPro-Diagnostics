CREATE TABLE [Access].[UserRoles] (
    [UserGuid] UNIQUEIDENTIFIER NOT NULL,
    [RoleGuid] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Access.UserRoles] PRIMARY KEY CLUSTERED ([UserGuid] ASC, [RoleGuid] ASC),
    CONSTRAINT [FK_Access.UserRoles_Access.Roles_RoleGuid] FOREIGN KEY ([RoleGuid]) REFERENCES [Access].[Roles] ([RoleGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Access.UserRoles_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE
);








GO



GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Access].[UserRoles]([UserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleGuid]
    ON [Access].[UserRoles]([RoleGuid] ASC);

