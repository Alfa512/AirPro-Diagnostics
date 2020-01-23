CREATE TABLE [Scan].[Decisions] (
    [DecisionId]          INT                IDENTITY (1, 1) NOT NULL,
    [DecisionText]        NVARCHAR (MAX)     NULL,
    [ActiveInd]           BIT                NOT NULL,
    [CreatedByUserGuid]   UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]           DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]   UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]           DATETIMEOFFSET (7) NULL,
    [DefaultTextSeverity] INT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Scan.Decisions] PRIMARY KEY CLUSTERED ([DecisionId] ASC),
    CONSTRAINT [FK_Scan.Decisions_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.Decisions_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[Decisions]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[Decisions]([CreatedByUserGuid] ASC);

