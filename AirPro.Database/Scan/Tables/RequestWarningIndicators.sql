CREATE TABLE [Scan].[RequestWarningIndicators] (
    [RequestId]          INT NOT NULL,
    [WarningIndicatorId] INT NOT NULL,
    CONSTRAINT [PK_Scan.RequestWarningIndicators] PRIMARY KEY CLUSTERED ([RequestId] ASC, [WarningIndicatorId] ASC),
    CONSTRAINT [FK_Scan.RequestWarningIndicators_Scan.Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Scan].[Requests] ([RequestId]),
    CONSTRAINT [FK_Scan.RequestWarningIndicators_Scan.WarningIndicators_WarningIndicatorId] FOREIGN KEY ([WarningIndicatorId]) REFERENCES [Scan].[WarningIndicators] ([WarningIndicatorId])
);




GO
CREATE NONCLUSTERED INDEX [IX_WarningIndicatorID]
    ON [Scan].[RequestWarningIndicators]([WarningIndicatorID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestID]
    ON [Scan].[RequestWarningIndicators]([RequestID] ASC);

