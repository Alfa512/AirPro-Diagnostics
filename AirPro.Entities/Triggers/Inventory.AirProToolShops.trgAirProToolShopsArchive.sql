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
GO

ALTER TABLE [Inventory].[AirProToolShops] ENABLE TRIGGER [trgAirProToolShopsArchive]
GO