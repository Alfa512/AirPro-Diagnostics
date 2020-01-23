CREATE TABLE [Access].[GroupsArchive] (
    [ArchiveId]         INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]         DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [Description]       NVARCHAR (MAX)     NULL,
    [GroupGuid]         UNIQUEIDENTIFIER   NOT NULL,
    [Name]              NVARCHAR (MAX)     NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Access.GroupsArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC)
);

