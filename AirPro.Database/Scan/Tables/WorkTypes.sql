CREATE TABLE [Scan].[WorkTypes] (
    [WorkTypeId]          INT                IDENTITY (1, 1) NOT NULL,
    [WorkTypeName]        NVARCHAR (MAX)     NULL,
    [WorkTypeSortOrder]   INT                NULL,
    [WorkTypeGroupId]     INT                NOT NULL,
    [WorkTypeActiveInd]   BIT                NOT NULL,
    [WorkTypeDescription] NVARCHAR (MAX)     NULL,
    [CreatedByUserGuid]   UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]           DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]   UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]           DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Scan.WorkTypes] PRIMARY KEY CLUSTERED ([WorkTypeId] ASC),
    CONSTRAINT [FK_Scan.WorkTypes_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.WorkTypes_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.WorkTypes_Scan.WorkTypeGroups_WorkTypeGroupId] FOREIGN KEY ([WorkTypeGroupId]) REFERENCES [Scan].[WorkTypeGroups] ([WorkTypeGroupId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[WorkTypes]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[WorkTypes]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkTypeGroupId]
    ON [Scan].[WorkTypes]([WorkTypeGroupId] ASC);

