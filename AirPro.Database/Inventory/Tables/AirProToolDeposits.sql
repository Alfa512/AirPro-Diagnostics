CREATE TABLE [Inventory].[AirProToolDeposits] (
    [ToolDepositId]     INT                IDENTITY (1, 1) NOT NULL,
    [ToolId]            INT                NOT NULL,
    [Date]              DATETIMEOFFSET (7) NOT NULL,
    [Description]       NVARCHAR (MAX)     NULL,
    [Amount]            DECIMAL (18, 2)    NOT NULL,
    [DeleteInd]         BIT                NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Inventory.AirProToolDeposits] PRIMARY KEY CLUSTERED ([ToolDepositId] ASC, [ToolId] ASC),
    CONSTRAINT [FK_Inventory.AirProToolDeposits_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProToolDeposits_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProToolDeposits_Inventory.AirProTools_ToolId] FOREIGN KEY ([ToolId]) REFERENCES [Inventory].[AirProTools] ([ToolId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Inventory].[AirProToolDeposits]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Inventory].[AirProToolDeposits]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ToolId]
    ON [Inventory].[AirProToolDeposits]([ToolId] ASC);

