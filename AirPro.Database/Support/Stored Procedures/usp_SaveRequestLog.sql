
CREATE PROCEDURE Support.usp_SaveRequestLog
	@UserGuid UNIQUEIDENTIFIER NULL,
	@SessionId NVARCHAR(24) NULL,
	@UserAddress NVARCHAR(45) NULL,
	@UserAgent NVARCHAR(MAX) NULL,
	@RawUrl NVARCHAR(max) NULL,
	@RouteUrl NVARCHAR(100) NULL,
	@RouteArea NVARCHAR(100) NULL,
	@RouteController NVARCHAR(100) NULL,
	@RouteAction NVARCHAR(100) NULL,
	@RouteId NVARCHAR(100) NULL,
	@RequestMethod NVARCHAR(10) NULL,
	@ActionStartTime DATETIMEOFFSET(7) NULL,
	@ActionEndTime DATETIMEOFFSET(7) NULL,
	@ResultStartTime DATETIMEOFFSET(7) NULL,
	@ResultEndTime DATETIMEOFFSET(7) NULL,
	@ActionExceptionMessage NVARCHAR(MAX) NULL,
	@ActionExceptionStackTrace NVARCHAR(MAX) NULL,
	@ResultExceptionMessage NVARCHAR(MAX) NULL,
	@ResultExceptionStackTrace NVARCHAR(MAX) NULL
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	-- FIND USER AGENT.
	DECLARE @UserAgentId INT
	SELECT @UserAgentId = UserAgentId FROM Support.RequestLogUserAgents WHERE UserAgentHash = CHECKSUM(@UserAgent)

	-- INSERT USER AGENT.
	IF (@UserAgentId IS NULL)
	BEGIN
		DECLARE	@UserAgentAdd TABLE (Id INT)
		INSERT INTO Support.RequestLogUserAgents (UserAgentString)
		OUTPUT INSERTED.UserAgentId INTO @UserAgentAdd
		VALUES (@UserAgent)
		SELECT @UserAgentId = Id FROM @UserAgentAdd
	END

	-- CHECK ACTION EXCEPTION.
	DECLARE @ActionExceptionId INT
	IF (@ActionExceptionMessage IS NOT NULL OR @ActionExceptionStackTrace IS NOT NULL)
	BEGIN
		-- LOOKUP EXCEPTION.
		SELECT @ActionExceptionId = ExcptionId FROM Support.RequestLogExceptions WHERE ExceptionHash = CHECKSUM(@ActionExceptionMessage, @ActionExceptionStackTrace)

		-- INSERT ACTION EXCEPTION.
		IF (@ActionExceptionId IS NULL)
		BEGIN
			DECLARE @ActionExceptionAdd TABLE (Id INT)
			INSERT INTO Support.RequestLogExceptions (ExceptionMessage, ExceptionStackTrace)
			OUTPUT INSERTED.ExcptionId INTO @ActionExceptionAdd
			VALUES (@ActionExceptionMessage, @ActionExceptionStackTrace)
			SELECT @ActionExceptionId =	Id FROM @ActionExceptionAdd
		END
	END

	-- CHECK RESULT EXCEPTION.
	DECLARE @ResultExceptionId INT
	IF (@ResultExceptionMessage IS NOT NULL OR @ResultExceptionStackTrace IS NOT NULL)
	BEGIN
		-- LOOKUP EXCEPTION.
		SELECT @ResultExceptionId = ExcptionId FROM Support.RequestLogExceptions WHERE ExceptionHash = CHECKSUM(@ResultExceptionMessage, @ResultExceptionStackTrace)

		-- INSERT RESULT EXCEPTION.
		IF (@ResultExceptionId IS NULL)
		BEGIN
			DECLARE @ResultExceptionAdd TABLE (Id INT)
			INSERT INTO Support.RequestLogExceptions (ExceptionMessage, ExceptionStackTrace)
			OUTPUT INSERTED.ExcptionId INTO @ResultExceptionAdd
			VALUES (@ResultExceptionMessage, @ResultExceptionStackTrace)
			SELECT @ResultExceptionId =	Id FROM @ResultExceptionAdd
		END
	END

	-- INSERT REQUEST LOG.
	INSERT INTO	Support.RequestLogs
	( 
		UserGuid,
		SessionId,
		UserAgentId,
		UserAddress,
		RawUrl,
		RouteUrl,
		RouteArea,
		RouteController,
		RouteAction,
		RouteId,
		RequestMethod,
		ActionStartTime,
		ActionEndTime,
		ResultStartTime,
		ResultEndTime,
		ActionExceptionId,
		ResultExceptionId
	)
	VALUES
	( 
		@UserGuid, -- UserId - uniqueidentifier
		@SessionId, -- SessionId - nvarchar(24)
		@UserAgentId, -- UserAgentId - int
		@UserAddress, -- UserAddress - nvarchar(45)
		@RawUrl, -- RawUrl - nvarchar(max)
		@RouteUrl, -- RouteUrl - nvarchar(100)
		@RouteArea, -- RouteArea - nvarchar(100)
		@RouteController, -- RouteController - nvarchar(100)
		@RouteAction, -- RouteAction - nvarchar(100)
		@RouteId, -- RouteId - nvarchar(100)
		@RequestMethod, -- RequestMethod - nvarchar(10)
		@ActionStartTime, -- ActionStartTime - datetimeoffset(7)
		@ActionEndTime, -- ActionEndTime - datetimeoffset(7)
		@ResultStartTime, -- ResultStartTime - datetimeoffset(7)
		@ResultEndTime, -- ResultEndTime - datetimeoffset(7)
		@ActionExceptionId, -- ActionExceptionId - int
		@ResultExceptionId  -- ResultExceptionId - int
	)

END