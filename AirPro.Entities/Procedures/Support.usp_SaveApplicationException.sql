
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Support' AND ROUTINE_NAME ='usp_SaveApplicationException')
	DROP PROCEDURE Support.usp_SaveApplicationException
GO

CREATE PROCEDURE [Support].[usp_SaveApplicationException]
	@ExceptionParentId INT
	,@ExceptionMessage NVARCHAR(MAX)
	,@ExceptionStackTrace NVARCHAR(MAX)
	,@ExceptionObjectInfo NVARCHAR(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRAN

		BEGIN TRY

			INSERT INTO Support.ApplicationExceptions
			(
				ExceptionParentId
				,ExceptionMessage
				,ExceptionStackTrace
				,ExceptionObjectInfo
				,ExceptionOccuredDt
			)
			OUTPUT INSERTED.ExceptionId
			VALUES
			(
				@ExceptionParentId
				,@ExceptionMessage
				,@ExceptionStackTrace
				,@ExceptionObjectInfo
				,GETUTCDATE()
			)

		END TRY
		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

END
GO