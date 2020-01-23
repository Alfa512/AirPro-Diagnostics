
CREATE PROCEDURE Service.usp_UpdateMitchellRegistration
	@MitchellAccountId VARCHAR(MAX)
	,@UserEmail VARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE mr
		SET mr.CallbackPerformedDt = GETUTCDATE()
	FROM Service.MitchellRegistrations mr
	WHERE mr.MitchellAccountId = @MitchellAccountId
		AND mr.UserEmail = @UserEmail

END