CREATE TABLE [Diagnostic].[Tools] (
    [DiagnosticToolId]   INT           IDENTITY (1, 1) NOT NULL,
    [DiagnosticToolName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Diagnostic.Tools] PRIMARY KEY CLUSTERED ([DiagnosticToolId] ASC)
);

