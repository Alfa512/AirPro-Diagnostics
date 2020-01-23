CREATE TABLE [Access].[ShopVehicleMakes] (
    [ShopId]        UNIQUEIDENTIFIER NOT NULL,
    [VehicleMakeId] INT              NOT NULL,
    CONSTRAINT [PK_Access.ShopVehicleMakes] PRIMARY KEY CLUSTERED ([ShopId] ASC, [VehicleMakeId] ASC),
    CONSTRAINT [FK_Access.ShopVehicleMakes_Access.Shops_ShopId] FOREIGN KEY ([ShopId]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Access.ShopVehicleMakes_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Access].[ShopVehicleMakes]([VehicleMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopId]
    ON [Access].[ShopVehicleMakes]([ShopId] ASC);

