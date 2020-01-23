CREATE TABLE [Scan].[ReportsArchive] (
    [ArchiveId]                     INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]                     DATETIMEOFFSET (7) NOT NULL,
    [ReportId]                      INT                NOT NULL,
    [TechnicianNotes]               NVARCHAR (MAX)     NULL,
    [ReportNotes]                   NVARCHAR (MAX)     NULL,
    [ReportFooterHTML]              NVARCHAR (MAX)     NULL,
    [CompletedInd]                  BIT                NOT NULL,
    [CanceledInd]                   BIT                NULL,
    [CancellationNotes]             NVARCHAR (MAX)     NULL,
    [CompletedDt]                   DATETIMEOFFSET (7) NULL,
    [CompletedByUserGuid]           UNIQUEIDENTIFIER   NULL,
    [InvoicedInd]                   BIT                NOT NULL,
    [InvoicedByUserGuid]            UNIQUEIDENTIFIER   NULL,
    [InvoicedDt]                    DATETIMEOFFSET (7) NULL,
    [InvoiceAmount]                 DECIMAL (18, 2)    NULL,
    [ResponsibleTechnicianUserGuid] UNIQUEIDENTIFIER   NULL,
    [ResponsibleSetDt]              DATETIMEOFFSET (7) NULL,
    [DiagnosticResultId]            INT                NULL,
    [CreatedByUserGuid]             UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]                     DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]             UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                     DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Scan.ReportsArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC)
);






GO



GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_ScanReportsArchive_ReportId]
    ON [Scan].[ReportsArchive]([ReportId] ASC);

