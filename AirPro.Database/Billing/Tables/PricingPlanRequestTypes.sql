CREATE TABLE [Billing].[PricingPlanRequestTypes] (
    [PricingPlanRequestTypeId] INT             IDENTITY (1, 1) NOT NULL,
    [PricingPlanId]            INT             NOT NULL,
    [RequestTypeId]            INT             NOT NULL,
    [VehicleMakeTypeId]        INT             NOT NULL,
    [LineItemCost]             DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Billing.PricingPlanRequestTypes] PRIMARY KEY CLUSTERED ([PricingPlanRequestTypeId] ASC),
    CONSTRAINT [FK_Billing.PricingPlanRequestTypes_Billing.PricingPlans_PricingPlanId] FOREIGN KEY ([PricingPlanId]) REFERENCES [Billing].[PricingPlans] ([PricingPlanId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Billing.PricingPlanRequestTypes_Repair.VehicleMakeTypes_VehicleMakeTypeId] FOREIGN KEY ([VehicleMakeTypeId]) REFERENCES [Repair].[VehicleMakeTypes] ([VehicleMakeTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Billing.PricingPlanRequestTypes_Scan.RequestTypes_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [Scan].[RequestTypes] ([RequestTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeTypeId]
    ON [Billing].[PricingPlanRequestTypes]([VehicleMakeTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Billing].[PricingPlanRequestTypes]([RequestTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PricingPlanId]
    ON [Billing].[PricingPlanRequestTypes]([PricingPlanId] ASC);

