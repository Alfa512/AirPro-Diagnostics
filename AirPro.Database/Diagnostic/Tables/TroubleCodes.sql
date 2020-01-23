CREATE TABLE [Diagnostic].[TroubleCodes] (
    [TroubleCodeId]          INT             IDENTITY (1, 1) NOT NULL,
    [TroubleCode]            NVARCHAR (20)   NULL,
    [TroubleCodeDescription] NVARCHAR (1000) NULL,
    [TroubleCodeHash]        AS              (checksum([TroubleCode],[TroubleCodeDescription])),
    CONSTRAINT [PK_Diagnostic.TroubleCodes] PRIMARY KEY CLUSTERED ([TroubleCodeId] ASC)
);






GO



GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_DiagnosticTroubleCodes_TroubleCodeHash]
    ON [Diagnostic].[TroubleCodes]([TroubleCodeHash] ASC);

