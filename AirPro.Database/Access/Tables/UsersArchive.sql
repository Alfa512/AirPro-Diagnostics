CREATE TABLE [Access].[UsersArchive] (
    [ArchiveId]                 INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]                 DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [UserGuid]                  UNIQUEIDENTIFIER   NOT NULL,
    [UserName]                  NVARCHAR (MAX)     NULL,
    [Email]                     NVARCHAR (MAX)     NULL,
    [EmailConfirmed]            BIT                NOT NULL,
    [ContactNumber]             NVARCHAR (MAX)     NULL,
    [FirstName]                 NVARCHAR (MAX)     NULL,
    [JobTitle]                  NVARCHAR (MAX)     NULL,
    [LastName]                  NVARCHAR (MAX)     NULL,
    [PhoneNumber]               NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed]      BIT                NOT NULL,
    [SessionId]                 NVARCHAR (MAX)     NULL,
    [ShopBillingNotification]   BIT                NOT NULL,
    [ShopReportNotification]    BIT                NOT NULL,
    [TimeZoneInfoId]            NVARCHAR (MAX)     NULL,
    [TwoFactorEnabled]          BIT                NOT NULL,
    [LockoutEnabled]            BIT                NOT NULL,
    [LockoutEndDateUtc]         DATETIME           NULL,
    [AccessFailedCount]         INT                NOT NULL,
    [CreatedByUserGuid]         UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]                 DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]         UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                 DATETIMEOFFSET (7) NULL,
    [ShopStatementNotification] BIT                DEFAULT ((0)) NOT NULL,
    [EmployeeInd]               BIT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Access.UsersArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC)
);



