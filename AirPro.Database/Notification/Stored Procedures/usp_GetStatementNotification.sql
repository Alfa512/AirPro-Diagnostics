
CREATE PROCEDURE Notification.usp_GetStatementNotification
	@PaymentId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		p.PaymentId,
		s.ShopGuid,
		s.Name ShopName,
		p.PaymentAmount,
		p.PaymentReferenceNumber,
		p.PaymentMemo,
		pt.PaymentTypeName PaymentType,
		c.Name PaymentCurrency,
		p.DiscountPercentage
	FROM Billing.Payments p
	INNER JOIN Access.Shops s ON s.ShopGuid = p.PaymentReceivedFromShopGuid
	INNER JOIN Billing.Currencies c ON c.CurrencyId = p.CurrencyId
	INNER JOIN Billing.PaymentTypes pt ON pt.PaymentTypeId = p.PaymentTypeId

	WHERE p.PaymentId = @PaymentId

END