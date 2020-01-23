
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
