
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