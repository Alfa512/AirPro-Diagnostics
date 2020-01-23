CREATE TABLE [Scan].[ReportDiagnosticResults] (
    [ReportId]           INT NOT NULL,
    [DiagnosticResultId] INT NOT NULL,
    CONSTRAINT [PK_Scan.ReportDiagnosticResults] PRIMARY KEY CLUSTERED ([ReportId] ASC, [DiagnosticResultId] ASC),
    CONSTRAINT [FK_Scan.ReportDiagnosticResults_Diagnostic.Results_DiagnosticResultId] FOREIGN KEY ([DiagnosticResultId]) REFERENCES [Diagnostic].[Results] ([ResultId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scan.ReportDiagnosticResults_Scan.Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_DiagnosticResultId]
    ON [Scan].[ReportDiagnosticResults]([DiagnosticResultId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReportId]
    ON [Scan].[ReportDiagnosticResults]([ReportId] ASC);

