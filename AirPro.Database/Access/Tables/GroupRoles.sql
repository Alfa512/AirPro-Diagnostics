CREATE TABLE [Access].[GroupRoles] (
    [GroupGuid]         UNIQUEIDENTIFIER   NOT NULL,
    [RoleGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Access.GroupRoles] PRIMARY KEY CLUSTERED ([GroupGuid] ASC, [RoleGuid] ASC),
    CONSTRAINT [FK_Access.GroupRoles_Access.Groups_GroupGuid] FOREIGN KEY ([GroupGuid]) REFERENCES [Access].[Groups] ([GroupGuid]),
    CONSTRAINT [FK_Access.GroupRoles_Access.Roles_RoleGuid] FOREIGN KEY ([RoleGuid]) REFERENCES [Access].[Roles] ([RoleGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Access.GroupRoles_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.GroupRoles_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[GroupRoles]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[GroupRoles]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleGuid]
    ON [Access].[GroupRoles]([RoleGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_GroupGuid]
    ON [Access].[GroupRoles]([GroupGuid] ASC);


GO
CREATE TRIGGER [Access].[trgAccessGroupRolesArchive] ON [Access].[GroupRoles]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.GroupRolesArchive
	(
		GroupGuid
		,RoleGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	)
	SELECT
		GroupGuid
		,RoleGuid
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
	FROM deleted

END