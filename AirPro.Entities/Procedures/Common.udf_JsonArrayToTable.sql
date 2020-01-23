

CREATE FUNCTION [Common].[udf_JsonArrayToTable] (@String VARCHAR(MAX))
RETURNS @Result TABLE(Val VARCHAR(MAX))
AS
BEGIN
 
	INSERT INTO @Result            
	SELECT Val
	FROM OPENJSON(@String)
		WITH
		(
			Val VARCHAR(MAX) '$'
		)

    RETURN

END
GO