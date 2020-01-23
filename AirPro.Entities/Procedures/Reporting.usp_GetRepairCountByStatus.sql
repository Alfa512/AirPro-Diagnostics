
CREATE PROCEDURE Reporting.usp_GetRepairCountByStatus
	@UserGuid UNIQUEIDENTIFIER,
	@ShopGuid UNIQUEIDENTIFIER = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		o.Status
		,COUNT(1) [StatusCount]
	FROM Repair.Orders o
	WHERE (@ShopGuid IS NULL OR o.ShopGuid = @ShopGuid)
		AND o.Status IN (1, 3, 4)
			OR (o.Status IN (2, 5) AND o.UpdatedDt > CAST(GETUTCDATE() AS DATE))
	GROUP BY o.Status

END
GO