
DROP PROCEDURE IF EXISTS Scan.usp_GetRequestTypeDisplayList;
GO

CREATE PROCEDURE Scan.usp_GetRequestTypeDisplayList
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		rt.RequestTypeId
		,rt.TypeName [RequestTypeName]
	FROM Scan.RequestTypes rt
	WHERE rt.ActiveFlag = 1
		AND rt.RequestTypeId NOT IN (6, 7) -- Exclude Self Scan & Scan Analysis.
	ORDER BY rt.SortOrder

END
GO