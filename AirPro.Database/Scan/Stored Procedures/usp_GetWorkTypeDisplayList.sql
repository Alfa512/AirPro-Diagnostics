
CREATE PROCEDURE Scan.usp_GetWorkTypeDisplayList
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		wt.WorkTypeId
		,wt.WorkTypeName
	FROM Scan.WorkTypes wt
	WHERE wt.WorkTypeActiveInd = 1
	ORDER BY wt.WorkTypeSortOrder

END