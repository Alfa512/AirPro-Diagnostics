
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetWorkTypeDisplayList')
	DROP PROCEDURE Scan.usp_GetWorkTypeDisplayList
GO

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
GO