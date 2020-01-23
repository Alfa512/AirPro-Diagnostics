
CREATE PROCEDURE Scan.usp_SaveTroubleCodeRecommendation
	@UserGuid UNIQUEIDENTIFIER
	,@TroubleCodeRecommendationId INT
	,@TroubleCodeRecommendationText NVARCHAR(MAX)
	,@ActiveInd BIT
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			IF EXISTS (SELECT 1 FROM Scan.TroubleCodeRecommendations WHERE @TroubleCodeRecommendationId = 0 AND TroubleCodeRecommendationHash = CHECKSUM(@TroubleCodeRecommendationText))
				THROW 50000, 'Error: Duplicate Recommendation.', 1;

			MERGE Scan.TroubleCodeRecommendations AS t
			USING
			(
				SELECT
					@TroubleCodeRecommendationId [TroubleCodeRecommendationId]
					,@TroubleCodeRecommendationText [TroubleCodeRecommendationText]
					,@ActiveInd [ActiveInd]
			) AS s
			ON (t.TroubleCodeRecommendationId = s.TroubleCodeRecommendationId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (TroubleCodeRecommendationText, ActiveInd, CreatedByUserGuid, CreatedDt)
				VALUES (TroubleCodeRecommendationText, ActiveInd, @UserGuid, GETUTCDATE())
			WHEN MATCHED THEN
				UPDATE
					SET t.TroubleCodeRecommendationText = s.TroubleCodeRecommendationText
						,t.ActiveInd = s.ActiveInd
						,t.UpdatedByUserGuid = @UserGuid
						,t.UpdatedDt = GETUTCDATE()
			OUTPUT INSERTED.*;

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END