CREATE TABLE [Scan].[ReportDecisions] (
    [ReportDecisionId]  INT                IDENTITY (1, 1) NOT NULL,
    [ReportId]          INT                NOT NULL,
    [DecisionId]        INT                NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    [TextSeverity]      INT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Scan.ReportDecisions] PRIMARY KEY CLUSTERED ([ReportDecisionId] ASC),
    CONSTRAINT [FK_Scan.ReportDecisions_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.ReportDecisions_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.ReportDecisions_Scan.Decisions_DecisionId] FOREIGN KEY ([DecisionId]) REFERENCES [Scan].[Decisions] ([DecisionId]),
    CONSTRAINT [FK_Scan.ReportDecisions_Scan.Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [Scan].[Reports] ([ReportId])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[ReportDecisions]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[ReportDecisions]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DecisionId]
    ON [Scan].[ReportDecisions]([DecisionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReportId]
    ON [Scan].[ReportDecisions]([ReportId] ASC);

