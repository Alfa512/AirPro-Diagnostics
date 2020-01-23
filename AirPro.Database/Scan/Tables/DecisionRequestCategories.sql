CREATE TABLE [Scan].[DecisionRequestCategories] (
    [DecisionRequestCategoryId] INT                IDENTITY (1, 1) NOT NULL,
    [DecisionId]                INT                NOT NULL,
    [RequestCategoryId]         INT                NOT NULL,
    [PreSelectedInd]            BIT                NOT NULL,
    [CreatedByUserGuid]         UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]                 DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]         UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                 DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Scan.DecisionRequestCategories] PRIMARY KEY CLUSTERED ([DecisionRequestCategoryId] ASC),
    CONSTRAINT [FK_Scan.DecisionRequestCategories_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.DecisionRequestCategories_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.DecisionRequestCategories_Scan.Decisions_DecisionId] FOREIGN KEY ([DecisionId]) REFERENCES [Scan].[Decisions] ([DecisionId]),
    CONSTRAINT [FK_Scan.DecisionRequestCategories_Scan.RequestCategories_RequestCategoryId] FOREIGN KEY ([RequestCategoryId]) REFERENCES [Scan].[RequestCategories] ([RequestCategoryId])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[DecisionRequestCategories]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[DecisionRequestCategories]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestCategoryId]
    ON [Scan].[DecisionRequestCategories]([RequestCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DecisionId]
    ON [Scan].[DecisionRequestCategories]([DecisionId] ASC);

