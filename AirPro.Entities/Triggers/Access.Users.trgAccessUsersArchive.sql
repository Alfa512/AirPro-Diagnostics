CREATE TRIGGER [Access].[trgAccessUsersArchive] ON [Access].[Users]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.UsersArchive
	(
		UserGuid
		,Email
		,EmailConfirmed
		,PhoneNumber
		,PhoneNumberConfirmed
		,TwoFactorEnabled
		,LockoutEndDateUtc
		,LockoutEnabled
		,AccessFailedCount
		,UserName
		,FirstName
		,LastName
		,JobTitle
		,ContactNumber
		,TimeZoneInfoId
		,ShopBillingNotification
		,ShopReportNotification
		,SessionId
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,ShopStatementNotification
		,EmployeeInd
	)
	SELECT
		UserGuid
		,Email
		,EmailConfirmed
		,PhoneNumber
		,PhoneNumberConfirmed
		,TwoFactorEnabled
		,LockoutEndDateUtc
		,LockoutEnabled
		,AccessFailedCount
		,UserName
		,FirstName
		,LastName
		,JobTitle
		,ContactNumber
		,TimeZoneInfoId
		,ShopBillingNotification
		,ShopReportNotification
		,SessionId
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,ShopStatementNotification
		,EmployeeInd
	FROM deleted

END
GO

ALTER TABLE [Access].[Users] ENABLE TRIGGER [trgAccessUsersArchive]
GO