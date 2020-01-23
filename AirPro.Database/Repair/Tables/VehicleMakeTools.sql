CREATE TABLE [Repair].[VehicleMakeTools] (
    [VehicleMakeToolId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (MAX) NULL,
    [VehicleMakeId]     INT            NOT NULL,
    CONSTRAINT [PK_Repair.VehicleMakeTools] PRIMARY KEY CLUSTERED ([VehicleMakeToolId] ASC),
    CONSTRAINT [FK_Repair.VehicleMakeTools_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Repair].[VehicleMakeTools]([VehicleMakeId] ASC);

