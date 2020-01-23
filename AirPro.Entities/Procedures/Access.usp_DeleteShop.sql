
CREATE PROCEDURE Access.usp_DeleteShop
	@ShopGuid UNIQUEIDENTIFIER
	,@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			-- Validate User Role.
			IF NOT EXISTS (SELECT 1 FROM Access.UserRoles WHERE UserGuid = @UserGuid AND RoleGuid = '038CE263-B64B-403D-93A6-ADC96A18E59F')
				THROW 50000, 'Access Denied: User Role Missing.', 1;

			-- Update Shop.
			UPDATE s
				SET s.ActiveInd = 0
			FROM Access.Shops s
			LEFT JOIN
			(
				SELECT ShopGuid
				FROM Repair.Orders
				WHERE Status IN (1, 3, 4)
				GROUP BY ShopGuid
			) a ON s.ShopGuid = a.ShopGuid
			WHERE s.ShopGuid = @ShopGuid
				AND a.ShopGuid IS NULL

			-- Check Result.
			IF @@ROWCOUNT = 0 THROW 50000, 'Error: Unable to Delete Shop.', 1;

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END
GO