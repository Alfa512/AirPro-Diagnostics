CREATE TABLE [Access].[Users] (
    [UserGuid]                  UNIQUEIDENTIFIER   CONSTRAINT [DF_Access.Users_UserGuid] DEFAULT (newsequentialid()) NOT NULL,
    [Email]                     NVARCHAR (256)     NULL,
    [EmailConfirmed]            BIT                NOT NULL,
    [PasswordHash]              NVARCHAR (MAX)     NULL,
    [SecurityStamp]             NVARCHAR (MAX)     NULL,
    [PhoneNumber]               NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed]      BIT                NOT NULL,
    [TwoFactorEnabled]          BIT                NOT NULL,
    [LockoutEndDateUtc]         DATETIME           NULL,
    [LockoutEnabled]            BIT                NOT NULL,
    [AccessFailedCount]         INT                NOT NULL,
    [UserName]                  NVARCHAR (256)     NOT NULL,
    [FirstName]                 NVARCHAR (MAX)     NULL,
    [LastName]                  NVARCHAR (MAX)     NULL,
    [JobTitle]                  NVARCHAR (MAX)     NULL,
    [ContactNumber]             NVARCHAR (MAX)     NULL,
    [TimeZoneInfoId]            NVARCHAR (MAX)     NULL,
    [ShopBillingNotification]   BIT                DEFAULT ((0)) NOT NULL,
    [ShopReportNotification]    BIT                DEFAULT ((0)) NOT NULL,
    [SessionId]                 NVARCHAR (24)      NULL,
    [CreatedByUserGuid]         UNIQUEIDENTIFIER   DEFAULT ([Common].[udf_GetEmptyGuid]()) NOT NULL,
    [CreatedDt]                 DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [UpdatedByUserGuid]         UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                 DATETIMEOFFSET (7) NULL,
    [ShopStatementNotification] BIT                DEFAULT ((0)) NOT NULL,
    [DisplayName]               AS                 (([LastName]+', ')+[FirstName]),
    [EmployeeInd]               BIT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Access.Users] PRIMARY KEY CLUSTERED ([UserGuid] ASC)
);


















GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [Access].[Users]([UserName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SessionId]
    ON [Access].[Users]([SessionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_UserGuid_LastName_FirstName]
    ON [Access].[Users]([UserGuid] ASC)
    INCLUDE([LastName], [FirstName]);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[Users]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[Users]([CreatedByUserGuid] ASC);


GO
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
CREATE NONCLUSTERED INDEX [IDX_AccessUsers_UserTimeZoneId]
    ON [Access].[Users]([UserGuid] ASC)
    INCLUDE([TimeZoneInfoId]);

