
CREATE PROCEDURE Scan.usp_GetRecommendationSearch
	@SearchPhrase NVARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT TOP 15
		tcr.TroubleCodeRecommendationId
		,tcr.TroubleCodeRecommendationText
	FROM Scan.TroubleCodeRecommendations tcr
	WHERE tcr.ActiveInd = 1
		AND (NULLIF(@SearchPhrase, '') IS NULL
			OR tcr.TroubleCodeRecommendationText LIKE '%' + @SearchPhrase + '%')
	ORDER BY tcr.TroubleCodeRecommendationText

END