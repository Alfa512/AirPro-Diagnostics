CREATE TABLE [Access].[UserShops] (
    [UserGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [ShopGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Access.UserShops] PRIMARY KEY CLUSTERED ([UserGuid] ASC, [ShopGuid] ASC),
    CONSTRAINT [FK_Access.UserShops_Access.Shops_ShopGuid] FOREIGN KEY ([ShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Access.UserShops_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.UserShops_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.UserShops_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[UserShops]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[UserShops]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopGuid]
    ON [Access].[UserShops]([ShopGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Access].[UserShops]([UserGuid] ASC);


GO
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