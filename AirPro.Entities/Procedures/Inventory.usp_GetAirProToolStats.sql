
CREATE PROCEDURE [Inventory].[usp_GetAirProToolStats]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		s.Type
		,s.TotalCount
		,s.AssignedCount
		,s.TotalCount - s.AssignedCount [UnAssignedCount]
		,s.ThisMonth [ProducedThisMonth]
		,s.LastMonth [ProducedLastMonth]
	FROM
	(
		SELECT
			t.[Type]
			,COUNT(1) [TotalCount]
			,SUM(a.HasAccess) [AssignedCount]
			,SUM(CASE WHEN DATEPART(MONTH, t.CreatedDt) = DATEPART(MONTH, GETUTCDATE()) THEN 1 ELSE 0 END) [ThisMonth]
			,SUM(CASE WHEN DATEPART(MONTH, t.CreatedDt) = DATEPART(MONTH, GETUTCDATE()) - 1 THEN 1 ELSE 0 END) [LastMonth]
		FROM Inventory.AirProTools t
		OUTER APPLY
		(
			SELECT 1 [HasAccess]
			WHERE EXISTS (SELECT 1 FROM Inventory.AirProToolAccounts act WHERE act.ToolId = t.ToolId)
				OR EXISTS (SELECT 1 FROM Inventory.AirProToolShops shp WHERE shp.ToolId = t.ToolId)
		) a		
		Group BY t.Type
	) s
	ORDER BY s.Type
END
GO