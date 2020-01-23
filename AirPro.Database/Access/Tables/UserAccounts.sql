CREATE TABLE [Access].[UserAccounts] (
    [UserGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [AccountGuid]       UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Access.UserAccounts] PRIMARY KEY CLUSTERED ([UserGuid] ASC, [AccountGuid] ASC),
    CONSTRAINT [FK_Access.UserAccounts_Access.Accounts_AccountGuid] FOREIGN KEY ([AccountGuid]) REFERENCES [Access].[Accounts] ([AccountGuid]),
    CONSTRAINT [FK_Access.UserAccounts_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.UserAccounts_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.UserAccounts_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[UserAccounts]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[UserAccounts]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AccountGuid]
    ON [Access].[UserAccounts]([AccountGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Access].[UserAccounts]([UserGuid] ASC);


GO
CREATE TRIGGER [Access].[trgAccessUserAccountsArchive] ON [Access].[UserAccounts]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.UserAccountsArchive
	(
		UserGuid
		,AccountGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		UserGuid
		,AccountGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM deleted

END