
CREATE PROCEDURE Scan.usp_GetWorkTypeGroups
	@Search VARCHAR(MAX) = NULL
	,@WorkTypeGroupId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SET @Search = '%' + ISNULL(@Search, '') + '%';
	SET @WorkTypeGroupId = NULLIF(@WorkTypeGroupId, 0);

	SELECT
		wtg.WorkTypeGroupId
		,wtg.WorkTypeGroupName
		,wtg.WorkTypeGroupSortOrder
		,wtg.WorkTypeGroupActiveInd
		,ISNULL(COUNT(wt.WorkTypeId), 0) [WorkTypesAssigned]
	FROM Scan.WorkTypeGroups wtg
	LEFT JOIN Scan.WorkTypes wt
		ON wtg.WorkTypeGroupId = wt.WorkTypeGroupId
	WHERE (@WorkTypeGroupId IS NOT NULL AND wtg.WorkTypeGroupId = @WorkTypeGroupId)
		OR
		(
			@WorkTypeGroupId IS NULL
			AND
			(
				wt.WorkTypeName LIKE @Search
				OR wt.WorkTypeDescription LIKE @Search
				OR wtg.WorkTypeGroupName LIKE @Search
			)
		)
	GROUP BY
		wtg.WorkTypeGroupId
		,wtg.WorkTypeGroupName
		,wtg.WorkTypeGroupSortOrder
		,wtg.WorkTypeGroupActiveInd

END