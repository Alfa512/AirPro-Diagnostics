-- =============================================
-- Author:		Michael Sanders
-- Create date: 07/13/2018
-- Description:	Convert XML to Tabl.
-- =============================================
CREATE FUNCTION Common.udf_XmlToTable 
(	
	@Xml XML
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT 
		t.l.value('local-name(..)', 'VARCHAR(MAX)') [ParentElement]
		,t.l.value('local-name(.)', 'VARCHAR(MAX)') [Element]
		,t.l.value('.', 'VARCHAR(MAX)') [Attribute]
	FROM @Xml.nodes('//*[text()], //@*') AS t(l)
)