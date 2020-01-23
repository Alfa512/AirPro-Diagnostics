
DROP PROCEDURE IF EXISTS Scan.usp_GetValidationRuleDisplayList;
GO

CREATE PROCEDURE Scan.usp_GetValidationRuleDisplayList
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		vr.ValidationRuleId
		,vr.ValidationRuleText
	FROM Scan.ValidationRules vr
	WHERE vr.ValidationRuleActiveInd = 1
	ORDER BY vr.ValidationRuleSortOrder

END
GO