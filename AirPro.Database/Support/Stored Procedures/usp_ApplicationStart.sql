


CREATE PROCEDURE [Support].[usp_ApplicationStart]
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			DELETE FROM Support.Connections;

			UPDATE Support.ConnectionLogs
				SET ConnectionEndDt = GETUTCDATE()
			WHERE ConnectionEndDt IS NULL;

			UPDATE Access.Users
				SET SessionId = NULL
			WHERE SessionId IS NOT NULL

		END TRY
		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

END