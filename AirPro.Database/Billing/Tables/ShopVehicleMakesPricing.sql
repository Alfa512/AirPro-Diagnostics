CREATE TABLE [Billing].[ShopVehicleMakesPricing] (
    [ShopId]        UNIQUEIDENTIFIER NOT NULL,
    [VehicleMakeId] INT              NOT NULL,
    [PricingPlanId] INT              NOT NULL,
    CONSTRAINT [PK_Billing.ShopVehicleMakesPricing] PRIMARY KEY CLUSTERED ([ShopId] ASC, [VehicleMakeId] ASC),
    CONSTRAINT [FK_Billing.ShopVehicleMakesPricing_Access.Shops_ShopId] FOREIGN KEY ([ShopId]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Billing.ShopVehicleMakesPricing_Billing.PricingPlans_PricingPlanId] FOREIGN KEY ([PricingPlanId]) REFERENCES [Billing].[PricingPlans] ([PricingPlanId]),
    CONSTRAINT [FK_Billing.ShopVehicleMakesPricing_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PricingPlanId]
    ON [Billing].[ShopVehicleMakesPricing]([PricingPlanId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Billing].[ShopVehicleMakesPricing]([VehicleMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopId]
    ON [Billing].[ShopVehicleMakesPricing]([ShopId] ASC);

