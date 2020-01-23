CREATE TABLE [Diagnostic].[ResultTroubleCodes] (
    [ResultTroubleCodeId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [ResultId]               INT            NOT NULL,
    [ControllerId]           INT            NOT NULL,
    [TroubleCodeId]          INT            NOT NULL,
    [TroubleCodeInformation] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Diagnostic.ResultTroubleCodes] PRIMARY KEY CLUSTERED ([ResultTroubleCodeId] ASC),
    CONSTRAINT [FK_Diagnostic.ResultTroubleCodes_Diagnostic.Controllers_ControllerId] FOREIGN KEY ([ControllerId]) REFERENCES [Diagnostic].[Controllers] ([ControllerId]),
    CONSTRAINT [FK_Diagnostic.ResultTroubleCodes_Diagnostic.Results_ResultId] FOREIGN KEY ([ResultId]) REFERENCES [Diagnostic].[Results] ([ResultId]),
    CONSTRAINT [FK_Diagnostic.ResultTroubleCodes_Diagnostic.TroubleCodes_TroubleCodeId] FOREIGN KEY ([TroubleCodeId]) REFERENCES [Diagnostic].[TroubleCodes] ([TroubleCodeId])
);


GO
CREATE NONCLUSTERED INDEX [IDX_DiagnosticResultTroubleCodes_ResultId]
    ON [Diagnostic].[ResultTroubleCodes]([ResultId] ASC)
    INCLUDE([ControllerId], [TroubleCodeId]);


GO
CREATE NONCLUSTERED INDEX [IX_TroubleCodeId]
    ON [Diagnostic].[ResultTroubleCodes]([TroubleCodeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ControllerId]
    ON [Diagnostic].[ResultTroubleCodes]([ControllerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResultId]
    ON [Diagnostic].[ResultTroubleCodes]([ResultId] ASC);

