CREATE FUNCTION Common.udf_CommaListToTable (@String varchar(MAX))
RETURNS @Result TABLE(Val VARCHAR(MAX))
AS
BEGIN
 
	DECLARE @x XML 
	SELECT @x = CAST('<A>'+ REPLACE(@String,',','</A><A>')+ '</A>' AS XML)
     
	INSERT INTO @Result            
	SELECT t.value('.', 'varchar(max)') AS inVal
	FROM @x.nodes('/A') AS x(t)

    RETURN

END