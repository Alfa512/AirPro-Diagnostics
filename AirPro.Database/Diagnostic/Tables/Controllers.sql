CREATE TABLE [Diagnostic].[Controllers] (
    [ControllerId]   INT            IDENTITY (1, 1) NOT NULL,
    [ControllerName] NVARCHAR (200) NULL,
    [ControllerHash] AS             (checksum([ControllerName])),
    CONSTRAINT [PK_Diagnostic.Controllers] PRIMARY KEY CLUSTERED ([ControllerId] ASC)
);




GO



GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_DiagnosticControllers_ControllerHash]
    ON [Diagnostic].[Controllers]([ControllerHash] ASC);

