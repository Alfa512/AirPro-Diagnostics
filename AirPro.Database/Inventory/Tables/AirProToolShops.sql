CREATE TABLE [Inventory].[AirProToolShops] (
    [ToolId]            INT                NOT NULL,
    [ShopGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Inventory.AirProToolShops] PRIMARY KEY CLUSTERED ([ToolId] ASC, [ShopGuid] ASC),
    CONSTRAINT [FK_Inventory.AirProToolShops_Access.Shops_ShopGuid] FOREIGN KEY ([ShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Inventory.AirProToolShops_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProToolShops_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProToolShops_Inventory.AirProTools_ToolId] FOREIGN KEY ([ToolId]) REFERENCES [Inventory].[AirProTools] ([ToolId]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_ShopGuid]
    ON [Inventory].[AirProToolShops]([ShopGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ToolId]
    ON [Inventory].[AirProToolShops]([ToolId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Inventory].[AirProToolShops]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Inventory].[AirProToolShops]([CreatedByUserGuid] ASC);


GO
CREATE TRIGGER [Inventory].[trgAirProToolShopsArchive] ON [Inventory].[AirProToolShops] 
FOR UPDATE, DELETE
    AS
	    INSERT INTO Inventory.AirProToolShopsArchive
	    (
		    ToolId
			,ShopGuid
		    ,CreatedByUserGuid
		    ,CreatedDt
		    ,UpdatedByUserGuid
		    ,UpdatedDt
			,ArchiveDt
	    )
	    SELECT
		    ToolId
			,ShopGuid
		    ,CreatedByUserGuid
		    ,CreatedDt
		    ,UpdatedByUserGuid
		    ,UpdatedDt
			,GETUTCDATE()
		FROM DELETED