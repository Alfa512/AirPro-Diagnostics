CREATE TRIGGER [Inventory].[trgAirProToolAccountsArchive] ON [Inventory].[AirProToolAccounts] 
FOR UPDATE, DELETE
    AS
	    INSERT INTO Inventory.AirProToolAccountsArchive
	    (
		    AccountGuid
			,ToolId
		    ,CreatedByUserGuid
		    ,CreatedDt
		    ,UpdatedByUserGuid
		    ,UpdatedDt
			,ArchiveDt
	    )
	    SELECT
		    AccountGuid
			,ToolId
		    ,CreatedByUserGuid
		    ,CreatedDt
		    ,UpdatedByUserGuid
		    ,UpdatedDt
			,GETUTCDATE()
	    FROM DELETED
GO

ALTER TABLE [Inventory].[AirProToolAccounts] ENABLE TRIGGER [trgAirProToolAccountsArchive]
GO