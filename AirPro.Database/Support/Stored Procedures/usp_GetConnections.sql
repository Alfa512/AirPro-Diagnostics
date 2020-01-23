
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