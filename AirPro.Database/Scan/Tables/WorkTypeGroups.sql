CREATE TABLE [Scan].[WorkTypeGroups] (
    [WorkTypeGroupId]        INT                IDENTITY (1, 1) NOT NULL,
    [WorkTypeGroupName]      NVARCHAR (MAX)     NULL,
    [WorkTypeGroupSortOrder] INT                NULL,
    [WorkTypeGroupActiveInd] BIT                NOT NULL,
    [CreatedByUserGuid]      UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]              DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]      UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]              DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Scan.WorkTypeGroups] PRIMARY KEY CLUSTERED ([WorkTypeGroupId] ASC),
    CONSTRAINT [FK_Scan.WorkTypeGroups_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.WorkTypeGroups_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[WorkTypeGroups]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[WorkTypeGroups]([CreatedByUserGuid] ASC);

