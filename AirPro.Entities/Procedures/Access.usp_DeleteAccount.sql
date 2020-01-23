
CREATE PROCEDURE Access.usp_DeleteAccount
	@AccountGuid UNIQUEIDENTIFIER
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			-- Validate User Role.
			IF NOT EXISTS (SELECT 1 FROM Access.UserRoles WHERE UserGuid = @UserGuid AND RoleGuid = '4E7AB500-06A1-49AB-922A-4715A58298CD')
				THROW 50000, 'Access Denied: User Role Missing.', 1;

			-- Validate Shops in Account.
			IF EXISTS (SELECT 1 FROM Access.Shops s WHERE s.AccountGuid = @AccountGuid
						AND s.ShopGuid IN (SELECT ShopGuid FROM Repair.Orders WHERE Status IN (1, 3, 4) GROUP BY ShopGuid))
				THROW 50000, 'Error: Unable to Delete Account.', 1; 

			-- Delete Shops.
			UPDATE s
				SET s.ActiveInd = 0
			FROM Access.Shops s
			WHERE s.AccountGuid = @AccountGuid

			-- Delete Account.
			UPDATE a
				SET a.ActiveInd = 0
			FROM Access.Accounts a
			WHERE a.AccountGuid = @AccountGuid

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

	RETURN 1;

END
GO