
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Support' AND ROUTINE_NAME = 'usp_SaveConnection')
	DROP PROCEDURE Support.usp_SaveConnection
GO

CREATE PROCEDURE Support.usp_SaveConnection
	@ConnectionGuid UNIQUEIDENTIFIER
	,@UserGuid UNIQUEIDENTIFIER
	,@PageUrl NVARCHAR(MAX)
	,@ConnectionStartInd BIT
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			IF EXISTS (SELECT 1 FROM Support.Connections WHERE ConnectionGuid = @ConnectionGuid)
				DELETE Support.Connections WHERE @ConnectionStartInd = 0 AND ConnectionGuid = @ConnectionGuid;
			ELSE
				INSERT INTO Support.Connections (ConnectionGuid, UserGuid, PageUrl, ConnectionStartDt)
				SELECT @ConnectionGuid, @UserGuid, @PageUrl, GETUTCDATE()
				WHERE @ConnectionStartInd = 1

		END TRY
		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

END
GO