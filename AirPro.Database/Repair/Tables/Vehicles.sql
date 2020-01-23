CREATE TABLE [Repair].[Vehicles] (
    [VehicleVIN]      NVARCHAR (128) NOT NULL,
    [Make]            NVARCHAR (100) NOT NULL,
    [Model]           NVARCHAR (300) NOT NULL,
    [Year]            NVARCHAR (MAX) NOT NULL,
    [Transmission]    NVARCHAR (200) NOT NULL,
    [VehicleLookupId] INT            NULL,
    [VehicleMakeId]   INT            NOT NULL,
    CONSTRAINT [PK_Repair.Vehicles] PRIMARY KEY CLUSTERED ([VehicleVIN] ASC),
    CONSTRAINT [FK_Repair.Vehicles_Repair.VehicleLookups_VehicleLookup_LookupID] FOREIGN KEY ([VehicleLookupId]) REFERENCES [Repair].[VehicleLookups] ([VehicleLookupId]),
    CONSTRAINT [FK_Repair.Vehicles_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId])
);






GO
CREATE NONCLUSTERED INDEX [IX_VehicleLookup_LookupID]
    ON [Repair].[Vehicles]([VehicleLookupId] ASC);




GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Repair].[Vehicles]([VehicleMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleVIN_VehicleMakeId_Model_Year]
    ON [Repair].[Vehicles]([VehicleVIN] ASC)
    INCLUDE([VehicleMakeId], [Model], [Year]);


GO
CREATE NONCLUSTERED INDEX [IX_RepairVehicles_VehicleLookupId]
    ON [Repair].[Vehicles]([VehicleLookupId] ASC)
    INCLUDE([VehicleMakeId], [Model], [Year]);

