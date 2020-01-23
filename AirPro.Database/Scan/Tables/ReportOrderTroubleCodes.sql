CREATE TABLE [Scan].[ReportOrderTroubleCodes] (
    [ReportOrderTroubleCodeId] BIGINT             IDENTITY (1, 1) NOT NULL,
    [OrderId]                  INT                NOT NULL,
    [ControllerId]             INT                NOT NULL,
    [ControllerIdOrig]         INT                NOT NULL,
    [TroubleCodeId]            INT                NOT NULL,
    [TroubleCodeIdOrig]        INT                NOT NULL,
    [CreatedByUserGuid]        UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]                DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]        UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                DATETIMEOFFSET (7) NULL,
    [RequestId]                INT                NULL,
    CONSTRAINT [PK_Scan.ReportOrderTroubleCodes] PRIMARY KEY CLUSTERED ([ReportOrderTroubleCodeId] ASC),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Diagnostic.Controllers_ControllerId] FOREIGN KEY ([ControllerId]) REFERENCES [Diagnostic].[Controllers] ([ControllerId]),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Diagnostic.Controllers_ControllerIdOrig] FOREIGN KEY ([ControllerIdOrig]) REFERENCES [Diagnostic].[Controllers] ([ControllerId]),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Diagnostic.TroubleCodes_TroubleCodeId] FOREIGN KEY ([TroubleCodeId]) REFERENCES [Diagnostic].[TroubleCodes] ([TroubleCodeId]),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Diagnostic.TroubleCodes_TroubleCodeIdOrig] FOREIGN KEY ([TroubleCodeIdOrig]) REFERENCES [Diagnostic].[TroubleCodes] ([TroubleCodeId]),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Repair.Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Repair].[Orders] ([OrderId]),
    CONSTRAINT [FK_Scan.ReportOrderTroubleCodes_Scan.Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Scan].[Requests] ([RequestId])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[ReportOrderTroubleCodes]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[ReportOrderTroubleCodes]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TroubleCodeIdOrig]
    ON [Scan].[ReportOrderTroubleCodes]([TroubleCodeIdOrig] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TroubleCodeId]
    ON [Scan].[ReportOrderTroubleCodes]([TroubleCodeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ControllerIdOrig]
    ON [Scan].[ReportOrderTroubleCodes]([ControllerIdOrig] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ControllerId]
    ON [Scan].[ReportOrderTroubleCodes]([ControllerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrderId]
    ON [Scan].[ReportOrderTroubleCodes]([OrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestId]
    ON [Scan].[ReportOrderTroubleCodes]([RequestId] ASC);

