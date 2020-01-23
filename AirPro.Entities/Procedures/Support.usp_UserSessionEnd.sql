﻿
CREATE PROCEDURE Support.usp_UserSessionEnd
	@SessionId NVARCHAR(24)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			UPDATE Access.Users
				SET SessionId = NULL
			WHERE SessionId = @SessionId

		END TRY
		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

END
GO