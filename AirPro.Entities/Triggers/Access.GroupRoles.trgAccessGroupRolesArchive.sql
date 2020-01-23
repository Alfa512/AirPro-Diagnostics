CREATE TRIGGER [Access].[trgAccessGroupRolesArchive] ON [Access].[GroupRoles]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.GroupRolesArchive
	(
		GroupGuid
		,RoleGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		GroupGuid
		,RoleGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM deleted

END
GO

ALTER TABLE [Access].[GroupRoles] ENABLE TRIGGER [trgAccessGroupRolesArchive]
GO