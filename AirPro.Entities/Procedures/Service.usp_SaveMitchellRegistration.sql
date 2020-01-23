
DROP PROCEDURE IF EXISTS Service.usp_SaveMitchellRegistration;
GO

CREATE PROCEDURE Service.usp_SaveMitchellRegistration
	@MitchellAccountId VARCHAR(MAX)
	,@CallbackUrl VARCHAR(MAX)
	,@UserEmail VARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN Reg

		BEGIN TRY

			/**********************************************************
				Step 1: Validate Email Address.
			**********************************************************/
			IF NOT EXISTS (SELECT 1 FROM Service.MitchellRegistrations mr WHERE mr.UserEmail = @UserEmail AND mr.MitchellAccountId = @MitchellAccountId)
				AND (EXISTS (SELECT 1 FROM Access.Users u WHERE u.Email = @UserEmail)
					OR EXISTS (SELECT 1 FROM Access.Registrations r WHERE r.Email = @UserEmail))
						THROW 50000, 'SaveMitchellRegistration: Email Address Already in Use.', 1

			/**********************************************************
				Step 2: Create Mitchell Registration.
			**********************************************************/
			DECLARE @MitchellRegistrations TABLE (MitchellRegistrationId INT)
			MERGE Service.MitchellRegistrations AS t
			USING
			(
				SELECT
					@MitchellAccountId [MitchellAccountId]
					,@CallbackUrl [CallbackUrl]
					,@UserEmail [UserEmail]
				WHERE @MitchellAccountId IS NOT NULL
					AND NOT EXISTS (SELECT 1 FROM Service.MitchellRegistrations WHERE MitchellAccountId = @MitchellAccountId)
			) AS s
			ON (t.MitchellAccountId = s.MitchellAccountId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (MitchellAccountId, CallbackUrl, UserEmail)
				VALUES (MitchellAccountId, CallbackUrl, UserEmail)
			OUTPUT INSERTED.MitchellRegistrationId
			INTO @MitchellRegistrations;

			/**********************************************************
				Step 3: Create Registration.
			**********************************************************/
			CREATE TABLE #Registrations (RegistrationId UNIQUEIDENTIFIER)
			IF EXISTS (SELECT 1 FROM @MitchellRegistrations)
			BEGIN
				DECLARE @EmptyGuid UNIQUEIDENTIFIER = Common.udf_GetEmptyGuid();
				INSERT INTO #Registrations
				EXEC Access.usp_SaveRegistration @RegistrationId = @EmptyGuid, @Email = @UserEmail, @CallbackUrl = @CallbackUrl, @SendToMitchellInd = 1, @UserGuid = @EmptyGuid
			END

			/**********************************************************
				Step 4: Load Registration Status.
			**********************************************************/
			SELECT
				mr.MitchellAccountId
				,ISNULL(r.ShopGuid, Common.udf_GetEmptyGuid()) [ShopGuid]
				,r.RegistrationStatus [Status]
			FROM Service.MitchellRegistrations mr
			LEFT JOIN Access.Registrations r
				ON mr.UserEmail = r.Email
			WHERE mr.MitchellAccountId = @MitchellAccountId

			/**********************************************************
				Step 5: Load Registrations for Notifications.
			**********************************************************/
			SELECT r.RegistrationShopId
			FROM Access.Registrations r
			INNER JOIN #Registrations nr
				ON r.RegistrationId = nr.RegistrationId

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN Reg;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN Reg;

END
GO