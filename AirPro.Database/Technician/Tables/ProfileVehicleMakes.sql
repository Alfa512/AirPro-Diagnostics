CREATE TABLE [Technician].[ProfileVehicleMakes] (
    [UserGuid]      UNIQUEIDENTIFIER NOT NULL,
    [VehicleMakeId] INT              NOT NULL,
    CONSTRAINT [PK_Technician.ProfileVehicleMakes] PRIMARY KEY CLUSTERED ([UserGuid] ASC, [VehicleMakeId] ASC),
    CONSTRAINT [FK_Technician.ProfileVehicleMakes_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Technician.ProfileVehicleMakes_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId]),
    CONSTRAINT [FK_Technician.ProfileVehicleMakes_Technician.Profiles_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Technician].[Profiles] ([UserGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Technician].[ProfileVehicleMakes]([VehicleMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Technician].[ProfileVehicleMakes]([UserGuid] ASC);

