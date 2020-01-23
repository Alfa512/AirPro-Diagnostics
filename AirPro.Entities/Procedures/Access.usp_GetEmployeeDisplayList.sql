DROP PROCEDURE IF EXISTS Access.usp_GetEmployeeDisplayList;
GO

CREATE PROCEDURE Access.usp_GetEmployeeDisplayList
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		UserGuid,
		DisplayName
	FROM Access.Users
	WHERE EmployeeInd = 1
END
GO