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
GO

ALTER TABLE [Access].[UserAccounts] ENABLE TRIGGER [trgAccessUserAccountsArchive]
GO