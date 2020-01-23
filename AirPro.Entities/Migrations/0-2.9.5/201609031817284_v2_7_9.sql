IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'usp_ModifyAdminRights')
	DROP PROCEDURE [Access].[usp_ModifyAdminRights]
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 06/19/2016
-- Description:	Allow Management of Admin Rights.
-- =============================================
CREATE PROCEDURE [Access].[usp_ModifyAdminRights] 
	@Email VARCHAR(256),
	@GrantAdmin BIT = 0
AS
BEGIN

	SET NOCOUNT ON;

	-- Load User Id.
	DECLARE @UserId NVARCHAR(128)
	SELECT @UserId = u.UserId
	FROM Access.Users u
	WHERE u.Email = @Email
	
	IF NULLIF(@UserId, '') IS NULL
		BEGIN
			RAISERROR (N'UserID Not Found.', 10, 1);
			RETURN 0;
		END
	ELSE
		BEGIN
			PRINT 'UserId Located: ' + @UserId
		END

	-- Check Admin Access.
	IF NOT EXISTS (SELECT 1 FROM Access.UserRoles ur WHERE ur.UserId = @UserId AND ur.RoleId = 1)
	BEGIN
		
		IF @GrantAdmin = 1
			BEGIN
				PRINT 'Granting Admin Access.'
				INSERT INTO Access.UserRoles (UserId, RoleId)
				VALUES (@UserId, 1)
			END
		ELSE
			BEGIN
				PRINT 'No Admin Access Found to Remove.'
			END

	END
	ELSE -- Existing Admin Access
	BEGIN

		IF @GrantAdmin = 0
			BEGIN
				PRINT 'Removing Admin Access.'
				DELETE FROM Access.UserRoles
				WHERE UserId = @UserId
					AND RoleId = 1
			END
		ELSE
			BEGIN
				PRINT 'Existing Admin Access Found.'
			END

	END

END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'usp_ViewAdminRights')
	DROP PROCEDURE [Access].[usp_ViewAdminRights]
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 6/19/2016
-- Description:	Display Current User Rights.
-- =============================================
CREATE PROCEDURE [Access].[usp_ViewAdminRights] 
AS
BEGIN

	SET NOCOUNT ON;

	SELECT Email, r.Name [RoleName]
	FROM Access.Users u
	INNER JOIN Access.UserRoles ur
		ON u.UserId = ur.UserId
	INNER JOIN Access.Roles r
		ON ur.RoleId = r.Id
	WHERE r.Id = 1
	ORDER BY Email

END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'usp_LookupScanByVIN')
	DROP PROCEDURE [Scan].[usp_LookupScanByVIN]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'usp_ProcessScan')
	DROP PROCEDURE [Scan].[usp_ProcessScan]
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

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'usp_GetSystemStatus')
	DROP PROCEDURE [Support].[usp_GetSystemStatus]
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 06/21/2016
-- Description:	Return System Status Info.
-- =============================================
CREATE PROCEDURE [Support].[usp_GetSystemStatus] 
AS
BEGIN

	SET NOCOUNT ON;

	-- User Count By Shop
	SELECT s.Name [ShopName], COUNT(1) [UserCount]
	FROM Access.Shops s
	LEFT JOIN Access.Users u
		ON s.ID = u.Shop_ID
	GROUP BY s.Name

	-- User Count By Role
	SELECT r.Name [RoleName], COUNT(1) [UserCount]
	FROM Access.Roles r
	INNER JOIN Access.UserRoles ur
		ON r.Id = ur.RoleId
	INNER JOIN Access.Users u
		ON ur.UserId = u.UserId
	GROUP BY r.Name

	-- Repair Count By Shop
	SELECT s.Name [ShopName], COUNT(1) [RepairCount]
	FROM Access.Shops s
	INNER JOIN Repair.Orders o
		ON s.ID = o.Shop_ID
	GROUP BY s.Name

	-- Repair Count By Status
	SELECT
		CASE o.Status
			WHEN 1 THEN 'Active'
			WHEN 2 THEN 'Canceled'
			WHEN 3 THEN 'Completed'
			WHEN 4 THEN 'Invoiced'
			ELSE 'Unknown'
		END [RepairStatus],
		COUNT(1) [RepairCount]
	FROM Repair.Orders o
	GROUP BY o.Status

	-- Request Count by Type and Status
	SELECT
		CASE r.TypeOfScan
			WHEN 1 THEN 'QuickScan'
			WHEN 2 THEN 'Diagnostic'
			WHEN 3 THEN 'Completion'
			WHEN 4 THEN 'FollowUp'
			ELSE 'Unknown'
		END [RequestType],
		CASE WHEN r.ReportID IS NULL THEN 'Open' ELSE 'Closed' END [RequestStatus],
		COUNT(1) [RequestCount]
	FROM Scan.Requests r
	GROUP BY r.TypeOfScan, CASE WHEN r.ReportID IS NULL THEN 'Open' ELSE 'Closed' END
	ORDER BY r.TypeOfScan, CASE WHEN r.ReportID IS NULL THEN 'Open' ELSE 'Closed' END DESC

	-- Uploads Missing Request
	SELECT
		u.SCAN_UPLOAD_ID [UploadId],
		u.UPLOAD_XML [UploadedXml],
		u.CreatedDt [UploadedDt],
		usr.Email [UploadedBy]
	FROM Scan.Uploads u
	INNER JOIN Access.Users usr
		ON u.CreatedBy_Id = usr.UserId
	WHERE NULLIF(u.RequestId, 0) IS NULL

	-- Last 10 Exceptions
	SELECT TOP 10 *
	FROM Support.ApplicationExceptions ae
	ORDER BY ae.ExceptionDateTime DESC

	-- Last 10 Edmunds Lookups
	SELECT TOP 10 *
	FROM Repair.VehicleLookups vl
	ORDER BY vl.RequestDt DESC

	-- Last 10 Uploads
	SELECT TOP 10 *
	FROM Scan.Uploads u
	ORDER BY u.CreatedDt DESC

END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'f_DTCResults')
	DROP FUNCTION [Scan].[f_DTCResults] 
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 05/02/2016
-- Description:	Process Scan DTC Info.
-- =============================================
CREATE FUNCTION [Scan].[f_DTCResults] 
(	
	@Scan XML
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		A.b.query('CONTROLLER_NAME').value('(/CONTROLLER_NAME)[1]', 'NVARCHAR(MAX)') [CONTROLLER_NAME]
		,x.DTC_DESCRIPTION
	FROM   @Scan.nodes('/CONTROLLER') A(b)
	OUTER APPLY
	(
		SELECT
			C.d.query('DTC_DESCRIPTION').value('(DTC_DESCRIPTION)[1]', 'NVARCHAR(MAX)') [DTC_DESCRIPTION]
		FROM A.b.nodes('DTC_RESULTS') C(d)
	) x
)
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'f_FFResults')
	DROP FUNCTION [Scan].[f_FFResults] 
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
	DROP FUNCTION [Scan].[f_ScanResult] 
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

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = 'f_TestIssues')
	DROP FUNCTION [Scan].[f_TestIssues] 
GO

-- =============================================
-- Author:		Michael Sanders
-- Create date: 05/02/2016
-- Description:	Process Scan Issue Info.
-- =============================================
CREATE FUNCTION [Scan].[f_TestIssues] 
(	
	@Scan XML
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		A.b.query('TESTABILITY_ISSUE').value('(/TESTABILITY_ISSUE)[1]', 'NVARCHAR(MAX)') [TESTABILITY_ISSUE]
	FROM   @Scan.nodes('/TESTABILITY_ISSUES') A(b)
)
GO