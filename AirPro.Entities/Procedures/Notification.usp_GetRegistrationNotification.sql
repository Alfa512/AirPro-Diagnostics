
CREATE PROCEDURE Notification.usp_GetRegistrationNotification
	@RegistrationShopId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT r.RegistrationId, r.Email, u.DisplayName [CreatedBy], c.DisplayName [ClientName]
	FROM Access.Registrations r
	INNER JOIN Access.Users u ON r.CreatedByUserGuid = u.UserGuid
	INNER JOIN Access.RegistrationShops rs ON r.RegistrationShopId = rs.RegistrationShopId
	LEFT JOIN Access.Users c ON r.ClientUserGuid = c.UserGuid
	WHERE rs.RegistrationShopId = @RegistrationShopId

END
GO