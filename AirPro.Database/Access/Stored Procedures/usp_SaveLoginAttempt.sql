
CREATE PROCEDURE Access.usp_SaveLoginAttempt
	@UserGuid UNIQUEIDENTIFIER
	,@LoginName NVARCHAR(MAX)
	,@UserAgent NVARCHAR(MAX)
	,@UserHostAddress NVARCHAR(MAX)
	,@UserHostName NVARCHAR(MAX)
	,@AccountLockedOut BIT
	,@TwoFactorChallenge BIT
	,@TwoFactorVerified BIT
AS
BEGIN

	SET XACT_ABORT ON;

	BEGIN TRAN
	
		BEGIN TRY

			INSERT INTO Access.Logins
			(
			    UserGuid,
			    LoginName,
			    UserAgent,
			    UserHostAddress,
			    UserHostName,
			    AccountLockedOut,
			    TwoFactorChallenge,
			    TwoFactorVerified,
			    LoginAttemptDt
			)
			VALUES
			(
				@UserGuid
				,@LoginName
				,@UserAgent
				,@UserHostAddress
				,@UserHostName
				,@AccountLockedOut
				,@TwoFactorChallenge
				,@TwoFactorVerified
				,SYSDATETIMEOFFSET()
			)

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END