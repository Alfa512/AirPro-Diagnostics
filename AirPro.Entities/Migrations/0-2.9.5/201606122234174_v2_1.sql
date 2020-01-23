
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'f_FFInfoes')
BEGIN
	PRINT N'Dropping [Scan].[f_FFInfoes]...';
	DROP FUNCTION [Scan].[f_FFInfoes];
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'f_Infoes')
BEGIN
	PRINT N'Dropping [Scan].[f_Infoes]...';
	DROP FUNCTION [Scan].[f_Infoes];
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'f_FFResults')
BEGIN
	PRINT N'Dropping [Scan].[f_FFResults]...';
	DROP FUNCTION [Scan].[f_FFResults];
END
GO

PRINT N'Creating [Scan].[f_FFResults]...';
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 05/02/2016
-- Description:	Process Scan FF Info.
-- =============================================
CREATE FUNCTION [Scan].[f_FFResults] 
(	
	@Scan XML
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		A.b.query('CONTROLLER_NAME').value('(/CONTROLLER_NAME)[1]', 'NVARCHAR(MAX)') [CONTROLLER_NAME]
		,x.FF_DTC
		,x.FF_SENSOR_ONE
		,x.FF_VALUE_ONE
		,x.FF_UNITS_ONE
		,x.FF_SENSOR_TWO
		,x.FF_VALUE_TWO
		,x.FF_UNITS_TWO
	FROM   @Scan.nodes('/CONTROLLER') A(b)
	OUTER APPLY
	(
		SELECT
			C.d.query('FF_DTC').value('(FF_DTC)[1]', 'NVARCHAR(MAX)') [FF_DTC]
			,y.FF_SENSOR_ONE
			,y.FF_VALUE_ONE
			,y.FF_UNITS_ONE
			,y.FF_SENSOR_TWO
			,y.FF_VALUE_TWO
			,y.FF_UNITS_TWO
		FROM A.b.nodes('FF_INFO') C(d)
		OUTER APPLY
		(
			SELECT
				E.f.query('FF_SENSOR_ONE').value('(FF_SENSOR_ONE)[1]', 'NVARCHAR(MAX)') [FF_SENSOR_ONE]
				,E.f.query('FF_VALUE_ONE').value('(FF_VALUE_ONE)[1]', 'NVARCHAR(MAX)') [FF_VALUE_ONE]
				,E.f.query('FF_UNITS_ONE').value('(FF_UNITS_ONE)[1]', 'NVARCHAR(MAX)') [FF_UNITS_ONE]
				,E.f.query('FF_SENSOR_TWO').value('(FF_SENSOR_TWO)[1]', 'NVARCHAR(MAX)') [FF_SENSOR_TWO]
				,E.f.query('FF_VALUE_TWO').value('(FF_VALUE_TWO)[1]', 'NVARCHAR(MAX)') [FF_VALUE_TWO]
				,E.f.query('FF_UNITS_TWO').value('(FF_UNITS_TWO)[1]', 'NVARCHAR(MAX)') [FF_UNITS_TWO]
			FROM C.d.nodes('FF_SENSORS') E(f)
		) y
	) x
	WHERE x.FF_DTC IS NOT NULL
)
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'f_ScanResult')
BEGIN
	PRINT N'Dropping [Scan].[f_ScanResult]...';
	DROP FUNCTION [Scan].[f_ScanResult];
END
GO

PRINT N'Creating [Scan].[f_ScanResult]...';
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 05/02/2016
-- Description:	Process Scan Info.
-- =============================================
CREATE FUNCTION [Scan].[f_ScanResult] 
(	
	@Scan XML
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		@Scan.value('(/VEHICLE_INFO/VIN)[1]', 'NVARCHAR(MAX)') [VEHICLE_VIN]
		,@Scan.value('(/VEHICLE_INFO/MAKE)[1]', 'NVARCHAR(MAX)') [VEHICLE_MAKE]
		,@Scan.value('(/VEHICLE_INFO/MODEL)[1]', 'NVARCHAR(MAX)') [VEHICLE_MODEL]
		,@Scan.value('(/VEHICLE_INFO/YEAR)[1]', 'NVARCHAR(MAX)') [VEHICLE_YEAR]
		,@Scan.value('(/MIL_STATUS/MIL_STATE)[1]', 'NVARCHAR(MAX)') [MIL_STATE]
		,@Scan.value('(/SHOP_INFO/SHOP_NAME)[1]', 'NVARCHAR(MAX)') [SHOP_NAME]
		,@Scan.value('(/SHOP_INFO/SHOP_ADDRESS)[1]', 'NVARCHAR(MAX)') [SHOP_ADDRESS]
		,@Scan.value('(/SHOP_INFO/SHOP_PHONE)[1]', 'NVARCHAR(MAX)') [SHOP_PHONE]
		,@Scan.value('(/SHOP_INFO/SHOP_FAX)[1]', 'NVARCHAR(MAX)') [SHOP_FAX]
		,@Scan.value('(/SHOP_INFO/SHOP_EMAIL)[1]', 'NVARCHAR(MAX)') [SHOP_EMAIL]
		,@Scan.value('(/SHOP_INFO/DATE_TIME/DATE_TIME)[1]', 'DATETIME') [SCAN_DT]
)
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'usp_ProcessScan')
BEGIN
	PRINT N'Dropping [Scan].[usp_ProcessScan]...';
	DROP PROCEDURE [Scan].[usp_ProcessScan];
