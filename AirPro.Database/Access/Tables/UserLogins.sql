CREATE TABLE [Access].[UserLogins] (
    [LoginProvider] NVARCHAR (128)   NOT NULL,
    [ProviderKey]   NVARCHAR (128)   NOT NULL,
    [UserGuid]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Access.UserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserGuid] ASC),
    CONSTRAINT [FK_Access.UserLogins_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Access].[UserLogins]([UserGuid] ASC);

