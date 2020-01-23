

CREATE FUNCTION Common.udf_GetDisplayName
(
	@LastName VARCHAR(MAX)
	,@FirstName VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN

	DECLARE @Result VARCHAR(MAX)

	SELECT @Result = @LastName + ', ' + @FirstName

	RETURN @Result

END
GO