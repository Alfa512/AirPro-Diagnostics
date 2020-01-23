CREATE TABLE [Scan].[ReportSaveAudit] (
    [ReportAuditId]         INT                IDENTITY (1, 1) NOT NULL,
    [ReportAuditDt]         DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [RequestId]             INT                NULL,
    [RequestTypeId]         INT                NULL,
    [RequestCategoryId]     INT                NULL,
    [DiagnosticResultId]    INT                NULL,
    [AirProToolId]          INT                NULL,
    [ReportFooterHTML]      NVARCHAR (MAX)     NULL,
    [ReportHeaderHTML]      NVARCHAR (MAX)     NULL,
    [TechnicianNotes]       NVARCHAR (MAX)     NULL,
    [CompleteReport]        BIT                NULL,
    [CancelReport]          BIT                NULL,
    [CancelNotes]           NVARCHAR (MAX)     NULL,
    [ResponsibleTechUserId] UNIQUEIDENTIFIER   NULL,
    [ReportVersion]         VARBINARY (8)      NULL,
    [WorkTypeIds]           NVARCHAR (MAX)     NULL,
    [ResultIds]             NVARCHAR (MAX)     NULL,
    [UserGuid]              UNIQUEIDENTIFIER   NULL,
    [RecommendationJson]    NVARCHAR (MAX)     NULL,
    [DecisionJson]          NVARCHAR (MAX)     NULL,
    PRIMARY KEY CLUSTERED ([ReportAuditId] ASC)
);

