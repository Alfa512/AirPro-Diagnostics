CREATE TABLE [Billing].[EstimatePlanVehicles] (
    [EstimatePlanId] INT             NOT NULL,
    [VehicleMakeId]  INT             NOT NULL,
    [CompletionCost] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Billing.EstimatePlanVehicles] PRIMARY KEY CLUSTERED ([EstimatePlanId] ASC, [VehicleMakeId] ASC),
    CONSTRAINT [FK_Billing.EstimatePlanVehicles_Billing.EstimatePlans_EstimatePlanId] FOREIGN KEY ([EstimatePlanId]) REFERENCES [Billing].[EstimatePlans] ([EstimatePlanId]),
    CONSTRAINT [FK_Billing.EstimatePlanVehicles_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId])
);




GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Billing].[EstimatePlanVehicles]([VehicleMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EstimatePlanId]
    ON [Billing].[EstimatePlanVehicles]([EstimatePlanId] ASC);

