CREATE TABLE [Scan].[ReportValidationRules] (
    [ReportId]                     INT              NOT NULL,
    [ValidationRuleId]             INT              NOT NULL,
    [ValidationRuleResultInd]      BIT              NOT NULL,
    [ResultAcknowledgedInd]        BIT              NULL,
    [ResultAcknowledgedByUserGuid] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Scan.ReportValidationRules] PRIMARY KEY CLUSTERED ([ReportId] ASC, [ValidationRuleId] ASC),
    CONSTRAINT [FK_Scan.ReportValidationRules_Scan.Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scan.ReportValidationRules_Scan.ValidationRules_ValidationRuleId] FOREIGN KEY ([ValidationRuleId]) REFERENCES [Scan].[ValidationRules] ([ValidationRuleId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ValidationRuleId]
    ON [Scan].[ReportValidationRules]([ValidationRuleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReportId]
    ON [Scan].[ReportValidationRules]([ReportId] ASC);