END
GO

PRINT N'Altering [Scan].[usp_ProcessScan]...';
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 05/02/2016
-- Description:	Process Scan.
-- =============================================
CREATE PROCEDURE [Scan].[usp_ProcessScan]
	@SCAN_UPLOAD_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ScanXML XML;
	SELECT @ScanXML = u.UPLOAD_XML
	FROM Scan.Uploads u
	WHERE u.SCAN_UPLOAD_ID = @SCAN_UPLOAD_ID

	IF NOT EXISTS (SELECT 1 FROM Scan.Results WHERE SCAN_UPLOAD_ID = @SCAN_UPLOAD_ID)
	BEGIN

		INSERT INTO Scan.Results
		(
			SCAN_UPLOAD_ID
			,VEHICLE_VIN
			,VEHICLE_MAKE
			,VEHICLE_MODEL
			,VEHICLE_YEAR
			,SHOP_NAME
			,SHOP_ADDRESS
			,SHOP_PHONE
			,SHOP_FAX
			,SHOP_EMAIL
			,SCAN_DT
		)
		SELECT
			@SCAN_UPLOAD_ID
			,NULLIF(RTRIM(LTRIM(VEHICLE_VIN)), '')
			,NULLIF(RTRIM(LTRIM(VEHICLE_MAKE)), '')
			,NULLIF(RTRIM(LTRIM(VEHICLE_MODEL)), '')
			,NULLIF(RTRIM(LTRIM(VEHICLE_YEAR)), '')
			,NULLIF(RTRIM(LTRIM(SHOP_NAME)), '')
			,NULLIF(RTRIM(LTRIM(SHOP_ADDRESS)), '')
			,NULLIF(RTRIM(LTRIM(SHOP_PHONE)), '')
			,NULLIF(RTRIM(LTRIM(SHOP_FAX)), '')
			,NULLIF(RTRIM(LTRIM(SHOP_EMAIL)), '')
			,NULLIF(RTRIM(LTRIM(SCAN_DT)), '')
		FROM Scan.f_ScanResult(@ScanXML)

		DECLARE @SCAN_ID INT
		SET @SCAN_ID = SCOPE_IDENTITY()

		INSERT INTO Scan.TestIssues
		(
			SCAN_ID
			,TESTABILITY_ISSUE
		)
		SELECT
			@SCAN_ID
			,TESTABILITY_ISSUE
		FROM Scan.f_TestIssues(@ScanXML)

		INSERT INTO Scan.DTCResults
		(
			SCAN_ID
			,CONTROLLER_NAME
			,DTC_DESCRIPTION
		)
		SELECT
			@SCAN_ID
			,NULLIF(RTRIM(LTRIM(CONTROLLER_NAME)), '')
			,NULLIF(RTRIM(LTRIM(DTC_DESCRIPTION)), '')
		FROM Scan.f_DTCResults(@ScanXML)

		INSERT INTO Scan.FFResults
		(
			SCAN_ID
			,CONTROLLER_NAME
			,FF_DTC
			,FF_SENSOR_ONE
			,FF_VALUE_ONE
			,FF_UNITS_ONE
			,FF_SENSOR_TWO
			,FF_VALUE_TWO
			,FF_UNITS_TWO
		)
		SELECT
			@SCAN_ID
			,NULLIF(RTRIM(LTRIM(CONTROLLER_NAME)), '')
			,NULLIF(RTRIM(LTRIM(FF_DTC)), '')
			,NULLIF(RTRIM(LTRIM(FF_SENSOR_ONE)), '')
			,NULLIF(RTRIM(LTRIM(FF_VALUE_ONE)), '')
			,NULLIF(RTRIM(LTRIM(FF_UNITS_ONE)), '')
			,NULLIF(RTRIM(LTRIM(FF_SENSOR_TWO)), '')
			,NULLIF(RTRIM(LTRIM(FF_VALUE_TWO)), '')
			,NULLIF(RTRIM(LTRIM(FF_UNITS_TWO)), '')
		FROM Scan.f_FFResults(@ScanXML)
	
	END
END
GO