
CREATE PROCEDURE Diagnostic.usp_GetTroubleCodesSearch
	@SearchPhrase NVARCHAR(200) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT TOP 15
		tc.TroubleCodeId
		,tc.TroubleCode
		,tc.TroubleCodeDescription
	FROM Diagnostic.TroubleCodes tc
	WHERE NULLIF(@SearchPhrase, '') IS NULL
		OR tc.TroubleCode LIKE '%' + @SearchPhrase + '%'
		OR tc.TroubleCodeDescription LIKE '%' + @SearchPhrase + '%'
	ORDER BY
		tc.TroubleCode
		,tc.TroubleCodeDescription

END