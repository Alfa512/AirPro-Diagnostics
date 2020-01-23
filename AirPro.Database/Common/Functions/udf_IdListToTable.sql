
CREATE FUNCTION Common.udf_IdListToTable (@String varchar(4000))
RETURNS @Result TABLE(Id BIGINT)
AS
BEGIN
 
	DECLARE @x XML 
	SELECT @x = CAST('<A>'+ REPLACE(@String,',','</A><A>')+ '</A>' AS XML)
     
	INSERT INTO @Result            
	SELECT t.value('.', 'int') AS inVal
	FROM @x.nodes('/A') AS x(t)
	WHERE t.value('.', 'int') > 0

    RETURN

END