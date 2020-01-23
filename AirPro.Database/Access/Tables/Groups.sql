CREATE TABLE [Access].[Groups] (
    [GroupGuid]         UNIQUEIDENTIFIER   CONSTRAINT [DF_Access.Groups_GroupGuid] DEFAULT (newsequentialid()) NOT NULL,
    [Name]              NVARCHAR (MAX)     NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    [Description]       NVARCHAR (MAX)     NULL,
    CONSTRAINT [PK_Access.Groups] PRIMARY KEY CLUSTERED ([GroupGuid] ASC),
    CONSTRAINT [FK_Access.Groups_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Groups_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[Groups]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[Groups]([CreatedByUserGuid] ASC);


GO
CREATE TRIGGER [Access].[trgAccessGroupsArchive] ON [Access].[Groups]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.GroupsArchive
	(
		GroupGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Description
	)
	SELECT
		GroupGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Description
	FROM deleted

END