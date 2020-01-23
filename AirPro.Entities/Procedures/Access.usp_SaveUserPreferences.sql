

CREATE PROCEDURE [Access].[usp_SaveUserPreferences]
	@UserGuid UNIQUEIDENTIFIER
	,@ControlId VARCHAR(128)
	,@SettingsJson VARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	MERGE Access.UserPreferences AS t
	USING (SELECT @UserGuid, @ControlId, @SettingsJson) AS s
		(UserGuid, ControlId, SettingsJson)
	ON (t.UserGuid = s.UserGuid AND t.ControlId = s.ControlId)
	WHEN MATCHED THEN
		UPDATE SET SettingsJson = s.SettingsJson, UpdatedDt = GETUTCDATE()
	WHEN NOT MATCHED THEN
		INSERT (UserGuid, ControlId, SettingsJson, UpdatedDt)
		VALUES (s.UserGuid, s.ControlId, s.SettingsJson, GETUTCDATE())
		OUTPUT inserted.*;

END
GO