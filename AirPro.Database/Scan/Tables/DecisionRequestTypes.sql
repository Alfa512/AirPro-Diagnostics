CREATE TABLE [Scan].[DecisionRequestTypes] (
    [DecisionRequestTypeId] INT                IDENTITY (1, 1) NOT NULL,
    [DecisionId]            INT                NOT NULL,
    [RequestTypeId]         INT                NOT NULL,
    [PreSelectedInd]        BIT                NOT NULL,
    [CreatedByUserGuid]     UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]             DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]     UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]             DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Scan.DecisionRequestTypes] PRIMARY KEY CLUSTERED ([DecisionRequestTypeId] ASC),
    CONSTRAINT [FK_Scan.DecisionRequestTypes_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.DecisionRequestTypes_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.DecisionRequestTypes_Scan.Decisions_DecisionId] FOREIGN KEY ([DecisionId]) REFERENCES [Scan].[Decisions] ([DecisionId]),
    CONSTRAINT [FK_Scan.DecisionRequestTypes_Scan.RequestTypes_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [Scan].[RequestTypes] ([RequestTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[DecisionRequestTypes]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[DecisionRequestTypes]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Scan].[DecisionRequestTypes]([RequestTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DecisionId]
    ON [Scan].[DecisionRequestTypes]([DecisionId] ASC);

