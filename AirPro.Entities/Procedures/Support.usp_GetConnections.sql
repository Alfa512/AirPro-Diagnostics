
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Support' AND ROUTINE_NAME = 'usp_GetConnections')
	DROP PROCEDURE Support.usp_GetConnections
GO

CREATE PROCEDURE Support.usp_GetConnections
	@ShopGuid UNIQUEIDENTIFIER = NULL
	,@PageUrl nVARCHAR(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		c.ConnectionGuid
		,c.UserGuid
		,c.PageUrl
	FROM Support.Connections c
	WHERE (@ShopGuid IS NULL OR c.UserGuid IN (SELECT UserGuid FROM Access.vwUserMemberships WHERE ShopGuid = @ShopGuid))
		AND (@PageUrl IS NULL OR c.PageUrlHash = CHECKSUM(@PageUrl))

END
GO