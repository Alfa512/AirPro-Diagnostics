CREATE TABLE [Repair].[VehicleMakeTypes] (
    [VehicleMakeTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [VehicleMakeTypeName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Repair.VehicleMakeTypes] PRIMARY KEY CLUSTERED ([VehicleMakeTypeId] ASC)
);

