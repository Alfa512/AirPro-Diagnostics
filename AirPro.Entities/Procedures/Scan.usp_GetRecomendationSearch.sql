DECLARE @UserGuid UNIQUEIDENTIFIER, @UserTimeZone VARCHAR(100);
SELECT TOP 1 @UserGuid = UserGuid, @UserTimeZone = TimeZoneInfoId
FROM Access.Users WHERE Email IN ('sandersmw@unimatrixdesigns.com', 'dev@umd.tech');

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetRecommendationSearch')
	DROP PROCEDURE Scan.usp_GetRecommendationSearch
GO

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
GO


EXEC Scan.usp_GetRecommendationSearch '';
GO
