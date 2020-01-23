CREATE TABLE [Access].[Logins] (
    [LoginId]            INT                IDENTITY (1, 1) NOT NULL,
    [UserGuid]           UNIQUEIDENTIFIER   NULL,
    [LoginName]          NVARCHAR (MAX)     NULL,
    [UserAgent]          NVARCHAR (MAX)     NULL,
    [UserHostAddress]    NVARCHAR (MAX)     NULL,
    [UserHostName]       NVARCHAR (MAX)     NULL,
    [AccountLockedOut]   BIT                NOT NULL,
    [TwoFactorChallenge] BIT                NOT NULL,
    [TwoFactorVerified]  BIT                NOT NULL,
    [LoginAttemptDt]     DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_Access.Logins] PRIMARY KEY CLUSTERED ([LoginId] ASC),
    CONSTRAINT [FK_Access.Logins_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Access].[Logins]([UserGuid] ASC);

