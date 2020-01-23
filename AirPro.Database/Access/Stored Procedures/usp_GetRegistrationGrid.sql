CREATE PROCEDURE [Access].[usp_GetRegistrationGrid]
	 @UserGuid UNIQUEIDENTIFIER
	,@Search VARCHAR(MAX)
	,@Status int
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @UserTimeZone NVARCHAR(MAX)
	SELECT @UserTimeZone = u.TimeZoneInfoId
	FROM Access.Users u
	WHERE u.UserGuid = @UserGuid 

	SET @Search = '%' + NULLIF(RTRIM(@Search), '') + '%';

WITH RegistrationGrid
	AS
	(
		SELECT
		 r.RegistrationId [RegistrationId]
        ,r.RegistrationStatus [RegistrationStatus]    
	    ,r.Email [Email]
	    ,r.CreatedByUserGuid [CreatedByUserGuid]
	    ,CAST(r.CreatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [CreatedDt]
	    ,CAST(r.UpdatedDt AT TIME ZONE @UserTimeZone AS DATETIME) [UpdatedDt]
	    ,u.DisplayName [CreatedBy]

		FROM Access.Registrations r
		INNER JOIN Access.Users u
				ON r.CreatedByUserGuid = u.UserGuid
				
	
	)

	SELECT
		rg.RegistrationId
		,rg.RegistrationStatus
		,rg.Email
		,rg.CreatedByUserGuid
		,rg.CreatedDt
		,rg.UpdatedDt
		,rg.CreatedBy
	
	FROM RegistrationGrid rg
	WHERE  rg.RegistrationStatus = (CASE WHEN @Status = -1 THEN  rg.RegistrationStatus ELSE @Status END)
	AND (@Search IS NULL
		OR
		(
			rg.RegistrationStatus = TRY_CAST(REPLACE(@Search, '%', '') AS INT)
			OR rg.Email LIKE @Search
			OR rg.CreatedBy LIKE @Search
		))
END