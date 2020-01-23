-- =============================================
-- Author:		Michael Sanders
-- Create date: 06/14/2018
-- Description:	Return Empty Guid
-- =============================================
CREATE FUNCTION Common.udf_GetEmptyGuid ()
RETURNS UNIQUEIDENTIFIER
AS
BEGIN

	RETURN CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)

END