

IF (OBJECT_ID('Scan.trgScanReportsArchive') IS NOT NULL)
	DROP TRIGGER Scan.trgScanReportsArchive
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ReportsArchive')
	DROP TABLE [Scan].[ReportsArchive]
GO

/****** Object:  Table [Scan].[ReportsArchive]    Script Date: 11/21/2016 2:35:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Scan].[ReportsArchive](
	[ReportArchiveID] [int] IDENTITY(1,1) NOT NULL,
	[ReportID] [int] NOT NULL,
	[CreatedDt] [datetimeoffset](7) NOT NULL,
	[UpdatedDt] [datetimeoffset](7) NULL,
	[CreatedBy_Id] [nvarchar](128) NOT NULL,
	[UpdatedBy_Id] [nvarchar](128) NULL,
	[ReportNotes] [nvarchar](max) NULL,
	[CompletedInd] [bit] NOT NULL,
	[InvoicedInd] [bit] NOT NULL DEFAULT ((0)),
	[InvoicedDt] [datetimeoffset](7) NULL,
	[CompletedByID] [nvarchar](128) NULL,
	[InvoiceAmount] [decimal](18, 2) NULL,
	[CompletedDt] [datetimeoffset](7) NULL,
	[InvoicedByID] [nvarchar](128) NULL,
	[TechnicianNotes] [nvarchar](max) NULL,
	[ResponsibleTechnicianID] [nvarchar](128) NULL,
	[ResponsibleSetDt] [datetimeoffset](7) NULL,
	[ArchiveDt] [datetimeoffset](7) NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_Scan.ReportsArchive] PRIMARY KEY CLUSTERED 
(
	[ReportArchiveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [Scan].[ReportsArchive]  WITH CHECK ADD  CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_CreatedBy_Id] FOREIGN KEY([CreatedBy_Id])
REFERENCES [Access].[Users] ([UserId])
GO

ALTER TABLE [Scan].[ReportsArchive] CHECK CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_CreatedBy_Id]
GO

ALTER TABLE [Scan].[ReportsArchive]  WITH CHECK ADD  CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_InvoicedBy_Id] FOREIGN KEY([CompletedByID])
REFERENCES [Access].[Users] ([UserId])
GO

ALTER TABLE [Scan].[ReportsArchive] CHECK CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_InvoicedBy_Id]
GO

ALTER TABLE [Scan].[ReportsArchive]  WITH CHECK ADD  CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_InvoicedByID] FOREIGN KEY([InvoicedByID])
REFERENCES [Access].[Users] ([UserId])
GO

ALTER TABLE [Scan].[ReportsArchive] CHECK CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_InvoicedByID]
GO

ALTER TABLE [Scan].[ReportsArchive]  WITH CHECK ADD  CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_ResponsibleTechnicianID] FOREIGN KEY([ResponsibleTechnicianID])
REFERENCES [Access].[Users] ([UserId])
GO

ALTER TABLE [Scan].[ReportsArchive] CHECK CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_ResponsibleTechnicianID]
GO

ALTER TABLE [Scan].[ReportsArchive]  WITH CHECK ADD  CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_UpdatedBy_Id] FOREIGN KEY([UpdatedBy_Id])
REFERENCES [Access].[Users] ([UserId])
GO

ALTER TABLE [Scan].[ReportsArchive] CHECK CONSTRAINT [FK_Scan.ReportsArchive_Access.Users_UpdatedBy_Id]
GO

ALTER TABLE [Scan].[ReportsArchive]  WITH CHECK ADD  CONSTRAINT [FK_Scan.ReportsArchive_Scan.Reports_ReportId] FOREIGN KEY([ReportID])
REFERENCES [Scan].[Reports] ([ReportID])
GO

ALTER TABLE [Scan].[ReportsArchive] CHECK CONSTRAINT [FK_Scan.ReportsArchive_Scan.Reports_ReportId]
GO

/****** Object:  Trigger [Scan].[trgScanReportsArchive]    Script Date: 11/21/2016 2:35:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [Scan].[trgScanReportsArchive] ON [Scan].[Reports]
AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO [Scan].[ReportsArchive]
	(
		ReportID
		,CreatedDt
		,UpdatedDt
		,CreatedBy_Id
		,UpdatedBy_Id
		,ReportNotes
		,CompletedInd
		,InvoicedInd
		,InvoicedDt
		,CompletedByID
		,InvoiceAmount
		,CompletedDt
		,InvoicedByID
		,TechnicianNotes
		,ResponsibleTechnicianID
		,ResponsibleSetDt
		,ArchiveDt
	)
	SELECT
		ReportID
		,CreatedDt
		,UpdatedDt
		,CreatedBy_Id
		,UpdatedBy_Id
		,ReportNotes
		,CompletedInd
		,InvoicedInd
		,InvoicedDt
		,CompletedByID
		,InvoiceAmount
		,CompletedDt
		,InvoicedByID
		,TechnicianNotes
		,ResponsibleTechnicianID
		,ResponsibleSetDt
		,GETUTCDATE()
	FROM deleted

END

GO

