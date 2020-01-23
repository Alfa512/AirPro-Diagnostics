
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Diagnostic' AND ROUTINE_NAME = 'usp_GetControllersSearch')
	DROP PROCEDURE Diagnostic.usp_GetControllersSearch;
GO

CREATE PROCEDURE Diagnostic.usp_GetControllersSearch
	@SearchPhrase NVARCHAR(200) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT TOP 15
		c.ControllerId
		,c.ControllerName
	FROM Diagnostic.Controllers c
	WHERE NULLIF(@SearchPhrase, '') IS NULL
		OR c.ControllerName LIKE '%' + @SearchPhrase + '%'
	ORDER BY c.ControllerName

END
GO

EXEC Diagnostic.usp_GetControllersSearch ''