CREATE TABLE [Inventory].[AirProToolSubscriptions] (
    [ToolSubscriptionId] INT            IDENTITY (1, 1) NOT NULL,
    [ToolId]             INT            NOT NULL,
    [Vendor]             NVARCHAR (MAX) NULL,
    [Username]           NVARCHAR (MAX) NULL,
    [Password]           NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Inventory.AirProToolSubscriptions] PRIMARY KEY CLUSTERED ([ToolSubscriptionId] ASC, [ToolId] ASC),
    CONSTRAINT [FK_Inventory.AirProToolSubscriptions_Inventory.AirProTools_ToolId] FOREIGN KEY ([ToolId]) REFERENCES [Inventory].[AirProTools] ([ToolId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ToolId]
    ON [Inventory].[AirProToolSubscriptions]([ToolId] ASC);

