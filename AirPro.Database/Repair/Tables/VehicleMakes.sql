CREATE TABLE [Repair].[VehicleMakes] (
    [VehicleMakeId]       INT            IDENTITY (1, 1) NOT NULL,
    [VehicleMakeName]     NVARCHAR (100) NULL,
    [VehicleMakeTypeId]   INT            NOT NULL,
    [ProgramName]         NVARCHAR (MAX) NULL,
    [ProgramInstructions] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Repair.VehicleMakes] PRIMARY KEY CLUSTERED ([VehicleMakeId] ASC),
    CONSTRAINT [FK_Repair.VehicleMakes_Repair.VehicleMakeTypes_VehicleMakeTypeId] FOREIGN KEY ([VehicleMakeTypeId]) REFERENCES [Repair].[VehicleMakeTypes] ([VehicleMakeTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeTypeId]
    ON [Repair].[VehicleMakes]([VehicleMakeTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId_VehicleMakeName]
    ON [Repair].[VehicleMakes]([VehicleMakeId] ASC)
    INCLUDE([VehicleMakeName]);

