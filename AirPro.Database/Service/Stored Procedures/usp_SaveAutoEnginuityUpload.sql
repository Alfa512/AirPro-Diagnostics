
CREATE PROCEDURE Service.usp_SaveAutoEnginuityUpload
	@RequestQuery NVARCHAR(MAX)
	,@RequestBody NVARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRAN

		BEGIN TRY

			INSERT INTO Service.AutoEnginuityUploads (RequestQuery, RequestBody)
			VALUES (@RequestQuery, @RequestBody)

		END TRY
		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

END