
DROP PROCEDURE IF EXISTS Diagnostic.usp_GetDiagnosticQueueByShop;
GO

CREATE PROCEDURE Diagnostic.usp_GetDiagnosticQueueByShop
	@ShopGuid UNIQUEIDENTIFIER
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZone NVARCHAR(MAX) = Common.udf_GetUserTimeZoneId(@UserGuid);

	WITH Tools
	AS
	(
		SELECT apt.ToolKey
		FROM Inventory.AirProTools apt
		WHERE apt.ToolId IN
		(
			SELECT apta.ToolId
			FROM Inventory.AirProToolAccounts apta
			INNER JOIN Access.Shops s
				ON apta.AccountGuid = s.AccountGuid
			WHERE s.ShopGuid = @ShopGuid

			UNION

			SELECT apts.ToolId
			FROM Inventory.AirProToolShops apts
			WHERE apts.ShopGuid = @ShopGuid
		)
	)

	SELECT
		uq.ResultId
		,uq.VehicleVin
		,uq.VehicleMake
		,uq.VehicleModel
		,uq.VehicleYear
		,CAST(uq.ScanDateTime AT TIME ZONE @UserTimeZone AS DATETIME) [ScanDateTime]
	FROM Diagnostic.vwUploadQueue uq
	LEFT JOIN Tools t
		ON uq.SearchGuid = t.ToolKey
	WHERE (uq.SearchGuid = @ShopGuid
			OR t.ToolKey IS NOT NULL)
	ORDER BY uq.ResultId DESC

END
GO
