CREATE TABLE [Scan].[WorkTypeVehicleMakes] (
    [WorkTypeId]    INT NOT NULL,
    [VehicleMakeId] INT NOT NULL,
    CONSTRAINT [PK_Scan.WorkTypeVehicleMakes] PRIMARY KEY CLUSTERED ([WorkTypeId] ASC, [VehicleMakeId] ASC),
    CONSTRAINT [FK_Scan.WorkTypeVehicleMakes_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scan.WorkTypeVehicleMakes_Scan.WorkTypes_WorkTypeId] FOREIGN KEY ([WorkTypeId]) REFERENCES [Scan].[WorkTypes] ([WorkTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Scan].[WorkTypeVehicleMakes]([VehicleMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkTypeId]
    ON [Scan].[WorkTypeVehicleMakes]([WorkTypeId] ASC);

