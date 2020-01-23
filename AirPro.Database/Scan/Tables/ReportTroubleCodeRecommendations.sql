CREATE TABLE [Scan].[ReportTroubleCodeRecommendations] (
    [ReportTroubleCodeRecommendationId] BIGINT             IDENTITY (1, 1) NOT NULL,
    [ReportOrderTroubleCodeId]          BIGINT             NOT NULL,
    [ReportId]                          INT                NOT NULL,
    [ResultTroubleCodeId]               BIGINT             NULL,
    [InformCustomerInd]                 BIT                NOT NULL,
    [AccidentRelatedInd]                BIT                NULL,
    [ExcludeFromReportInd]              BIT                NOT NULL,
    [TroubleCodeRecommendationId]       INT                NULL,
    [CreatedByUserGuid]                 UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]                         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]                 UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                         DATETIMEOFFSET (7) NULL,
    [TroubleCodeNoteText]               NVARCHAR (MAX)     NULL,
    [CodeClearedInd]                    BIT                DEFAULT ((0)) NOT NULL,
    [TextSeverity]                      INT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Scan.ReportTroubleCodeRecommendations] PRIMARY KEY CLUSTERED ([ReportTroubleCodeRecommendationId] ASC),
    CONSTRAINT [FK_Scan.ReportTroubleCodeRecommendations_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.ReportTroubleCodeRecommendations_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.ReportTroubleCodeRecommendations_Diagnostic.ResultTroubleCodes_ResultTroubleCodeId] FOREIGN KEY ([ResultTroubleCodeId]) REFERENCES [Diagnostic].[ResultTroubleCodes] ([ResultTroubleCodeId]),
    CONSTRAINT [FK_Scan.ReportTroubleCodeRecommendations_Scan.ReportOrderTroubleCodes_ReportOrderTroubleCodeId] FOREIGN KEY ([ReportOrderTroubleCodeId]) REFERENCES [Scan].[ReportOrderTroubleCodes] ([ReportOrderTroubleCodeId]),
    CONSTRAINT [FK_Scan.ReportTroubleCodeRecommendations_Scan.Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId]),
    CONSTRAINT [FK_Scan.ReportTroubleCodeRecommendations_Scan.TroubleCodeRecommendations_TroubleCodeRecommendationId] FOREIGN KEY ([TroubleCodeRecommendationId]) REFERENCES [Scan].[TroubleCodeRecommendations] ([TroubleCodeRecommendationId])
);






GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[ReportTroubleCodeRecommendations]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[ReportTroubleCodeRecommendations]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TroubleCodeRecommendationId]
    ON [Scan].[ReportTroubleCodeRecommendations]([TroubleCodeRecommendationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResultTroubleCodeId]
    ON [Scan].[ReportTroubleCodeRecommendations]([ResultTroubleCodeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReportId]
    ON [Scan].[ReportTroubleCodeRecommendations]([ReportId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReportOrderTroubleCodeId]
    ON [Scan].[ReportTroubleCodeRecommendations]([ReportOrderTroubleCodeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ScanReportTroubleCodeRecommendations_TroubleCodeRecommendationId_ReportOrderTroubleCodeId]
    ON [Scan].[ReportTroubleCodeRecommendations]([TroubleCodeRecommendationId] ASC)
    INCLUDE([ReportOrderTroubleCodeId]);

