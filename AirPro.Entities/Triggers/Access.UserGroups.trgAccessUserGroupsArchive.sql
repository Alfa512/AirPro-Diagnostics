CREATE TRIGGER [Access].[trgAccessUserGroupsArchive] ON [Access].[UserGroups]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.UserGroupsArchive
	(
		UserGuid
		,GroupGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		UserGuid
		,GroupGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM deleted

END
GO

ALTER TABLE [Access].[UserGroups] ENABLE TRIGGER [trgAccessUserGroupsArchive]
GO