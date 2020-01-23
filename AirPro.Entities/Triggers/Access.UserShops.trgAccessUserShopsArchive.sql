CREATE TRIGGER [Access].[trgAccessUserShopsArchive] ON [Access].[UserShops]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.UserShopsArchive
	(
		UserGuid
		,ShopGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		UserGuid
		,ShopGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM deleted

END
GO

ALTER TABLE [Access].[UserShops] ENABLE TRIGGER [trgAccessUserShopsArchive]
GO