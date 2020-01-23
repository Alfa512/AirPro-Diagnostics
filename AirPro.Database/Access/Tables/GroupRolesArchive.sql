CREATE TABLE [Access].[GroupRolesArchive] (
    [ArchiveId]         INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]         DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [GroupGuid]         UNIQUEIDENTIFIER   NOT NULL,
    [RoleGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Access.GroupRolesArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC)
);

