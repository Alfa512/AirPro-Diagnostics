CREATE TABLE [Repair].[Feedback] (
    [RepairId]                    INT                NOT NULL,
    [ResponseTimeRate]            INT                NOT NULL,
    [RequestTimeRate]             INT                NOT NULL,
    [TechnicianKnowledgeRate]     INT                NOT NULL,
    [ReportCompletionRate]        INT                NOT NULL,
    [ConcernsAddressedRate]       INT                NOT NULL,
    [TechnicianCommunicationRate] INT                NOT NULL,
    [AdditionalFeedback]          NVARCHAR (512)     NULL,
    [CreatedByUserGuid]           UNIQUEIDENTIFIER   DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [CreatedDt]                   DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NOT NULL,
    [UpdatedByUserGuid]           UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                   DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Repair.Feedback] PRIMARY KEY CLUSTERED ([RepairId] ASC),
    CONSTRAINT [FK_Repair.Feedback_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Repair.Feedback_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Repair.Feedback_Repair.Orders_RepairId] FOREIGN KEY ([RepairId]) REFERENCES [Repair].[Orders] ([OrderId])
);




GO
CREATE NONCLUSTERED INDEX [IX_RepairId]
    ON [Repair].[Feedback]([RepairId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Repair].[Feedback]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Repair].[Feedback]([CreatedByUserGuid] ASC);

