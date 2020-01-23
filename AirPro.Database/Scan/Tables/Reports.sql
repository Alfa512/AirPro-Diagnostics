CREATE TABLE [Scan].[Reports] (
    [ReportId]                      INT                IDENTITY (1, 1) NOT NULL,
    [CreatedDt]                     DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDt]                     DATETIMEOFFSET (7) NULL,
    [CreatedByUserGuid]             UNIQUEIDENTIFIER   NOT NULL,
    [UpdatedByUserGuid]             UNIQUEIDENTIFIER   NULL,
    [ReportNotes]                   NVARCHAR (MAX)     NULL,
    [CompletedInd]                  BIT                NOT NULL,
    [InvoicedInd]                   BIT                DEFAULT ((0)) NOT NULL,
    [InvoicedDt]                    DATETIMEOFFSET (7) NULL,
    [CompletedByUserGuid]           UNIQUEIDENTIFIER   NULL,
    [InvoiceAmount]                 DECIMAL (18, 2)    NULL,
    [CompletedDt]                   DATETIMEOFFSET (7) NULL,
    [InvoicedByUserGuid]            UNIQUEIDENTIFIER   NULL,
    [TechnicianNotes]               NVARCHAR (MAX)     NULL,
    [ResponsibleTechnicianUserGuid] UNIQUEIDENTIFIER   NULL,
    [ResponsibleSetDt]              DATETIMEOFFSET (7) NULL,
    [CodesClearedInd]               BIT                DEFAULT ((0)) NOT NULL,
    [DangerCodeCount]               INT                DEFAULT ((0)) NOT NULL,
    [WarningCodeCount]              INT                DEFAULT ((0)) NOT NULL,
    [CautionCodeCount]              INT                DEFAULT ((0)) NOT NULL,
    [OtherCodeCount]                INT                DEFAULT ((0)) NOT NULL,
    [ReportVersion]                 ROWVERSION         NOT NULL,
    [CanceledInd]                   BIT                DEFAULT ((0)) NOT NULL,
    [CancellationNotes]             NVARCHAR (MAX)     NULL,
    [ReportFooterHTML]              NVARCHAR (MAX)     NULL,
    [CancelReasonTypeId]            INT                NULL,
    CONSTRAINT [PK_Scan.Reports] PRIMARY KEY CLUSTERED ([ReportId] ASC),
    CONSTRAINT [FK_Scan.Reports_Access.Users_CompletedByUserGuid] FOREIGN KEY ([CompletedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Reports_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Reports_Access.Users_InvoicedByUserGuid] FOREIGN KEY ([InvoicedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Reports_Access.Users_ResponsibleTechnicianUserGuid] FOREIGN KEY ([ResponsibleTechnicianUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Reports_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Reports_Scan.CancelReasonTypes_CancelReasonTypeId] FOREIGN KEY ([CancelReasonTypeId]) REFERENCES [Scan].[CancelReasonTypes] ([CancelReasonTypeId])
);
















GO



GO



GO



GO



GO



GO
CREATE TRIGGER [Scan].[trgScanReportsArchive] ON [Scan].[Reports]
AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO Scan.ReportsArchive
	(
		ArchiveDt
		,ReportId
		,TechnicianNotes
		,ReportNotes
		,ReportFooterHTML
		,CompletedInd
		,CanceledInd
		,CancellationNotes
		,CompletedDt
		,CompletedByUserGuid
		,InvoicedInd
		,InvoicedByUserGuid
		,InvoicedDt
		,InvoiceAmount
		,ResponsibleTechnicianUserGuid
		,ResponsibleSetDt
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		GETUTCDATE()
		,ReportId
		,TechnicianNotes
		,ReportNotes
		,ReportFooterHTML
		,CompletedInd
		,CanceledInd
		,CancellationNotes
		,CompletedDt
		,CompletedByUserGuid
		,InvoicedInd
		,InvoicedByUserGuid
		,InvoicedDt
		,InvoiceAmount
		,ResponsibleTechnicianUserGuid
		,ResponsibleSetDt
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM DELETED

END
GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[Reports]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResponsibleTechnicianUserGuid]
    ON [Scan].[Reports]([ResponsibleTechnicianUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvoicedByUserGuid]
    ON [Scan].[Reports]([InvoicedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[Reports]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CompletedByUserGuid]
    ON [Scan].[Reports]([CompletedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_ReportId_CompletedInd_ResponsibleTech]
    ON [Scan].[Reports]([ReportId] ASC)
    INCLUDE([CompletedInd], [ResponsibleTechnicianUserGuid]);


GO



GO
CREATE NONCLUSTERED INDEX [IX_CancelReasonTypeId]
    ON [Scan].[Reports]([CancelReasonTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ScanReports_CompletedInd]
    ON [Scan].[Reports]([CompletedInd] ASC);

