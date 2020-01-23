
CREATE FUNCTION Common.udf_GetUserTimeZoneId 
(
	@UserGuid UNIQUEIDENTIFIER
)
RETURNS VARCHAR(50)
AS
BEGIN

	DECLARE @Result VARCHAR(50)

	SELECT TOP 1 @Result = u.TimeZoneInfoId
	FROM Access.Users u WITH (NOLOCK)
	WHERE u.UserGuid = @UserGuid

	RETURN @Result

END