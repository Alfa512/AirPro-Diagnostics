CREATE PROCEDURE [Billing].[usp_VoidPayment]
	@PaymentId INT,
	@UserEmail VARCHAR(500)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRY

		IF (@PaymentId IS NULL)
			THROW 51000, 'Payment ID is Empty.', 1;

		DECLARE @UserGuid UNIQUEIDENTIFIER
		SELECT @UserGuid = u.UserGuid
		FROM Access.Users u
		WHERE u.Email = @UserEmail

		IF (@UserEmail IS NULL OR @UserGuid IS NULL)
			THROW 51000, 'User Email Empty or Not Found.', 1;

		BEGIN TRAN

			UPDATE p
				SET p.PaymentVoidInd = 1
					,p.PaymentVoidByUserGuid = @UserGuid
					,p.PaymentVoidDt = GETUTCDATE()
			FROM Billing.Payments p
			WHERE p.PaymentID = @PaymentId
				AND p.PaymentVoidInd = 0

			UPDATE pt
				SET pt.PaymentTransactionVoidInd = 1
					,pt.PaymentTransactionVoidByUserGuid = @UserGuid
					,pt.PaymentTransactionVoidDt = GETUTCDATE()
			FROM Billing.PaymentTransactions pt
			WHERE pt.PaymentId = @PaymentId
				AND pt.PaymentTransactionVoidInd = 0

		COMMIT TRAN

		SELECT
			p.PaymentID
			,s.Name [ShopName]
			,pt.PaymentTypeName [PaymentType]
			,p.PaymentAmount
			,p.PaymentReferenceNumber
			,p.PaymentMemo
			,cu.LastName + ', ' + cu.FirstName [PaymentCreatedBy]
			,CAST(p.CreatedDt AS DATETIME) [PaymentCreatedDt]
			,p.PaymentVoidInd
			,vu.LastName + ', ' + vu.FirstName [PaymentVoidedBy]
			,CAST(p.PaymentVoidDt AS DATETIME) [PaymentVoidDt]
		FROM Billing.Payments p
		INNER JOIN Billing.PaymentTypes pt
			ON p.PaymentTypeId = pt.PaymentTypeId
		INNER JOIN Access.Shops s
			ON p.PaymentReceivedFromShopGuid = s.ShopGuid
		INNER JOIN Access.Users cu
			ON p.CreatedByUserGuid = cu.UserGuid
		LEFT JOIN Access.Users vu
			ON p.PaymentVoidByUserGuid = vu.UserGuid
		WHERE p.PaymentID = @PaymentId

		SELECT
			pt.PaymentId
			,pt.PaymentTransactionId [TransactionId]
			,pt.InvoiceId
			,pt.InvoiceAmountApplied
			,cu.LastName + ', ' + cu.FirstName [TransactionCreatedBy]
			,CAST(pt.CreatedDt AS DATETIME) [TransactionCreatedDt]
			,pt.PaymentTransactionVoidInd [TransactionVoidInd]
			,vu.LastName + ', ' + vu.FirstName [TransactionVoidedBy]
			,CAST(pt.PaymentTransactionVoidDt AS DATETIME) [TransactionVoidDt]
		FROM Billing.PaymentTransactions pt
		INNER JOIN Access.Users cu
			ON pt.CreatedByUserGuid = cu.UserGuid
		LEFT JOIN Access.Users vu
			ON pt.PaymentTransactionVoidByUserGuid = vu.UserGuid
		WHERE pt.PaymentId = @PaymentId

	END TRY
	BEGIN CATCH

		IF (@@TRANCOUNT > 0)
			ROLLBACK TRAN;

		THROW;

	END CATCH

END