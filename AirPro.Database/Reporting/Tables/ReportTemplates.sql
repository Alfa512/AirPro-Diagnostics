CREATE TABLE [Reporting].[ReportTemplates] (
    [TemplateId]          INT             IDENTITY (1, 1) NOT NULL,
    [TemplateName]        NVARCHAR (200)  NULL,
    [TemplateDescription] NVARCHAR (1000) NULL,
    [TemplateSortOrder]   INT             NOT NULL,
    [ProcedureName]       NVARCHAR (500)  NULL,
    [AccessRoles]         NVARCHAR (500)  NULL,
    [ActiveInd]           BIT             NOT NULL,
    CONSTRAINT [PK_Reporting.ReportTemplates] PRIMARY KEY CLUSTERED ([TemplateId] ASC)
);

