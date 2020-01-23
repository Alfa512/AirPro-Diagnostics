CREATE TABLE [Scan].[ReportWorkTypes] (
    [ReportId]           INT              NOT NULL,
    [WorkTypeId]         INT              NOT NULL,
    [InvoicedInd]        BIT              DEFAULT ((0)) NOT NULL,
    [InvoicedByUserGuid] UNIQUEIDENTIFIER NULL,
    [InvoiceAmount]      DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_Scan.ReportWorkTypes] PRIMARY KEY CLUSTERED ([ReportId] ASC, [WorkTypeId] ASC),
    CONSTRAINT [FK_Scan.ReportWorkTypes_Access.Users_InvoicedByUserGuid] FOREIGN KEY ([InvoicedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.ReportWorkTypes_Scan.Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scan.ReportWorkTypes_Scan.WorkTypes_WorkTypeId] FOREIGN KEY ([WorkTypeId]) REFERENCES [Scan].[WorkTypes] ([WorkTypeId]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_WorkTypeId]
    ON [Scan].[ReportWorkTypes]([WorkTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReportId]
    ON [Scan].[ReportWorkTypes]([ReportId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvoicedByUserGuid]
    ON [Scan].[ReportWorkTypes]([InvoicedByUserGuid] ASC);

