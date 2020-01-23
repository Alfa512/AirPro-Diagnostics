CREATE TABLE [Access].[UserClaims] (
    [ClaimId]    INT              IDENTITY (1, 1) NOT NULL,
    [UserGuid]   UNIQUEIDENTIFIER NOT NULL,
    [ClaimType]  NVARCHAR (MAX)   NULL,
    [ClaimValue] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Access.UserClaims] PRIMARY KEY CLUSTERED ([ClaimId] ASC),
    CONSTRAINT [FK_Access.UserClaims_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Access].[UserClaims]([UserGuid] ASC);

