CREATE TABLE [Scan].[ReportVehicleMakeTools] (
    [ReportId]          INT            NOT NULL,
    [VehicleMakeToolId] INT            NOT NULL,
    [ToolVersion]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Scan.ReportVehicleMakeTools] PRIMARY KEY CLUSTERED ([ReportId] ASC, [VehicleMakeToolId] ASC),
    CONSTRAINT [FK_Scan.ReportVehicleMakeTools_Repair.VehicleMakeTools_VehicleMakeToolId] FOREIGN KEY ([VehicleMakeToolId]) REFERENCES [Repair].[VehicleMakeTools] ([VehicleMakeToolId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scan.ReportVehicleMakeTools_Scan.Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleMakeToolId]
    ON [Scan].[ReportVehicleMakeTools]([VehicleMakeToolId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReportId]
    ON [Scan].[ReportVehicleMakeTools]([ReportId] ASC);

