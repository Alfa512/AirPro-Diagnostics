CREATE TABLE [Diagnostic].[VehicleControllers] (
    [VehicleMakeId]    INT            NOT NULL,
    [VehicleModelName] NVARCHAR (128) NOT NULL,
    [VehicleYear]      NVARCHAR (128) NOT NULL,
    [ControllerId]     INT            NOT NULL,
    [LastRecordedDt]   DATE           NOT NULL,
    CONSTRAINT [PK_Diagnostic.VehicleControllers] PRIMARY KEY CLUSTERED ([VehicleMakeId] ASC, [VehicleModelName] ASC, [VehicleYear] ASC, [ControllerId] ASC),
    CONSTRAINT [FK_Diagnostic.VehicleControllers_Diagnostic.Controllers_ControllerId] FOREIGN KEY ([ControllerId]) REFERENCES [Diagnostic].[Controllers] ([ControllerId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Diagnostic.VehicleControllers_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ControllerId]
    ON [Diagnostic].[VehicleControllers]([ControllerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Diagnostic].[VehicleControllers]([VehicleMakeId] ASC);

