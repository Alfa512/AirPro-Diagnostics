CREATE TABLE [Support].[Connections] (
    [ConnectionGuid]    UNIQUEIDENTIFIER   NOT NULL,
    [UserGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [PageUrl]           NVARCHAR (MAX)     NULL,
    [ConnectionStartDt] DATETIMEOFFSET (7) NOT NULL,
    [PageUrlHash]       AS                 (checksum([PageUrl])),
    CONSTRAINT [PK_Support.Connections] PRIMARY KEY CLUSTERED ([ConnectionGuid] ASC),
    CONSTRAINT [FK_Support.Connections_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);




GO



GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Support].[Connections]([UserGuid] ASC);


GO

CREATE TRIGGER Support.tgrConnectionLog
   ON  Support.Connections
   AFTER DELETE
AS 
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    MERGE Support.ConnectionLogs AS t
	USING DELETED AS s
	ON (t.ConnectionGuid = s.ConnectionGuid)
	WHEN NOT MATCHED THEN
		INSERT (ConnectionGuid, UserGuid, PageUrl, ConnectionStartDt, ConnectionEndDt)
		VALUES (ConnectionGuid, UserGuid, PageUrl, ConnectionStartDt, GETUTCDATE())
	WHEN MATCHED THEN
		UPDATE SET ConnectionEndDt = GETUTCDATE()
	OUTPUT INSERTED.*;

END
GO
CREATE NONCLUSTERED INDEX [IDX_SupportConnections_PageUrlHash]
    ON [Support].[Connections]([PageUrlHash] ASC);

