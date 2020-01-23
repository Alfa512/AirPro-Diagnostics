DECLARE @UserGuid UNIQUEIDENTIFIER, @UserTimeZone VARCHAR(100);
SELECT TOP 1 @UserGuid = UserGuid, @UserTimeZone = TimeZoneInfoId
FROM Access.Users WHERE Email IN ('sandersmw@unimatrixdesigns.com', 'dev@umd.tech');

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Diagnostic' AND ROUTINE_NAME = 'usp_GetTroubleCodesSearch')
	DROP PROCEDURE Diagnostic.usp_GetTroubleCodesSearch
GO

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
GO

EXEC Diagnostic.usp_GetTroubleCodesSearch '';