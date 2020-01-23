
CREATE PROCEDURE Service.usp_GetMitchellRegistration
	@RegistrationId UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	SELECT TOP 1
		mr.MitchellAccountId
		,r.ShopGuid
		,r.RegistrationStatus [Status]
	FROM Service.MitchellRegistrations mr
	INNER JOIN Access.Registrations r
		ON mr.UserEmail = r.Email
	WHERE r.RegistrationId = @RegistrationId

END