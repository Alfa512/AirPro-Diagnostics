CREATE TABLE [Inventory].[AirProToolAccountsArchive] (
    [ArchiveId]         INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]         DATETIMEOFFSET (7) NOT NULL,
    [ToolId]            INT                NOT NULL,
    [AccountGuid]       UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Inventory.AirProToolAccountsArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC)
);

