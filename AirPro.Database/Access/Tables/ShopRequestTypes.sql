CREATE TABLE [Access].[ShopRequestTypes] (
    [ShopGuid]      UNIQUEIDENTIFIER NOT NULL,
    [RequestTypeId] INT              NOT NULL,
    CONSTRAINT [PK_Access.ShopRequestTypes] PRIMARY KEY CLUSTERED ([ShopGuid] ASC, [RequestTypeId] ASC),
    CONSTRAINT [FK_Access.ShopRequestTypes_Access.Shops_ShopGuid] FOREIGN KEY ([ShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Access.ShopRequestTypes_Scan.RequestTypes_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [Scan].[RequestTypes] ([RequestTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Access].[ShopRequestTypes]([RequestTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopGuid]
    ON [Access].[ShopRequestTypes]([ShopGuid] ASC);

