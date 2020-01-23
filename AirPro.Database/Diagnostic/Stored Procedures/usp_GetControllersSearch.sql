
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