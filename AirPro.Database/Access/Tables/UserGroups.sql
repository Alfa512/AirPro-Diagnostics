CREATE TABLE [Access].[UserGroups] (
    [UserGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [GroupGuid]         UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Access.UserGroups] PRIMARY KEY CLUSTERED ([UserGuid] ASC, [GroupGuid] ASC),
    CONSTRAINT [FK_Access.UserGroups_Access.Groups_GroupGuid] FOREIGN KEY ([GroupGuid]) REFERENCES [Access].[Groups] ([GroupGuid]),
    CONSTRAINT [FK_Access.UserGroups_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.UserGroups_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.UserGroups_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[UserGroups]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[UserGroups]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_GroupGuid]
    ON [Access].[UserGroups]([GroupGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Access].[UserGroups]([UserGuid] ASC);


GO
CREATE TRIGGER [Access].[trgAccessUserGroupsArchive] ON [Access].[UserGroups]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.UserGroupsArchive
	(
		UserGuid
		,GroupGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		UserGuid
		,GroupGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM deleted

END