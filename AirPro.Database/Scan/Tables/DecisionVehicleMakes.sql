CREATE TABLE [Scan].[DecisionVehicleMakes] (
    [DecisionVehicleMakeId] INT                IDENTITY (1, 1) NOT NULL,
    [DecisionId]            INT                NOT NULL,
    [VehicleMakeId]         INT                NOT NULL,
    [PreSelectedInd]        BIT                NOT NULL,
    [CreatedByUserGuid]     UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]             DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]     UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]             DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Scan.DecisionVehicleMakes] PRIMARY KEY CLUSTERED ([DecisionVehicleMakeId] ASC),
    CONSTRAINT [FK_Scan.DecisionVehicleMakes_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.DecisionVehicleMakes_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.DecisionVehicleMakes_Repair.VehicleMakes_VehicleMakeId] FOREIGN KEY ([VehicleMakeId]) REFERENCES [Repair].[VehicleMakes] ([VehicleMakeId]),
    CONSTRAINT [FK_Scan.DecisionVehicleMakes_Scan.Decisions_DecisionId] FOREIGN KEY ([DecisionId]) REFERENCES [Scan].[Decisions] ([DecisionId])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[DecisionVehicleMakes]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[DecisionVehicleMakes]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeId]
    ON [Scan].[DecisionVehicleMakes]([VehicleMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DecisionId]
    ON [Scan].[DecisionVehicleMakes]([DecisionId] ASC);

