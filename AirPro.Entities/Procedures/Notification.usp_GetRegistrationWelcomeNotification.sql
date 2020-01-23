
CREATE PROCEDURE Notification.usp_GetRegistrationWelcomeNotification
	@RegistrationShopId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT r.RegistrationId, r.Email, u.DisplayName [CreatedBy], c.DisplayName [ClientName], ru.FirstName, ru.LastName, ru.JobTitle, ra.Name [AccountName], rs.Name [ShopName] 
	FROM Access.Registrations r
	INNER JOIN Access.Users u ON r.CreatedByUserGuid = u.UserGuid
	INNER JOIN Access.RegistrationShops rs ON r.RegistrationShopId = rs.RegistrationShopId
	INNER JOIN Access.RegistrationAccounts ra ON r.RegistrationAccountId = ra.RegistrationAccountId
	INNER JOIN Access.RegistrationUsers ru ON r.RegistrationUserId = ru.RegistrationUserId
	LEFT JOIN Access.Users c ON r.ClientUserGuid = c.UserGuid
	WHERE rs.RegistrationShopId = @RegistrationShopId

END
GO