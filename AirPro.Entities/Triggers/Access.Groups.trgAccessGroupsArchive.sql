CREATE TRIGGER [Access].[trgAccessGroupsArchive] ON [Access].[Groups]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.GroupsArchive
	(
		GroupGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Description
	)
	SELECT
		GroupGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Description
	FROM deleted

END
GO

ALTER TABLE [Access].[Groups] ENABLE TRIGGER [trgAccessGroupsArchive]
GO