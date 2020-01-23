CREATE TABLE [Repair].[OrderPointOfImpacts] (
    [OrderID]         INT NOT NULL,
    [PointOfImpactId] INT NOT NULL,
    CONSTRAINT [PK_Repair.OrderPointOfImpacts] PRIMARY KEY CLUSTERED ([OrderID] ASC, [PointOfImpactId] ASC),
    CONSTRAINT [FK_Repair.OrderPointOfImpacts_Repair.Orders_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Repair].[Orders] ([OrderId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Repair.OrderPointOfImpacts_Repair.PointOfImpacts_PointOfImpactId] FOREIGN KEY ([PointOfImpactId]) REFERENCES [Repair].[PointOfImpacts] ([PointOfImpactId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PointOfImpactId]
    ON [Repair].[OrderPointOfImpacts]([PointOfImpactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrderID]
    ON [Repair].[OrderPointOfImpacts]([OrderID] ASC);

