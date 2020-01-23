
-- =============================================
-- Author:		Michael Sanders
-- Create date: 06/19/2016
-- Description:	Allow Management of Admin Rights.
-- =============================================
CREATE PROCEDURE Access.usp_ModifyAdminRights 
	@Email VARCHAR(256),
	@GrantAdmin BIT = 0
AS
BEGIN

	SET NOCOUNT ON;

	-- Load User Id.
	DECLARE @UserGuid UNIQUEIDENTIFIER
	SELECT @UserGuid = u.UserGuid
	FROM Access.Users u
	WHERE u.Email = @Email
	
	IF NULLIF(@UserGuid, '') IS NULL
		BEGIN
			RAISERROR (N'UserID Not Found.', 10, 1);
			RETURN 0;
		END
	ELSE
		BEGIN
			PRINT 'UserId Located: ' + CONVERT(VARCHAR(50), @UserGuid)
		END

	-- Check Admin Access.
	IF NOT EXISTS (SELECT 1 FROM Access.UserRoles ur WHERE ur.UserGuid = @UserGuid AND ur.RoleGuid = '4FD55BFD-9E79-48C8-9FB0-DB5281D5005F')
	BEGIN
		
		IF @GrantAdmin = 1
			BEGIN
				PRINT 'Granting Admin Access.'
				INSERT INTO Access.UserRoles (UserGuid, RoleGuid)
				VALUES (@UserGuid, '4FD55BFD-9E79-48C8-9FB0-DB5281D5005F')
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
				WHERE UserGuid = @UserGuid
					AND RoleGuid = '4FD55BFD-9E79-48C8-9FB0-DB5281D5005F'
			END
		ELSE
			BEGIN
				PRINT 'Existing Admin Access Found.'
			END

	END

END