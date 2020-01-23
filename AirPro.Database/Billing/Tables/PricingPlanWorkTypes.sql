CREATE TABLE [Billing].[PricingPlanWorkTypes] (
    [PricingPlanWorkTypeId] INT             IDENTITY (1, 1) NOT NULL,
    [PricingPlanId]         INT             NOT NULL,
    [WorkTypeId]            INT             NOT NULL,
    [VehicleMakeTypeId]     INT             NOT NULL,
    [LineItemCost]          DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Billing.PricingPlanWorkTypes] PRIMARY KEY CLUSTERED ([PricingPlanWorkTypeId] ASC),
    CONSTRAINT [FK_Billing.PricingPlanWorkTypes_Billing.PricingPlans_PricingPlanId] FOREIGN KEY ([PricingPlanId]) REFERENCES [Billing].[PricingPlans] ([PricingPlanId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Billing.PricingPlanWorkTypes_Repair.VehicleMakeTypes_VehicleMakeTypeId] FOREIGN KEY ([VehicleMakeTypeId]) REFERENCES [Repair].[VehicleMakeTypes] ([VehicleMakeTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Billing.PricingPlanWorkTypes_Scan.WorkTypes_WorkTypeId] FOREIGN KEY ([WorkTypeId]) REFERENCES [Scan].[WorkTypes] ([WorkTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeTypeId]
    ON [Billing].[PricingPlanWorkTypes]([VehicleMakeTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkTypeId]
    ON [Billing].[PricingPlanWorkTypes]([WorkTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PricingPlanId]
    ON [Billing].[PricingPlanWorkTypes]([PricingPlanId] ASC);

