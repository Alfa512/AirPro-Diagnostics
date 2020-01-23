CREATE TABLE [Inventory].[AirProToolAccounts] (
    [ToolId]            INT                NOT NULL,
    [AccountGuid]       UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Inventory.AirProToolAccounts] PRIMARY KEY CLUSTERED ([ToolId] ASC, [AccountGuid] ASC),
    CONSTRAINT [FK_Inventory.AirProToolAccounts_Access.Accounts_AccountGuid] FOREIGN KEY ([AccountGuid]) REFERENCES [Access].[Accounts] ([AccountGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Inventory.AirProToolAccounts_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProToolAccounts_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProToolAccounts_Inventory.AirProTools_ToolId] FOREIGN KEY ([ToolId]) REFERENCES [Inventory].[AirProTools] ([ToolId]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_AccountGuid]
    ON [Inventory].[AirProToolAccounts]([AccountGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ToolId]
    ON [Inventory].[AirProToolAccounts]([ToolId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Inventory].[AirProToolAccounts]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Inventory].[AirProToolAccounts]([CreatedByUserGuid] ASC);


GO
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